using System.Collections;
using UnityEngine;

public class MaskPickup : MonoBehaviour
{
	public bool inSceneObj;

	public bool isLoot;

	public Renderer[] MASK;

	public int MASK_ID;

	public AudioClip pickupSound;

	private void Awake()
	{
		if (isLoot)
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
	}

	private void Start()
	{
		if (inSceneObj)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_ITEM_MASKS_BS"));
			gameObject.GetComponent<MaskPickup>().isLoot = isLoot;
			gameObject.transform.position = base.transform.position;
			gameObject.transform.rotation = base.transform.rotation;
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		HideAllHelmets();
		if (isLoot)
		{
			base.name = "MASK_LOOT_ID: " + BaseLogic.LOOT_MASK;
			BaseLogic.LOOT_MASK++;
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
			GenerateHelmets();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>(), 5f);
		MASK[MASK_ID - 1].enabled = true;
		for (int i = 0; i < MASK.Length; i++)
		{
			if (i != MASK_ID - 1)
			{
				UnityEngine.Object.Destroy(MASK[i].gameObject);
			}
		}
	}

	private void HideAllHelmets()
	{
		for (int i = 0; i < MASK.Length; i++)
		{
			MASK[i].enabled = false;
		}
	}

	private void GenerateHelmets()
	{
		int num = UnityEngine.Random.Range(0, 2000);
		if (num < 100)
		{
			int num2 = MASK_ID = UnityEngine.Random.Range(1, MASK.Length + 1);
			MASK[MASK_ID - 1].enabled = true;
			for (int i = 0; i < MASK.Length; i++)
			{
				if (i != MASK_ID - 1)
				{
					UnityEngine.Object.Destroy(MASK[i].gameObject);
				}
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void PickUpItem()
	{
		MASK[MASK_ID - 1].enabled = false;
		GetComponent<Collider>().enabled = false;
		if ((bool)pickupSound)
		{
			AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, 0.75f);
		}
		if ((bool)Network.PLAYER)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("DestroyObject", PhotonTargets.Others, base.gameObject.name);
		}
		StartCoroutine("Pickup");
	}

	private IEnumerator Pickup()
	{
		yield return new WaitForSeconds(1f);
		if (BaseLogic._ClothMask != 0 && (bool)Network.PLAYER)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("CreateWeapon", PhotonTargets.All, new Vector2(BaseLogic._ClothMask + 599, 0f));
		}
		//BaseLogic.SetClothMask(MASK_ID);
		if ((bool)Network.PLAYER)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("SetMask", PhotonTargets.Others, MASK_ID);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
