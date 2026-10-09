using UnityEngine;

public class MouseOrb : MonoBehaviour
{
	public bool _inGame;

	public bool _HeroOsmotr;

	public Transform target;

	public Transform _pos1;

	public Transform _pos2;

	public float targetHeight = 2f;

	public float distance = 2.8f;

	public float maxDistance = 10f;

	public float minDistance = 0.5f;

	public float xSpeed = 250f;

	public float ySpeed = 120f;

	public float yMinLimit = -40f;

	public float yMaxLimit = 80f;

	public float zoomRate = 20f;

	public float rotationDampening = 3f;

	private float x;

	private float y;

	public bool isTalking;

	private int layerMask;

	private void Awake()
	{
		layerMask = (1 << LayerMask.NameToLayer("TransparentFX")) | (1 << LayerMask.NameToLayer("Arm")) | (1 << LayerMask.NameToLayer("Vehicle")) | (1 << LayerMask.NameToLayer("VehicleCollider")) | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("IgnoreHit"));
		layerMask = ~layerMask;
	}

	private void SetTarget(int _targ)
	{
		if (_targ == 1)
		{
			target.position = _pos1.position;
			distance = 1.35f;
		}
		if (_targ == 2)
		{
			target.position = _pos2.position;
			distance = 3.35f;
		}
	}

	public void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		x = eulerAngles.y;
		y = eulerAngles.x;
		if ((bool)base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().freezeRotation = true;
		}
	}

	public void LateUpdate()
	{
		if ((bool)target && (BaseLogic._control || BaseLogic.ADMIN == 89))
		{
			Quaternion identity = Quaternion.identity;
			Vector3 zero = Vector3.zero;
			if ((Input.GetKey(KeyCode.Mouse0) && _HeroOsmotr) || _HeroOsmotr)
			{
			}
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			distance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(distance);
			distance = Mathf.Clamp(distance, minDistance, maxDistance);
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			identity = Quaternion.Euler(y, x, 0f);
			base.transform.rotation = identity;
			zero = target.position - (identity * Vector3.forward * distance + new Vector3(0f, 0f - targetHeight, 0f));
			base.transform.position = zero;
			Vector3 vector = target.transform.position - new Vector3(0f, 0f - targetHeight, 0f);
			RaycastHit hitInfo;
			if (Physics.Linecast(vector, base.transform.position, out hitInfo, layerMask))
			{
				float num = Vector3.Distance(vector, hitInfo.point) - 0.28f;
				zero = target.position - (identity * Vector3.forward * num + new Vector3(0f, 0f - targetHeight, 0f));
				base.transform.position = zero;
			}
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
