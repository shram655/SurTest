using UnityEngine;

public class RigidBodyExplore : MonoBehaviour
{
	public bool isBoom;

	public bool isColliderDestroy;

	public float Power = 15f;

	private bool explode;

	private void Awake()
	{
	}

	private void Start()
	{
		if (!explode)
		{
			if (!base.GetComponent<Rigidbody>())
			{
				base.gameObject.AddComponent<Rigidbody>();
				base.GetComponent<Rigidbody>().mass = 100f;
			}
			base.GetComponent<Rigidbody>().angularVelocity = new Vector3(Power / 15f, Power / 8f, Power / 15f);
			if (isBoom)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("_EXPLOSION"));
				gameObject.GetComponent<Explosion>()._carBoom = true;
				gameObject.transform.position = base.transform.position;
				gameObject.transform.rotation = Quaternion.identity;
			}
			explode = true;
			Object.Destroy(base.GetComponent<Rigidbody>(), 10f);
			if (isColliderDestroy)
			{
				Object.Destroy(base.GetComponent<Collider>(), 10f);
			}
		}
	}
}
