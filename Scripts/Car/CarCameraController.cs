using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    [Header("═══ ЦЕЛЬ И ОРБИТА ═══")]
    public Transform target;
    [Tooltip("Фиксированная дистанция камеры до машины")]
    public float distance = 6f;
    public float height = 1.8f;
    public float lookHeight = 1f;
    public float mouseSensitivity = 3f;

    [Header("═══ ОГРАНИЧЕНИЯ ПО ВЫСОТЕ (PITCH) ═══")]
    public float minPitch = -10f;
    public float maxPitch = 70f;

    [Header("═══ ИНВЕРСИИ ═══")]
    public bool invertX = false;
    public bool invertY = false;

    [Header("═══ КОЛЛИЗИИ ═══")]
    public LayerMask collisionMask = -1;

    [Header("═══ СГЛАЖИВАНИЕ ═══")]
    [Range(2f, 30f)]
    public float followSmooth = 20f;
    [Range(2f, 20f)]
    public float lookSmooth = 15f;

    private float yaw = 0f;
    private float pitch = 20f;
    private bool isActive = false;
    private bool snapNext = false;
    private Vector3 smoothedTargetPos;
    private Car cachedCar;

    public void SetTarget(Transform newTarget) {
        target = newTarget;
        cachedCar = null;
        if (target != null) {
            isActive = true;
            snapNext = true;
            
            // Инициализируем углы камеры в соответствии с текущим направлением
            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = 20f;

            if (target.parent != null) cachedCar = target.GetComponentInParent<Car>();
            if (cachedCar == null && target.root != null) cachedCar = target.root.GetComponent<Car>();
        } else {
            isActive = false;
        }
    }

    Vector2 ReadMouseDelta() {
#if ENABLE_INPUT_SYSTEM
        var m = UnityEngine.InputSystem.Mouse.current;
        if (m != null) return m.delta.ReadValue() * 0.05f;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
        return new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
#endif
        return Vector2.zero;
    }

    void LateUpdate() {
        if (!isActive || target == null) return;

        // 1. Свободное вращение камеры мышкой независимо от поворота машины (стиль WoT)
        Vector2 md = ReadMouseDelta();
        float mx = invertX ? -md.x : md.x;
        float my = invertY ? -md.y : md.y;

        yaw += mx * mouseSensitivity;
        pitch -= my * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // (Зум колесиком мыши полностью удалён)

        // 2. Получаем сглаженную позицию машины без рывков
        Vector3 carPos = target.position;
        if (cachedCar != null) {
            carPos = cachedCar.GetSmoothPosition();
        }

        if (snapNext) {
            smoothedTargetPos = carPos;
            snapNext = false;
        } else {
            float smoothFactor = 1f - Mathf.Exp(-followSmooth * Time.deltaTime);
            smoothedTargetPos = Vector3.Lerp(smoothedTargetPos, carPos, smoothFactor);
        }

        // 3. Точка фокуса (центр машины + высота взгляда)
        Vector3 lookTarget = smoothedTargetPos + Vector3.up * lookHeight;

        // 4. Расчет позиции камеры по сферическим координатам вокруг машины с фиксированной дистанцией
        Quaternion orbitRotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = lookTarget + Vector3.up * height - (orbitRotation * Vector3.forward * distance);

        // 5. Проверка препятствий (чтобы камера не пролетала сквозь стены/террейн)
        Vector3 from = lookTarget + Vector3.up * height;
        Vector3 dir = desiredPosition - from;
        if (dir.sqrMagnitude > 0.001f) {
            RaycastHit hit;
            if (Physics.Raycast(from, dir.normalized, out hit, dir.magnitude, collisionMask, QueryTriggerInteraction.Ignore)) {
                if (!hit.transform.IsChildOf(target)) {
                    desiredPosition = from + dir.normalized * Mathf.Max(0.5f, hit.distance - 0.2f);
                }
            }
        }

        // 6. Плавное применение позиции и вращения камеры
        transform.position = desiredPosition;

        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f - Mathf.Exp(-lookSmooth * Time.deltaTime));
    }
}