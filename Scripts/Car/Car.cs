using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviourPun
{
    [Header("═══ КОЛЁСА (Визуал) ═══")]
    public Transform wheelMeshFL;
    public Transform wheelMeshFR;
    public Transform wheelMeshRL;
    public Transform wheelMeshRR;
    public float wheelRadius = 0.4f;
    [Tooltip("Если колёса крутятся не в ту сторону — поставь -1")]
    public float wheelSpinDirection = 1f;

    [Header("═══ СКОРОСТЬ ═══")]
    public float accel = 12f;
    public float engineBrake = 6f;
    public float brakeDecel = 20f;
    public float maxSpeed = 30f;
    public float maxReverseSpeed = 10f;

    [Header("═══ ПОВОРОТЫ ═══")]
    public float turnDegreesPerSec = 120f;
    public float steerResponse = 6f;
    [Range(0f, 1f)] public float slipFactor = 0.25f;
    public float lateralGrip = 6f;
    public float handbrakeGrip = 1.2f;
    public float bankAngle = 6f;
    public float accelPitch = 3f;
    [Tooltip("Если вправо рулит влево — поставь галочку")]
    public bool invertSteer = false;

    [Header("═══ ЗЕМЛЯ ═══")]
    public float rideHeight = 0.95f;
    public float rayOriginHeight = 0.9f;
    public float rayLength = 8f;
    public float groundSnap = 8f;
    public float groundAlign = 10f;
    public float gravity = 25f;
    public bool snapOnStart = true;

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
    private bool wasGrounded = true;
    private bool isGrounded = true;
    private float groundErr = 0f;
    private float wheelSpin = 0f;

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

    public float CurrentSpeed { get { return rb != null ? rb.velocity.magnitude : 0f; } }
    public bool HasDriver { get { return driver != null; } }
    public bool IsLocalDriver { get { return driver != null && driver.IsLocal; } }
    public float ThrottleInput { get { return inputThrottle; } }
    public bool IsSkidding { get; private set; }

    private bool iControl { get { return driver != null && driver.IsLocal; } }

    bool KeyDown(KeyCode k)
    {
        #if ENABLE_INPUT_SYSTEM
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb != null)
        {
            switch (k)
            {
                case KeyCode.W: if (kb.wKey.isPressed) return true; break;
                case KeyCode.S: if (kb.sKey.isPressed) return true; break;
                case KeyCode.A: if (kb.aKey.isPressed) return true; break;
                case KeyCode.D: if (kb.dKey.isPressed) return true; break;
                case KeyCode.UpArrow: if (kb.upArrowKey.isPressed) return true; break;
                case KeyCode.DownArrow: if (kb.downArrowKey.isPressed) return true; break;
                case KeyCode.LeftArrow: if (kb.leftArrowKey.isPressed) return true; break;
                case KeyCode.RightArrow: if (kb.rightArrowKey.isPressed) return true; break;
                case KeyCode.Space: if (kb.spaceKey.isPressed) return true; break;
                case KeyCode.LeftShift: if (kb.leftShiftKey.isPressed) return true; break;
            }
        }
        #endif
        #if ENABLE_LEGACY_INPUT_MANAGER
        return UnityEngine.Input.GetKey(k);
        #endif
        return false;
    }

    bool KeyPressed(KeyCode k)
    {
        #if ENABLE_INPUT_SYSTEM
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb != null)
        {
            switch (k)
            {
                case KeyCode.E: if (kb.eKey.wasPressedThisFrame) return true; break;
            }
        }
        #endif
        #if ENABLE_LEGACY_INPUT_MANAGER
        return UnityEngine.Input.GetKeyDown(k);
        #endif
        return false;
    }

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        rb.mass = 1500f;
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.WakeUp();

        foreach (WheelCollider wc in GetComponentsInChildren<WheelCollider>())
            wc.enabled = false;

        CaptureWheels();

        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Start()
    {
        if (promptUI != null) promptUI.SetActive(false);

        if (snapOnStart)
        {
            RaycastHit h;
            if (ProbeGround(out h, 50f))
            {
                transform.position = h.point + Vector3.up * rideHeight;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    void CaptureWheels()
    {
        if (wheelMeshFL == null) wheelMeshFL = FindMesh("FL");
        if (wheelMeshFR == null) wheelMeshFR = FindMesh("FR");
        if (wheelMeshRL == null) wheelMeshRL = FindMesh("RL");
        if (wheelMeshRR == null) wheelMeshRR = FindMesh("RR");

        wheelBasePos[0] = wheelMeshFL != null ? wheelMeshFL.localPosition : Vector3.zero;
        wheelBasePos[1] = wheelMeshFR != null ? wheelMeshFR.localPosition : Vector3.zero;
        wheelBasePos[2] = wheelMeshRL != null ? wheelMeshRL.localPosition : Vector3.zero;
        wheelBasePos[3] = wheelMeshRR != null ? wheelMeshRR.localPosition : Vector3.zero;
    }

    Transform FindMesh(string tag)
    {
        foreach (Transform t in GetComponentsInChildren<Transform>())
            if (t.name.ToUpper().Contains(tag) && t.GetComponent<Renderer>() != null)
                return t;
        return null;
    }

    bool ProbeGround(out RaycastHit best, float length = -1f)
    {
        best = default(RaycastHit);
        if (length < 0f) length = rayLength;

        Vector3 origin = transform.position + Vector3.up * rayOriginHeight;
        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down, length, -1, QueryTriggerInteraction.Ignore);

        float nearest = float.MaxValue;
        bool found = false;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.IsChildOf(transform)) continue;
            if (hits[i].distance < nearest)
            {
                nearest = hits[i].distance;
                best = hits[i];
                found = true;
            }
        }
        return found;
    }

    float ComputeHeightError(bool found, RaycastHit hit)
    {
        if (!found) return 999f;
        float groundLocalY = rayOriginHeight - hit.distance;
        return groundLocalY + rideHeight;
    }

    void Update()
    {
        if (driver == null && passenger == null
            && KeyPressed(KeyCode.E)
            && Time.unscaledTime - lastExitTime > seatCooldown)
        {
            PlayerController localPc = GetLocalPlayerController();
            if (localPc != null && !localPc.isInCar && !localPc.isPlayerDead)
            {
                if (Vector3.Distance(localPc.transform.position, transform.position) <= enterRadius)
                {
                    Player local = PhotonNetwork.LocalPlayer;
                    if (local != null) TryInteract(local);
                }
            }
        }

        if (iControl)
        {
            if (PhotonNetwork.IsConnected && !pv.IsMine)
                pv.RequestOwnership();
            HandleDrivingInput();
        }

        UpdateWheelVisuals(Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (iControl)
        {
            ApplyPhysics();
            if (PhotonNetwork.IsConnected && Time.frameCount % 6 == 0)
                pv.RPC("RPC_SyncCarState", RpcTarget.Others, rb.position, rb.rotation, rb.velocity);
        }
        else if (driver == null)
        {
            ApplyParkingBrake();
            if (PhotonNetwork.IsConnected && pv.IsMine && Time.frameCount % 6 == 0)
                pv.RPC("RPC_SyncCarState", RpcTarget.Others, rb.position, rb.rotation, rb.velocity);
        }
        else if (hasTarget)
        {
            float t = Time.fixedDeltaTime * 15f;
            rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, t));
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, t));
        }
    }

    void ApplyPhysics()
    {
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
        groundErr = ComputeHeightError(found, hit);
        isGrounded = found && Mathf.Abs(groundErr) < 1.5f;

        if (isGrounded && !wasGrounded) airYVel = rb.velocity.y;
        wasGrounded = isGrounded;

        float speed01 = Mathf.Clamp01(Mathf.Abs(motorSpeed) / maxSpeed);
        float speedFactor = Mathf.Clamp01(Mathf.Abs(motorSpeed) / 6f);
        float airControl = isGrounded ? 1f : 0.35f;

        // ✅ БЕЗ МИНУСА: D (steerSmooth +1) → поворот ВПРАВО
        float turnRate = steerSmooth * turnDegreesPerSec * (1f - 0.5f * speed01)
                         * speedFactor * (motorSpeed >= 0 ? 1 : -1) * airControl;

        Quaternion yawStep = Quaternion.Euler(0f, turnRate * dt, 0f);
        if (isGrounded)
        {
            Quaternion align = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            // ✅ крен: вправо → кузов кренится влево (наружу)
            Quaternion lean = Quaternion.Euler(-accelPitch * inputThrottle, 0f, steerSmooth * speedFactor * bankAngle);
            Quaternion target = align * yawStep * lean;
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, target, Mathf.Clamp01(groundAlign * dt)));
        }
        else
        {
            rb.MoveRotation(rb.rotation * yawStep);
        }

        // ✅ занос: вправо (turnRate +) → толкает НАРУЖУ (влево, -right)
        latVel += -turnRate * Mathf.Deg2Rad * Mathf.Abs(motorSpeed) * slipFactor * dt;
        float damp = (inputHandbrake > 0.1f) ? handbrakeGrip : lateralGrip;
        latVel -= latVel * Mathf.Clamp01(damp * dt);
        latVel = Mathf.Clamp(latVel, -12f, 12f);

        IsSkidding = (inputHandbrake > 0.1f && Mathf.Abs(motorSpeed) > 4f) || Mathf.Abs(latVel) > 3f;

        Vector3 vel = transform.forward * motorSpeed + transform.right * latVel;

        if (isGrounded)
        {
            float yVel = Mathf.Clamp(groundErr * groundSnap, -10f, 6f);
            rb.velocity = vel + Vector3.up * yVel;
            airYVel = 0f;
        }
        else
        {
            airYVel -= gravity * dt;
            rb.velocity = vel + Vector3.up * airYVel;
        }
    }

    void ApplyParkingBrake()
    {
        float dt = Time.fixedDeltaTime;
        motorSpeed = Mathf.MoveTowards(motorSpeed, 0f, brakeDecel * dt);
        latVel -= latVel * Mathf.Clamp01(lateralGrip * dt);
        steerSmooth = Mathf.MoveTowards(steerSmooth, 0f, steerResponse * dt);

        RaycastHit hit;
        bool found = ProbeGround(out hit);
        groundErr = ComputeHeightError(found, hit);
        isGrounded = found && Mathf.Abs(groundErr) < 1.5f;

        if (isGrounded)
        {
            Quaternion align = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, align, Mathf.Clamp01(groundAlign * dt)));
            float yVel = Mathf.Clamp(groundErr * groundSnap, -10f, 6f);
            rb.velocity = transform.right * latVel + Vector3.up * yVel;
            airYVel = 0f;
        }
        else
        {
            airYVel -= gravity * dt;
            rb.velocity = transform.right * latVel + Vector3.up * airYVel;
        }
        IsSkidding = false;
    }

    void HandleDrivingInput()
    {
        if (KeyPressed(KeyCode.E) && Time.unscaledTime - lastSeatTime > seatCooldown)
        {
            ExitCar();
            return;
        }

        inputThrottle = ((KeyDown(KeyCode.W) || KeyDown(KeyCode.UpArrow)) ? 1f : 0f)
                      - ((KeyDown(KeyCode.S) || KeyDown(KeyCode.DownArrow)) ? 1f : 0f);
        inputSteering = ((KeyDown(KeyCode.D) || KeyDown(KeyCode.RightArrow)) ? 1f : 0f)
                      - ((KeyDown(KeyCode.A) || KeyDown(KeyCode.LeftArrow)) ? 1f : 0f);
        inputBrake = KeyDown(KeyCode.Space) ? 1f : 0f;
        inputHandbrake = KeyDown(KeyCode.LeftShift) ? 1f : 0f;
    }

    void UpdateWheelVisuals(float dt)
    {
        wheelSpin += (motorSpeed / Mathf.Max(0.1f, wheelRadius)) * Mathf.Rad2Deg * dt * wheelSpinDirection;
        ApplyWheel(wheelMeshFL, wheelBasePos[0], true);
        ApplyWheel(wheelMeshFR, wheelBasePos[1], true);
        ApplyWheel(wheelMeshRL, wheelBasePos[2], false);
        ApplyWheel(wheelMeshRR, wheelBasePos[3], false);
    }

    void ApplyWheel(Transform m, Vector3 basePos, bool front)
    {
        if (m == null) return;
        m.localPosition = basePos;
        // ✅ БЕЗ МИНУСА: вправо (steerSmooth +1) → колёса ВПРАВО
        float steer = front ? steerSmooth * 30f : 0f;
        m.localRotation = Quaternion.Euler(0f, steer, 0f)
                        * Quaternion.Euler(0f, 0f, 90f)
                        * Quaternion.Euler(0f, wheelSpin, 0f);
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 220, 420, 140));
        GUILayout.Label($"🚗 driver: {(driver != null ? driver.NickName : "null")}   iControl: {iControl}");
        GUILayout.Label($"throttle: {inputThrottle:F2}   steer: {inputSteering:F2}");
        GUILayout.Label($"speed: {motorSpeed:F2}   slip: {latVel:F2}");
        GUILayout.Label($"grounded: {isGrounded}   groundErr: {groundErr:F2}");
        GUILayout.EndArea();
    }

    [PunRPC]
    void RPC_SyncCarState(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        targetPosition = position;
        targetRotation = rotation;
        hasTarget = true;
        rb.velocity = velocity;
    }

    public bool TryInteract(Player player)
    {
        if (player == null) return false;

        if (driver == player || passenger == player) { ExitCar(); return false; }
        if (driver != null && passenger != null) { ShowPrompt("❌ Машина полная!"); return false; }

        if (driver == null)
        {
            driver = player;
            EnterAsDriver(player);
            if (PhotonNetwork.IsConnected)
            {
                pv.RPC("RPC_SetDriver", RpcTarget.Others, player.ActorNumber);
                pv.RPC("RPC_ParentSeat", RpcTarget.Others, player.ActorNumber, 0);
            }
            return true;
        }

        if (passenger == null)
        {
            passenger = player;
            EnterAsPassenger(player);
            if (PhotonNetwork.IsConnected)
            {
                pv.RPC("RPC_SetPassenger", RpcTarget.Others, player.ActorNumber);
                pv.RPC("RPC_ParentSeat", RpcTarget.Others, player.ActorNumber, 1);
            }
            return true;
        }
        return false;
    }

    void ResetMotion()
    {
        motorSpeed = 0f;
        latVel = 0f;
        steerSmooth = 0f;
        airYVel = 0f;
        inputThrottle = 0f; inputSteering = 0f; inputBrake = 0f; inputHandbrake = 0f;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void EnterAsDriver(Player player)
    {
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
        foreach (Collider col in playerObj.GetComponentsInChildren<Collider>())
            if (col != null) col.enabled = false;

        playerObj.transform.SetParent(driverSeat);
        playerObj.transform.localPosition = Vector3.zero;
        playerObj.transform.localRotation = Quaternion.identity;
        playerObj.transform.localScale = Vector3.one;
        SetPlayerModelVisible(playerObj, false);

        if (player.IsLocal && PhotonNetwork.IsConnected) pv.TransferOwnership(player);

        Debug.Log($"🚗 {player.NickName} сел за руль");
    }

    void EnterAsPassenger(Player player)
    {
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
        foreach (Collider col in playerObj.GetComponentsInChildren<Collider>())
            if (col != null) col.enabled = false;

        playerObj.transform.SetParent(passengerSeat);
        playerObj.transform.localPosition = Vector3.zero;
        playerObj.transform.localRotation = Quaternion.identity;
        playerObj.transform.localScale = Vector3.one;
        SetPlayerModelVisible(playerObj, false);

        Debug.Log($"🚗 {player.NickName} сел пассажиром");
    }

    void SaveCamera(PlayerController pc)
    {
        if (pc == null || pc.playerCamera == null || camSaved) return;
        Transform cam = pc.playerCamera.transform;
        savedCamParent = cam.parent;
        savedCamLocalPos = cam.localPosition;
        savedCamLocalRot = cam.localRotation;
        camSaved = true;
    }

    void RestoreCamera(PlayerController pc)
    {
        if (pc == null || pc.playerCamera == null || !camSaved) return;
        Transform cam = pc.playerCamera.transform;
        cam.SetParent(savedCamParent);
        cam.localPosition = savedCamLocalPos;
        cam.localRotation = savedCamLocalRot;
        camSaved = false;
    }

    public void ExitCar()
    {
        if (driver != null && driver.IsLocal) ExitAsDriver();
        else if (passenger != null && passenger.IsLocal) ExitAsPassenger();
    }

    void ExitAsDriver()
    {
        if (driver == null || !driver.IsLocal) return;
        GameObject playerObj = GetPlayerObject(driver);
        if (playerObj == null) return;
        PlayerController pc = playerObj.GetComponent<PlayerController>();
        Player player = driver;
        driver = null;

        lastExitTime = Time.unscaledTime;
        ResetMotion();

        if (pc != null)
        {
            playerObj.transform.SetParent(null);
            playerObj.transform.localScale = Vector3.one;
            Vector3 exitPos = transform.position - transform.right * 2.5f;
            exitPos.y = transform.position.y + 0.2f;
            playerObj.transform.position = exitPos;
            playerObj.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

            foreach (Collider col in playerObj.GetComponentsInChildren<Collider>())
                if (col != null) col.enabled = true;
            CharacterController cc = playerObj.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = true;

            RestoreCamera(pc);
            pc.ExitCarMode();
            pc.movement.enabled = true;
            pc.building.enabled = true;
            SetPlayerModelVisible(playerObj, true);
        }

        if (PhotonNetwork.IsConnected)
        {
            pv.RPC("RPC_ClearDriver", RpcTarget.Others);
            pv.RPC("RPC_UnparentSeat", RpcTarget.Others, player.ActorNumber, 0);
        }

        Debug.Log($"🚪 {player.NickName} вышел из машины");
    }

    void ExitAsPassenger()
    {
        if (passenger == null || !passenger.IsLocal) return;
        GameObject playerObj = GetPlayerObject(passenger);
        if (playerObj == null) return;
        PlayerController pc = playerObj.GetComponent<PlayerController>();
        Player player = passenger;
        passenger = null;

        lastExitTime = Time.unscaledTime;

        if (pc != null)
        {
            playerObj.transform.SetParent(null);
            playerObj.transform.localScale = Vector3.one;
            Vector3 exitPos = transform.position + transform.right * 2.5f;
            exitPos.y = transform.position.y + 0.2f;
            playerObj.transform.position = exitPos;
            playerObj.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

            foreach (Collider col in playerObj.GetComponentsInChildren<Collider>())
                if (col != null) col.enabled = true;
            CharacterController cc = playerObj.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = true;

            RestoreCamera(pc);
            pc.ExitCarMode();
            pc.movement.enabled = true;
            pc.building.enabled = true;
            SetPlayerModelVisible(playerObj, true);
        }

        if (PhotonNetwork.IsConnected)
        {
            pv.RPC("RPC_ClearPassenger", RpcTarget.Others);
            pv.RPC("RPC_UnparentSeat", RpcTarget.Others, player.ActorNumber, 1);
        }

        Debug.Log($"🚪 {player.NickName} вышел из машины");
    }

    [PunRPC]
    void RPC_ParentSeat(int actorNumber, int seatIndex, PhotonMessageInfo info)
    {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        if (p == null || p.IsLocal) return;

        GameObject obj = GetPlayerObject(p);
        if (obj == null) return;

        Transform seat = seatIndex == 0 ? driverSeat : passengerSeat;
        if (seat == null) return;

        PlayerController pc = obj.GetComponent<PlayerController>();
        if (pc != null) { pc.isInCar = true; pc.currentCar = this; }

        CharacterController cc = obj.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        foreach (Collider c in obj.GetComponentsInChildren<Collider>())
            if (c != null) c.enabled = false;

        obj.transform.SetParent(seat);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        SetPlayerModelVisible(obj, false);
    }

    [PunRPC]
    void RPC_UnparentSeat(int actorNumber, int seatIndex, PhotonMessageInfo info)
    {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        if (p == null || p.IsLocal) return;

        GameObject obj = GetPlayerObject(p);
        if (obj == null) return;

        PlayerController pc = obj.GetComponent<PlayerController>();
        if (pc != null) { pc.isInCar = false; pc.currentCar = null; }

        obj.transform.SetParent(null);
        Vector3 exitPos = transform.position + (seatIndex == 0 ? -transform.right : transform.right) * 2.5f;
        exitPos.y = transform.position.y + 0.2f;
        obj.transform.position = exitPos;
        obj.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        foreach (Collider c in obj.GetComponentsInChildren<Collider>())
            if (c != null) c.enabled = true;
        CharacterController cc = obj.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = true;
        SetPlayerModelVisible(obj, true);
    }

    void SetPlayerModelVisible(GameObject playerObj, bool visible)
    {
        foreach (Renderer r in playerObj.GetComponentsInChildren<Renderer>())
            if (r != null) r.enabled = visible;
    }

    PlayerController GetLocalPlayerController()
    {
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
            if (pc.view != null && pc.view.IsMine) return pc;
        return null;
    }

    GameObject GetPlayerObject(Player player)
    {
        if (player == null) return null;
        if (player.IsLocal)
        {
            foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
                if (pc.view != null && pc.view.IsMine) return pc.gameObject;
        }
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
        {
            if (pc.view != null && pc.view.Owner != null && pc.view.Owner.ActorNumber == player.ActorNumber)
                return pc.gameObject;
        }
        return null;
    }

    [PunRPC] void RPC_SetDriver(int actorNumber, PhotonMessageInfo info)
    { Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber); if (p != null && driver == null) driver = p; }

    [PunRPC] void RPC_SetPassenger(int actorNumber, PhotonMessageInfo info)
    { Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber); if (p != null && passenger == null) passenger = p; }

    [PunRPC] void RPC_ClearDriver() { driver = null; }
    [PunRPC] void RPC_ClearPassenger() { passenger = null; }

    public bool IsFull() { return driver != null && passenger != null; }

    public string GetInteractPrompt()
    {
        if (driver == null) return "[E] Сесть за руль";
        if (passenger == null) return "[E] Сесть пассажиром";
        return null;
    }

    public void ShowPrompt(string text)
    {
        if (promptUI != null && promptText != null)
        {
            promptUI.SetActive(true);
            promptText.text = text;
            StartCoroutine(HidePromptAfter(2f));
        }
    }

    System.Collections.IEnumerator HidePromptAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (promptUI != null) promptUI.SetActive(false);
    }

    void OnDrawGizmos()
    {
        if (driverSeat != null) { Gizmos.color = Color.red; Gizmos.DrawWireSphere(driverSeat.position, 0.5f); }
        if (passengerSeat != null) { Gizmos.color = Color.blue; Gizmos.DrawWireSphere(passengerSeat.position, 0.5f); }
    }
}