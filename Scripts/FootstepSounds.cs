using UnityEngine;
using Photon.Pun;

/// <summary>
/// Звуки шагов игрока.
/// Повесить на ПРЕФАБ игрока (туда же, где PlayerController / PlayerMovement).
/// 🆕 В МАШИНЕ ШАГИ НЕ ИГРАЮТ.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FootstepSounds : MonoBehaviourPun
{
    [Header("═══ 🔊 ЗВУКИ ШАГОВ ═══")]
    [Tooltip("Клип(ы) шагов — чем больше, тем естественнее (случайный выбор)")]
    public AudioClip[] footstepClips;
    [Tooltip("Дистанция (м) между шагами. Меньше = чаще")]
    public float stepDistance = 2f;
    [Tooltip("Мин. скорость (м/с), ниже которой шаги не играют (стоя на месте)")]
    public float minSpeed = 0.5f;

    [Header("═══ ГРОМКОСТЬ ═══")]
    [Tooltip("Громкость СВОИХ шагов (локальный игрок)")]
    [Range(0f, 1f)]
    public float localVolume = 1f;
    [Tooltip("Громкость шагов ДРУГИХ игроков")]
    [Range(0f, 1f)]
    public float remoteVolume = 0.35f;

    [Header("═══ ПИТЧ ═══")]
    [Tooltip("Разброс питча — делает шаги естественными, не 'механическими'")]
    [Range(0f, 0.3f)]
    public float pitchVariation = 0.1f;

    [Header("═══ БЕГ ═══")]
    [Tooltip("На сколько чаще шаги при спринте (множитель)")]
    public float sprintFrequencyMultiplier = 1.5f;

    // ═══ Ссылки ═══
    private AudioSource src;
    private CharacterController controller;
    private PlayerController playerController;
    private PlayerInventory playerInventory;
    private PlayerHealth playerHealth;

    // ═══ Состояние ═══
    private Vector3 lastPos;
    private float distAccum;
    private bool inited;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
        playerInventory = GetComponent<PlayerInventory>();
        playerHealth = GetComponent<PlayerHealth>();

        src = GetComponent<AudioSource>();
        if (src == null) src = gameObject.AddComponent<AudioSource>();

        src.playOnAwake = false;
        src.loop = false;
        src.spatialBlend = 0f;
    }

    void Start()
    {
        lastPos = transform.position;
        inited = true;

        bool isMine = (photonView == null || photonView.IsMine);
        if (!isMine)
        {
            src.spatialBlend = 1f;
            src.minDistance = 1f;
            src.maxDistance = 30f;
            src.rolloffMode = AudioRolloffMode.Linear;
        }
    }

    void Update()
    {
        if (!inited || src == null) return;

        bool isMine = (photonView == null || photonView.IsMine);

        src.volume = isMine ? localVolume : remoteVolume;

        // 🆕 В МАШИНЕ ШАГИ НЕ ИГРАЮТ (и свои, и чужие)
        if (playerController != null && playerController.isInCar)
        {
            ResetStep();
            return;
        }

        if (isMine)
        {
            if (playerController != null && playerController.isPlayerDead) { ResetStep(); return; }
            if (playerHealth != null && playerHealth.IsDead()) { ResetStep(); return; }
            if (playerInventory != null && playerInventory.IsInventoryOpen) { ResetStep(); return; }
            if (ChatManager.IsChatOpen) { ResetStep(); return; }
        }

        Vector3 delta = transform.position - lastPos;
        lastPos = transform.position;
        delta.y = 0f;

        float speed = delta.magnitude / Mathf.Max(Time.deltaTime, 1e-4f);
        bool grounded = controller != null && controller.isGrounded;

        if (grounded && speed > minSpeed)
        {
            bool sprinting = Input.GetKey(KeyCode.LeftShift);
            float effectiveDistance = stepDistance / (sprinting ? sprintFrequencyMultiplier : 1f);

            distAccum += delta.magnitude;
            if (distAccum >= effectiveDistance)
            {
                distAccum = 0f;
                PlayStep();
            }
        }
        else
        {
            ResetStep();
        }
    }

    void ResetStep()
    {
        distAccum = 0f;
    }

    void PlayStep()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        if (clip == null) return;

        src.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        src.PlayOneShot(clip);
    }

    void OnEnable()
    {
        lastPos = transform.position;
    }
}