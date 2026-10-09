using UnityEngine;

public class SoldierAI : MonoBehaviour
{
	public bool onlyStand;

	public bool onlyCrouch;

	private float distanceWall = 5f;

	public float _reactionTime = 1f;

	public Transform TARGET_CAMERA;

	private bool moved;

	private bool attacked;

	private bool moveBack;

	public float fireRate = 0.12f;

	public int fireLenght = 15;

	private int oldfireLenght = 15;

	private int fireBullets;

	public Transform[] Waypoints;

	public Transform fireTarget;

	private int waypoindID;

	private CharacterController characterController;

	private Animator animator;

	private Transform myTransform;

	public bool dead;

	private float updateTimer;

	private float fireTimer;

	private float fireDelay;

	private float reloadTimer;

	public float AttackRadius = 15000f;

	private float startTimer;

	private Vector3 movedPosition;

	public float zveno_x;

	public float zveno_z;

	public int startWeapon;

	private float moveAttackTimer;

	private float rayHitTimer;

	private Vector3 movePosition = Vector3.zero;

	private float animWalkSpeed = 0.48f;

	public float alarmTimer = -60f;

	public AudioClip[] snd_hit;

	public AudioClip[] snd_attack;

	private void Awake()
	{
		base.gameObject.layer = 13;
		AttackRadius = 30000f;
		base.GetComponent<AudioSource>().minDistance = 5f;
		base.GetComponent<AudioSource>().maxDistance = 80f;
		oldfireLenght = fireLenght;
		animWalkSpeed = Random.Range(0.48f, 0.52f);
		startWeapon = GetComponent<HeroBase>().BOT_WEAPON;
		startTimer = Time.time + Random.Range(0.1f, 3f);
		myTransform = base.transform;
		moved = true;
		animator = GetComponent<Animator>();
		if (onlyCrouch)
		{
			animator.SetBool("Crouched", true);
		}
		animator.SetInteger("AnimID", 0);
		characterController = GetComponent<CharacterController>();
	}

	private void Start()
	{
		GetComponent<HeroBase>().WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 = 0;
		GetComponent<HeroBase>().WEAPONS_RIFLE_OBJ.GetComponent<WeaponsBack>()._slotWeapon2 = startWeapon;
	}

	private void Fire()
	{
		if (startWeapon == 10 || startWeapon == 11 || startWeapon == 17)
		{
			TARGET_CAMERA.eulerAngles = new Vector3(TARGET_CAMERA.eulerAngles.x + Random.Range(-0.1f, 0.1f), TARGET_CAMERA.eulerAngles.y + Random.Range(-0.2f, 0f), 0f);
		}
		else if (startWeapon == 6 || startWeapon == 16 || startWeapon == 18)
		{
			TARGET_CAMERA.eulerAngles = new Vector3(TARGET_CAMERA.eulerAngles.x + Random.Range(-1.1f, 1.1f), TARGET_CAMERA.eulerAngles.y + Random.Range(-1.1f, 0.5f), 0f);
		}
		else
		{
			TARGET_CAMERA.eulerAngles = new Vector3(TARGET_CAMERA.eulerAngles.x + Random.Range(-1.4f, 1.4f), TARGET_CAMERA.eulerAngles.y + Random.Range(-1.4f, 1f), 0f);
		}
		SendMessage("FireEffectLocal", TARGET_CAMERA.rotation);
	}

	public void UnHideWeapon()
	{
		animator.SetInteger("AnimID", 2);
		GetComponent<HeroBase>().WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 = startWeapon;
		GetComponent<HeroBase>().WEAPONS_RIFLE_OBJ.GetComponent<WeaponsBack>()._slotWeapon2 = 0;
	}

	public void Attack()
	{
		if (dead || startWeapon == 0)
		{
			return;
		}
		if (!base.GetComponent<AudioSource>().isPlaying)
		{
			base.GetComponent<AudioSource>().clip = snd_attack[Random.Range(0, snd_attack.Length)];
			base.GetComponent<AudioSource>().Play();
		}
		fireLenght = oldfireLenght + Random.Range(oldfireLenght / 4, oldfireLenght / 3);
		alarmTimer = Time.time;
		UnHideWeapon();
		if (!onlyStand)
		{
			int num = Random.Range(0, 10);
			if (num > 3)
			{
				animator.SetBool("Crouched", true);
			}
			else if (!onlyCrouch)
			{
				animator.SetBool("Crouched", false);
			}
		}
		moved = false;
		attacked = true;
		fireDelay = Time.time + 1f;
	}

	private void RotateTowards(Vector3 position)
	{
		Vector3 forward = position - myTransform.position;
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(forward), 1f * Time.deltaTime);
		myTransform.eulerAngles = new Vector3(0f, myTransform.eulerAngles.y, 0f);
		TARGET_CAMERA.rotation = Quaternion.LookRotation(forward);
	}

	private void CheckWaypoint()
	{
		if (moveBack)
		{
			if (waypoindID > 0)
			{
				waypoindID--;
			}
			else
			{
				waypoindID = 1;
				moveBack = false;
			}
		}
		else if (waypoindID < Waypoints.Length - 1)
		{
			waypoindID++;
		}
		else
		{
			waypoindID = Waypoints.Length - 2;
			moveBack = true;
		}
		_reactionTime = Time.time;
	}

	private void Update()
	{
		if (dead || !(Time.time > startTimer))
		{
			return;
		}
		if (attacked && Time.time > fireTimer + fireRate && Time.time > fireDelay)
		{
			if (fireBullets < fireLenght)
			{
				fireBullets++;
				Fire();
			}
			else
			{
				movePosition = fireTarget.position;
				fireTarget = null;
				fireBullets = 0;
				attacked = false;
				moved = true;
				if (!onlyCrouch)
				{
					animator.SetBool("Crouched", false);
				}
				reloadTimer = Time.time + 0.5f;
				moveAttackTimer = Time.time + 20f;
				myTransform.eulerAngles = new Vector3(0f, myTransform.eulerAngles.y, 0f);
			}
			fireTimer = Time.time;
		}
		if (attacked && (bool)fireTarget)
		{
			float @float = animator.GetFloat("Speed");
			float value = Mathf.Lerp(@float, 0f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value);
			animator.SetLookAtWeight(1f, 1f, 1f);
			animator.SetLookAtPosition(new Vector3(fireTarget.position.x, fireTarget.position.y - 6f, fireTarget.position.z));
			RotateTowards(new Vector3(fireTarget.position.x, fireTarget.position.y - 0.8f, fireTarget.position.z));
		}
		if (Time.time > reloadTimer && Waypoints.Length != 0 && moved)
		{
			RaycastHit hitInfo;
			if (Time.time > moveAttackTimer)
			{
				if (Time.time > rayHitTimer + 2f)
				{
					if (Physics.Raycast(GetComponent<HeroBase>().Head.position, base.transform.TransformDirection(Vector3.forward), out hitInfo, distanceWall, BaseLogic.layerSet))
					{
						CheckWaypoint();
					}
					rayHitTimer = Time.time;
				}
				float num = Vector3.Distance(myTransform.position, new Vector3(Waypoints[waypoindID].position.x + zveno_x, Waypoints[waypoindID].position.y, Waypoints[waypoindID].position.z + zveno_z));
				if (num < 3f)
				{
					CheckWaypoint();
				}
				RotateTowards(new Vector3(Waypoints[waypoindID].position.x + zveno_x, Waypoints[waypoindID].position.y, Waypoints[waypoindID].position.z + zveno_z));
				Vector3 vector = myTransform.TransformDirection(Vector3.forward);
				float float2 = animator.GetFloat("Speed");
				if (alarmTimer + 60f > Time.time)
				{
					Vector3 speed = vector * 4.4f;
					float value2 = Mathf.Lerp(float2, 1f, Time.deltaTime * 4f);
					animator.SetFloat("Speed", value2);
					characterController.SimpleMove(speed);
				}
				else
				{
					Vector3 speed2 = vector * 2.2f;
					float value3 = Mathf.Lerp(float2, animWalkSpeed, Time.deltaTime * 4f);
					animator.SetFloat("Speed", value3);
					characterController.SimpleMove(speed2);
				}
			}
			else
			{
				if (Time.time > rayHitTimer + 2f)
				{
					if (Physics.Raycast(GetComponent<HeroBase>().Head.position, base.transform.TransformDirection(Vector3.forward), out hitInfo, 15f, BaseLogic.layerSet))
					{
						moveAttackTimer = -1000f;
					}
					rayHitTimer = Time.time;
				}
				RotateTowards(new Vector3(movePosition.x + zveno_x, movePosition.y, movePosition.z + zveno_z));
				Vector3 vector2 = myTransform.TransformDirection(Vector3.forward);
				float float3 = animator.GetFloat("Speed");
				Vector3 speed3 = vector2 * 4.4f;
				float value4 = Mathf.Lerp(float3, 1f, Time.deltaTime * 4f);
				animator.SetFloat("Speed", value4);
				characterController.SimpleMove(speed3);
			}
		}
		if (!(Time.time > updateTimer + 2f))
		{
			return;
		}
		float sqrMagnitude = (Camera.main.transform.position - myTransform.position).sqrMagnitude;
		if (sqrMagnitude < AttackRadius)
		{
			RaycastHit hitInfo3;
			if ((bool)Network.PLAYER && FPSPlayer.hitPointsReal > 0 && Network.PLAYER.GetComponent<HeroBase>().VEHICLE == null)
			{
				RaycastHit hitInfo2;
				if (Physics.Linecast(GetComponent<HeroBase>().Head.position, Camera.main.transform.position, out hitInfo2, BaseLogic.layerSet) && hitInfo2.collider.tag == "Player" && fireTarget == null)
				{
					fireTarget = BaseLogic._player.transform;
					Attack();
					if ((bool)Network.PLAYER)
					{
						Network.PLAYER.GetComponent<PhotonView>().RPC("SetTargetAnimal", PhotonTargets.Others, string.Empty + base.name + ":" + base.transform.position.x + ":" + base.transform.position.y + ":" + base.transform.position.z + ":" + 100);
					}
				}
			}
			else if ((bool)Network.PLAYER && FPSPlayer.hitPointsReal > 0 && Network.PLAYER.GetComponent<HeroBase>().VEHICLE != null && Physics.Linecast(GetComponent<HeroBase>().Head.position, BaseLogic._player.transform.position, out hitInfo3, BaseLogic.layerSet) && (bool)hitInfo3.collider.GetComponent<DamageZone>() && fireTarget == null)
			{
				fireTarget = BaseLogic._player.transform;
				Attack();
				if ((bool)Network.PLAYER)
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("SetTargetAnimal", PhotonTargets.Others, string.Empty + base.name + ":" + base.transform.position.x + ":" + base.transform.position.y + ":" + base.transform.position.z + ":100");
				}
			}
		}
		updateTimer = Time.time;
	}
}
