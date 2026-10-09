using UnityEngine;

public class AdminNoclip : MonoBehaviour
{
	private float speed;

	public bool activate;

	private void Update()
	{
		if (activate)
		{
			if (Input.GetKey(KeyCode.W))
			{
				base.transform.position += Camera.main.transform.forward / speed;
				GetComponent<Rigidbody>().isKinematic = true;
				GetComponent<CapsuleCollider>().enabled = false;
				GetComponent<SphereCollider>().enabled = false;
			}
			if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
			{
				speed = 0.6f;
			}
			else
			{
				speed = 4f;
			}
		}
	}

	~AdminNoclip()
	{
	}

	private void Start()
	{
		activate = false;
		speed = 4f;
	}

	public void Switch()
	{
		activate = !activate;
		if (activate)
		{
			GetComponent<Rigidbody>().isKinematic = true;
			GetComponent<CapsuleCollider>().enabled = false;
			GetComponent<SphereCollider>().enabled = false;
		}
		else
		{
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<CapsuleCollider>().enabled = true;
			GetComponent<SphereCollider>().enabled = true;
		}
	}
}
