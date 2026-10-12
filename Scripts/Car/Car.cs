using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviourPunCallbacks {
    [Header("═══ КОЛЁСА (Визуал) ═══")]
    public Transform wheelMeshFL;
    public Transform wheelMeshFR;
    public Transform wheelMeshRL;
    public Transform wheelMeshRR;
    public float wheelRadius = 0.4f;
    public float wheelSpinDirection = 1f;

    [Header("═══ СКОРОСТЬ ═══")]
    public float accel = 10f;
    public float engineBrake = 6f;
    public float brakeDecel = 20f;
    public float maxSpeed = 18f;
    public float maxReverseSpeed = 6f;

    [Header("═══ ПОВОРОТЫ ═══")]
    public float turnDegreesPerSec = 70f;
    public float steerResponse = 4f;
    [Range(0f, 1f)] public float slipFactor = 0.03f;
    public float lateralGrip = 15f;
    public float handbrakeGrip = 1.2f;
    public bool invertSteer = false;

    [Header("═══ ЗЕМЛЯ ═══")]
    public float rideHeight = 0.3f;
    public float wheelGroundOffset = 0.2f;
    public float rayOriginOffset = 4f;
    public float rayLength = 12f;
    public float gravity = 30f;
    public bool snapOnStart = true;
    [Range(0.01f, 0.5f)] public float groundSmoothing = 0.15f;

    [Header("═══ ВЗАИМОДЕЙСТВИЕ ═══")]
    public float enterRadius = 3.5f;
    public float seatCooldown = 0.3f;

    [Header("═══ ТОЧКИ ПОСАДКИ ═══")]
    public Transform driverSeat;
    public Transform passengerSeat;
    public Transform cameraPoint;

    [Header("═══ UI ═══")]
    public GameObject promptUI;
    public TMPro.TextMeshProUGUI promptText;

    private PhotonView pv;
    private Rigidbody rb;
    private Player driver;
    private Player passenger;
    private float inputThrottle = 0f;
    private float inputSteering = 0f;
    private float inputBrake = 0f;
    private float inputHandbrake = 0f;
    private float motorSpeed = 0f;
    private float latVel = 0f;
    private float steerSmooth = 0f;
    private float airYVel = 0f;
    private bool isGrounded = true;
    private float groundY = 0f;
    private float smoothedGroundY = 0f;
    private bool groundInitialized = false;
    private float wheelSpin = 0f;
    private float currentYaw = 0f;
    private Vector3[] wheelBasePos = new Vector3[4];

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool hasTarget = false;

    private Transform savedCamParent;
    private Vector3 savedCamLocalPos;
    private Quaternion savedCamLocalRot;
    private bool camSaved = false;
    private float lastSeatTime = -10f;
    private float lastExitTime = -10f;
    private Vector3 prevPhysPos;
    private Quaternion prevPhysRot;
    private float lastFixedTime;

    public float CurrentSpeed { get { return Mathf.Abs(motorSpeed); } }
    public bool HasDriver { get { return driver != null; } }
    public bool IsLocalDriver { get { return driver != null && driver.IsLocal; } }
    public float ThrottleInput { get { return inputThrottle; } }
    public bool IsSkidding { get; private set; }
    public float GroundHeight { get { return smoothedGroundY; } }

    private bool iControl { get { return driver != null && driver.IsLocal; } }

    public Vector3 GetSmoothPosition() {
        float t = Mathf.Clamp01((Time.time - lastFixedTime) / Mathf.Max(Time.fixedDeltaTime, 1e-4f));
        return Vector3.Lerp(prevPhysPos, rb.position, t);
    }

    public Quaternion GetSmoothRotation() {
        float t = Mathf.Clamp01((Time.time - lastFixedTime) / Mathf.Max(Time.fixedDeltaTime, 1e-4f));
        return Quaternion.Slerp(prevPhysRot, rb.rotation, t);
    }

    void Awake() {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        // Включаем встроенную физическую интерполяцию для безупречной плавности у всех клиентов
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        foreach (WheelCollider wc in GetComponentsInChildren<WheelCollider>()) wc.enabled = false;
        CaptureWheels();
        prevPhysPos = rb.position;
        prevPhysRot = rb.rotation;
        currentYaw = transform.eulerAngles.y;
        smoothedGroundY = transform.position.y;
        groundInitialized = false;
    }

    void Start() {
        if (promptUI != null) promptUI.SetActive(false);
        if (snapOnStart) {
            RaycastHit h;
            if (ProbeGround(out h, 50f)) {
                groundY = h.point.y;
                smoothedGroundY = groundY;
                groundInitialized = true;
                Vector3 p = transform.position;
                p.y = groundY + rideHeight + wheelGroundOffset;
                rb.position = p;
                prevPhysPos = rb.position;
                targetPosition = p;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        if (PhotonNetwork.IsMasterClient) {
            int driverId = driver != null ? driver.ActorNumber : -1;
            int passengerId = passenger != null ? passenger.ActorNumber : -1;
            if (driverId != -1 || passengerId != -1) {
                pv.RPC("RPC_SyncOccupants", newPlayer, driverId, passengerId);
            }
        }
    }

    [PunRPC]
    void RPC_SyncOccupants(int driverActor, int passengerActor) {
        if (driverActor != -1) {
            Player p = PhotonNetwork.CurrentRoom.GetPlayer(driverActor);
            if (p != null) {
                driver = p;
                ParentPlayerToSeat(p, 0);
            }
        }
        if (passengerActor != -1) {
            Player p = PhotonNetwork.CurrentRoom.GetPlayer(passengerActor);
            if (p != null) {
                passenger = p;
                ParentPlayerToSeat(p, 1);
            }
        }
    }

    void ParentPlayerToSeat(Player player, int seatIndex) {
        GameObject playerObj = GetPlayerObject(player);
        if (playerObj == null) return;
        Transform seat = seatIndex == 0 ? driverSeat : passengerSeat;
        if (seat == null) return;
        PlayerController pc = playerObj.GetComponent<PlayerController>();
        if (pc != null) {
            pc.isInCar = true;
            pc.currentCar = this;
            pc.movement.enabled = false;
            pc.building.enabled = false;
        }
        CharacterController cc = playerObj.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        foreach (Collider c in playerObj.GetComponentsInChildren<Collider>()) {
            if (c != null) c.enabled = false;
        }
        playerObj.transform.SetParent(seat);
        playerObj.transform.localPosition = Vector3.zero;
        playerObj.transform.localRotation = Quaternion.identity;
        SetPlayerModelVisible(playerObj, true);
    }

    void CaptureWheels() {
        if (wheelMeshFL == null) wheelMeshFL = FindMesh("FL");
        if (wheelMeshFR == null) wheelMeshFR = FindMesh("FR");
        if (wheelMeshRL == null) wheelMeshRL = FindMesh("RL");
        if (wheelMeshRR == null) wheelMeshRR = FindMesh("RR");
        wheelBasePos[0] = wheelMeshFL != null ? wheelMeshFL.localPosition : Vector3.zero;
        wheelBasePos[1] = wheelMeshFR != null ? wheelMeshFR.localPosition : Vector3.zero;
        wheelBasePos[2] = wheelMeshRL != null ? wheelMeshRL.localPosition : Vector3.zero;
        wheelBasePos[3] = wheelMeshRR != null ? wheelMeshRR.localPosition : Vector3.zero;
    }

    Transform FindMesh(string tag) {
        foreach (Transform t in GetComponentsInChildren<Transform>()) {
            if (t.name.ToUpper().Contains(tag) && t.GetComponent<MeshFilter>() != null) return t;
        }
        return null;
    }

    bool ProbeGround(out RaycastHit best, float length = -1f) {
        best = default(RaycastHit);
        if (length < 0f) length = rayLength;
        Vector3 origin = rb.position + Vector3.up * rayOriginOffset;
        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down, rayOriginOffset + length, -1, QueryTriggerInteraction.Ignore);
        float nearest = float.MaxValue;
        bool found = false;
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].transform.IsChildOf(transform)) continue;
            if (hits[i].distance < nearest) {
                nearest = hits[i].distance;
                best = hits[i];
                found = true;
            }
        }
        return found;
    }

    void Update() {
        if (driver == null && passenger == null && Input.GetKeyDown(KeyCode.E) && Time.unscaledTime - lastExitTime > seatCooldown) {
            PlayerController localPc = GetLocalPlayerController();
            if (localPc != null && !localPc.isInCar && !localPc.isPlayerDead) {
                if (Vector3.Distance(localPc.transform.position, transform.position) <= enterRadius) {
                    Player local = PhotonNetwork.LocalPlayer;
                    if (local != null) TryInteract(local);
                }
            }
        }

        bool isLocalOccupant = (driver != null && driver.IsLocal) || (passenger != null && passenger.IsLocal);
        if (isLocalOccupant) {
            if (driver != null && driver.IsLocal && PhotonNetwork.IsConnected && !pv.IsMine) {
                pv.RequestOwnership();
            }
            HandleDrivingInput();
        }

        UpdateWheelVisuals(Time.deltaTime);
    }

    void FixedUpdate() {
        prevPhysPos = rb.position;
        prevPhysRot = rb.rotation;
        lastFixedTime = Time.time;

        if (iControl) {
            ApplyPhysics();
            if (PhotonNetwork.IsConnected) {
                // Отправляем текущую позицию и поворот на каждом физическом шаге для максимальной плавности
                pv.RPC("RPC_SyncCarState", RpcTarget.Others, rb.position, rb.rotation);
            }
        }
        else if (driver == null) {
            ApplyParkingBrake();
            if (PhotonNetwork.IsConnected && pv.IsMine) {
                pv.RPC("RPC_SyncCarState", RpcTarget.Others, rb.position, rb.rotation);
            }
        }
        else {
            // ДЛЯ ПАССАЖИРА И ДРУГИХ ИГРОКОВ: передаем позиции напрямую в Rigidbody.
// Встроенная интерполяция Unity сделает движение абсолютно плавным без дерганий.
            if (hasTarget) {
                rb.MovePosition(targetPosition);
                rb.MoveRotation(targetRotation);
            }
        }
    }

    void ApplyPhysics() {
        float dt = Time.fixedDeltaTime;
        float steerSigned = inputSteering * (invertSteer ? -1f : 1f);
        steerSmooth = Mathf.MoveTowards(steerSmooth, steerSigned, steerResponse * dt);
        if (inputThrottle > 0.1f) motorSpeed += accel * dt * inputThrottle;
        else if (inputThrottle < -0.1f) motorSpeed += accel * dt * inputThrottle * 0.6f;
        else motorSpeed = Mathf.MoveTowards(motorSpeed, 0f, engineBrake * dt);

        if (inputBrake > 0.1f) motorSpeed = Mathf.MoveTowards(motorSpeed, 0f, brakeDecel * dt);
        motorSpeed = Mathf.Clamp(motorSpeed, -maxReverseSpeed, maxSpeed);

        RaycastHit hit;
        bool found = ProbeGround(out hit);
        if (found) {
            groundY = hit.point.y;
            isGrounded = true;
            float smoothFactor = 1f - Mathf.Exp(-groundSmoothing * dt * 50f);
            if (!groundInitialized) {
                smoothedGroundY = groundY;
                groundInitialized = true;
            } else {
                smoothedGroundY = Mathf.Lerp(smoothedGroundY, groundY, smoothFactor);
            }
        } else {
            isGrounded = false;
        }

        float speedFactor = Mathf.Clamp01(Mathf.Abs(motorSpeed) / 0.5f);
        float airControl = isGrounded ? 1f : 0.2f;
        float turnRate = steerSmooth * turnDegreesPerSec * speedFactor * (motorSpeed >= 0 ? 1 : -1) * airControl;
        currentYaw += turnRate * dt;
        latVel += -turnRate * Mathf.Deg2Rad * Mathf.Abs(motorSpeed) * slipFactor * dt;

        float damp = (inputHandbrake > 0.1f) ? handbrakeGrip : lateralGrip;
        latVel -= latVel * Mathf.Clamp01(damp * dt);
        latVel = Mathf.Clamp(latVel, -2f, 2f);

        IsSkidding = (inputHandbrake > 0.1f && Mathf.Abs(motorSpeed) > 3f) || Mathf.Abs(latVel) > 1.5f;

        Vector3 horizontalVel = transform.forward * motorSpeed + transform.right * latVel;
        Vector3 newPos = rb.position + horizontalVel * dt;

        if (isGrounded) {
            float targetY = smoothedGroundY + rideHeight + wheelGroundOffset;
            newPos.y = Mathf.Lerp(rb.position.y, targetY, dt * 25f);
            airYVel = 0f;
        } else {
            airYVel -= gravity * dt;
            newPos.y += airYVel * dt;
        }

        rb.MovePosition(newPos);
        rb.MoveRotation(Quaternion.Euler(0f, currentYaw, 0f));
    }

    void ApplyParkingBrake() {
        float dt = Time.fixedDeltaTime;
        motorSpeed = Mathf.MoveTowards(motorSpeed, 0f, brakeDecel * dt);
        latVel -= latVel * Mathf.Clamp01(lateralGrip * dt);
        steerSmooth = Mathf.MoveTowards(steerSmooth, 0f, steerResponse * dt);

        RaycastHit hit;
        bool found = ProbeGround(out hit);
        if (found) {
            groundY = hit.point.y;
            isGrounded = true;
            float smoothFactor = 1f - Mathf.Exp(-groundSmoothing * dt * 50f);
            if (!groundInitialized) {
                smoothedGroundY = groundY;
                groundInitialized = true;
            } else {
                smoothedGroundY = Mathf.Lerp(smoothedGroundY, groundY, smoothFactor);
            }
            Vector3 newPos = rb.position;
            float targetY = smoothedGroundY + rideHeight + wheelGroundOffset;
            newPos.y = Mathf.Lerp(rb.position.y, targetY, dt * 25f);
            rb.MovePosition(newPos);
            airYVel = 0f;
        } else {
            isGrounded = false;
        }
        IsSkidding = false;
    }

    void HandleDrivingInput() {
        if (Input.GetKeyDown(KeyCode.E) && Time.unscaledTime - Mathf.Max(lastSeatTime, lastExitTime) > seatCooldown) {
            ExitCar();
            return;
        }

        if (driver != null && driver.IsLocal) {
            inputThrottle = ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ? 1f : 0f) - ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? 1f : 0f);
            inputSteering = ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1f : 0f) - ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? 1f : 0f);
            inputBrake = Input.GetKey(KeyCode.Space) ? 1f : 0f;
            inputHandbrake = Input.GetKey(KeyCode.LeftShift) ? 1f : 0f;
        } else {
            inputThrottle = 0f;
            inputSteering = 0f;
            inputBrake = 0f;
            inputHandbrake = 0f;
        }
    }

    void UpdateWheelVisuals(float dt) {
        wheelSpin += (motorSpeed / Mathf.Max(0.1f, wheelRadius)) * Mathf.Rad2Deg * dt * wheelSpinDirection;
        ApplyWheel(wheelMeshFL, wheelBasePos[0], true);
        ApplyWheel(wheelMeshFR, wheelBasePos[1], true);
        ApplyWheel(wheelMeshRL, wheelBasePos[2], false);
        ApplyWheel(wheelMeshRR, wheelBasePos[3], false);
    }

    void ApplyWheel(Transform m, Vector3 basePos, bool front) {
        if (m == null) return;
        m.localPosition = basePos;
        float steer = front ? steerSmooth * 30f : 0f;
        m.localRotation = Quaternion.Euler(0f, steer, 0f) * Quaternion.Euler(0f, 0f, 90f) * Quaternion.Euler(0f, wheelSpin, 0f);
    }

    [PunRPC]
    void RPC_SyncCarState(Vector3 position, Quaternion rotation) {
        targetPosition = position;
        targetRotation = rotation;
        hasTarget = true;
    }

    public bool TryInteract(Player player) {
        if (player == null) return false;
        if (driver == player || passenger == player) {
            ExitCar();
            return false;
        }
        if (driver != null && passenger != null) {
            ShowPrompt("❌ Машина полная!");
            return false;
        }
        if (driver == null) {
            driver = player;
            EnterAsDriver(player);
            if (PhotonNetwork.IsConnected) {
                pv.RPC("RPC_SetDriver", RpcTarget.Others, player.ActorNumber);
                pv.RPC("RPC_ParentSeat", RpcTarget.Others, player.ActorNumber, 0);
            }
            return true;
        }
        if (passenger == null) {
            passenger = player;
            EnterAsPassenger(player);
            if (PhotonNetwork.IsConnected) {
                pv.RPC("RPC_SetPassenger", RpcTarget.Others, player.ActorNumber);
                pv.RPC("RPC_ParentSeat", RpcTarget.Others, player.ActorNumber, 1);
            }
            return true;
        }
        return false;
    }

    void ResetMotion() {
        motorSpeed = 0f;
        latVel = 0f;
        steerSmooth = 0f;
        airYVel = 0f;
        inputThrottle = 0f;
        inputSteering = 0f;
        inputBrake = 0f;
        inputHandbrake = 0f;
    }

    void EnterAsDriver(Player player) {
        GameObject playerObj = GetPlayerObject(player);
        if (playerObj == null) return;
        PlayerController pc = playerObj.GetComponent<PlayerController>();
        if (pc == null) return;
        ResetMotion();
        lastSeatTime = Time.unscaledTime;
        if (player.IsLocal) SaveCamera(pc);
        pc.EnterCarMode(this);
        pc.movement.enabled = false;
        pc.building.enabled = false;
        CharacterController cc = playerObj.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        foreach (Collider col in playerObj.GetComponentsInChildren<Collider>()) {
            if (col != null) col.enabled = false;
        }
        playerObj.transform.SetParent(driverSeat);
        playerObj.transform.localPosition = Vector3.zero;
        playerObj.transform.localRotation = Quaternion.identity;
        playerObj.transform.localScale = Vector3.one;
        SetPlayerModelVisible(playerObj, true);
        if (player.IsLocal && PhotonNetwork.IsConnected) {
            pv.TransferOwnership(player);
        }
        Debug.Log($"🚗 {player.NickName} сел за руль");
    }

    void EnterAsPassenger(Player player) {
        GameObject playerObj = GetPlayerObject(player);
        if (playerObj == null) return;
        PlayerController pc = playerObj.GetComponent<PlayerController>();
        if (pc == null) return;
        lastSeatTime = Time.unscaledTime;
        if (player.IsLocal) SaveCamera(pc);
        pc.EnterCarMode(this);
        pc.movement.enabled = false;
        pc.building.enabled = false;
        CharacterController cc = playerObj.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        foreach (Collider col in playerObj.GetComponentsInChildren<Collider>()) {
            if (col != null) col.enabled = false;
        }
        playerObj.transform.SetParent(passengerSeat);
        playerObj.transform.localPosition = Vector3.zero;
        playerObj.transform.localRotation = Quaternion.identity;
        playerObj.transform.localScale = Vector3.one;
        SetPlayerModelVisible(playerObj, true);
        Debug.Log($"🚗 {player.NickName} сел пассажиром");
    }

    void SaveCamera(PlayerController pc) {
        if (pc == null || pc.playerCamera == null || camSaved) return;
        Transform cam = pc.playerCamera.transform;
        savedCamParent = cam.parent;
        savedCamLocalPos = cam.localPosition;
        savedCamLocalRot = cam.localRotation;
        camSaved = true;
    }

    void RestoreCamera(PlayerController pc) {
        if (pc == null || pc.playerCamera == null || !camSaved) return;
        Transform cam = pc.playerCamera.transform;
        cam.SetParent(savedCamParent);
        cam.localPosition = savedCamLocalPos;
        cam.localRotation = savedCamLocalRot;
        camSaved = false;
    }

    public void ExitCar() {
        if (driver != null && driver.IsLocal) ExitAsDriver();
        else if (passenger != null && passenger.IsLocal) ExitAsPassenger();
    }

    void ExitAsDriver() {
        if (driver == null || !driver.IsLocal) return;
        GameObject playerObj = GetPlayerObject(driver);
        if (playerObj == null) return;
        PlayerController pc = playerObj.GetComponent<PlayerController>();
        Player player = driver;
        driver = null;
        lastExitTime = Time.unscaledTime;
        ResetMotion();
        if (pc != null) {
            playerObj.transform.SetParent(null);
            playerObj.transform.localScale = Vector3.one;
            Vector3 exitPos = transform.position - transform.right * 2.5f;
            exitPos.y = transform.position.y + 0.2f;
            playerObj.transform.position = exitPos;
            playerObj.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            foreach (Collider col in playerObj.GetComponentsInChildren<Collider>()) {
                if (col != null) col.enabled = true;
            }
            CharacterController cc = playerObj.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = true;
            RestoreCamera(pc);
            pc.ExitCarMode();
            pc.movement.enabled = true;
            pc.building.enabled = true;
            SetPlayerModelVisible(playerObj, true);
        }
        if (PhotonNetwork.IsConnected) {
            pv.RPC("RPC_ClearDriver", RpcTarget.Others);
            pv.RPC("RPC_UnparentSeat", RpcTarget.Others, player.ActorNumber, 0);
        }
        Debug.Log($"🚶 {player.NickName} вышел из машины");
    }

    void ExitAsPassenger() {
        if (passenger == null || !passenger.IsLocal) return;
        GameObject playerObj = GetPlayerObject(passenger);
        if (playerObj == null) return;
        PlayerController pc = playerObj.GetComponent<PlayerController>();
        Player player = passenger;
        passenger = null;
        lastExitTime = Time.unscaledTime;
        if (pc != null) {
            playerObj.transform.SetParent(null);
            playerObj.transform.localScale = Vector3.one;
            Vector3 exitPos = transform.position + transform.right * 2.5f;
            exitPos.y = transform.position.y + 0.2f;
            playerObj.transform.position = exitPos;
            playerObj.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            foreach (Collider col in playerObj.GetComponentsInChildren<Collider>()) {
                if (col != null) col.enabled = true;
            }
            CharacterController cc = playerObj.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = true;
            RestoreCamera(pc);
            pc.ExitCarMode();
            pc.movement.enabled = true;
            pc.building.enabled = true;
            SetPlayerModelVisible(playerObj, true);
        }
        if (PhotonNetwork.IsConnected) {
            pv.RPC("RPC_ClearPassenger", RpcTarget.Others);
            pv.RPC("RPC_UnparentSeat", RpcTarget.Others, player.ActorNumber, 1);
        }
        Debug.Log($"🚶 {player.NickName} вышел из машины");
    }

    [PunRPC]
    void RPC_ParentSeat(int actorNumber, int seatIndex, PhotonMessageInfo info) {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        if (p == null || p.IsLocal) return;
        ParentPlayerToSeat(p, seatIndex);
    }

    [PunRPC]
    void RPC_UnparentSeat(int actorNumber, int seatIndex, PhotonMessageInfo info) {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        if (p == null || p.IsLocal) return;
        GameObject obj = GetPlayerObject(p);
        if (obj == null) return;
        PlayerController pc = obj.GetComponent<PlayerController>();
        if (pc != null) {
            pc.isInCar = false;
            pc.currentCar = null;
        }
        obj.transform.SetParent(null);
        Vector3 exitPos = transform.position + (seatIndex == 0 ? -transform.right : transform.right) * 2.5f;
        exitPos.y = transform.position.y + 0.2f;
        obj.transform.position = exitPos;
        obj.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        foreach (Collider c in obj.GetComponentsInChildren<Collider>()) {
            if (c != null) c.enabled = true;
        }
        CharacterController cc = obj.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = true;
        SetPlayerModelVisible(obj, true);
    }

    void SetPlayerModelVisible(GameObject playerObj, bool visible) {
        foreach (Renderer r in playerObj.GetComponentsInChildren<Renderer>()) {
            if (r != null) r.enabled = visible;
        }
    }

    PlayerController GetLocalPlayerController() {
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>()) {
            if (pc.view != null && pc.view.IsMine) return pc;
        }
        return null;
    }

    GameObject GetPlayerObject(Player player) {
        if (player == null) return null;
        if (player.IsLocal) {
            foreach (PlayerController pc in FindObjectsOfType<PlayerController>()) {
                if (pc.view != null && pc.view.IsMine) return pc.gameObject;
            }
        }
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>()) {
            if (pc.view != null && pc.view.Owner != null && pc.view.Owner.ActorNumber == player.ActorNumber) {
                return pc.gameObject;
            }
        }
        return null;
    }

    [PunRPC]
    void RPC_SetDriver(int actorNumber, PhotonMessageInfo info) {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        if (p != null) driver = p;
    }

    [PunRPC]
    void RPC_SetPassenger(int actorNumber, PhotonMessageInfo info) {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        if (p != null) passenger = p;
    }

    [PunRPC]
    void RPC_ClearDriver() {
        driver = null;
    }

    [PunRPC]
    void RPC_ClearPassenger() {
        passenger = null;
    }

    public bool IsFull() {
        return driver != null && passenger != null;
    }

    public string GetInteractPrompt() {
        if (driver == null) return "[E] Сесть за руль";
        if (passenger == null) return "[E] Сесть пассажиром";
        return null;
    }

    public void ShowPrompt(string text) {
        if (promptUI != null && promptText != null) {
            promptUI.SetActive(true);
            promptText.text = text;
            StartCoroutine(HidePromptAllowed(2f));
        }
    }

    System.Collections.IEnumerator HidePromptAllowed(float delay) {
        yield return new WaitForSeconds(delay);
        if (promptUI != null) promptUI.SetActive(false);
    }
}