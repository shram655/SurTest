using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(PlayerBuilding))]
[RequireComponent(typeof(PlayerWeaponManager))]
public class PlayerController : MonoBehaviourPunCallbacks
{
    [Header("Ссылки на компоненты")]
    public PhotonView view;
    public Camera playerCamera;
    public TextMeshPro nick;
    public GameObject Torch;

    [Header("═══ СИНХРОНИЗАЦИЯ ПОЗИЦИИ ═══")]
    [Range(0.05f, 1f)]
    public float sendInterval = 0.1f;

    [Range(2f, 30f)]
    public float smoothSpeed = 12f;

    [Range(0.5f, 10f)]
    public float teleportThreshold = 3f;

    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public PlayerInventory inventory;
    [HideInInspector] public PlayerBuilding building;
    [HideInInspector] public PlayerWeaponManager weaponManager;
    [HideInInspector] public PlayerHealth health;
    [HideInInspector] public PlayerHunger hunger;

    [HideInInspector] public bool isPlayerDead = false;

    // ═══ 🚗 ТРАНСПОРТ ═══
    [HideInInspector] public bool isInCar = false;
    [HideInInspector] public Car currentCar = null;

    private Vector3 targetPos;
    private Quaternion targetRot = Quaternion.identity;
    private bool hasTarget = false;

    private CarCameraController carCamera;
    private mouseLook[] mouseLooks;

    void Awake()
    {
        view = GetComponent<PhotonView>();
        movement = GetComponent<PlayerMovement>();
        inventory = GetComponent<PlayerInventory>();
        building = GetComponent<PlayerBuilding>();
        weaponManager = GetComponent<PlayerWeaponManager>();
        health = GetComponent<PlayerHealth>();
        hunger = GetComponent<PlayerHunger>();
    }

    void Start()
    {
        string nickname = PhotonNetwork.NickName;
        if (string.IsNullOrEmpty(nickname)) nickname = "Player";
        if (nick != null) nick.text = nickname;

        mouseLooks = GetComponentsInChildren<mouseLook>(true);

        if (playerCamera != null)
        {
            carCamera = playerCamera.GetComponent<CarCameraController>();
            if (carCamera == null)
            {
                carCamera = playerCamera.gameObject.AddComponent<CarCameraController>();
            }
            carCamera.enabled = false;
        }

        if (!view.IsMine)
        {
            enabled = false;
            movement.enabled = false;
            inventory.enabled = false;
            building.enabled = false;
            weaponManager.enabled = false;
            if (playerCamera != null) playerCamera.gameObject.SetActive(false);

            foreach (Canvas c in GetComponentsInChildren<Canvas>(true))
            {
                c.gameObject.SetActive(false);
            }

            PhotonTransformView ptv = GetComponent<PhotonTransformView>();
            if (ptv != null) ptv.enabled = false;

            StartCoroutine(RequestInitialSync());

            Debug.Log($"👥 Чужой игрок {nickname}: UI отключён, интерполяция включена");
            return;
        }

        StartCoroutine(PeriodicTransformSync());

        Debug.Log($"✅ Локальный игрок создан: {nickname}");
    }

    IEnumerator PeriodicTransformSync()
    {
        WaitForSeconds wait = new WaitForSeconds(sendInterval);
        while (true)
        {
            if (view != null && view.IsMine && PhotonNetwork.IsConnected)
            {
                if (!isInCar)
                {
                    view.RPC("RPC_SyncTransform", RpcTarget.Others,
                        transform.position, transform.eulerAngles);
                }
            }
            yield return wait;
        }
    }

    IEnumerator RequestInitialSync()
    {
        for (int i = 0; i < 5; i++)
        {
            if (this == null) yield break;
            if (view != null && view.Owner != null)
            {
                view.RPC("RPC_RequestSync", view.Owner);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    [PunRPC]
    void RPC_RequestSync(PhotonMessageInfo info)
    {
        if (view == null || !view.IsMine) return;

        view.RPC("RPC_SyncTransform", info.Sender,
            transform.position, transform.eulerAngles);
    }

    [PunRPC]
    void RPC_SyncTransform(Vector3 pos, Vector3 rot)
    {
        if (view == null || view.IsMine) return;

        targetRot = Quaternion.Euler(rot);

        if (!hasTarget || Vector3.Distance(transform.position, pos) > teleportThreshold)
        {
            transform.position = pos;
            transform.rotation = targetRot;
        }

        targetPos = pos;
        hasTarget = true;
    }

    void LateUpdate()
    {
        if (view == null || view.IsMine || !hasTarget) return;

        // 🆕 КЛОН СИДИТ В МАШИНЕ — не выдёргиваем его из сиденья
        if (isInCar) return;

        float t = Time.deltaTime * smoothSpeed;
        transform.position = Vector3.Lerp(transform.position, targetPos, t);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, t);
    }

    void Update()
    {
        if (!view.IsMine || isPlayerDead) return;

        if (isInCar) return;

        if (ChatManager.IsChatOpen) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Torch != null) Torch.SetActive(!Torch.activeSelf);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Confined;
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("LobbyScene");
        }
    }

    public void EnterCarMode(Car car)
    {
        isInCar = true;
        currentCar = car;

        if (mouseLooks != null)
        {
            foreach (mouseLook ml in mouseLooks)
            {
                if (ml != null) ml.enabled = false;
            }
        }

        if (carCamera != null && car.cameraPoint != null)
        {
            carCamera.car = car;
            carCamera.enabled = true;
            carCamera.SetTarget(car.cameraPoint);
        }
    }

    public void ExitCarMode()
    {
        isInCar = false;
        currentCar = null;

        if (mouseLooks != null)
        {
            foreach (mouseLook ml in mouseLooks)
            {
                if (ml != null) ml.enabled = true;
            }
        }

        if (carCamera != null)
        {
            carCamera.enabled = false;
            carCamera.SetTarget(null);
        }
    }

    public void SetDead(bool dead)
    {
        isPlayerDead = dead;
        if (dead)
        {
            if (isInCar && currentCar != null)
            {
                currentCar.ExitCar();
            }

            if (weaponManager != null) weaponManager.UnequipCurrentWeapon();
            if (inventory != null) inventory.ClearInventory();
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetRotationY()
    {
        return transform.eulerAngles.y;
    }

    public bool IsLocalPlayer()
    {
        return view != null && view.IsMine;
    }
}