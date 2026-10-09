using UnityEngine;

public class AntiRayCastCheat : MonoBehaviour
{
	private float timer;

	public bool BANNED;

	private void Start()
	{
	}

	private void Update()
	{
		if (!(Time.time > timer + 10f) || BANNED)
		{
			return;
		}
		RaycastHit hitInfo;
		if (!Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.forward), out hitInfo, 35f, BaseLogic.layerSet) && (bool)Network.PLAYER)
		{
			if ((bool)Network.PLAYER.GetComponent<HeroBase>().hahaha)
			{
				AudioSource.PlayClipAtPoint(Network.PLAYER.GetComponent<HeroBase>().hahaha, base.transform.position);
			}
			BaseLogic.SetBan(9996);
			BaseLogic._player.SendMessage("SaveHero");
			if (PhotonNetwork.room != null)
			{
				PhotonNetwork.LeaveRoom();
			}
			BANNED = true;
		}
		timer = Time.time;
	}
}
