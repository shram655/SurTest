using UnityEngine;

public class DestroyFracture : MonoBehaviour
{
	public bool Effect;

	private float HP = 200f;

	private bool DEAD;

	private void Start()
	{
		if (Effect)
		{
			base.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-8f, 8f), Random.Range(7f, 15f), Random.Range(-8f, 8f));
			base.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-13f, 13f), Random.Range(-13f, 13f), Random.Range(-13f, 13f));
			Object.Destroy(base.GetComponent<Rigidbody>(), 5f);
			Object.Destroy(base.GetComponent<Collider>(), 5f);
		}
	}

	private void Update()
	{
	}

	private void ApplyDamage(float damage)
	{
		if (!Effect && !DEAD)
		{
			HP -= damage;
			if (HP <= 0f)
			{
				base.GetComponent<Rigidbody>().isKinematic = false;
				DEAD = true;
				Object.Destroy(base.gameObject, 30f);
			}
		}
	}
}
