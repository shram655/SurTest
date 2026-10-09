using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class FCarWheel : MonoBehaviour
{
	public bool damaged;

	[HideInInspector]
	public WheelCollider wheelColl;

	public Transform wheelTransform;

	private float _emitTime;

	public bool isTorqued;

	public bool wdWheel;

	public bool TANK;

	public WheelFrictionCurve fwdFrictionCurve;

	public WheelFrictionCurve sdwFrictionCurve;

	[HideInInspector]
	public float wheelRot;

	private Skidmarks skidmarks;

	private ParticleSystem skidSmoke;

	private float[] skidmarkTime = new float[4];

	public Rigidbody carRigidbody;

	private int lastSkidmark = -1;

	private float lastEmitTime;

	private Vector3 lastEmitPosition;

	private WheelFrictionCurve SdwCurve;

	private int wheelCount;

	private void Awake()
	{
		wheelColl = GetComponent<WheelCollider>();
		fwdFrictionCurve = wheelColl.forwardFriction;
		sdwFrictionCurve = wheelColl.sidewaysFriction;
		skidmarks = (Skidmarks)Object.FindObjectOfType(typeof(Skidmarks));
		if ((bool)skidmarks)
		{
			skidSmoke = skidmarks.GetComponentInChildren<ParticleSystem>();
		}
		if (!TANK && (bool)base.transform.parent.transform.parent)
		{
			carRigidbody = base.transform.parent.transform.parent.GetComponent<Rigidbody>();
		}
		lastEmitTime = Time.time;
	}

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		if (TANK)
		{
			bool flag = false;
			WheelHit hit;
			if (wheelColl.GetGroundHit(out hit))
			{
				Vector3 pointVelocity = carRigidbody.GetPointVelocity(hit.point);
				Vector3 vector = wheelTransform.InverseTransformDirection(pointVelocity);
				if (carRigidbody.velocity.magnitude > 2f)
				{
					flag = true;
				}
				if (!skidmarks)
				{
					return;
				}
				if (skidmarkTime[wheelCount] < 0.02f && lastSkidmark != -1)
				{
					skidmarkTime[wheelCount] += Time.deltaTime;
					return;
				}
				float num = ((skidmarkTime[wheelCount] != 0f) ? skidmarkTime[wheelCount] : Time.deltaTime);
				skidmarkTime[wheelCount] = 0f;
				float num2 = ((!flag || isTorqued) ? 0f : (pointVelocity.magnitude * 0.3f));
				float num3 = Mathf.Abs(vector.x) - 15f;
				if (flag && (num3 > 0f || num2 > 0f))
				{
					/*Vector3 vector2 = base.transform.TransformDirection(skidSmoke.localVelocity) + skidSmoke.worldVelocity;
					if (lastSkidmark != -1)
					{
						float num4 = Random.Range(skidSmoke.minEmission, skidSmoke.maxEmission);
						float num5 = lastEmitTime * num4;
						float num6 = Time.time * num4;
						int num7 = Mathf.CeilToInt(num6) - Mathf.CeilToInt(num5);
						int num8 = Mathf.CeilToInt(num5);
						for (int i = 0; i <= num7; i++)
						{
							float t = Mathf.InverseLerp(num5, num6, num8 + i);
							if (Time.time > _emitTime + 0.4f)
							{
								_emitTime = Time.time;
								skidSmoke.Emit(Vector3.Lerp(lastEmitPosition, hit.point, t) + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), vector2 + pointVelocity * 0.05f, Random.Range(skidSmoke.minSize, skidSmoke.maxSize) * Mathf.Clamp(num3 * 0.1f, 0.5f, 1f), Random.Range(skidSmoke.minEnergy, skidSmoke.maxEnergy), Color.white);
							}
						}
					}
					else if (Time.time > _emitTime + 0.4f)
					{
						_emitTime = Time.time;
						skidSmoke.Emit(hit.point + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), vector2 + pointVelocity * 0.05f, Random.Range(skidSmoke.minSize, skidSmoke.maxSize) * Mathf.Clamp(num3 * 0.1f, 0.5f, 1f), Random.Range(skidSmoke.minEnergy, skidSmoke.maxEnergy), Color.white);
					}*/
					lastEmitPosition = hit.point;
					lastEmitTime = Time.time;
					lastSkidmark = skidmarks.AddSkidMark(hit.point + carRigidbody.velocity * num, hit.normal, (num3 * 0.1f + num2) * Mathf.Clamp01(hit.force / wheelColl.suspensionSpring.spring), lastSkidmark);
				}
				else
				{
					lastSkidmark = -1;
				}
			}
			else if ((bool)skidmarks)
			{
				lastSkidmark = -1;
			}
			return;
		}
		bool flag2 = false;
		WheelHit hit2;
		if (wheelColl.GetGroundHit(out hit2))
		{
			Vector3 pointVelocity2 = carRigidbody.GetPointVelocity(hit2.point);
			Vector3 vector3 = wheelTransform.InverseTransformDirection(pointVelocity2);
			if (wheelColl.forwardFriction.stiffness < 0.06f || wheelColl.sidewaysFriction.stiffness < 0.01f)
			{
				flag2 = true;
			}
			if (damaged)
			{
				flag2 = true;
			}
			if (!skidmarks)
			{
				return;
			}
			if (skidmarkTime[wheelCount] < 0.02f && lastSkidmark != -1)
			{
				skidmarkTime[wheelCount] += Time.deltaTime;
				return;
			}
			float num9 = ((skidmarkTime[wheelCount] != 0f) ? skidmarkTime[wheelCount] : Time.deltaTime);
			skidmarkTime[wheelCount] = 0f;
			float num10 = ((!flag2 || isTorqued) ? 0f : (pointVelocity2.magnitude * 0.3f));
			float num11 = Mathf.Abs(vector3.x) - 15f;
			if (flag2 && (num11 > 0f || num10 > 0f))
			{
				/*Vector3 vector4 = base.transform.TransformDirection(skidSmoke.localVelocity) + skidSmoke.worldVelocity;
				if (lastSkidmark != -1)
				{
					float num12 = Random.Range(skidSmoke.minEmission, skidSmoke.maxEmission);
					float num13 = lastEmitTime * num12;
					float num14 = Time.time * num12;
					int num15 = Mathf.CeilToInt(num14) - Mathf.CeilToInt(num13);
					int num16 = Mathf.CeilToInt(num13);
					for (int j = 0; j <= num15; j++)
					{
						float t2 = Mathf.InverseLerp(num13, num14, num16 + j);
						if (Time.time > _emitTime + 0.2f)
						{
							_emitTime = Time.time;
							skidSmoke.Emit(Vector3.Lerp(lastEmitPosition, hit2.point, t2) + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), vector4 + pointVelocity2 * 0.05f, Random.Range(skidSmoke.minSize, skidSmoke.maxSize) * Mathf.Clamp(num11 * 0.1f, 0.5f, 1f), Random.Range(skidSmoke.minEnergy, skidSmoke.maxEnergy), Color.white);
						}
					}
				}
				else if (Time.time > _emitTime + 0.2f)
				{
					_emitTime = Time.time;
					skidSmoke.Emit(hit2.point + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), vector4 + pointVelocity2 * 0.05f, Random.Range(skidSmoke.minSize, skidSmoke.maxSize) * Mathf.Clamp(num11 * 0.1f, 0.5f, 1f), Random.Range(skidSmoke.minEnergy, skidSmoke.maxEnergy), Color.white);
				}*/
				lastEmitPosition = hit2.point;
				lastEmitTime = Time.time;
				lastSkidmark = skidmarks.AddSkidMark(hit2.point + carRigidbody.velocity * num9, hit2.normal, (num11 * 0.1f + num10) * Mathf.Clamp01(hit2.force / wheelColl.suspensionSpring.spring), lastSkidmark);
			}
			else
			{
				lastSkidmark = -1;
			}
		}
		else if ((bool)skidmarks)
		{
			lastSkidmark = -1;
		}
	}
}
