using System;
using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine;

public class Ballistic : MonoBehaviour
{
	public Renderer _renderer;

	public int ownerID;

	public bool isBOT;

	public bool MINE;

	public int SIDE;

	public GameObject _explosion;

	private Vector3 hitPos;

	private bool prostrel;

	public AudioClip[] wzgSounds;

	public bool isOthers;

	public GameObject weaponObj;

	private int damage;

	private int oldDamage;

	public float bulletSpeed = 1f;

	private int poletTimer;

	public GameObject[] impactEffect;

	public bool isSleep = true;

	public void RestartBullet()
	{
		if ((bool)base.GetComponent<Renderer>())
		{
			base.GetComponent<Renderer>().enabled = false;
		}
		if ((bool)_renderer)
		{
			_renderer.enabled = false;
		}
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		isSleep = true;
		isOthers = false;
		poletTimer = 0;
		GetComponent<TrailRenderer>().enabled = false;
		if (MINE)
		{
			base.GetComponent<Rigidbody>().isKinematic = true;
			base.GetComponent<Collider>().enabled = false;
		}
		base.enabled = false;
	}

	private IEnumerator GoToSleepBullet(float timerz)
	{
		yield return new WaitForSeconds(timerz);
		RestartBullet();
	}

	private void Awake()
	{
		RestartBullet();
	}

	public void StartBullet()
	{
		if ((bool)base.GetComponent<Renderer>())
		{
			base.GetComponent<Renderer>().enabled = true;
		}
		if ((bool)_renderer)
		{
			_renderer.enabled = true;
		}
		if (MINE)
		{
			base.GetComponent<Rigidbody>().isKinematic = false;
			base.GetComponent<Collider>().enabled = true;
		}
		isSleep = false;
		base.GetComponent<AudioSource>().clip = wzgSounds[UnityEngine.Random.Range(0, wzgSounds.Length)];
		base.GetComponent<AudioSource>().Play();
		if (!MINE)
		{
			StartCoroutine("GoToSleepBullet", 1.5f);
		}
		else
		{
			base.GetComponent<Rigidbody>().velocity = base.transform.TransformDirection(new Vector3(0f, 0f, 25f * bulletSpeed));
			StartCoroutine("GoToSleepBullet", 30f);
		}
		if (!isOthers)
		{
			CheckHits();
		}
	}

	public void SetDamage(int damg)
	{
		damage = damg;
		oldDamage = damg * 657;
	}

	private void FixedUpdate()
	{
		if (isSleep)
		{
			return;
		}
		if (!MINE)
		{
			base.transform.Translate(Vector3.forward * 4f);
		}
		if (damage != oldDamage / 657)
		{
			SetDamage(0);
			BaseLogic.SetBan(15000);
			BaseLogic.Banz();
		}
		if (poletTimer > 4)
		{
			if (MINE)
			{
				base.transform.rotation = Quaternion.LookRotation(base.GetComponent<Rigidbody>().velocity);
			}
			if (!isOthers)
			{
				CheckHits();
			}
			poletTimer = 0;
		}
		else
		{
			poletTimer++;
		}
	}

	private void CheckHits()
	{
		RaycastHit hitInfo;
		if (!Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.forward), out hitInfo, 25f, BaseLogic.layerSet) || !hitInfo.collider)
		{
			return;
		}
		if (MINE)
		{
			if ((bool)_explosion)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(_explosion, hitInfo.point, Quaternion.identity) as GameObject;
				gameObject.GetComponent<Explosion>().ownerID = ownerID;
			}
			RestartBullet();
			return;
		}
		if (isOthers)
		{
			weaponObj = BaseLogic._playerWeapon;
		}
		if (!prostrel && (hitInfo.collider.tag == "Flesh" || hitInfo.collider.tag == "Glass" || hitInfo.collider.tag == "Water" || hitInfo.collider.tag == "ProstrelMetal" || hitInfo.collider.tag == "Prostrel" || hitInfo.collider.tag == "Usable"))
		{
			Ballistic ballistic = null;
			for (int i = 0; i < BaseLogic._poolBullets.Length; i++)
			{
				ballistic = BaseLogic._poolBullets[i].GetComponent<Ballistic>();
				if (ballistic.isSleep)
				{
					ballistic.enabled = true;
					ballistic.transform.position = hitInfo.point + base.transform.TransformDirection(Vector3.forward) * 0.1f;
					ballistic.transform.rotation = base.transform.rotation;
					ballistic.weaponObj = weaponObj;
					ballistic.SetDamage(damage);
					ballistic.prostrel = true;
					ballistic.StartBullet();
					break;
				}
			}
			if (hitInfo.collider.tag == "Prostrel")
			{
				UnityEngine.Object.Instantiate(impactEffect[0], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
			}
			else if (hitInfo.collider.tag == "Usable" || hitInfo.collider.tag == "ProstrelMetal")
			{
				UnityEngine.Object.Instantiate(impactEffect[2], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
			}
			else
			{
				UnityEngine.Object.Instantiate(impactEffect[4], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
			}
			RestartBullet();
			return;
		}
		if (isBOT)
		{
			if (hitInfo.collider.tag == "Player")
			{
				hitInfo.collider.SendMessage("ApplyDamage", UnityEngine.Random.Range(12, 20));
			}
			else if (hitInfo.collider.tag == "Usable" && (bool)hitInfo.collider.GetComponent<DamageZone>())
			{
				if ((bool)hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.GetComponent<CarSystem>())
				{
					if (hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.GetComponent<CarSystem>()._control)
					{
						Network.PLAYER.GetComponent<PhotonView>().RPC("ApplyVehicleDamage", PhotonTargets.Others, hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.name, Convert.ToInt32(damage));
					}
				}
				else if ((bool)hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.GetComponent<HeliSystem>())
				{
					if (hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.GetComponent<HeliSystem>()._control)
					{
						Network.PLAYER.GetComponent<PhotonView>().RPC("ApplyVehicleDamage", PhotonTargets.Others, hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.name, Convert.ToInt32(damage));
					}
				}
				else if ((bool)hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.GetComponent<TrackSystem>() && hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.GetComponent<TrackSystem>()._control)
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("ApplyVehicleDamage", PhotonTargets.Others, hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.name, Convert.ToInt32(damage));
				}
			}
		}
		if (!isOthers)
		{
			if (hitInfo.collider.tag == "armAnimal")
			{
				UnityEngine.Object.Instantiate(impactEffect[1], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
				RaycastHit hitInfo2;
				if (Physics.Raycast(hitInfo.point + base.transform.TransformDirection(Vector3.forward) * 0.22f, base.transform.TransformDirection(Vector3.forward), out hitInfo2, 7f, BaseLogic.layerSet))
				{
					weaponObj.SendMessage("BloodMarks", hitInfo2);
				}
				if ((bool)hitInfo.collider.GetComponent<InputDamage>())
				{
					AnimalAI component = hitInfo.collider.GetComponent<InputDamage>().HeroBase.GetComponent<AnimalAI>();
					if (component.HP > 0f)
					{
						float damageMult = hitInfo.collider.GetComponent<InputDamage>().DamageMult;
						component.SendMessage("ApplyDamage", (float)damage * damageMult);
						if ((bool)Network.PLAYER && Time.time > Network.PLAYER.GetComponent<HeroBase>()._updateMobAttackRPC + 3f)
						{
							Network.PLAYER.GetComponent<PhotonView>().RPC("SetTargetAnimal", PhotonTargets.Others, string.Empty + component.name + ":" + component.transform.position.x + ":" + component.transform.position.y + ":" + component.transform.position.z + ":" + component._animalType);
							Network.PLAYER.GetComponent<HeroBase>()._updateMobAttackRPC = Time.time;
						}
					}
				}
				else
				{
					hitInfo.collider.SendMessage("ApplyDamage", damage);
					Network.PLAYER.GetComponent<PhotonView>().RPC("DeadAnimal", PhotonTargets.Others, string.Empty + hitInfo.collider.name);
				}
			}
			else if (hitInfo.collider.tag == "Usable")
			{
				UnityEngine.Object.Instantiate(impactEffect[2], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
				if ((bool)hitInfo.collider.GetComponent<DamageZone>())
				{
					if ((bool)hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.GetComponent<CarSystem>())
					{
						Network.PLAYER.GetComponent<PhotonView>().RPC("ApplyVehicleDamage", PhotonTargets.Others, hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.name, Convert.ToInt32(damage));
					}
					else if ((bool)hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.GetComponent<HeliSystem>())
					{
						Network.PLAYER.GetComponent<PhotonView>().RPC("ApplyVehicleDamage", PhotonTargets.Others, hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.name, Convert.ToInt32(damage));
					}
					else if ((bool)hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.GetComponent<TrackSystem>())
					{
						Network.PLAYER.GetComponent<PhotonView>().RPC("ApplyVehicleDamage", PhotonTargets.Others, hitInfo.collider.GetComponent<DamageZone>().BaseVehicle.name, Convert.ToInt32(damage));
					}
				}
			}
			else if (hitInfo.collider.tag == "arm")
			{
				UnityEngine.Object.Instantiate(impactEffect[1], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
				RaycastHit hitInfo3;
				if (Physics.Raycast(hitInfo.point + base.transform.TransformDirection(Vector3.forward) * 0.1f, base.transform.TransformDirection(Vector3.forward), out hitInfo3, 5f, BaseLogic.layerSet) && (bool)weaponObj)
				{
					weaponObj.SendMessage("BloodMarks", hitInfo3);
				}
				HeroBase component2 = hitInfo.collider.GetComponent<InputDamage>().HeroBase.GetComponent<HeroBase>();
				GameObject heroBase = hitInfo.collider.GetComponent<InputDamage>().HeroBase;
				float damageMult2 = hitInfo.collider.GetComponent<InputDamage>().DamageMult;
				float num = ((damageMult2 > 2f) ? ((float)damage * damageMult2 / component2.headArmor) : ((damageMult2 != 1f && damageMult2 != 0.85f) ? ((float)damage * damageMult2) : ((float)damage * damageMult2 / ((float)component2.bodyArmorCoef / 10f))));
				float distance = hitInfo.distance;
				num = ((!(base.name == "R_SHOTGUN") && !(base.name == "R_DOUBLESHOTGUN") && !(base.name == "R_SAYGA")) ? ((!(base.name == "R_SVD") && !(base.name == "R_AKMS") && !(base.name == "R_AK12") && !(base.name == "R_SKS") && !(base.name == "R_TRG") && !(base.name == "R_PECHENEG") && !(base.name == "R_MOSINA")) ? (num - distance / 20f) : (num - distance / 30f)) : (num - distance / 10f));
				if (num < 1f)
				{
					RestartBullet();
					return;
				}
				if ((!component2._godMode && component2.SIDE != BaseLogic.SideReal && BaseLogic.GAME_MODE != 2 && BaseLogic.GAME_MODE != 4) || component2.BOT)
				{
					if (component2.BOT)
					{
						component2.SetHitPoints(component2.hitPointsReal - Convert.ToInt32(num));
						if (!component2._syncDead && (float)component2.hitPointsReal <= num)
						{
							if (damageMult2 > 2f)
							{
								AudioSource.PlayClipAtPoint(weaponObj.GetComponent<PlayerWeapons>()._headShoot, base.transform.position, 0.95f);
								if (BaseLogic.GAME_MODE == 6)
								{
									BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY + 350);
								}
							}
							else if (BaseLogic.GAME_MODE == 6)
							{
								BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY + 250);
								AudioSource.PlayClipAtPoint(BaseLogic._player.GetComponent<BaseLogic>().KASSA, base.transform.position, 0.95f);
							}
							ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
							hashtable.Add("kills", (int)PhotonNetwork.player.customProperties["kills"] + 1);
							PhotonNetwork.player.SetCustomProperties(hashtable);
							component2.SendMessage("Dead");
							Network.PLAYER.GetComponent<PhotonView>().RPC("DeadAnimal", PhotonTargets.Others, string.Empty + hitInfo.collider.GetComponent<InputDamage>().HeroBase.name);
						}
						else
						{
							if (!component2.GetComponent<AudioSource>().isPlaying && !component2._syncDead)
							{
								component2.GetComponent<AudioSource>().clip = component2.GetComponent<SoldierAI>().snd_hit[UnityEngine.Random.Range(0, component2.GetComponent<SoldierAI>().snd_hit.Length)];
								component2.GetComponent<AudioSource>().Play();
							}
							if (component2.GetComponent<SoldierAI>().fireTarget == null)
							{
								Network.PLAYER.GetComponent<PhotonView>().RPC("SetTargetAnimal", PhotonTargets.Others, string.Empty + component2.name + ":" + component2.transform.position.x + ":" + component2.transform.position.y + ":" + component2.transform.position.z + ":100");
							}
						}
					}
					else if (!(num > 30f) || !(damageMult2 < 2f))
					{
						component2.SetHitPoints(component2.hitPointsReal - Convert.ToInt32(num));
						if (!component2._syncDead && (float)component2.hitPointsReal <= num)
						{
							if (component2.WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 > 0)
							{
								if (damageMult2 > 2f && !component2._syncDead && BaseLogic.GAME_MODE != 2 && BaseLogic.GAME_MODE != 4 && component2.SIDE != BaseLogic.SideReal)
								{
									AudioSource.PlayClipAtPoint(weaponObj.GetComponent<PlayerWeapons>()._headShoot, base.transform.position, 0.95f);
									if (BaseLogic.GAME_MODE == 6)
									{
										BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY + 150);
									}
									else
									{
										BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY + 25);
									}
								}
								if (BaseLogic.GAME_MODE == 6)
								{
									BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY + 350);
									AudioSource.PlayClipAtPoint(BaseLogic._player.GetComponent<BaseLogic>().KASSA, base.transform.position, 0.95f);
								}
								else
								{
									BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY + 50);
									AudioSource.PlayClipAtPoint(BaseLogic._player.GetComponent<BaseLogic>().KASSA, base.transform.position, 0.95f);
								}
							}
							ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
							hashtable2.Add("kills", (int)PhotonNetwork.player.customProperties["kills"] + 1);
							PhotonNetwork.player.SetCustomProperties(hashtable2);
							heroBase.GetComponent<Heroserialize>().enabled = false;
							component2.animator.enabled = false;
							component2.RagdollOn();
							component2.photonView.RPC("Death", PhotonTargets.All, PhotonNetwork.player.ID, component2.photonView.ownerId);
							component2._syncDead = true;
						}
						else
						{
							component2.photonView.RPC("Attack", PhotonTargets.Others, Convert.ToInt32(num));
						}
					}
				}
			}
			else if (hitInfo.collider.tag == "Wood")
			{
				UnityEngine.Object.Instantiate(impactEffect[0], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
			}
			else if (hitInfo.collider.tag == "Metal" || hitInfo.collider.tag == "Usable")
			{
				UnityEngine.Object.Instantiate(impactEffect[2], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
			}
			else if (hitInfo.collider.tag == "Snow")
			{
				UnityEngine.Object.Instantiate(impactEffect[3], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
			}
			else
			{
				UnityEngine.Object.Instantiate(impactEffect[4], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
			}
		}
		else if (hitInfo.collider.tag == "armAnimal" || hitInfo.collider.tag == "arm")
		{
			UnityEngine.Object.Instantiate(impactEffect[1], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
		}
		else if (hitInfo.collider.tag == "Wood")
		{
			UnityEngine.Object.Instantiate(impactEffect[0], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
		}
		else if (hitInfo.collider.tag == "Metal" || hitInfo.collider.tag == "Usable")
		{
			UnityEngine.Object.Instantiate(impactEffect[2], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
		}
		else if (hitInfo.collider.tag == "Snow")
		{
			UnityEngine.Object.Instantiate(impactEffect[3], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
		}
		else
		{
			UnityEngine.Object.Instantiate(impactEffect[4], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
		}
		if ((bool)hitInfo.rigidbody && hitInfo.rigidbody.useGravity && !hitInfo.rigidbody.isKinematic)
		{
			hitInfo.rigidbody.AddForceAtPosition(bulletSpeed * 1.55f * (float)damage * base.transform.TransformDirection(Vector3.forward) / (Time.fixedDeltaTime * 100f), hitInfo.point);
		}
		RestartBullet();
	}
}
