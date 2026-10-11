using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    [Header("═══ ОРБИТА ВОКРУГ МАШИНЫ ═══")]
    public Transform target;
    public float distance = 6f;
    public float height = 1.8f;
    public float lookHeight = 1f;
    public float mouseSensitivity = 3f;

    [Header("═══ ОГРАНИЧЕНИЯ ═══")]
    public float minPitch = -15f;
    public float maxPitch = 60f;

    [Header("═══ ИНВЕРСИИ ═══")]
    public bool invertX = false;
    public bool invertY = false;

    [Header("═══ КОЛЛИЗИИ ═══")]
    public LayerMask collisionMask = -1;

    [Header("═══ СГЛАЖИВАНИЕ ═══")]
    [Range(2f, 30f)]
    public float followXZ_Smooth = 20f;
    [Range(0.2f, 5f)]
    public float followY_Smooth = 0.5f;
    [Range(2f, 20f)]
    public float lookSmooth = 5f;

    private float yawRelative = 0f;
    private float pitch = 12f;
    private bool isActive = false;
    private bool snapNext = false;

    private Vector3 virtualPos;
    private float virtualYaw;
    private float virtualY;
    private bool initialized = false;

    private Car cachedCar;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        cachedCar = null;
        initialized = false;

        if (target != null)
        {
            yawRelative = 0f;
            pitch = 12f;
            isActive = true;
            snapNext = true;

            if (target.parent != null)
                cachedCar = target.GetComponentInParent<Car>();
            if (cachedCar == null && target.root != null)
                cachedCar = target.root.GetComponent<Car>();
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

        yawRelative += mx * mouseSensitivity;
        pitch -= my * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Vector3 carPos = target.position;
        Quaternion carRot = target.rotation;

        if (cachedCar != null)
        {
            carPos = cachedCar.GetSmoothPosition();
            carRot = cachedCar.GetSmoothRotation();
        }

        float carYaw = carRot.eulerAngles.y;

        if (!initialized)
        {
            virtualPos = carPos;
            virtualYaw = carYaw;
            virtualY = carPos.y;
            initialized = true;
        }

        float xzFactor = 1f - Mathf.Exp(-followXZ_Smooth * Time.deltaTime);
        virtualPos = Vector3.Lerp(virtualPos, carPos, xzFactor);

        float yFactor = 1f - Mathf.Exp(-followY_Smooth * Time.deltaTime);
        virtualY = Mathf.Lerp(virtualY, carPos.y, yFactor);

        float yawFactor = 1f - Mathf.Exp(-followXZ_Smooth * Time.deltaTime);
        virtualYaw = Mathf.LerpAngle(virtualYaw, carYaw, yawFactor);

        Vector3 smoothTarget = new Vector3(virtualPos.x, virtualY, virtualPos.z);

        Quaternion orbitRot = Quaternion.Euler(pitch, virtualYaw + yawRelative, 0f);
        Vector3 desired = smoothTarget + orbitRot * new Vector3(0f, 0f, -distance) + Vector3.up * height;

        Vector3 from = smoothTarget + Vector3.up * height;
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
            float posFactor = 1f - Mathf.Exp(-followXZ_Smooth * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desired, posFactor);
        }

        Vector3 lookTarget = smoothTarget + Vector3.up * lookHeight;
        Quaternion desiredRot = Quaternion.LookRotation(lookTarget - transform.position);
        float rotFactor = 1f - Mathf.Exp(-lookSmooth * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, rotFactor);
    }
}