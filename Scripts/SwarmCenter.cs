using UnityEngine;

public class SwarmCenter : MonoBehaviour
{
	public GameObject SMD_Root;

	public float speed;

	private float _updateTimer;

	private bool MOVER;

	private void Start()
	{
		speed = -8f;
		_updateTimer = Time.time - 60f;
		base.transform.Rotate(new Vector3(0f, Random.Range(0f, 360f), 0f));
	}

	private void Update()
	{
		if (Time.time > _updateTimer + 60f)
		{
			float num = Vector3.Distance(Camera.main.transform.position, base.transform.position);
			if (num < 150f)
			{
				SMD_Root.SetActive(true);
				MOVER = true;
			}
			else
			{
				SMD_Root.SetActive(false);
				MOVER = false;
			}
			_updateTimer = Time.time + Random.Range(1f, 10f);
		}
		if (MOVER)
		{
			base.transform.Rotate(new Vector3(0f, speed * Time.deltaTime, 0f));
		}
	}
}
