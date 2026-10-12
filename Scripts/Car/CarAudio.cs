using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class CarAudio : MonoBehaviourPun
{
    [Header("═══ 🔊 ДВИГАТЕЛЬ ═══")]
    [Tooltip("Зацикленный клип двигателя (loop)")]
    public AudioClip engineClip;
    [Range(0f, 1f)] public float engineVolume = 0.6f;
    public float idlePitch = 0.8f;
    public float maxPitch = 2.0f;

    [Header("═══ 🔊 ПРОБУКСОВКА ═══")]
    [Tooltip("Зацикленный клип визга шин (loop)")]
    public AudioClip skidClip;
    [Range(0f, 1f)] public float skidVolume = 0.5f;

    [Header("═══ 🔊 ГУДОК ═══")]
    public AudioClip hornClip;
    [Range(0f, 1f)] public float hornVolume = 0.9f;
    public KeyCode hornKey = KeyCode.H;

    [Header("═══ 💡 ФАРЫ ═══")]
    public Light headlightL;
    public Light headlightR;
    public KeyCode headlightKey = KeyCode.L;

    private AudioSource engineSrc;
    private AudioSource skidSrc;
    private AudioSource hornSrc;
    private Car car;
    private bool lightsOn;

    void Awake()
    {
        car = GetComponent<Car>();

        engineSrc = gameObject.AddComponent<AudioSource>();
        engineSrc.playOnAwake = false;
        engineSrc.loop = true;
        engineSrc.spatialBlend = 1f;
        engineSrc.minDistance = 3f;
        engineSrc.maxDistance = 50f;
        engineSrc.clip = engineClip;

        skidSrc = gameObject.AddComponent<AudioSource>();
        skidSrc.playOnAwake = false;
        skidSrc.loop = true;
        skidSrc.spatialBlend = 1f;
        skidSrc.minDistance = 2f;
        skidSrc.maxDistance = 30f;
        skidSrc.clip = skidClip;

        hornSrc = gameObject.AddComponent<AudioSource>();
        hornSrc.playOnAwake = false;
        hornSrc.spatialBlend = 1f;
        hornSrc.minDistance = 3f;
        hornSrc.maxDistance = 60f;

        if (headlightL != null) headlightL.enabled = false;
        if (headlightR != null) headlightR.enabled = false;
    }

    void Update()
    {
        if (car == null) return;

        bool hasDriver = car.HasDriver;
        float speed01 = Mathf.Clamp01(car.CurrentSpeed / car.maxSpeed);

        // ── ДВИГАТЕЛЬ: питч растёт со скоростью ──
        if (engineSrc.clip != null)
        {
            if (hasDriver && !engineSrc.isPlaying) engineSrc.Play();
            else if (!hasDriver && engineSrc.isPlaying) engineSrc.Stop();

            if (hasDriver)
            {
                float p = Mathf.Lerp(idlePitch, maxPitch, speed01);
                if (car.ThrottleInput > 0.1f) p += 0.15f;
                engineSrc.pitch = p;
                engineSrc.volume = engineVolume * (0.5f + 0.5f * speed01);
            }
        }

        // ── ПРОБУКСОВКА: визг при дрифте ──
        if (skidSrc.clip != null)
        {
            if (car.IsSkidding && !skidSrc.isPlaying) skidSrc.Play();
            else if (!car.IsSkidding && skidSrc.isPlaying) skidSrc.Stop();
            skidSrc.volume = skidVolume;
        }

        // ── ГУДОК И ФАРЫ: только локальный водитель ──
        if (hasDriver && car.IsLocalDriver)
        {
            if (Input.GetKeyDown(hornKey) && hornClip != null)
            {
                hornSrc.PlayOneShot(hornClip, hornVolume);
                photonView.RPC("RPC_PlayHorn", RpcTarget.Others);
            }

            if (Input.GetKeyDown(headlightKey))
            {
                SetLights(!lightsOn);
                photonView.RPC("RPC_SetLights", RpcTarget.Others, lightsOn);
            }
        }
    }

    public void SetLights(bool on)
    {
        lightsOn = on;
        if (headlightL != null) headlightL.enabled = on;
        if (headlightR != null) headlightR.enabled = on;
    }

    [PunRPC]
    void RPC_PlayHorn()
    {
        if (hornClip != null) hornSrc.PlayOneShot(hornClip, hornVolume);
    }

    [PunRPC]
    void RPC_SetLights(bool on)
    {
        SetLights(on);
    }
}