using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public int ownerID;

	public float explosionTime = 1f;

	public float explosionRadius = 5f;

	public float explosionPower = 2000f;

	public float _expTime;

	public string _throwName;

	public bool _carBoom;

	public bool attacked;

	private void Start()
	{
		_expTime = Time.time;
		Object.Destroy(base.GetComponent<Rigidbody>(), explosionTime);
		Object.Destroy(base.GetComponent<Collider>(), explosionTime);
		Object.Destroy(base.gameObject, 7f);
		StartCoroutine("explosePowerSet");
		if (!Network.PLAYER)
		{
			return;
		}
		float num = Vector3.Distance(Network.PLAYER.transform.position, base.transform.position);
		if (num < explosionRadius / 1.5f)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("DeathExplode", PhotonTargets.All, ownerID, Network.PLAYER.GetComponent<PhotonView>().ownerId);
		}
		else if (num < explosionRadius * 1.5f)
		{
			Vector3 direction = Camera.main.transform.position - base.transform.position;
			int num2 = (1 << LayerMask.NameToLayer("Water")) | (1 << LayerMask.NameToLayer("Gun")) | (1 << LayerMask.NameToLayer("Ragdolls/Pickups")) | (1 << LayerMask.NameToLayer("Objects")) | (1 << LayerMask.NameToLayer("Interactive Objs")) | (1 << LayerMask.NameToLayer("Arm")) | (1 << LayerMask.NameToLayer("NoCollisionPlayer")) | (1 << LayerMask.NameToLayer("WeaponPickups"));
			num2 = ~num2;
			RaycastHit hitInfo;
			if (Physics.Raycast(base.transform.position, direction, out hitInfo, 30f, num2) && hitInfo.collider.tag == "Player")
			{
				MonoBehaviour.print("HIT GRENADE!!!" + Time.time);
				Network.PLAYER.GetComponent<PhotonView>().RPC("DeathExplode", PhotonTargets.All, ownerID, Network.PLAYER.GetComponent<PhotonView>().ownerId);
			}
		}
	}

	private IEnumerator explosePowerSet()
	{
		yield return new WaitForSeconds(0.35f);
		Collider[] colliders = Physics.OverlapSphere(base.transform.position, explosionRadius * 2f);
		for (int i = 0; i < colliders.Length; i++)
		{
			if ((bool)colliders[i].GetComponent<Rigidbody>())
			{
				if (colliders[i].GetComponent<Rigidbody>().mass < 1000f)
				{
					colliders[i].GetComponent<Rigidbody>().AddExplosionForce(explosionPower * colliders[i].GetComponent<Rigidbody>().mass, base.transform.position, explosionRadius * 2f);
				}
				else
				{
					colliders[i].GetComponent<Rigidbody>().AddExplosionForce(explosionPower / 3f * colliders[i].GetComponent<Rigidbody>().mass, base.transform.position, explosionRadius * 2f);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 13)
		{
			other.gameObject.SendMessage("Dead");
		}
		if ((bool)other.GetComponent<DestroyFracture>() && other.gameObject.layer != 12)
		{
			other.SendMessage("ApplyDamage", explosionPower);
		}
		if (!_carBoom && !attacked && (bool)other.GetComponent<DamageZone>())
		{
			Debug.Log("Attack to: " + other.name);
			if ((bool)other.GetComponent<DamageZone>().BaseVehicle.GetComponent<CarSystem>())
			{
				other.GetComponent<DamageZone>().BaseVehicle.SendMessage("DestroyVehicle");
				attacked = true;
			}
			if ((bool)other.GetComponent<DamageZone>().BaseVehicle.GetComponent<TrackSystem>())
			{
				other.GetComponent<DamageZone>().BaseVehicle.SendMessage("DestroyVehicle");
				attacked = true;
			}
			if ((bool)other.GetComponent<DamageZone>().BaseVehicle.GetComponent<HeliSystem>())
			{
				other.GetComponent<DamageZone>().BaseVehicle.SendMessage("DestroyVehicle");
				attacked = true;
			}
		}
	}
}
