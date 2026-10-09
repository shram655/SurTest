using System.Collections;
using UnityEngine;

public class FireObject : MonoBehaviour
{
	private float _timer;

	private float _timer2;

	public string type = string.Empty;

	public GameObject EXPLOSION;

	private bool OBOGREV;

	public GameObject obj;

	public GameObject _smokeEffect;

	public GameObject _smokeEffect2;

	private void Start()
	{
		Object.Destroy(base.gameObject.GetComponent<Rigidbody>(), 5f);
		Object.Destroy(base.gameObject, 300f);
		_timer = Time.time;
		_timer2 = Time.time;
	}

	private IEnumerator offSmoke()
	{
		yield return new WaitForSeconds(40f);
		_smokeEffect.GetComponent<ParticleSystem>().loop = false;
		_smokeEffect2.GetComponent<ParticleSystem>().loop = false;
	}

	private void Update()
	{
		if (type == "ammonal")
		{
			if (Time.time > _timer + 10f)
			{
				Object.Instantiate(EXPLOSION, base.transform.position, Quaternion.identity);
				Object.Destroy(base.gameObject);
			}
			return;
		}
		if (type == "grenade")
		{
			if (Time.time > _timer + 4.2f)
			{
				if ((bool)_smokeEffect)
				{
					_smokeEffect.SetActive(true);
					base.GetComponent<Rigidbody>().isKinematic = true;
					_smokeEffect.transform.eulerAngles = new Vector3(270f, 0f, 0f);
					StartCoroutine("offSmoke");
					Object.Destroy(base.gameObject, 60f);
					base.enabled = false;
				}
				else
				{
					Object.Instantiate(EXPLOSION, base.transform.position, Quaternion.identity);
					Object.Destroy(base.gameObject);
				}
			}
			return;
		}
		if (Time.time > _timer + 3f)
		{
			_timer = Time.time + 1000f;
		}
		if (OBOGREV && Time.time > _timer2 + 1f)
		{
			if (FPSPlayer.temperaturePoints > 0)
			{
				FPSPlayer.SetTemperaturePoints(FPSPlayer.temperaturePoints - 1);
			}
			_timer2 = Time.time;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			OBOGREV = true;
		}
		if (other.tag == "Usable" && (bool)other.GetComponent<ItemPickup>())
		{
			if (other.GetComponent<ItemPickup>().ITEM_ID == 50)
			{
				obj = other.gameObject;
				StartCoroutine("CreateFood", 52);
			}
			else if (other.GetComponent<ItemPickup>().ITEM_ID == 3)
			{
				obj = other.gameObject;
				StartCoroutine("CreateFood", 55);
			}
			else if (other.GetComponent<ItemPickup>().ITEM_ID == 55)
			{
				obj = other.gameObject;
				StartCoroutine("CreateFood", 57);
			}
			else if (other.GetComponent<ItemPickup>().ITEM_ID == 60)
			{
				obj = other.gameObject;
				StartCoroutine("CreateFood", 62);
			}
		}
	}

	private IEnumerator CreateFood(int foodid)
	{
		yield return new WaitForSeconds(1f);
		if (!obj)
		{
			yield break;
		}
		if ((bool)Network.PLAYER)
		{
			GameObject pateto = (GameObject)Object.Instantiate(Resources.Load("_ITEM_BS"));
			if ((bool)pateto.GetComponent<Rigidbody>())
			{
				pateto.GetComponent<Rigidbody>().isKinematic = true;
			}
			pateto.SendMessage("ShowItem", foodid);
			pateto.GetComponent<ItemPickup>().ITEM_COUNT = 100;
			pateto.GetComponent<ItemPickup>().isLoot = false;
			pateto.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
			pateto.transform.position = obj.transform.position;
			pateto.name = "ITEM: " + Network.PLAYER.GetComponent<PhotonView>().owner.name + ". " + foodid;
			Network.PLAYER.GetComponent<PhotonView>().RPC("DestroyObject", PhotonTargets.Others, obj.name);
		}
		Object.Destroy(obj);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			OBOGREV = false;
		}
	}
}
