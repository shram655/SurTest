using UnityEngine;

public class CameraInVehicle : MonoBehaviour
{
	public bool isGunner;

	private void Start()
	{
	}

	private void Update()
	{
		if (BaseLogic._control)
		{
			base.transform.Rotate(0f, 2.5f * Input.GetAxis("Mouse X"), 0f);
		}
	}
}
