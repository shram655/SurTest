using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    [Header("═══ ОРБИТА ВОКРУГ МАШИНЫ ═══")]
    public Transform target;
    public float distance = 6f;
    public float height = 1.8f;
    public float lookHeight = 1f;
    public float smoothSpeed = 12f;
    public float mouseSensitivity = 3f;

    [Header("═══ ОГРАНИЧЕНИЯ ═══")]
    public float minPitch = -15f;
    public float maxPitch = 60f;

    [Header("═══ ИНВЕРСИИ (если мышь зеркалит) ═══")]
    public bool invertX = false;
    public bool invertY = false;

    [Header("═══ КОЛЛИЗИИ ═══")]
    public LayerMask collisionMask = -1;

    private float yawRelative = 0f;
    private float pitch = 12f;
    private bool isActive = false;
    private bool snapNext = false;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            yawRelative = 0f;
            pitch = 12f;
            isActive = true;
            snapNext = true;
        }
        else
        {
            isActive = false;
        }
    }

    Vector2 ReadMouseDelta()
    {
        #if ENABLE_INPUT_SYSTEM
        var m = UnityEngine.InputSystem.Mouse.current;
        if (m != null) return m.delta.ReadValue() * 0.05f;
        #endif
        #if ENABLE_LEGACY_INPUT_MANAGER
        return new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
        #endif
        return Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isActive || target == null) return;

        Vector2 md = ReadMouseDelta();
        float mx = invertX ? -md.x : md.x;
        float my = invertY ? -md.y : md.y;

        // ✅ ПЛЮС: мышь ВПРАВО → вид ВПРАВО
        yawRelative += mx * mouseSensitivity;
        pitch -= my * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        float carYaw = target.eulerAngles.y;
        Quaternion rot = Quaternion.Euler(pitch, carYaw + yawRelative, 0f);

        Vector3 desired = target.position + rot * new Vector3(0f, 0f, -distance) + Vector3.up * height;

        Vector3 from = target.position + Vector3.up * height;
        Vector3 dir = desired - from;
        if (dir.sqrMagnitude > 0.001f)
        {
            RaycastHit hit;
            if (Physics.Raycast(from, dir.normalized, out hit, dir.magnitude, collisionMask, QueryTriggerInteraction.Ignore))
            {
                if (!hit.transform.IsChildOf(target))
                    desired = from + dir.normalized * Mathf.Max(0.5f, hit.distance - 0.2f);
            }
        }

        if (snapNext)
        {
            transform.position = desired;
            snapNext = false;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, desired, 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime));
        }

        transform.LookAt(target.position + Vector3.up * lookHeight);
    }
}