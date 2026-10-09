using UnityEngine;

public class FollowPlayerPrefab : MonoBehaviour
{
	public Transform _player;

	private Transform myTransform;

	private void Start()
	{
		myTransform = base.transform;
		GameObject gameObject = GameObject.Find("!!!FPS Player");
		if ((bool)gameObject)
		{
			_player = gameObject.transform;
		}
	}

	private void Update()
	{
		if ((bool)_player)
		{
			myTransform.position = _player.position;
		}
	}
}
