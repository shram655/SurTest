using UnityEngine;

public class AnimalAI : MonoBehaviour
{
	public GameObject SMD_Root;

	public bool _lodSys = true;

	private float _lodSysTimer;

	public bool Moved;

	public Transform MovedTarget;

	private float targetSearchMovedTimer;

	public AudioClip snd_attack;

	public AudioClip[] snd_dead;

	private float _updateTimer;

	public float HP;

	public float damage;

	public GameObject target;

	public int MoveSpeed;

	public int RotationSpeed;

	public float AttackRadius = 50f;

	public float MaxAttackRadius = 100f;

	public float BackRadius = 5f;

	public float AttackDistance = 2f;

	public bool AttackPlayer = true;

	public float DistanceToTarget;

	public bool HaveTarget;

	private Transform myTransform;

	private GameObject[] gos;

	public GameObject _model;

	private float _attackTimer;

	public string status = string.Empty;

	private Rigidbody[] rbs;

	private Component[] tmp;

	private int i;

	public Transform Bip01;

	public BoxCollider Bip01bc;

	public Rigidbody Bip01rb;

	private Transform LThigh;

	public CapsuleCollider LThighbc;

	private Rigidbody LThighrb;

	private CharacterJoint LThighhj;

	public Transform RThigh;

	public CapsuleCollider RThighbc;

	private Rigidbody RThighrb;

	private CharacterJoint RThighhj;

	private Transform LCalf;

	public CapsuleCollider LCalfbc;

	private Rigidbody LCalfrb;

	private CharacterJoint LCalfhj;

	private Transform RCalf;

	public CapsuleCollider RCalfbc;

	private Rigidbody RCalfrb;

	private CharacterJoint RCalfhj;

	private Transform Spine3;

	public BoxCollider Spine3bc;

	private Rigidbody Spine3rb;

	private CharacterJoint Spine3hj;

	public Transform Head;

	public SphereCollider Headbc;

	private Rigidbody Headrb;

	private CharacterJoint Headhj;

	private Transform LUpperArm;

	public CapsuleCollider LUpperArmbc;

	private Rigidbody LUpperArmrb;

	private CharacterJoint LUpperArmhj;

	private Transform RUpperArm;

	public CapsuleCollider RUpperArmbc;

	private Rigidbody RUpperArmrb;

	private CharacterJoint RUpperArmhj;

	public Transform LForearm;

	public CapsuleCollider LForearmbc;

	private Rigidbody LForearmrb;

	private CharacterJoint LForearmhj;

	public Transform RForearm;

	public CapsuleCollider RForearmbc;

	private Rigidbody RForearmrb;

	private CharacterJoint RForearmhj;

	public GameObject prefab;

	public Transform targetX;

	public bool isDead;

	private string Bip01_NAME = string.Empty;

	private string LThigh_NAME = string.Empty;

	private string RThigh_NAME = string.Empty;

	private string LCalf_NAME = string.Empty;

	private string RCalf_NAME = string.Empty;

	private string Spine3_NAME = string.Empty;

	private string Head_NAME = string.Empty;

	private string LUpperArm_NAME = string.Empty;

	private string RUpperArm_NAME = string.Empty;

	private string LForearm_NAME = string.Empty;

	private string RForearm_NAME = string.Empty;

	public int _animalType;

	private string _animRun = string.Empty;

	private string _animWalk = string.Empty;

	private string _animIdle = string.Empty;

	public string _animIdle2 = string.Empty;

	private string _animAttack = string.Empty;

	private string _animJump = string.Empty;

	private float _scaleColls;

	public int PRICE_MONEY;

	public int PRICE_XP;

	private int layerSet;

	public CharacterController controller;

	private Vector3 moveDirection = Vector3.zero;

	private void Awake()
	{
		targetSearchMovedTimer = Time.time + (float)Random.Range(5, 25);
		controller = GetComponent<CharacterController>();
		controller.enabled = false;
		layerSet = (1 << LayerMask.NameToLayer("Objects")) | (1 << LayerMask.NameToLayer("Arm")) | (1 << LayerMask.NameToLayer("Arm2")) | (1 << LayerMask.NameToLayer("NPCs"));
		layerSet = ~layerSet;
		if (_animalType == 1)
		{
			PRICE_MONEY = 100;
			PRICE_XP = 10;
			_scaleColls = 1f;

			_animRun = "run_1";
			_animIdle = "idleLookAround";
			_animIdle2 = _animIdle;
			_animAttack = "standBite";
			_animWalk = "walk_1";
			_animJump = "run_1";
			Bip01_NAME = "STANDARD_WOLF_";
			LThigh_NAME = "STANDARD_WOLF_ L Thigh";
			RThigh_NAME = "STANDARD_WOLF_ R Thigh";
			LCalf_NAME = "STANDARD_WOLF_ L Calf";
			RCalf_NAME = "STANDARD_WOLF_ R Calf";
			Spine3_NAME = "STANDARD_WOLF_ Spine1";
			Head_NAME = "STANDARD_WOLF_ Head";
			LUpperArm_NAME = "STANDARD_WOLF_ L UpperArm";
			RUpperArm_NAME = "STANDARD_WOLF_ R UpperArm";
			LForearm_NAME = "STANDARD_WOLF_ L Forearm";
			RForearm_NAME = "STANDARD_WOLF_ R Forearm";
			_model.GetComponent<Animation>()[_animRun].speed = 1.7f;
		}
		if (_animalType == 2)
		{
			PRICE_MONEY = 150;
			PRICE_XP = 15;
			_scaleColls = 1f;
			_animRun = "run_0";
			_animIdle = "idle2";
			_animIdle2 = _animIdle;
			_animAttack = "attack1";
			_animWalk = "walk_0";
			_animJump = "run_0";
			Bip01_NAME = "BOAR_";
			LThigh_NAME = "BOAR_ L Thigh";
			RThigh_NAME = "BOAR_ R Thigh";
			LCalf_NAME = "BOAR_ L Calf";
			RCalf_NAME = "BOAR_ R Calf";
			Spine3_NAME = "BOAR_ Spine1";
			Head_NAME = "BOAR_ Head";
			LUpperArm_NAME = "BOAR_ L UpperArm";
			RUpperArm_NAME = "BOAR_ R UpperArm";
			LForearm_NAME = "BOAR_ L Forearm";
			RForearm_NAME = "BOAR_ R Forearm";
			_model.GetComponent<Animation>()[_animRun].speed = 1.9f;
		}
		if (_animalType == 3)
		{
			PRICE_MONEY = 500;
			PRICE_XP = 50;
			_scaleColls = 2f;
			_animRun = "run";
			_animIdle = "idle4Legs";
			_animIdle2 = "idle4Legs";
			_animAttack = "4LegsClawsAttackR";
			_animWalk = "walk";
			_animJump = "run";
			Bip01_NAME = "BEAR_";
			LThigh_NAME = "BEAR_ L Thigh";
			RThigh_NAME = "BEAR_ R Thigh";
			LCalf_NAME = "BEAR_ L Calf";
			RCalf_NAME = "BEAR_ R Calf";
			Spine3_NAME = "BEAR_ Spine1";
			Head_NAME = "BEAR_ Head";
			LUpperArm_NAME = "BEAR_ L UpperArm";
			RUpperArm_NAME = "BEAR_ R UpperArm";
			LForearm_NAME = "BEAR_ L Forearm";
			RForearm_NAME = "BEAR_ R Forearm";
			_model.GetComponent<Animation>()[_animRun].speed = 1.6f;
		}
		if (_animalType == 4)
		{
			PRICE_MONEY = 100;
			PRICE_XP = 10;
			_scaleColls = 1f;
			_animRun = "run";
			_animIdle = "idle1";
			_animIdle2 = "idle2";
			_animAttack = "hornAttack1";
			_animWalk = "walk";
			Bip01_NAME = "STAG_";
			LThigh_NAME = "STAG_ L Thigh";
			RThigh_NAME = "STAG_ R Thigh";
			LCalf_NAME = "STAG_ L Calf";
			RCalf_NAME = "STAG_ R Calf";
			Spine3_NAME = "STAG_ Spine1";
			Head_NAME = "STAG_ Head";
			LUpperArm_NAME = "STAG_ L UpperArm";
			RUpperArm_NAME = "STAG_ R UpperArm";
			LForearm_NAME = "STAG_ L Forearm";
			RForearm_NAME = "STAG_ R Forearm";
			_model.GetComponent<Animation>()[_animRun].speed = 1.7f;
		}
		if (_animalType == 5)
		{
			PRICE_MONEY = 500;
			PRICE_XP = 50;
			_scaleColls = 2f;
			_animRun = "run";
			_animIdle = "idle";
			_animIdle2 = "idle";
			_animAttack = "attack3";
			_animWalk = "walk";
			_animJump = "jumpAttack";
			Bip01_NAME = "VAMPIRE_";
			LThigh_NAME = "VAMPIRE_ L Thigh";
			RThigh_NAME = "VAMPIRE_ R Thigh";
			LCalf_NAME = "VAMPIRE_ L Calf";
			RCalf_NAME = "VAMPIRE_ R Calf";
			Spine3_NAME = "VAMPIRE_ Spine1";
			Head_NAME = "VAMPIRE_ Head";
			LUpperArm_NAME = "VAMPIRE_ L UpperArm";
			RUpperArm_NAME = "VAMPIRE_ R UpperArm";
			LForearm_NAME = "VAMPIRE_ L Forearm";
			RForearm_NAME = "VAMPIRE_ R Forearm";
			_model.GetComponent<Animation>()[_animRun].speed = 1.6f;
		}
		if (_animalType == 6)
		{
			PRICE_MONEY = 500;
			PRICE_XP = 50;
			_scaleColls = 0.9f;
			_animRun = "run";
			_animIdle = "idle";
			_animIdle2 = "idle";
			_animAttack = "clawsAttack2HitCombo";
			_animWalk = "walk";
			_animJump = "jumpClawsAttack";
			Bip01_NAME = "GHOUL_";
			LThigh_NAME = "GHOUL_ L Thigh";
			RThigh_NAME = "GHOUL_ R Thigh";
			LCalf_NAME = "GHOUL_ L Calf";
			RCalf_NAME = "GHOUL_ R Calf";
			Spine3_NAME = "GHOUL_ Spine1";
			Head_NAME = "GHOUL_ Head";
			LUpperArm_NAME = "GHOUL_ L UpperArm";
			RUpperArm_NAME = "GHOUL_ R UpperArm";
			LForearm_NAME = "GHOUL_ L Forearm";
			RForearm_NAME = "GHOUL_ R Forearm";
			_model.GetComponent<Animation>()[_animRun].speed = 1.6f;
		}
		if (Moved)
		{
			status = "Walk";
		}
		else
		{
			status = "Idle";
		}
		_model.GetComponent<Animation>()[_animRun].wrapMode = WrapMode.Loop;
		_model.GetComponent<Animation>()[_animIdle].wrapMode = WrapMode.Loop;
		_model.GetComponent<Animation>()[_animAttack].wrapMode = WrapMode.Once;
		_model.GetComponent<Animation>()[_animJump].wrapMode = WrapMode.Once;
		_model.GetComponent<Animation>().CrossFade(_animIdle, 0.2f);
		myTransform = base.transform;
		CreateColidersWolf();
	}

	private void CreateColidersWolf()
	{
		Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren;
		foreach (Transform transform in array)
		{
			if (transform.name == Bip01_NAME)
			{
				Bip01 = transform;
				Bip01bc = Bip01.gameObject.AddComponent<BoxCollider>();
				Bip01bc.center = new Vector3(0.0113878f, 0.002480105f, 0.03862743f);
				Bip01bc.size = new Vector3(0.25f * _scaleColls, 0.3f * _scaleColls, 0.4f * _scaleColls);
				Bip01.tag = "armAnimal";
				Bip01.gameObject.layer = 21;

				Bip01.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
				Bip01.gameObject.GetComponent<InputDamage>().DamageMult = 1.5f;
			}
			else if (transform.name == LThigh_NAME)
			{
				LThigh = transform;
				LThighbc = LThigh.gameObject.AddComponent<CapsuleCollider>();
				LThighbc.center = new Vector3(-0.0828697f, 0f, 0f);
				LThighbc.radius = 0.112451494f * _scaleColls;
				LThighbc.height = 0.58840466f * _scaleColls;
				LThighbc.direction = 0;
				LThigh.tag = "armAnimal";
				LThigh.gameObject.layer = 21;

				LThigh.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
				LThigh.gameObject.GetComponent<InputDamage>().DamageMult = 1f;
			}
			else if (transform.name == RThigh_NAME)
			{
				RThigh = transform;
				RThighbc = RThigh.gameObject.AddComponent<CapsuleCollider>();
				RThighbc.center = new Vector3(-0.0828697f, 0f, 0f);
				RThighbc.radius = 0.112451494f * _scaleColls;
				RThighbc.height = 0.58840466f * _scaleColls;
				RThighbc.direction = 0;
				RThigh.tag = "armAnimal";
				RThigh.gameObject.layer = 21;

				RThigh.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
				RThigh.gameObject.GetComponent<InputDamage>().DamageMult = 1f;
			}
			else if (transform.name == LCalf_NAME)
			{
				LCalf = transform;
				LCalfbc = LCalf.gameObject.AddComponent<CapsuleCollider>();
				LCalfbc.center = new Vector3(-0.1024423f, 0f, 0f);
				LCalfbc.radius = 0.09902002f * _scaleColls;
				LCalfbc.height = 0.5795598f * _scaleColls;
				LCalfbc.direction = 0;
				LCalf.tag = "armAnimal";
				LCalf.gameObject.layer = 21;

				LCalf.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
				LCalf.gameObject.GetComponent<InputDamage>().DamageMult = 1f;
			}
			else if (transform.name == RCalf_NAME)
			{
				RCalf = transform;
				RCalfbc = RCalf.gameObject.AddComponent<CapsuleCollider>();
				RCalfbc.center = new Vector3(-0.1024423f, 0f, 0f);
				RCalfbc.radius = 0.09902002f * _scaleColls;
				RCalfbc.height = 0.5795598f * _scaleColls;
				RCalfbc.direction = 0;
				RCalf.tag = "armAnimal";
				RCalf.gameObject.layer = 21;

				RCalf.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
				RCalf.gameObject.GetComponent<InputDamage>().DamageMult = 1f;
			}
			else if (transform.name == Spine3_NAME)
			{
				Spine3 = transform;
				Spine3bc = Spine3.gameObject.AddComponent<BoxCollider>();
				Spine3bc.center = new Vector3(-0.03776952f, -0.003498547f, 1.41561E-07f);
				Spine3bc.size = new Vector3(0.3f * _scaleColls, 0.2f * _scaleColls, 0.4f * _scaleColls);
				Spine3.tag = "armAnimal";
				Spine3.gameObject.layer = 21;

				Spine3.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
				Spine3.gameObject.GetComponent<InputDamage>().DamageMult = 1.5f;
			}
			else if (transform.name == Head_NAME)
			{
				Head = transform;
				Headbc = Head.gameObject.AddComponent<SphereCollider>();
				if (_animalType == 6)
				{
					Headbc.center = new Vector3(-0.1f, 0.03f, 1.41561E-07f);
				}
				else
				{
					Headbc.center = new Vector3(0.05f, -0.04f, 1.41561E-07f);
				}
				Headbc.radius = 0.18f * _scaleColls;
				Head.tag = "armAnimal";
				Head.gameObject.layer = 21;

				Head.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
				Head.gameObject.GetComponent<InputDamage>().DamageMult = 5f;
			}
			else if (transform.name == LUpperArm_NAME)
			{
				LUpperArm = transform;
				LUpperArmbc = LUpperArm.gameObject.AddComponent<CapsuleCollider>();
				LUpperArmbc.center = new Vector3(-0.05121344f, 0f, 0f);
				LUpperArmbc.radius = 0.11060784f * _scaleColls;
				LUpperArmbc.height = 0.30728072f * _scaleColls;
				LUpperArmbc.direction = 0;
				LUpperArm.tag = "armAnimal";
				LUpperArm.gameObject.layer = 21;

				LUpperArm.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
				LUpperArm.gameObject.GetComponent<InputDamage>().DamageMult = 1f;
			}
			else if (transform.name == RUpperArm_NAME)
			{
				RUpperArm = transform;
				RUpperArmbc = RUpperArm.gameObject.AddComponent<CapsuleCollider>();
				RUpperArmbc.center = new Vector3(-0.05121344f, 0f, 0f);
				RUpperArmbc.radius = 0.11060784f * _scaleColls;
				RUpperArmbc.height = 0.30728072f * _scaleColls;
				RUpperArmbc.direction = 0;
				RUpperArm.tag = "armAnimal";
				RUpperArm.gameObject.layer = 21;

				RUpperArm.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
				RUpperArm.gameObject.GetComponent<InputDamage>().DamageMult = 1f;
			}
			else if (transform.name == LForearm_NAME)
			{
				LForearm = transform;
				LForearmbc = LForearm.gameObject.AddComponent<CapsuleCollider>();
				LForearmbc.center = new Vector3(-0.05f, 0f, 0f);
				LForearmbc.radius = 0.078f * _scaleColls;
				LForearmbc.height = 0.39f * _scaleColls;
				LForearmbc.direction = 0;
				LForearm.tag = "armAnimal";
				LForearm.gameObject.layer = 21;

				LForearm.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
				LForearm.gameObject.GetComponent<InputDamage>().DamageMult = 1f;
			}
			else if (transform.name == RForearm_NAME)
			{
				RForearm = transform;
				RForearmbc = RForearm.gameObject.AddComponent<CapsuleCollider>();
				RForearmbc.center = new Vector3(-0.05f, 0f, 0f);
				RForearmbc.radius = 0.078f * _scaleColls;
				RForearmbc.height = 0.39f * _scaleColls;
				RForearmbc.direction = 0;
				RForearm.tag = "armAnimal";
				RForearm.gameObject.layer = 21;

				RForearm.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
				RForearm.gameObject.GetComponent<InputDamage>().DamageMult = 1f;
			}
		}
		if (!Bip01.GetComponent<Rigidbody>())
		{
			Bip01rb = Bip01.gameObject.AddComponent<Rigidbody>();
			Bip01rb.mass = 0.3125f;
			LThighrb = LThigh.gameObject.AddComponent<Rigidbody>();
			LThighrb.mass = 0.1875f;
			LThighhj = LThigh.gameObject.AddComponent<CharacterJoint>();
			LThighhj.connectedBody = Bip01rb;
			LThighhj.axis = new Vector3(0f, -1f, 0f);
			LThighhj.swingAxis = new Vector3(0f, 0f, 1f);
			RThighrb = RThigh.gameObject.AddComponent<Rigidbody>();
			RThighrb.mass = 0.1875f;
			RThighhj = RThigh.gameObject.AddComponent<CharacterJoint>();
			RThighhj.connectedBody = Bip01rb;
			RThighhj.axis = new Vector3(0f, -1f, 0f);
			RThighhj.swingAxis = new Vector3(0f, 0f, 1f);
			LCalfrb = LCalf.gameObject.AddComponent<Rigidbody>();
			LCalfrb.mass = 0.1875f;
			LCalfhj = LCalf.gameObject.AddComponent<CharacterJoint>();
			LCalfhj.connectedBody = LThighrb;
			LCalfhj.axis = new Vector3(0f, -1f, 0f);
			LCalfhj.swingAxis = new Vector3(0f, 0f, 1f);
			RCalfrb = RCalf.gameObject.AddComponent<Rigidbody>();
			RCalfrb.mass = 0.1875f;
			RCalfhj = RCalf.gameObject.AddComponent<CharacterJoint>();
			RCalfhj.connectedBody = RThighrb;
			RCalfhj.axis = new Vector3(0f, -1f, 0f);
			RCalfhj.swingAxis = new Vector3(0f, 0f, 1f);
			Spine3rb = Spine3.gameObject.AddComponent<Rigidbody>();
			Spine3rb.mass = 0.3125f;
			Spine3hj = Spine3.gameObject.AddComponent<CharacterJoint>();
			Spine3hj.connectedBody = Bip01rb;
			Spine3hj.axis = new Vector3(0f, -1f, 0f);
			Spine3hj.swingAxis = new Vector3(0f, 0f, -1f);
			Headrb = Head.gameObject.AddComponent<Rigidbody>();
			Headrb.mass = 0.125f;
			Headhj = Head.gameObject.AddComponent<CharacterJoint>();
			Headhj.connectedBody = Spine3rb;
			Headhj.axis = new Vector3(0f, -1f, 0f);
			Headhj.swingAxis = new Vector3(0f, 0f, -1f);
			LUpperArmrb = LUpperArm.gameObject.AddComponent<Rigidbody>();
			LUpperArmrb.mass = 0.125f;
			LUpperArmhj = LUpperArm.gameObject.AddComponent<CharacterJoint>();
			LUpperArmhj.connectedBody = Spine3rb;
			LUpperArmhj.axis = new Vector3(0f, 0f, 1f);
			LUpperArmhj.swingAxis = new Vector3(1f, 0f, 0f);
			RUpperArmrb = RUpperArm.gameObject.AddComponent<Rigidbody>();
			RUpperArmrb.mass = 0.125f;
			RUpperArmhj = RUpperArm.gameObject.AddComponent<CharacterJoint>();
			RUpperArmhj.connectedBody = Spine3rb;
			RUpperArmhj.axis = new Vector3(0f, 0f, 1f);
			RUpperArmhj.swingAxis = new Vector3(1f, 0f, 0f);
			LForearmrb = LForearm.gameObject.AddComponent<Rigidbody>();
			LForearmrb.mass = 0.125f;
			LForearmhj = LForearm.gameObject.AddComponent<CharacterJoint>();
			LForearmhj.connectedBody = LUpperArmrb;
			LForearmhj.axis = new Vector3(1f, 0f, 0f);
			LForearmhj.swingAxis = new Vector3(0f, 0f, 1f);
			RForearmrb = RForearm.gameObject.AddComponent<Rigidbody>();
			RForearmrb.mass = 0.125f;
			RForearmhj = RForearm.gameObject.AddComponent<CharacterJoint>();
			RForearmhj.connectedBody = RUpperArmrb;
			RForearmhj.axis = new Vector3(1f, 0f, 0f);
			RForearmhj.swingAxis = new Vector3(0f, 0f, 1f);
			Bip01rb.GetComponent<Rigidbody>().isKinematic = true;
			LThighrb.GetComponent<Rigidbody>().isKinematic = true;
			RThighrb.GetComponent<Rigidbody>().isKinematic = true;
			LCalfrb.GetComponent<Rigidbody>().isKinematic = true;
			RCalfrb.GetComponent<Rigidbody>().isKinematic = true;
			Spine3rb.GetComponent<Rigidbody>().isKinematic = true;
			Headrb.GetComponent<Rigidbody>().isKinematic = true;
			LUpperArmrb.GetComponent<Rigidbody>().isKinematic = true;
			RUpperArmrb.GetComponent<Rigidbody>().isKinematic = true;
			LForearmrb.GetComponent<Rigidbody>().isKinematic = true;
			RForearmrb.GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	private void ApplyDamage(float _damage)
	{
		if (!base.GetComponent<AudioSource>().isPlaying)
		{
			base.GetComponent<AudioSource>().clip = snd_dead[Random.Range(0, snd_dead.Length)];
			base.GetComponent<AudioSource>().Play();
		}
		AttackRadius = 20000f;
		MaxAttackRadius = 25000f;
		HP -= _damage;
		if (HP <= 0f)
		{
			Death();
		}
	}

	private void CreateFood()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("_ITEM_BS"), base.transform.position, Quaternion.identity);
		if (_animalType >= 1 && _animalType <= 3)
		{
			gameObject.SendMessage("ShowItem", 56);
			gameObject.GetComponent<ItemPickup>().ITEM_COUNT = Random.Range(50, 100);
			gameObject.GetComponent<ItemPickup>().isLoot = false;
		}
		else
		{
			gameObject.GetComponent<ItemPickup>().lootType = 0;
			gameObject.GetComponent<ItemPickup>().inSceneObj = false;
			gameObject.GetComponent<ItemPickup>().isLoot = false;
			gameObject.SendMessage("GenerateLoot");
		}
		gameObject.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
		gameObject.GetComponent<Rigidbody>().useGravity = true;
		gameObject.GetComponent<Rigidbody>().isKinematic = false;
		gameObject.GetComponent<Rigidbody>().angularVelocity = base.transform.TransformDirection(new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
		gameObject.name = "ITEM: " + base.name + ". Meat";
		if (BaseLogic.GAME_MODE == 6)
		{
			Object.Destroy(gameObject, 60f);
		}
		else
		{
			Object.Destroy(gameObject, 1200f);
		}
	}

	private void Death()
	{
		if (!isDead)
		{
			if ((bool)GetComponent<MissionObject>())
			{
				SendMessage("PickUpItem");
			}
			CreateFood();
			controller.enabled = false;
			_model.GetComponent<Animation>().Stop();
			_model.GetComponent<Animation>().enabled = false;
			isDead = true;
			base.GetComponent<AudioSource>().maxDistance = 35f;
			base.GetComponent<AudioSource>().clip = snd_dead[Random.Range(0, snd_dead.Length)];
			base.GetComponent<AudioSource>().volume = 0.6f;
			base.GetComponent<AudioSource>().Play();
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("DeadAnimal", PhotonTargets.Others, base.gameObject.name);
			}
			RagdollOn();
			if (BaseLogic.GAME_MODE == 6)
			{
				BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY + 50);
				AudioSource.PlayClipAtPoint(BaseLogic._player.GetComponent<BaseLogic>().KASSA, base.transform.position, 0.4f);
			}
		}
	}

	private void Death2()
	{
		if (!isDead)
		{
			if ((bool)GetComponent<MissionObject>())
			{
				SendMessage("PickUpItem");
			}
			CreateFood();
			controller.enabled = false;
			_model.GetComponent<Animation>().Stop();
			_model.GetComponent<Animation>().enabled = false;
			isDead = true;
			base.GetComponent<AudioSource>().maxDistance = 35f;
			base.GetComponent<AudioSource>().clip = snd_dead[Random.Range(0, snd_dead.Length)];
			base.GetComponent<AudioSource>().volume = 0.6f;
			base.GetComponent<AudioSource>().Play();
			RagdollOn();
		}
	}

	private void RagdollOn()
	{
		Bip01rb.GetComponent<Rigidbody>().isKinematic = false;
		LThighrb.GetComponent<Rigidbody>().isKinematic = false;
		RThighrb.GetComponent<Rigidbody>().isKinematic = false;
		LCalfrb.GetComponent<Rigidbody>().isKinematic = false;
		RCalfrb.GetComponent<Rigidbody>().isKinematic = false;
		Spine3rb.GetComponent<Rigidbody>().isKinematic = false;
		Headrb.GetComponent<Rigidbody>().isKinematic = false;
		LUpperArmrb.GetComponent<Rigidbody>().isKinematic = false;
		RUpperArmrb.GetComponent<Rigidbody>().isKinematic = false;
		LForearmrb.GetComponent<Rigidbody>().isKinematic = false;
		RForearmrb.GetComponent<Rigidbody>().isKinematic = false;
		Bip01.gameObject.layer = 26;
		LThigh.gameObject.layer = 26;
		RThigh.gameObject.layer = 26;
		LCalf.gameObject.layer = 26;
		RCalf.gameObject.layer = 26;
		Spine3.gameObject.layer = 26;
		Head.gameObject.layer = 26;
		LUpperArm.gameObject.layer = 26;
		RUpperArm.gameObject.layer = 26;
		LForearm.gameObject.layer = 26;
		RForearm.gameObject.layer = 26;
		Bip01rb.AddForceAtPosition(10f * base.transform.TransformDirection(Vector3.forward) / (Time.fixedDeltaTime * 100f), base.transform.position);
		Object.Destroy(base.gameObject, 10f);
	}

	private void RotateTowards(Vector3 position)
	{
		Vector3 forward = position - myTransform.position;
		forward.y = 0f;
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(forward), 5f * Time.deltaTime * 8f);
		myTransform.eulerAngles = new Vector3(0f, myTransform.eulerAngles.y, 0f);
	}

	private void MoveTowards(Vector3 position)
	{
		Vector3 forward = position - myTransform.position;
		forward.y = 0f;
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(forward), (float)RotationSpeed * Time.deltaTime);
		myTransform.eulerAngles = new Vector3(0f, myTransform.eulerAngles.y, 0f);
		Vector3 vector = myTransform.TransformDirection(Vector3.forward);
		float value = Vector3.Dot(vector, forward.normalized);
		value = Mathf.Clamp01(value);
		forward = ((!(status == "Walk")) ? (vector * MoveSpeed) : (vector * MoveSpeed / 2f));
		controller.SimpleMove(forward);
	}

	public void Jump()
	{
		if ((_animalType == 5 || _animalType == 6) && controller.isGrounded)
		{
			_model.GetComponent<Animation>().CrossFade(_animJump, 0.2f);
			moveDirection.y = 10f;
		}
	}

	private void Update()
	{
		if (isDead)
		{
			return;
		}
		if (Time.time > _lodSysTimer + 10f)
		{
			float num = Vector3.Distance(Camera.main.transform.position, base.transform.position);
			if (num < 150f)
			{
				SMD_Root.SetActive(true);
				_lodSys = false;
			}
			else
			{
				SMD_Root.SetActive(false);
				_lodSys = true;
			}
			_lodSysTimer = Time.time + Random.Range(1f, 3f);
		}
		if (_lodSys)
		{
			return;
		}
		if (Time.time > _updateTimer + 1.5f && target == null)
		{
			float sqrMagnitude = (Camera.main.transform.position - myTransform.position).sqrMagnitude;
			if (sqrMagnitude < AttackRadius)
			{
				RaycastHit hitInfo;
				if ((bool)Network.PLAYER && Network.PLAYER.GetComponent<HeroBase>().hitPointsReal > 0 && Network.PLAYER.GetComponent<HeroBase>().VEHICLE == null && Physics.Linecast(Head.position, Camera.main.transform.position, out hitInfo, layerSet) && hitInfo.collider.tag == "Player")
				{
					controller.enabled = true;
					base.GetComponent<AudioSource>().maxDistance = 35f;
					base.GetComponent<AudioSource>().clip = snd_attack;
					base.GetComponent<AudioSource>().volume = 0.6f;
					base.GetComponent<AudioSource>().Play();
					Network.PLAYER.GetComponent<PhotonView>().RPC("SetTargetAnimal", PhotonTargets.Others, string.Empty + base.name + ":" + base.transform.position.x + ":" + base.transform.position.y + ":" + base.transform.position.z + ":" + _animalType);
					target = Network.PLAYER;
					Jump();
				}
			}
			else
			{
				controller.enabled = false;
			}
			_updateTimer = Time.time;
		}
		else if ((bool)target && Time.time > _updateTimer + 2f)
		{
			RaycastHit hitInfo2;
			if ((bool)Network.PLAYER && target == Network.PLAYER && Physics.Linecast(Head.position, Camera.main.transform.position, out hitInfo2, layerSet) && !(hitInfo2.collider.tag == "Player"))
			{
				target = null;
				Network.PLAYER.GetComponent<PhotonView>().RPC("SetTargetAnimalNull", PhotonTargets.Others, string.Empty + base.gameObject.name);
				status = "Idle";
				_model.GetComponent<Animation>().CrossFade(_animIdle2, 0.2f);
			}
			_updateTimer = Time.time;
		}
		if (!isDead && (bool)target)
		{
			if (target.GetComponent<HeroBase>().hitPointsReal <= 0 || (bool)target.GetComponent<HeroBase>().VEHICLE)
			{
				status = "Idle";
				target = null;
				_model.GetComponent<Animation>().CrossFade(_animIdle2, 0.2f);
				Network.PLAYER.GetComponent<PhotonView>().RPC("SetTargetAnimalNull", PhotonTargets.Others, string.Empty + base.gameObject.name);
				return;
			}
			float sqrMagnitude2 = (target.transform.position - myTransform.position).sqrMagnitude;
			if (_animalType != 4)
			{
				if (sqrMagnitude2 <= AttackDistance * 1.25f)
				{
					if (status != "Attack")
					{
						status = "Attack";
					}
				}
				else if (sqrMagnitude2 < AttackRadius && status != "Run")
				{
					status = "Run";
				}
			}
			else if (status != "Alarm" && target.GetComponent<HeroBase>().hitPointsReal > 0 && sqrMagnitude2 < AttackRadius)
			{
				status = "Alarm";
			}
			if (status == "Run" && !_model.GetComponent<Animation>().IsPlaying(_animAttack))
			{
				if (!_model.GetComponent<Animation>().IsPlaying(_animRun) && !_model.GetComponent<Animation>().IsPlaying(_animJump))
				{
					_model.GetComponent<Animation>().Play(_animRun, PlayMode.StopAll);
				}
				RotateTowards(target.transform.position);
				MoveTowards(target.transform.position);
				if (moveDirection.y > 0f)
				{
					moveDirection.y -= 20f * Time.deltaTime;
					controller.Move(moveDirection * Time.deltaTime);
				}
			}
			else if (status == "Alarm")
			{
				if (!_model.GetComponent<Animation>().IsPlaying(_animRun))
				{
					_model.GetComponent<Animation>().Play(_animRun, PlayMode.StopAll);
				}
				RotateTowards(target.transform.position);
				MoveTowards(target.transform.position);
			}
			else if (status == "Idle")
			{
				if (!_model.GetComponent<Animation>().IsPlaying(_animIdle))
				{
					_model.GetComponent<Animation>().CrossFade(_animIdle2, 0.2f);
				}
			}
			else
			{
				if (!(status == "Attack") || !(Time.time > _attackTimer + 2f))
				{
					return;
				}
				if (target.GetComponent<HeroBase>().hitPointsReal > 0)
				{
					RaycastHit hitInfo3;
					if (!_model.GetComponent<Animation>().IsPlaying(_animAttack) && (bool)Network.PLAYER && target == Network.PLAYER && Physics.Linecast(Head.position, Camera.main.transform.position, out hitInfo3, layerSet))
					{
						BaseLogic._player.SendMessage("ApplyDamage", damage);
						base.GetComponent<AudioSource>().maxDistance = 35f;
						base.GetComponent<AudioSource>().clip = snd_attack;
						base.GetComponent<AudioSource>().volume = 0.6f;
						base.GetComponent<AudioSource>().Play();
						_model.GetComponent<Animation>().CrossFade(_animAttack, 0.2f);
					}
				}
				else
				{
					status = "Idle";
					target = null;
					_model.GetComponent<Animation>().CrossFade(_animIdle2, 0.2f);
					Network.PLAYER.GetComponent<PhotonView>().RPC("SetTargetAnimalNull", PhotonTargets.Others, string.Empty + base.gameObject.name);
				}
				_attackTimer = Time.time;
			}
		}
		else
		{
			if (!(status == "Walk"))
			{
				return;
			}
			if (!MovedTarget)
			{
				if (!(Time.time > targetSearchMovedTimer + 5f))
				{
					return;
				}
				if ((bool)Network.PLAYER && PhotonNetwork.isMasterClient)
				{
					Object[] array = Object.FindObjectsOfType(typeof(HeroBase));
					if (array.Length != 0)
					{
						int num2 = Random.Range(0, array.Length);
						Object[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							HeroBase heroBase = (HeroBase)array2[i];
							if (heroBase == array[num2])
							{
								Debug.Log("Mob Search ID: " + heroBase.GetComponent<PhotonView>().ownerId);
								MovedTarget = heroBase.transform;
								heroBase.GetComponent<PhotonView>().RPC("SetTargetAnimalSearch", PhotonTargets.Others, string.Empty + base.gameObject.name);
								break;
							}
						}
					}
				}
				targetSearchMovedTimer = Time.time + (float)Random.Range(1, 10);
			}
			else
			{
				if (!_model.GetComponent<Animation>().IsPlaying(_animWalk))
				{
					_model.GetComponent<Animation>().CrossFade(_animWalk, 0.2f);
				}
				RotateTowards(MovedTarget.position);
				MoveTowards(MovedTarget.position);
			}
		}
	}
}
