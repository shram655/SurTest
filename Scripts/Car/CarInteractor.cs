using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PlayerController))]
public class CarInteractor : MonoBehaviourPun
{
    [Header("═══ НАСТРОЙКИ ВЗАИМОДЕЙСТВИЯ ═══")]
    public float interactDistance = 3f;
    public LayerMask carLayer;

    [Header("═══ ПОДСВЕТКА ═══")]
    public Color highlightColor = new Color(1f, 1f, 0f, 0.3f);
    public Material highlightMaterial;

    [Header("═══ UI ═══")]
    public GameObject interactPromptUI;
    public TMPro.TextMeshProUGUI interactPromptText;

    private PlayerController pc;
    private Camera playerCamera;
    private Car currentHighlightedCar;
    private Renderer highlightedRenderer;

    void Start()
    {
        pc = GetComponent<PlayerController>();
        playerCamera = pc.playerCamera;

        if (interactPromptUI != null)
        {
            interactPromptUI.SetActive(false);
        }

        if (highlightMaterial == null)
        {
            highlightMaterial = new Material(Shader.Find("Sprites/Default"));
            highlightMaterial.color = highlightColor;
        }
    }

    void Update()
    {
        if (!pc.view.IsMine || pc.isInCar) return;
        if (ChatManager.IsChatOpen) return;

        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

        if (Physics.Raycast(ray, out hit, interactDistance, carLayer))
        {
            Car car = hit.collider.GetComponent<Car>();
            if (car != null)
            {
                if (currentHighlightedCar != car)
                {
                    UnhighlightCar();
                    HighlightCar(car);
                }

                string prompt = car.GetInteractPrompt();
                if (prompt != null) ShowPrompt(prompt);
                else ShowPrompt("❌ Машина полная");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    car.TryInteract(PhotonNetwork.LocalPlayer);
                    UnhighlightCar();
                    HidePrompt();
                }
            }
            else
            {
                UnhighlightCar();
                HidePrompt();
            }
        }
        else
        {
            UnhighlightCar();
            HidePrompt();
        }
    }

    void HighlightCar(Car car)
    {
        currentHighlightedCar = car;

        Renderer[] renderers = car.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            highlightedRenderer = renderers[0];
            Material[] mats = highlightedRenderer.materials;
            Material[] newMats = new Material[mats.Length + 1];
            mats.CopyTo(newMats, 0);
            newMats[mats.Length] = highlightMaterial;
            highlightedRenderer.materials = newMats;
        }
    }

    void UnhighlightCar()
    {
        if (highlightedRenderer != null)
        {
            Material[] mats = highlightedRenderer.materials;
            if (mats.Length > 1)
            {
                Material[] newMats = new Material[mats.Length - 1];
                for (int i = 0; i < newMats.Length; i++)
                    newMats[i] = mats[i];
                highlightedRenderer.materials = newMats;
            }
            highlightedRenderer = null;
        }
        currentHighlightedCar = null;
    }

    void ShowPrompt(string text)
    {
        if (interactPromptUI != null && interactPromptText != null)
        {
            interactPromptUI.SetActive(true);
            interactPromptText.text = text;
        }
    }

    void HidePrompt()
    {
        if (interactPromptUI != null)
            interactPromptUI.SetActive(false);
    }

    void OnDestroy()
    {
        UnhighlightCar();
    }
}