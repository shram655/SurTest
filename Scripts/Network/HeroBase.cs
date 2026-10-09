using System;
using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine;

public class HeroBase : MonoBehaviour
{
	private Vector3[] playersPos;

	private PhotonView[] playersPhoton;

	private int Killerid = -1;

	public Renderer[] BODY_ARMORS;

	private int bodyArmor;

	public int bodyArmorCoef = 10;

	public int SCHET;

	public int SIDE;

	public float _updateMobAttackRPC;

	public float headArmor = 1f;

	public AudioClip hahaha;

	public GameObject[] TRACERS;

	public bool BOT;

	public int BOT_ANIM;

	public AudioClip hit_arm;

	public AudioClip hit_ground;

	public AudioClip hit_metal;

	public AudioClip hit_water;

	public AudioClip hit_wood;

	public bool _godMode;

	private float _godModeTimer;

	public GameObject VEHICLE;

	public GameObject HOLE;

	public bool isMyHero;

	private float _updateTimer;

	private GameObject[] SPAWN = new GameObject[40];

	public float respawnTimer;

	private GameObject blood;

	private GameObject slowSmokeParticles;

	private GameObject fastSmokeParticles;

	private GameObject water2;

	public int hitPointsReal;

	private int hitPointsProtected;

	public GameObject SMD_Import;

	public GameObject SMD_Root;

	public PhotonView photonView;

	private bool _lodSys;

	public float _lodSysTimer;

	public float _distanceToHero;

	public Material[] CLOTH;

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

	private Transform Spine3a;

	public BoxCollider Spine3abc;

	private Rigidbody Spine3arb;

	private CharacterJoint Spine3ahj;

	private Transform Spine3b;

	public BoxCollider Spine3bbc;

	private Rigidbody Spine3brb;

	private CharacterJoint Spine3bhj;

	private Transform Spine3c;

	public BoxCollider Spine3cbc;

	private Rigidbody Spine3crb;

	private CharacterJoint Spine3chj;

	public Transform Head;

	public CapsuleCollider Headbc;

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

	public bool _syncDead;

	public Animator animator;

	public GameObject ITEM;

	private Transform Hand;

	public GameObject WEAPONS_HANDS_OBJ;

	public GameObject WEAPONS_PISTOL_OBJ;

	public GameObject WEAPONS_RIFLE_OBJ;

	private GameObject BACKPACK_OBJ;

	private GameObject HELMET_OBJ;

	private GameObject GLASSES_OBJ;

	public GameObject BACKPACK_ITEM;

	public GameObject HELMET_ITEM;

	public GameObject CLOTH_ITEM;

	public GameObject[] WEAPONS;

	public int SEX = 1;

	public int BOT_WEAPON;

	private float autorizeMasterTimer;

	private void SetDefaultParent()
	{
		GameObject gameObject = GameObject.Find("_BASEPARENTSYNC");
		base.transform.parent = gameObject.transform;
	}

	private void Awake()
	{
		photonView = GetComponent<PhotonView>();
		animator = GetComponent<Animator>();
		animator.logWarnings = false;

		if (BOT)
		{
			SetHitPoints(75);
			GetComponent<SoldierAI>().enabled = false;
		}
		else
		{
			SetHitPoints(100);
		}
		if (!isMyHero)
		{
			for (int i = 0; i < 40; i++)
			{
				SPAWN[i] = GameObject.Find("SPWAN" + i);
			}
			if (!BOT && !photonView.isMine)
			{
				GetComponent<Heroserialize>()._noSlowPolet = true;
				GetComponent<Heroserialize>()._noSlowPoletTimer = Time.time;
				SetDefaultParent();
			}
		}
	}

	public void SetHitPoints(int hp)
	{
		hitPointsReal = hp;
		hitPointsProtected = hp * 567;
		if (BOT && (bool)BaseLogic._player)
		{
			GetComponent<SoldierAI>().fireTarget = BaseLogic._player.transform;
			GetComponent<SoldierAI>().Attack();
		}
	}

	private void Start()
	{
		
		_godMode = true;
		_godModeTimer = Time.time;
		if (isMyHero || BOT)
		{
			CreateStandartBones();
			CollisionOff();
			CreateCustomize();
			if (BOT)
			{
				base.GetComponent<Collider>().enabled = false;
				CreateColiders();
				CreateRigidbodys();
				SetHelmetLocal(UnityEngine.Random.Range(1, 15));
				SetGlassesLocal(UnityEngine.Random.Range(0, 1));
				SetBackpackLocal(UnityEngine.Random.Range(1, 8));
			}
			else
			{
				HELMET_OBJ.GetComponent<Helmet>().isMyHero = true;
				BACKPACK_OBJ.GetComponent<BackPack>().isMyHero = true;
				WEAPONS_RIFLE_OBJ.GetComponent<WeaponsBack>().isMyHero = true;
				WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>().isMyHero = true;
				WEAPONS_PISTOL_OBJ.GetComponent<WeaponsBack>().isMyHero = true;
				GLASSES_OBJ.GetComponent<Glasses>().isMyHero = true;
			}
			return;
		}
		base.GetComponent<Collider>().enabled = false;
		blood = GameObject.Find("EF_Blood");
		slowSmokeParticles = GameObject.Find("EF_SlowSmoke");
		fastSmokeParticles = GameObject.Find("EF_FastSmoke");
		water2 = GameObject.Find("EF_Water2");
		if (photonView.isMine)
		{
			CreateStandartBones();
			CreateCustomize();
			SetCustomizeView(false);
			photonView.RPC("SetHeroItems", PhotonTargets.Others, BaseLogic._helmet, BaseLogic._glasses, BaseLogic._backpack, BaseLogic._cloth, BaseLogic._bodyArmorReal);
			SetHelmetLocal(BaseLogic._helmet);
			SetGlassesLocal(BaseLogic._glasses);
			SetBackpackLocal(BaseLogic._backpack);
			SetClothLocal(BaseLogic._cloth);
			SetBodyArmorLocal(BaseLogic._bodyArmorReal);
			if (BaseLogic._weaponSlot1_Real > 0)
			{
				photonView.RPC("SetWeapons", PhotonTargets.Others, new Quaternion(BaseLogic._weaponSlot1_Real, BaseLogic._weaponSlot2_Real, BaseLogic._weaponMod1, BaseLogic._weaponMod2));
				SetWeaponsLocal(new Quaternion(BaseLogic._weaponSlot1_Real, BaseLogic._weaponSlot2_Real, BaseLogic._weaponMod1, BaseLogic._weaponMod2));
			}
		}
		else
		{
			CreateStandartBones();
			CreateColiders();
			CreateRigidbodys();
			CollisionOff();
			RigidbodysOff();
			CreateHingeJoints();
			CreateCustomize();
		}
	}

	public void RigidbodysOn()
	{
		Bip01rb.isKinematic = false;
		LThighrb.isKinematic = false;
		RThighrb.isKinematic = false;
		LCalfrb.isKinematic = false;
		RCalfrb.isKinematic = false;
		Spine3rb.isKinematic = false;
		Spine3arb.isKinematic = false;
		Spine3brb.isKinematic = false;
		Spine3crb.isKinematic = false;
		Headrb.isKinematic = false;
		LUpperArmrb.isKinematic = false;
		RUpperArmrb.isKinematic = false;
		LForearmrb.isKinematic = false;
		RForearmrb.isKinematic = false;
	}

	public void RigidbodysOff()
	{
		Bip01rb.isKinematic = true;
		LThighrb.isKinematic = true;
		RThighrb.isKinematic = true;
		LCalfrb.isKinematic = true;
		RCalfrb.isKinematic = true;
		Spine3rb.isKinematic = true;
		Spine3arb.isKinematic = true;
		Spine3brb.isKinematic = true;
		Spine3crb.isKinematic = true;
		Headrb.isKinematic = true;
		LUpperArmrb.isKinematic = true;
		RUpperArmrb.isKinematic = true;
		LForearmrb.isKinematic = true;
		RForearmrb.isKinematic = true;
	}

	private void CreateCustomize()
	{
		WEAPONS_HANDS_OBJ = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("WEAPON_HANDS"));
		WEAPONS_HANDS_OBJ.transform.parent = Hand;
		WEAPONS_HANDS_OBJ.transform.localPosition = new Vector3(-0.1068641f, -0.01960563f, -0.02239574f);
		WEAPONS_HANDS_OBJ.transform.localEulerAngles = new Vector3(275.8742f, 319.5004f, 143.3918f);
		WEAPONS_HANDS_OBJ.transform.localScale = new Vector3(0.4007752f, 0.4007752f, 0.4007752f);
		WEAPONS_PISTOL_OBJ = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("WEAPON_PISTOL"));
		WEAPONS_PISTOL_OBJ.transform.parent = RThigh;
		WEAPONS_PISTOL_OBJ.transform.localPosition = new Vector3(-0.04360104f, 0.0009934211f, -0.04420786f);
		WEAPONS_PISTOL_OBJ.transform.localEulerAngles = new Vector3(272.9701f, 270.2361f, 0.06251397f);
		WEAPONS_PISTOL_OBJ.transform.localScale = new Vector3(0.4007752f, 0.4007752f, 0.4007752f);
		WEAPONS_RIFLE_OBJ = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("WEAPON_RIFLE"));
		WEAPONS_RIFLE_OBJ.transform.parent = Spine3;
		WEAPONS_RIFLE_OBJ.transform.localPosition = new Vector3(0.04459139f, -0.02350886f, -0.04150217f);
		WEAPONS_RIFLE_OBJ.transform.localEulerAngles = new Vector3(73.11497f, 145.1575f, 233.9649f);
		WEAPONS_RIFLE_OBJ.transform.localScale = new Vector3(0.4004524f, 0.4004524f, 0.4004524f);
		BACKPACK_OBJ = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("BACKPACK"));
		BACKPACK_OBJ.transform.parent = Spine3;
		BACKPACK_OBJ.transform.localPosition = new Vector3(0f, 0f, 0f);
		BACKPACK_OBJ.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		BACKPACK_OBJ.transform.localScale = new Vector3(1f, 1f, 1f);
		HELMET_OBJ = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("HELMETS"));
		HELMET_OBJ.transform.parent = Head;
		if (SEX == 1)
		{
			HELMET_OBJ.transform.localPosition = new Vector3(0f, 0f, 0f);
		}
		else
		{
			HELMET_OBJ.transform.localPosition = new Vector3(0.01157268f, 0.01786397f, -6.951908E-06f);
		}
		HELMET_OBJ.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		HELMET_OBJ.transform.localScale = new Vector3(1f, 1f, 1f);
		GLASSES_OBJ = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("GLASSES"));
		GLASSES_OBJ.transform.parent = Head;
		if (SEX == 1)
		{
			GLASSES_OBJ.transform.localPosition = new Vector3(-0.04706766f, -0.03659685f, 0.0001805761f);
		}
		else
		{
			GLASSES_OBJ.transform.localPosition = new Vector3(0.01157268f, 0.01786397f, -6.951908E-06f);
		}
		GLASSES_OBJ.transform.localEulerAngles = new Vector3(80.10016f, -90.00076f, -0.0007019043f);
		GLASSES_OBJ.transform.localScale = new Vector3(0.3333333f, 0.3333333f, 0.3333333f);
	}

	private void CreateStandartBones()
	{
		Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren;
		foreach (Transform transform in array)
		{
			if (transform.name == "ValveBiped.Bip01_Pelvis")
			{
				Bip01 = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_L_Thigh")
			{
				LThigh = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_R_Thigh")
			{
				RThigh = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_L_Calf")
			{
				LCalf = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_R_Calf")
			{
				RCalf = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_Spine4")
			{
				Spine3 = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_Spine")
			{
				Spine3a = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_Spine1")
			{
				Spine3b = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_Spine2")
			{
				Spine3c = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_Head1")
			{
				Head = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_L_UpperArm")
			{
				LUpperArm = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_R_UpperArm")
			{
				RUpperArm = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_L_Forearm")
			{
				LForearm = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_R_Forearm")
			{
				RForearm = transform;
			}
			else if (transform.name == "ValveBiped.Bip01_R_Hand")
			{
				Hand = transform;
			}
		}
	}

	private void CreateColiders()
	{
		Bip01bc = Bip01.gameObject.AddComponent<BoxCollider>();
		Bip01bc.center = new Vector3(0.001790131f, 0.06835206f, -0.008312285f);
		Bip01bc.size = new Vector3(0.139351f, 0.1112832f, 0.08708351f);
		Bip01.tag = "arm";
		Bip01.gameObject.layer = 21;

		Bip01.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		Bip01.gameObject.GetComponent<InputDamage>().DamageMult = 1f;

		LThighbc = LThigh.gameObject.AddComponent<CapsuleCollider>();
		LThighbc.center = new Vector3(-0.0828697f, 0f, 0f);
		LThighbc.radius = 0.03748383f;
		LThighbc.height = 0.1961349f;
		LThighbc.direction = 0;
		LThigh.tag = "arm";
		LThigh.gameObject.layer = 21;

		LThigh.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		LThigh.gameObject.GetComponent<InputDamage>().DamageMult = 0.85f;

		RThighbc = RThigh.gameObject.AddComponent<CapsuleCollider>();
		RThighbc.center = new Vector3(-0.0828697f, 0f, 0f);
		RThighbc.radius = 0.03748383f;
		RThighbc.height = 0.1961349f;
		RThighbc.direction = 0;
		RThigh.tag = "arm";
		RThigh.gameObject.layer = 21;

		RThigh.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		RThigh.gameObject.GetComponent<InputDamage>().DamageMult = 0.85f;

		LCalfbc = LCalf.gameObject.AddComponent<CapsuleCollider>();
		LCalfbc.center = new Vector3(-0.1024423f, 0f, 0f);
		LCalfbc.radius = 0.03300667f;
		LCalfbc.height = 0.1931866f;
		LCalfbc.direction = 0;
		LCalf.tag = "arm";
		LCalf.gameObject.layer = 21;

		LCalf.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		LCalf.gameObject.GetComponent<InputDamage>().DamageMult = 0.8f;

		RCalfbc = RCalf.gameObject.AddComponent<CapsuleCollider>();
		RCalfbc.center = new Vector3(-0.1024423f, 0f, 0f);
		RCalfbc.radius = 0.03300667f;
		RCalfbc.height = 0.1931866f;
		RCalfbc.direction = 0;
		RCalf.tag = "arm";
		RCalf.gameObject.layer = 21;

		RCalf.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		RCalf.gameObject.GetComponent<InputDamage>().DamageMult = 0.8f;

		Spine3bc = Spine3.gameObject.AddComponent<BoxCollider>();
		Spine3bc.center = new Vector3(0f, 0.03f, 0f);
		Spine3bc.size = new Vector3(0.1f, 0.1f, 0.14f);
		Spine3.tag = "arm";
		Spine3.gameObject.layer = 21;

		Spine3.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		Spine3.gameObject.GetComponent<InputDamage>().DamageMult = 0.85f;

		Spine3abc = Spine3a.gameObject.AddComponent<BoxCollider>();
		Spine3abc.center = new Vector3(0.022f, 0.025f, 0f);
		Spine3abc.size = new Vector3(0.08f, 0.1f, 0.14f);
		Spine3a.tag = "arm";
		Spine3a.gameObject.layer = 21;

		Spine3a.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		Spine3a.gameObject.GetComponent<InputDamage>().DamageMult = 0.85f;

		Spine3bbc = Spine3b.gameObject.AddComponent<BoxCollider>();
		Spine3bbc.center = new Vector3(0f, 0.03f, 0f);
		Spine3bbc.size = new Vector3(0.04f, 0.1f, 0.14f);
		Spine3b.tag = "arm";
		Spine3b.gameObject.layer = 21;

		Spine3b.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		Spine3b.gameObject.GetComponent<InputDamage>().DamageMult = 0.85f;

		Spine3cbc = Spine3c.gameObject.AddComponent<BoxCollider>();
		Spine3cbc.center = new Vector3(-0.015f, 0.03f, 0f);
		Spine3cbc.size = new Vector3(0.04f, 0.1f, 0.14f);
		Spine3c.tag = "arm";
		Spine3c.gameObject.layer = 21;

		Spine3c.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		Spine3c.gameObject.GetComponent<InputDamage>().DamageMult = 1f;

		Headbc = Head.gameObject.AddComponent<CapsuleCollider>();
		Headbc.center = new Vector3(-0.045f, -0.01f, 0f);
		Headbc.radius = 0.043f;
		Headbc.height = 0.12f;
		Headbc.direction = 0;
		Head.tag = "arm";
		Head.gameObject.layer = 21;

		Head.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		Head.gameObject.GetComponent<InputDamage>().DamageMult = 4f;

		LUpperArmbc = LUpperArm.gameObject.AddComponent<CapsuleCollider>();
		LUpperArmbc.center = new Vector3(-0.05121344f, 0f, 0f);
		LUpperArmbc.radius = 0.03686928f;
		LUpperArmbc.height = 0.1024269f;
		LUpperArmbc.direction = 0;
		LUpperArm.tag = "arm";
		LUpperArm.gameObject.layer = 21;

		LUpperArm.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		LUpperArm.gameObject.GetComponent<InputDamage>().DamageMult = 0.7f;

		RUpperArmbc = RUpperArm.gameObject.AddComponent<CapsuleCollider>();
		RUpperArmbc.center = new Vector3(-0.05121344f, 0f, 0f);
		RUpperArmbc.radius = 0.03686928f;
		RUpperArmbc.height = 0.1024269f;
		RUpperArmbc.direction = 0;
		RUpperArm.tag = "arm";
		RUpperArm.gameObject.layer = 21;

		RUpperArm.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		RUpperArm.gameObject.GetComponent<InputDamage>().DamageMult = 0.7f;

		LForearmbc = LForearm.gameObject.AddComponent<CapsuleCollider>();
		LForearmbc.center = new Vector3(-0.05f, 0f, 0f);
		LForearmbc.radius = 0.026f;
		LForearmbc.height = 0.13f;
		LForearmbc.direction = 0;
		LForearm.tag = "arm";
		LForearm.gameObject.layer = 21;

		LForearm.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		LForearm.gameObject.GetComponent<InputDamage>().DamageMult = 0.7f;

		RForearmbc = RForearm.gameObject.AddComponent<CapsuleCollider>();
		RForearmbc.center = new Vector3(-0.05f, 0f, 0f);
		RForearmbc.radius = 0.026f;
		RForearmbc.height = 0.13f;
		RForearmbc.direction = 0;
		RForearm.tag = "arm";
		RForearm.gameObject.layer = 21;

		RForearm.gameObject.AddComponent<InputDamage>().HeroBase = base.gameObject;
		RForearm.gameObject.GetComponent<InputDamage>().DamageMult = 0.7f;
	}

	private void CreateRigidbodys()
	{
		Bip01rb = Bip01.gameObject.AddComponent<Rigidbody>();
		Bip01rb.mass = 0.3125f;
		LThighrb = LThigh.gameObject.AddComponent<Rigidbody>();
		LThighrb.mass = 0.1875f;
		RThighrb = RThigh.gameObject.AddComponent<Rigidbody>();
		RThighrb.mass = 0.1875f;
		LCalfrb = LCalf.gameObject.AddComponent<Rigidbody>();
		LCalfrb.mass = 0.1875f;
		RCalfrb = RCalf.gameObject.AddComponent<Rigidbody>();
		RCalfrb.mass = 0.1875f;
		Spine3arb = Spine3a.gameObject.AddComponent<Rigidbody>();
		Spine3arb.mass = 0.3125f;
		Spine3brb = Spine3b.gameObject.AddComponent<Rigidbody>();
		Spine3brb.mass = 0.3125f;
		Spine3crb = Spine3c.gameObject.AddComponent<Rigidbody>();
		Spine3crb.mass = 0.3125f;
		Spine3rb = Spine3.gameObject.AddComponent<Rigidbody>();
		Spine3rb.mass = 0.3125f;
		Headrb = Head.gameObject.AddComponent<Rigidbody>();
		Headrb.mass = 0.125f;
		LUpperArmrb = LUpperArm.gameObject.AddComponent<Rigidbody>();
		LUpperArmrb.mass = 0.125f;
		RUpperArmrb = RUpperArm.gameObject.AddComponent<Rigidbody>();
		RUpperArmrb.mass = 0.125f;
		LForearmrb = LForearm.gameObject.AddComponent<Rigidbody>();
		LForearmrb.mass = 0.125f;
		RForearmrb = RForearm.gameObject.AddComponent<Rigidbody>();
		RForearmrb.mass = 0.125f;
		RigidbodysOff();
	}

	private void CreateHingeJoints()
	{
		LThighhj = LThigh.gameObject.AddComponent<CharacterJoint>();
		LThighhj.connectedBody = Bip01rb;
		LThighhj.axis = new Vector3(0f, -1f, 0f);
		LThighhj.swingAxis = new Vector3(0f, 0f, 1f);
		RThighhj = RThigh.gameObject.AddComponent<CharacterJoint>();
		RThighhj.connectedBody = Bip01rb;
		RThighhj.axis = new Vector3(0f, -1f, 0f);
		RThighhj.swingAxis = new Vector3(0f, 0f, 1f);
		LCalfhj = LCalf.gameObject.AddComponent<CharacterJoint>();
		LCalfhj.connectedBody = LThighrb;
		LCalfhj.axis = new Vector3(0f, -1f, 0f);
		LCalfhj.swingAxis = new Vector3(0f, 0f, 1f);
		RCalfhj = RCalf.gameObject.AddComponent<CharacterJoint>();
		RCalfhj.connectedBody = RThighrb;
		RCalfhj.axis = new Vector3(0f, -1f, 0f);
		RCalfhj.swingAxis = new Vector3(0f, 0f, 1f);
		Spine3ahj = Spine3a.gameObject.AddComponent<CharacterJoint>();
		Spine3ahj.connectedBody = Bip01rb;
		Spine3ahj.axis = new Vector3(0f, -1f, 0f);
		Spine3ahj.swingAxis = new Vector3(0f, 0f, -1f);
		Spine3bhj = Spine3b.gameObject.AddComponent<CharacterJoint>();
		Spine3bhj.connectedBody = Spine3arb;
		Spine3bhj.axis = new Vector3(0f, -1f, 0f);
		Spine3bhj.swingAxis = new Vector3(0f, 0f, -1f);
		Spine3chj = Spine3c.gameObject.AddComponent<CharacterJoint>();
		Spine3chj.connectedBody = Spine3brb;
		Spine3chj.axis = new Vector3(0f, -1f, 0f);
		Spine3chj.swingAxis = new Vector3(0f, 0f, -1f);
		Spine3hj = Spine3.gameObject.AddComponent<CharacterJoint>();
		Spine3hj.connectedBody = Spine3crb;
		Spine3hj.axis = new Vector3(0f, -1f, 0f);
		Spine3hj.swingAxis = new Vector3(0f, 0f, -1f);
		Headhj = Head.gameObject.AddComponent<CharacterJoint>();
		Headhj.connectedBody = Spine3rb;
		Headhj.axis = new Vector3(0f, -1f, 0f);
		Headhj.swingAxis = new Vector3(0f, 0f, -1f);
		LUpperArmhj = LUpperArm.gameObject.AddComponent<CharacterJoint>();
		LUpperArmhj.connectedBody = Spine3rb;
		LUpperArmhj.axis = new Vector3(0f, 0f, 1f);
		LUpperArmhj.swingAxis = new Vector3(1f, 0f, 0f);
		RUpperArmhj = RUpperArm.gameObject.AddComponent<CharacterJoint>();
		RUpperArmhj.connectedBody = Spine3rb;
		RUpperArmhj.axis = new Vector3(0f, 0f, 1f);
		RUpperArmhj.swingAxis = new Vector3(1f, 0f, 0f);
		LForearmhj = LForearm.gameObject.AddComponent<CharacterJoint>();
		LForearmhj.connectedBody = LUpperArmrb;
		LForearmhj.axis = new Vector3(1f, 0f, 0f);
		LForearmhj.swingAxis = new Vector3(0f, 0f, 1f);
		RForearmhj = RForearm.gameObject.AddComponent<CharacterJoint>();
		RForearmhj.connectedBody = RUpperArmrb;
		RForearmhj.axis = new Vector3(1f, 0f, 0f);
		RForearmhj.swingAxis = new Vector3(0f, 0f, 1f);
	}

	public void CollisionOn()
	{
		if ((bool)Bip01bc)
		{
			Bip01bc.enabled = true;
			LThighbc.enabled = true;
			RThighbc.enabled = true;
			LCalfbc.enabled = true;
			RCalfbc.enabled = true;
			Spine3bc.enabled = true;
			Spine3abc.enabled = true;
			Spine3bbc.enabled = true;
			Spine3cbc.enabled = true;
			Headbc.enabled = true;
			LUpperArmbc.enabled = true;
			LForearmbc.enabled = true;
			RUpperArmbc.enabled = true;
			RForearmbc.enabled = true;
			base.GetComponent<Collider>().enabled = false;
		}
	}

	public void CollisionOff()
	{
		if ((bool)Bip01bc)
		{
			Bip01bc.enabled = false;
			LThighbc.enabled = false;
			RThighbc.enabled = false;
			LCalfbc.enabled = false;
			RCalfbc.enabled = false;
			Spine3bc.enabled = false;
			Spine3abc.enabled = false;
			Spine3bbc.enabled = false;
			Spine3cbc.enabled = false;
			Headbc.enabled = false;
			LUpperArmbc.enabled = false;
			LForearmbc.enabled = false;
			RUpperArmbc.enabled = false;
			RForearmbc.enabled = false;
			if (SIDE != BaseLogic.SideReal)
			{
				base.GetComponent<Collider>().enabled = true;
			}
		}
	}

	private void SetCustomizeView(bool view)
	{
		if (view)
		{
			SMD_Import.layer = 0;
			for (int i = 0; i < BODY_ARMORS.Length; i++)
			{
				BODY_ARMORS[i].gameObject.layer = 0;
			}
			WEAPONS_HANDS_OBJ.SetActive(true);
			WEAPONS_PISTOL_OBJ.SetActive(true);
			WEAPONS_RIFLE_OBJ.SetActive(true);
			BACKPACK_OBJ.SetActive(true);
			HELMET_OBJ.SetActive(true);
			GLASSES_OBJ.SetActive(true);
			if (photonView.isMine)
			{
				if ((bool)VEHICLE.GetComponent<MortarSystem>() || ((bool)VEHICLE.GetComponent<CarSystem>() && VEHICLE.GetComponent<CarSystem>().GUNNER_CAR_KUZOV && VEHICLE.GetComponent<CarSystem>()._choiceSlot == 1))
				{
					Network.PLAYER.GetComponent<Animator>().SetBool("Crouched", true);
					Network.PLAYER.GetComponent<Animator>().SetFloat("Speed", 0f);
				}
				else
				{
					animator.SetBool("InCar", true);
				}
			}
		}
		else
		{
			SMD_Import.layer = 20;
			for (int j = 0; j < BODY_ARMORS.Length; j++)
			{
				BODY_ARMORS[j].gameObject.layer = 20;
			}
			WEAPONS_HANDS_OBJ.SetActive(false);
			WEAPONS_PISTOL_OBJ.SetActive(false);
			WEAPONS_RIFLE_OBJ.SetActive(false);
			BACKPACK_OBJ.SetActive(false);
			HELMET_OBJ.SetActive(false);
			GLASSES_OBJ.SetActive(false);
		}
	}

	private void Update()
	{
		if (isMyHero || BOT)
		{
			if (BOT)
			{
				if (_syncDead || !(Time.time > _lodSysTimer + 8f))
				{
					return;
				}
				if (!_syncDead)
				{
					CollisionOff();
					RigidbodysOff();
					if ((_distanceToHero = Vector3.Distance(Camera.main.transform.position, base.transform.position)) < 400f)
					{
						GetComponent<SoldierAI>().enabled = true;
						_lodSys = false;
						if (hitPointsReal > 0)
						{
							animator.enabled = true;
						}
					}
					else
					{
						GetComponent<SoldierAI>().enabled = false;
						_lodSys = true;
						animator.enabled = false;
					}
					_lodSysTimer = Time.time + UnityEngine.Random.Range(1f, 3f);
				}
				else
				{
					_lodSysTimer = Time.time + UnityEngine.Random.Range(1f, 3f);
				}
			}
			else if (BaseLogic._bodyArmorReal != bodyArmor)
			{
				SetBodyArmorLocal(BaseLogic._bodyArmorReal);
			}
			return;
		}
		if (photonView.isMine)
		{
			if (hitPointsReal != hitPointsProtected / 567)
			{
				SetHitPoints(0);
				if ((bool)hahaha)
				{
					AudioSource.PlayClipAtPoint(hahaha, base.transform.position);
				}
				BaseLogic.SetBan(9998);
				BaseLogic._player.SendMessage("SaveHero");
				if (PhotonNetwork.room != null)
				{
					PhotonNetwork.LeaveRoom();
				}
			}
			if (!_syncDead)
			{
				if (hitPointsReal <= 0)
				{
					Dead();
				}
				else if (Time.time > _updateTimer + 30f)
				{
					photonView.RPC("SetHeroItems", PhotonTargets.Others, BaseLogic._helmet, BaseLogic._glasses, BaseLogic._backpack, BaseLogic._cloth, BaseLogic._bodyArmorReal);
					SetHelmetLocal(BaseLogic._helmet);
					SetGlassesLocal(BaseLogic._glasses);
					SetBackpackLocal(BaseLogic._backpack);
					SetClothLocal(BaseLogic._cloth);
					SetBodyArmorLocal(BaseLogic._bodyArmorReal);
					if (BaseLogic._weaponSlot1_Real > 0)
					{
						photonView.RPC("SetWeapons", PhotonTargets.Others, new Quaternion(BaseLogic._weaponSlot1_Real, BaseLogic._weaponSlot2_Real, BaseLogic._weaponMod1, BaseLogic._weaponMod2));
						SetWeaponsLocal(new Quaternion(BaseLogic._weaponSlot1_Real, BaseLogic._weaponSlot2_Real, BaseLogic._weaponMod1, BaseLogic._weaponMod2));
					}
					_updateTimer = Time.time;
				}
				return;
			}
			int num = 60;
			num = ((BaseLogic.GAME_MODE != 6) ? 60 : 10);
			GameObject gameObject = GameObject.Find("!!!FPS Gui");
			if (Time.time > respawnTimer + 3f)
			{
				int num2 = (int)(respawnTimer + (float)num - Time.time);
				gameObject.GetComponent<AdminMenu>().AdminButtons[3].GetComponent<UILabel>().text = string.Empty + BaseLogic.target_playerID + "\n" + string.Format(Localization.Get("You_are_dead"), string.Empty + num2);
				if (!BaseLogic.RECON)
				{
					BaseLogic._playerWeapon.SetActive(false);
					gameObject.SendMessage("ShowReconPanel");
					UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(HeroBase));
					Camera.main.GetComponent<CameraKick>().enabled = false;
					Camera.main.transform.parent.GetComponent<SmoothMouseLook>().enabled = false;
					Camera.main.GetComponent<MouseOrb>().enabled = true;
					if (PhotonNetwork.player.ID == Killerid)
					{
						Camera.main.GetComponent<MouseOrb>().target = base.transform;
						gameObject.GetComponent<AdminMenu>().AdminButtons[3].GetComponent<UILabel>().text = string.Empty + PhotonNetwork.player.name + " / HP: " + hitPointsReal;
						BaseLogic.target_playerID = string.Empty + PhotonNetwork.player.name;
					}
					else
					{
						UnityEngine.Object[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							HeroBase heroBase = (HeroBase)array2[i];
							if ((bool)heroBase.photonView && heroBase.photonView.owner.ID == Killerid)
							{
								Camera.main.GetComponent<MouseOrb>().target = heroBase.transform;
								gameObject.GetComponent<AdminMenu>().AdminButtons[3].GetComponent<UILabel>().text = string.Empty + heroBase.photonView.owner.name + " / HP: " + heroBase.hitPointsReal;
								string empty = string.Empty;
								string text = heroBase.photonView.owner.name;
								object obj = heroBase.photonView.owner.customProperties["id"];
								BaseLogic.target_playerID = empty + text + "\nID: " + ((obj != null) ? obj.ToString() : null);
								break;
							}
						}
					}
					Camera.main.GetComponent<MouseOrb>()._HeroOsmotr = true;
					BaseLogic._respawnTimerGui.GetComponent<UILabel>().text = string.Empty;
					Screen.lockCursor = false;
					Cursor.visible = true;
					BaseLogic.RECON = true;
				}
			}
			else
			{
				BaseLogic._respawnTimerGui.GetComponent<UILabel>().text = string.Format(Localization.Get("You_are_dead"), (int)(respawnTimer + (float)num - Time.time));
			}
			if (Time.time > respawnTimer + (float)num)
			{
				if (BaseLogic._backpack != 0 && (bool)Network.PLAYER)
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("CreateWeapon", PhotonTargets.All, BaseLogic._backpack + 99);
				}
				if (BaseLogic._helmet != 0 && (bool)Network.PLAYER)
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("CreateWeapon", PhotonTargets.All, BaseLogic._helmet + 199);
				}
				GetComponent<Heroserialize>()._optimizedSync = true;
				GetComponent<Heroserialize>()._optimizedSyncTime = Time.time;
				BaseLogic._playerWeapon.SetActive(true);
				gameObject.SendMessage("ShowBasePanel");
				Camera.main.GetComponent<CameraKick>().enabled = true;
				Camera.main.transform.parent.GetComponent<SmoothMouseLook>().enabled = true;
				Camera.main.GetComponent<MouseOrb>()._HeroOsmotr = false;
				Camera.main.GetComponent<MouseOrb>().enabled = false;
				BaseLogic.RECON = false;
				AllRespawn();
			}
			return;
		}
		if (_godMode && Time.time > _godModeTimer + 15f)
		{
			_godMode = false;
		}
		if (!(Time.time > _lodSysTimer + 8f))
		{
			return;
		}
		if (!_syncDead)
		{
			CollisionOff();
			RigidbodysOff();
			float num3 = (_distanceToHero = Vector3.Distance(Camera.main.transform.position, base.transform.position));
			if (num3 < 400f)
			{
				SMD_Import.SetActive(true);
				SMD_Root.SetActive(true);
				_lodSys = false;
				if (num3 < 250f)
				{
					GetComponent<Heroserialize>()._optimizedSync = false;
				}
				else
				{
					GetComponent<Heroserialize>()._optimizedSync = true;
				}
				if (hitPointsReal > 0)
				{
					animator.enabled = true;
				}
				if (num3 < 40f)
				{
					GetComponent<Heroserialize>()._optimizedSync2 = true;
				}
				else
				{
					GetComponent<Heroserialize>()._optimizedSync2 = false;
				}
			}
			else
			{
				SMD_Import.SetActive(false);
				SMD_Root.SetActive(false);
				_lodSys = true;
				GetComponent<Heroserialize>()._optimizedSync = true;
				GetComponent<Heroserialize>()._optimizedSync2 = false;
				animator.enabled = false;
			}
			_lodSysTimer = Time.time + UnityEngine.Random.Range(1f, 3f);
		}
		else
		{
			_lodSysTimer = Time.time + UnityEngine.Random.Range(1f, 3f);
		}
	}

	public void RagdollOn()
	{
		if (!_lodSys)
		{
			StartCoroutine("PhysicsOff");
			if ((bool)Bip01rb && Bip01rb.isKinematic)
			{
				CollisionOn();
				RigidbodysOn();
				base.GetComponent<Collider>().enabled = false;
				Bip01rb.gameObject.layer = 26;
				LThighrb.gameObject.layer = 26;
				RThighrb.gameObject.layer = 26;
				LCalfrb.gameObject.layer = 26;
				RCalfrb.gameObject.layer = 26;
				Spine3rb.gameObject.layer = 26;
				Spine3arb.gameObject.layer = 26;
				Spine3brb.gameObject.layer = 26;
				Spine3crb.gameObject.layer = 26;
				Headrb.gameObject.layer = 26;
				LUpperArmrb.gameObject.layer = 26;
				RUpperArmrb.gameObject.layer = 26;
				LForearmrb.gameObject.layer = 26;
				RForearmrb.gameObject.layer = 26;
			}
		}
	}

	private void AllRespawn()
	{
		BaseLogic._respawnTimerGui.GetComponent<UILabel>().text = string.Empty;
		BaseLogic._inventory4.SetActive(true);
		BaseLogic.PLAYER_DEATH = false;
		_syncDead = false;
		if (BOT)
		{
			SetHitPoints(50);
		}
		else
		{
			SetHitPoints(100);
		}
		FPSPlayer.SetHitPoints(100);
		FPSPlayer.SetHungerPoints(0);
		FPSPlayer.SetThirstPoints(0);
		FPSPlayer.SetTemperaturePoints(0);
		BaseLogic._player.SendMessage("ShowInventory");
		BaseLogic.RADIO = 0;
		animator.enabled = true;
		photonView.RPC("Respawn", PhotonTargets.Others);
		GetComponent<Heroserialize>().enabled = true;
		SetRandomPosition();
		BaseLogic._player.GetComponent<FPSPlayer>().crosshairEnabled = true;
		Camera.main.transform.parent.GetComponent<SmoothMouseLook>().enabled = true;
		GameObject gameObject = GameObject.Find("!Water Zone Trigger");
		if ((bool)gameObject)
		{
			gameObject.SendMessage("OutWater");
		}
		if (BaseLogic.GAME_MODE != 2 && BaseLogic.GAME_MODE != 3 && BaseLogic.GAME_MODE != 6)
		{
			BaseLogic.AddVipLoot();
		}
		BaseLogic.CheckWeapons();
	}

	private void SetRandomPosition()
	{
		for (int i = 0; i < 40; i++)
		{
			SPAWN[i] = GameObject.Find("SPWAN" + i);
		}
		if (BaseLogic.GAME_MODE == 1 || BaseLogic.GAME_MODE == 6)
		{
			if (BaseLogic.SideReal == 1)
			{
				int num = UnityEngine.Random.Range(0, 9);
				base.transform.parent.transform.position = SPAWN[num].transform.position;
				if (BaseLogic.GAME_MODE == 6)
				{
					BaseLogic.setVipReal = true;
					BaseLogic.SetVIP_prot(501);
					BaseLogic.setVipReal = false;
					BaseLogic.inBuyZone = true;
					BaseLogic._player.SendMessage("ShowInventory");
					if (BaseLogic.PLAYER_MONEY < 200)
					{
						BaseLogic.SetMoney(200);
					}
				}
			}
			else if (BaseLogic.SideReal == 2)
			{
				int num2 = UnityEngine.Random.Range(10, 20);
				base.transform.parent.transform.position = SPAWN[num2].transform.position;
				if (BaseLogic.GAME_MODE == 6)
				{
					BaseLogic.setVipReal = true;
					BaseLogic.SetVIP_prot(502);
					BaseLogic.setVipReal = false;
					BaseLogic.inBuyZone = true;
					BaseLogic._player.SendMessage("ShowInventory");
					if (BaseLogic.PLAYER_MONEY < 200)
					{
						BaseLogic.SetMoney(200);
					}
				}
			}
			if (BaseLogic.GAME_MODE == 6)
			{
				if (BaseLogic.SideReal == 1)
				{
					ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
					hashtable.Add("side1", (int)PhotonNetwork.room.customProperties["side1"] - 1);
					PhotonNetwork.room.SetCustomProperties(hashtable);
				}
				else
				{
					ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
					hashtable2.Add("side2", (int)PhotonNetwork.room.customProperties["side2"] - 1);
					PhotonNetwork.room.SetCustomProperties(hashtable2);
				}
			}
		}
		else if (BaseLogic.GAME_MODE == 2 || BaseLogic.GAME_MODE == 4)
		{
			base.transform.parent.transform.position = SPAWN[0].transform.position;
			if (BaseLogic.GAME_MODE == 2)
			{
				BaseLogic.ClearLoot();
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(500);
				BaseLogic.setVipReal = false;
			}
		}
		else
		{
			int num3 = UnityEngine.Random.Range(0, 20);
			if ((bool)SPAWN[num3])
			{
				base.transform.parent.transform.position = SPAWN[num3].transform.position;
			}
			else
			{
				base.transform.parent.transform.position = SPAWN[0].transform.position;
			}
		}
		if (BaseLogic.GAME_MODE != 6)
		{
			BaseLogic.AddVipLoot();
			BaseLogic.CheckWeapons();
		}
	}

	private IEnumerator PhysicsOff()
	{
		yield return new WaitForSeconds(7f);
		RagdollOff();
	}

	private void RagdollOff()
	{
		if ((bool)Bip01rb && !Bip01rb.isKinematic)
		{
			RigidbodysOff();
			Bip01rb.gameObject.layer = 21;
			LThighrb.gameObject.layer = 21;
			RThighrb.gameObject.layer = 21;
			LCalfrb.gameObject.layer = 21;
			RCalfrb.gameObject.layer = 21;
			Spine3rb.gameObject.layer = 21;
			Spine3arb.gameObject.layer = 21;
			Spine3brb.gameObject.layer = 21;
			Spine3crb.gameObject.layer = 21;
			Headrb.gameObject.layer = 21;
			LUpperArmrb.gameObject.layer = 21;
			RUpperArmrb.gameObject.layer = 21;
			LForearmrb.gameObject.layer = 21;
			RForearmrb.gameObject.layer = 21;
		}
	}

	private IEnumerator waitRespwan()
	{
		yield return new WaitForSeconds(1.2f);
		animator.enabled = true;
	}

	[PunRPC]
	private void Respawn()
	{
		_syncDead = false;
		SetHitPoints(100);
		RagdollOff();
		StartCoroutine("waitRespwan");
		GetComponent<Heroserialize>().enabled = true;
		GetComponent<Heroserialize>()._noSlowPolet = true;
		GetComponent<Heroserialize>()._noSlowPoletTimer = Time.time;
		_godMode = true;
		_godModeTimer = Time.time;
	}

	[PunRPC]
	private void Attack(int hits)
	{
		SetHitPoints(hitPointsReal - hits);
		if (photonView.isMine)
		{
			BaseLogic._chatLabel.GetComponent<UILabel>().text = "Полученный урон: [FF0000]" + hits + " HP";
			BaseLogic._player.SendMessage("ApplyDamage2", hits, SendMessageOptions.DontRequireReceiver);
		}
	}

	[PunRPC]
	private void SetHP(int HP)
	{
		SetHitPoints(HP);
	}

	private void Dead()
	{
		if (_syncDead)
		{
			return;
		}
		if (BOT && !_syncDead && !GetComponent<SoldierAI>().dead)
		{
			base.GetComponent<AudioSource>().Stop();
			animator.enabled = false;
			SetHitPoints(0);
			_syncDead = true;
			GetComponent<SoldierAI>().dead = true;
			CreateWeaponLocal(BOT_WEAPON);
			WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 = 0;
			WEAPONS_RIFLE_OBJ.GetComponent<WeaponsBack>()._slotWeapon2 = 0;
			GetComponent<CharacterController>().enabled = false;
			GetComponent<SoldierAI>().enabled = false;
			RagdollOn();
			Headrb.velocity = new Vector3(0f, 1f, 0f);
			return;
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("deaths", (int)PhotonNetwork.player.customProperties["deaths"] + 1);
		PhotonNetwork.player.SetCustomProperties(hashtable);
		if ((bool)VEHICLE)
		{
			if ((bool)VEHICLE.GetComponent<CarSystem>())
			{
				VEHICLE.GetComponent<CarSystem>().ExitCar();
			}
			else if ((bool)VEHICLE.GetComponent<HeliSystem>())
			{
				VEHICLE.GetComponent<HeliSystem>().ExitCar();
			}
			else if ((bool)VEHICLE.GetComponent<TrackSystem>())
			{
				VEHICLE.GetComponent<TrackSystem>().ExitCar();
			}
		}
		BaseLogic._player.SendMessage("HideInventory");
		BaseLogic._respawnTimerGui.GetComponent<UILabel>().text = string.Empty;
		if (FPSPlayer.hitPointsReal > 0)
		{
			BaseLogic._player.SendMessage("ApplyDamage2", 1000);
		}
		GetComponent<Heroserialize>().enabled = false;
		_syncDead = true;
		respawnTimer = Time.time;
		int num = 0;
		for (int i = 0; i < BaseLogic.PLAYER_MAXSLOTS; i++)
		{
			if (BaseLogic.GAME_MODE != 6 && BaseLogic.PLAYER_SLOT_REAL[i] > 0)
			{
				photonView.RPC("CreateItem", PhotonTargets.All, new Vector2(BaseLogic.PLAYER_SLOT_REAL[i], BaseLogic.PLAYER_SLOT_COUNT_REAL[i]));
			}
		}
		BaseLogic.SetWeaponSlot1(0);
		BaseLogic.SetWeaponSlot2(0);
		BaseLogic.SetWeaponSlotAmmo1(0);
		BaseLogic.SetWeaponSlotAmmo2(0);
		BaseLogic._weaponMod1 = 0;
		BaseLogic._weaponMod2 = 0;
		BaseLogic._backpack = 0;
		BaseLogic._glasses = 0;
		BaseLogic._helmet = 0;
		BaseLogic._cloth = 0;
		BaseLogic.SetBodyArmor(0);
		BaseLogic._player.GetComponent<FPSRigidBodyWalker>().staminaForSprint = 40f;
		BaseLogic.ClearLoot();
		BaseLogic.ClearDataBase();
		BaseLogic._player.SendMessage("SaveHero");
	}

	private void DeathLocal(int killerid, int deadUser)
	{
		if (PhotonNetwork.isMasterClient)
		{
			Vector3 a = Vector3.zero;
			Vector3 b = Vector3.zero;
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(HeroBase));
			if (array.Length == 0)
			{
				Debug.Log("MASTER SERVER: #HeroBase is not in Scene!");
				return;
			}
			int num = 0;
			UnityEngine.Object[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				HeroBase heroBase = (HeroBase)array2[i];
				if (!heroBase.isMyHero && !heroBase.BOT)
				{
					if (heroBase.photonView.ownerId == killerid)
					{
						a = heroBase.transform.position;
					}
					else if (heroBase.photonView.ownerId == deadUser)
					{
						b = heroBase.transform.position;
					}
				}
			}
			float num2 = Vector3.Distance(a, b);
			if (num2 >= 500f)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("Ban", PhotonTargets.All, string.Empty + killerid + "\n" + 2);
			}
		}
		if (!_syncDead)
		{
			SetHitPoints(0);
			if (photonView.isMine)
			{
				Dead();
			}
			else
			{
				SetDefaultParent();
				GetComponent<Heroserialize>().enabled = false;
				animator.enabled = false;
				RagdollOn();
				_syncDead = true;
				SetWeaponsLocal(new Quaternion(0f, 0f, 0f, 0f));
			}
			Killerid = killerid;
			if (PhotonNetwork.player.ID == killerid)
			{
				photonView.RPC("GameChat", PhotonTargets.All, "[FF8888]*** " + photonView.owner.name + " был убит игроком " + PhotonNetwork.player.name + " ***[FFFFFF]");
			}
		}
	}

	[PunRPC]
	private void Death(int killerid, int deadUser)
	{
		DeathLocal(killerid, deadUser);
	}

	[PunRPC]
	private void DeathExplode(int killerid, int deadUser)
	{
		DeathLocal(killerid, deadUser);
		if (killerid != PhotonNetwork.player.ID)
		{
			return;
		}
		if (BaseLogic.SideReal == SIDE)
		{
			AudioSource.PlayClipAtPoint(BaseLogic._player.GetComponent<BaseLogic>().TEAMKILLER, base.transform.position, 0.4f);
			if (BaseLogic.GAME_MODE == 6)
			{
				BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY - 500);
			}
			else
			{
				BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY - 100);
			}
		}
		else
		{
			AudioSource.PlayClipAtPoint(BaseLogic._player.GetComponent<BaseLogic>().RAMPAGE, base.transform.position, 0.25f);
			if (BaseLogic.GAME_MODE == 6)
			{
				BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY + 250);
			}
			else
			{
				BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY + 50);
			}
		}
	}

	private void SetHelmetLocal(int helm)
	{
		if ((bool)HELMET_OBJ && HELMET_OBJ.activeInHierarchy)
		{
			HELMET_OBJ.SendMessage("ShowHelmet", helm);
			switch (helm)
			{
			case 6:
			case 9:
			case 31:
				headArmor = 2.2f;
				break;
			case 10:
			case 15:
			case 19:
			case 29:
			case 30:
				headArmor = 2.1f;
				break;
			case 1:
			case 14:
			case 27:
			case 28:
			case 32:
				headArmor = 1.2f;
				break;
			}
		}
	}

	private void SetGlassesLocal(int helm)
	{
		if ((bool)GLASSES_OBJ && GLASSES_OBJ.activeInHierarchy)
		{
			GLASSES_OBJ.SendMessage("ShowHelmet", helm);
		}
	}

	private void SetBackpackLocal(int back)
	{
		if ((bool)BACKPACK_OBJ)
		{
			BACKPACK_OBJ.SendMessageUpwards("ShowBackpack", back, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void SetWeaponsLocal(Quaternion weapons)
	{
		if ((bool)WEAPONS_HANDS_OBJ)
		{
			WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 = Convert.ToInt32(weapons.x);
			WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._slotWeapon2 = Convert.ToInt32(weapons.y);
			if (!_lodSys)
			{
				WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._modWeapon1 = Convert.ToInt32(weapons.z);
				WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._modWeapon2 = Convert.ToInt32(weapons.w);
			}
			WEAPONS_PISTOL_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 = Convert.ToInt32(weapons.x);
			WEAPONS_PISTOL_OBJ.GetComponent<WeaponsBack>()._slotWeapon2 = Convert.ToInt32(weapons.y);
			WEAPONS_RIFLE_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 = Convert.ToInt32(weapons.x);
			WEAPONS_RIFLE_OBJ.GetComponent<WeaponsBack>()._slotWeapon2 = Convert.ToInt32(weapons.y);
		}
	}

	private void SetBodyArmorLocal(int armor)
	{
		for (int i = 0; i < BODY_ARMORS.Length; i++)
		{
			BODY_ARMORS[i].enabled = false;
			if (!isMyHero && photonView.isMine)
			{
				BODY_ARMORS[i].gameObject.layer = 20;
			}
		}
		if (armor > 0)
		{
			BODY_ARMORS[armor].enabled = true;
		}
		bodyArmor = armor;
		switch (armor)
		{
		case 0:
			bodyArmorCoef = 10;
			break;
		case 3:
			bodyArmorCoef = 12;
			break;
		case 6:
			bodyArmorCoef = 12;
			break;
		case 7:
			bodyArmorCoef = 12;
			break;
		case 1:
			bodyArmorCoef = 15;
			break;
		case 4:
			bodyArmorCoef = 18;
			break;
		case 5:
			bodyArmorCoef = 20;
			break;
		case 2:
			bodyArmorCoef = 23;
			break;
		}
	}

	private void SetClothLocal(int cloth)
	{
		SMD_Import.GetComponent<Renderer>().material = CLOTH[cloth];
		if (photonView.isMine)
		{
			BaseLogic._cloth = cloth;
		}
	}

	private void ApplyDamage(int dmg)
	{
		SetHitPoints(hitPointsReal - dmg);
	}

	[PunRPC]
	private void SetBodyArmor(int armor)
	{
		SetBodyArmorLocal(armor);
	}

	[PunRPC]
	private void SetHelmet(int helm)
	{
		SetHelmetLocal(helm);
	}

	[PunRPC]
	private void SetGlasses(int helm)
	{
		SetGlassesLocal(helm);
	}

	[PunRPC]
	private void SetBackpack(int back)
	{
		SetBackpackLocal(back);
	}

	[PunRPC]
	private void SetWeapons(Quaternion weapons)
	{
		SetWeaponsLocal(weapons);
	}

	[PunRPC]
	private void SetCloth(int cloth)
	{
		SetClothLocal(cloth);
	}

	[PunRPC]
	private void CreateItem(Vector2 itemx)
	{
		int num = Convert.ToInt32(itemx.x);
		int iTEM_COUNT = Convert.ToInt32(itemx.y);
		GameObject gameObject = UnityEngine.Object.Instantiate(ITEM, new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), base.transform.position.y + UnityEngine.Random.Range(0.5f, 1.3f), base.transform.position.z), base.transform.rotation) as GameObject;
		gameObject.SendMessage("ShowItem", num);
		gameObject.GetComponent<ItemPickup>().ITEM_COUNT = iTEM_COUNT;
		gameObject.GetComponent<ItemPickup>().isLoot = false;
		gameObject.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
		gameObject.GetComponent<Rigidbody>().useGravity = true;
		gameObject.GetComponent<Rigidbody>().isKinematic = false;
		gameObject.GetComponent<Rigidbody>().velocity = base.transform.TransformDirection(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(1f, 2f), UnityEngine.Random.Range(2f, 3f)));
		gameObject.GetComponent<Rigidbody>().angularVelocity = base.transform.TransformDirection(new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f)));
		gameObject.name = "ITEM: " + photonView.owner.name + ". " + num;
		if (BaseLogic.GAME_MODE == 6)
		{
			UnityEngine.Object.Destroy(gameObject, 60f);
		}
		else
		{
			UnityEngine.Object.Destroy(gameObject, 1200f);
		}
	}

	[PunRPC]
	private void DestroyObject(string nameObj)
	{
		GameObject gameObject = GameObject.Find(nameObj);
		if ((bool)gameObject)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	private void CreateWeaponLocal(int weaponID)
	{
		GameObject gameObject = null;
		GameObject gameObject2 = null;
		switch (weaponID)
		{
		case 5000:
			gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_FIRE_OBJECT1"));
			gameObject.transform.position = new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), base.transform.position.y + UnityEngine.Random.Range(0.5f, 1.3f), base.transform.position.z);
			break;
		case 5001:
			gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_AMMONAL_OBJECT1"));
			gameObject.transform.position = new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), base.transform.position.y + UnityEngine.Random.Range(0.5f, 1.3f), base.transform.position.z);
			break;
		case 5002:
			gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_C4"));
			gameObject.transform.position = new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), base.transform.position.y + UnityEngine.Random.Range(0.5f, 1.3f), base.transform.position.z);
			break;
		case 5003:
		case 5004:
		case 5005:
		case 5006:
		case 5007:
		case 5008:
		case 5009:
		case 5010:
		case 5011:
		case 5012:
		case 5013:
		case 5014:
		case 5015:
		case 5016:
		case 5017:
		case 5018:
		case 5019:
		case 5020:
		case 5021:
		case 5022:
		case 5023:
		case 5024:
		case 5025:
		case 5026:
		case 5027:
		case 5028:
		case 5029:
		case 5030:
		case 5031:
		case 5032:
		case 5033:
		case 5034:
		case 5035:
		case 5036:
		case 5037:
		case 5038:
		case 5039:
		case 5040:
		case 5041:
		case 5042:
		case 5043:
		case 5044:
		case 5045:
		case 5046:
		case 5047:
		case 5048:
		case 5049:
		case 5050:
		case 5051:
		case 5052:
		case 5053:
		case 5054:
		case 5055:
		case 5056:
		case 5057:
		case 5058:
		case 5059:
		case 5060:
		case 5061:
		case 5062:
		case 5063:
		case 5064:
		case 5065:
		case 5066:
		case 5067:
		case 5068:
		case 5069:
		case 5070:
		case 5071:
		case 5072:
		case 5073:
		case 5074:
		case 5075:
		case 5076:
		case 5077:
		case 5078:
		case 5079:
		case 5080:
		case 5081:
		case 5082:
		case 5083:
		case 5084:
		case 5085:
		case 5086:
		case 5087:
		case 5088:
		case 5089:
		case 5090:
		case 5091:
		case 5092:
		case 5093:
		case 5094:
		case 5095:
		case 5096:
		case 5097:
		case 5098:
		case 5099:
		case 5100:
			gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_AT_MINE"));
			gameObject.transform.position = new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), base.transform.position.y + UnityEngine.Random.Range(0.5f, 1.3f), base.transform.position.z);
			gameObject.name = "_MINE_AT_" + photonView.owner.ID + "_id_" + weaponID;
			break;
		default:
			if (weaponID > 5100 && weaponID <= 5200)
			{
				gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_AP_MINE"));
				gameObject.transform.position = new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), base.transform.position.y + UnityEngine.Random.Range(0.5f, 1.3f), base.transform.position.z);
				gameObject.name = "_MINE_AP_" + photonView.owner.ID + "_id_" + weaponID;
				break;
			}
			switch (weaponID)
			{
			case 5201:
				gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_GRENADE_RGD"));
				gameObject.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 1.1f, base.transform.position.z);
				break;
			case 5202:
				gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_GRENADE_F1"));
				gameObject.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 1.1f, base.transform.position.z);
				break;
			case 5203:
				gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_GRENADE_SMOKE"));
				gameObject.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 1.1f, base.transform.position.z);
				break;
			case 100:
			case 101:
			case 102:
			case 103:
			case 104:
			case 105:
			case 106:
			case 107:
			case 108:
			case 109:
			case 110:
			case 111:
			case 112:
			case 113:
			case 114:
			case 115:
			case 116:
			case 117:
			case 118:
			case 119:
			case 120:
			case 121:
			case 122:
			case 123:
			case 124:
			case 125:
			case 126:
			case 127:
			case 128:
			case 129:
			case 130:
			case 131:
			case 132:
			case 133:
			case 134:
			case 135:
			case 136:
			case 137:
			case 138:
			case 139:
			case 140:
			case 141:
			case 142:
			case 143:
			case 144:
			case 145:
			case 146:
			case 147:
			case 148:
			case 149:
			case 150:
			case 151:
			case 152:
			case 153:
			case 154:
			case 155:
			case 156:
			case 157:
			case 158:
			case 159:
			case 160:
			case 161:
			case 162:
			case 163:
			case 164:
			case 165:
			case 166:
			case 167:
			case 168:
			case 169:
			case 170:
			case 171:
			case 172:
			case 173:
			case 174:
			case 175:
			case 176:
			case 177:
			case 178:
			case 179:
			case 180:
			case 181:
			case 182:
			case 183:
			case 184:
			case 185:
			case 186:
			case 187:
			case 188:
			case 189:
			case 190:
				gameObject2 = BACKPACK_ITEM;
				gameObject = UnityEngine.Object.Instantiate(gameObject2, new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), base.transform.position.y + UnityEngine.Random.Range(0.5f, 1.3f), base.transform.position.z), base.transform.rotation) as GameObject;
				gameObject.name = "BACKPACK_ITEM: " + photonView.owner.name + ". " + weaponID;
				gameObject.GetComponent<BackPackPickup>().BACKPACK_ID = weaponID - 99;
				break;
			default:
				if (weaponID >= 200 && weaponID <= 290)
				{
					gameObject2 = HELMET_ITEM;
					gameObject = UnityEngine.Object.Instantiate(gameObject2, new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), base.transform.position.y + UnityEngine.Random.Range(0.5f, 1.3f), base.transform.position.z), base.transform.rotation) as GameObject;
					gameObject.name = "HELMET_ITEM: " + photonView.owner.name + ". " + weaponID;
					gameObject.GetComponent<HelmetPickup>().HELMET_ID = weaponID - 199;
					break;
				}
				if (weaponID >= 300 && weaponID <= 390)
				{
					gameObject2 = CLOTH_ITEM;
					gameObject = UnityEngine.Object.Instantiate(gameObject2, new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), base.transform.position.y + UnityEngine.Random.Range(0.5f, 1.3f), base.transform.position.z), base.transform.rotation) as GameObject;
					gameObject.name = "CLOTH_ITEM: " + photonView.owner.name + ". " + weaponID;
					gameObject.GetComponent<ClothPickup>().CLOTH_ID = weaponID - 300;
					break;
				}
				gameObject2 = WEAPONS[weaponID];
				if (gameObject2 != null)
				{
					gameObject = UnityEngine.Object.Instantiate(gameObject2, new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), base.transform.position.y + UnityEngine.Random.Range(0.5f, 1.3f), base.transform.position.z), base.transform.rotation) as GameObject;
					if (BOT)
					{
						gameObject.name = "WEAPON: " + base.name + ". " + weaponID;
					}
					else
					{
						gameObject.name = "WEAPON: " + photonView.owner.name + ". " + weaponID;
					}
				}
				break;
			}
			break;
		}
		if (!gameObject)
		{
			return;
		}
		gameObject.GetComponent<Rigidbody>().useGravity = true;
		gameObject.GetComponent<Rigidbody>().isKinematic = false;
		if (weaponID == 5201 || weaponID == 5202 || weaponID == 5203)
		{
			animator.SetTrigger("Grenade");
			if (photonView.isMine)
			{
				gameObject.GetComponent<Rigidbody>().velocity = Camera.main.transform.TransformDirection(new Vector3(0f, 6f, 25f));
			}
			else
			{
				gameObject.GetComponent<Rigidbody>().velocity = Head.TransformDirection(new Vector3(-17f, -25f, 0f));
			}
			gameObject.GetComponent<Rigidbody>().angularVelocity = base.transform.TransformDirection(new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f)));
			return;
		}
		gameObject.GetComponent<Rigidbody>().velocity = base.transform.TransformDirection(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(2f, 3f), UnityEngine.Random.Range(3f, 4f)));
		if (weaponID < 5000)
		{
			gameObject.GetComponent<Rigidbody>().angularVelocity = base.transform.TransformDirection(new Vector3(UnityEngine.Random.Range(-3f, 3f), 0f, UnityEngine.Random.Range(-3f, 3f)));
		}
		if (BaseLogic.GAME_MODE == 6)
		{
			UnityEngine.Object.Destroy(gameObject, 60f);
		}
		else
		{
			UnityEngine.Object.Destroy(gameObject, 1200f);
		}
	}

	[PunRPC]
	private void CreateWeapon(int weaponID)
	{
		CreateWeaponLocal(weaponID);
	}

	private void FireEffectLocal(Quaternion dir)
	{
		if (!WEAPONS_HANDS_OBJ)
		{
			return;
		}
		if (_distanceToHero >= 180f)
		{
			WEAPONS_HANDS_OBJ.SendMessage("LongFire");
		}
		else
		{
			WEAPONS_HANDS_OBJ.SendMessage("Fire");
		}
		WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>().FLASH.transform.rotation = dir;
		if (WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 == 3 || WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 == 13 || WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 == 19)
		{
			for (int i = 0; i < 5; i++)
			{
				Ballistic ballistic = null;
				for (int j = 0; j < BaseLogic._poolBulletsShotgun.Length; j++)
				{
					ballistic = BaseLogic._poolBulletsShotgun[j].GetComponent<Ballistic>();
					if (ballistic.isSleep)
					{
						ballistic.enabled = true;
						ballistic.transform.position = WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>().FLASH.transform.position;
						ballistic.transform.rotation = dir;
						ballistic.bulletSpeed = 10f;
						ballistic.isOthers = true;
						ballistic.transform.eulerAngles = new Vector3(ballistic.transform.eulerAngles.x + UnityEngine.Random.Range(-1f, 1f), ballistic.transform.eulerAngles.y + UnityEngine.Random.Range(-1f, 1f), 0f);
						ballistic.transform.localScale = new Vector3(0.3f, 0.3f, 40f);
						ballistic.StartBullet();
						if (BOT)
						{
							ballistic.GetComponent<Ballistic>().isBOT = true;
						}
						break;
					}
				}
			}
			return;
		}
		if (WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 == 17)
		{
			Ballistic ballistic2 = null;
			for (int k = 0; k < BaseLogic._poolBulletsShotgun.Length; k++)
			{
				ballistic2 = BaseLogic._poolBulletsShotgun[k].GetComponent<Ballistic>();
				if (ballistic2.isSleep)
				{
					ballistic2.enabled = true;
					ballistic2.transform.position = WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>().FLASH.transform.position;
					ballistic2.transform.rotation = dir;
					ballistic2.isOthers = true;
					ballistic2.transform.localScale = new Vector3(0.3f, 0.3f, 40f);
					ballistic2.StartBullet();
					if (BOT)
					{
						ballistic2.GetComponent<Ballistic>().isBOT = true;
					}
					break;
				}
			}
			return;
		}
		Ballistic ballistic3 = null;
		for (int l = 0; l < BaseLogic._poolBullets.Length; l++)
		{
			ballistic3 = BaseLogic._poolBullets[l].GetComponent<Ballistic>();
			if (ballistic3.isSleep)
			{
				ballistic3.enabled = true;
				ballistic3.transform.position = WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>().FLASH.transform.position;
				ballistic3.transform.rotation = dir;
				ballistic3.isOthers = true;
				ballistic3.transform.localScale = new Vector3(0.4f, 0.4f, 40f);
				ballistic3.StartBullet();
				if (BOT)
				{
					ballistic3.GetComponent<Ballistic>().isBOT = true;
				}
				break;
			}
		}
	}

	[PunRPC]
	private void FireEffect(Quaternion dir)
	{
		if (!_lodSys)
		{
			FireEffectLocal(dir);
		}
	}

	[PunRPC]
	private void AnimatorTrigger(string anim)
	{
		if (_lodSys)
		{
			return;
		}
		if (anim == "MeleeAttack")
		{
			if ((bool)WEAPONS_HANDS_OBJ)
			{
				WEAPONS_HANDS_OBJ.SendMessage("Fire");
			}
		}
		else if (anim == "Reloading" && (bool)WEAPONS_HANDS_OBJ)
		{
			WEAPONS_HANDS_OBJ.SendMessage("Reload");
		}
		animator.SetTrigger(anim);
	}

	public void ImpactEffects2(RaycastHit hit, Vector3 direction)
	{
		if (_lodSys)
		{
			return;
		}
		if (hit.collider.tag == "armAnimal" || hit.collider.tag == "arm")
		{
			AudioSource.PlayClipAtPoint(hit_arm, hit.point);
		}
		else if (hit.collider.tag == "Snow" || hit.collider.tag == "Untagged")
		{
			AudioSource.PlayClipAtPoint(hit_ground, hit.point);
		}
		else if (hit.collider.tag == "Metal" || hit.collider.tag == "Usable")
		{
			AudioSource.PlayClipAtPoint(hit_metal, hit.point);
		}
		else if (hit.collider.tag == "Wood" || hit.collider.tag == "Prostrel")
		{
			AudioSource.PlayClipAtPoint(hit_wood, hit.point);
		}
		if (hit.collider.tag != "arm")
		{
			int qualityLevel = QualitySettings.GetQualityLevel();
			if (qualityLevel >= 3)
			{
				BulletMarks(hit);
			}
		}
		if (hit.collider.tag == "arm")
		{
			if ((bool)blood)
			{
				blood.transform.position = hit.point;
				blood.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				blood.GetComponent<ParticleSystem>().Play();
			}
			RaycastHit hitInfo;
			if (Physics.Raycast(hit.point + direction * 0.1f, direction, out hitInfo, 5f, BaseLogic.layerSet))
			{
				BloodMarks(hitInfo);
			}
			return;
		}
		if (hit.collider.tag == "Water" && (bool)water2)
		{
			water2.transform.position = hit.point;
			water2.transform.position = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);
			water2.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
			water2.GetComponent<ParticleSystem>().Play();
			return;
		}
		if ((bool)slowSmokeParticles && (bool)slowSmokeParticles)
		{
			slowSmokeParticles.transform.position = hit.point;
			slowSmokeParticles.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
			slowSmokeParticles.GetComponent<ParticleSystem>().Play();
		}
		if ((bool)fastSmokeParticles && (bool)fastSmokeParticles)
		{
			fastSmokeParticles.transform.position = hit.point;
			fastSmokeParticles.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
			fastSmokeParticles.GetComponent<ParticleSystem>().Play();
		}
	}

	private void BulletMarks(RaycastHit hit)
	{
		if ((bool)hit.collider && !(hit.collider.tag == "Water"))
		{
			if (hit.collider.tag == "Untagged" || hit.collider.tag == "Concrete" || hit.collider.tag == "Prostrel" || hit.collider.tag == "Snow")
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(HOLE, hit.point, Quaternion.FromToRotation(Vector3.forward, -hit.normal)) as GameObject;
				gameObject.GetComponent<BulletMarks>().GenerateDecal(1, hit.collider.gameObject);
				UnityEngine.Object.Destroy(gameObject.gameObject, 120f);
			}
			else if (hit.collider.tag == "Metal" || hit.collider.tag == "Usable")
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(HOLE, hit.point, Quaternion.FromToRotation(Vector3.forward, -hit.normal)) as GameObject;
				gameObject2.GetComponent<BulletMarks>().GenerateDecal(3, hit.collider.gameObject);
				UnityEngine.Object.Destroy(gameObject2.gameObject, 120f);
			}
			else if (hit.collider.tag == "Glass")
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate(HOLE, hit.point, Quaternion.FromToRotation(Vector3.forward, -hit.normal)) as GameObject;
				gameObject3.GetComponent<BulletMarks>().GenerateDecal(5, hit.collider.gameObject);
				UnityEngine.Object.Destroy(gameObject3.gameObject, 120f);
			}
			else if (hit.collider.tag == "Wood")
			{
				GameObject gameObject4 = UnityEngine.Object.Instantiate(HOLE, hit.point, Quaternion.FromToRotation(Vector3.forward, -hit.normal)) as GameObject;
				gameObject4.GetComponent<BulletMarks>().GenerateDecal(2, hit.collider.gameObject);
				UnityEngine.Object.Destroy(gameObject4.gameObject, 120f);
			}
			else if (hit.collider.tag == "Tile")
			{
				GameObject gameObject5 = UnityEngine.Object.Instantiate(HOLE, hit.point, Quaternion.FromToRotation(Vector3.forward, -hit.normal)) as GameObject;
				gameObject5.GetComponent<BulletMarks>().GenerateDecal(7, hit.collider.gameObject);
				UnityEngine.Object.Destroy(gameObject5.gameObject, 120f);
			}
		}
	}

	private void BloodMarks(RaycastHit hit)
	{
		if ((bool)hit.collider && hit.collider.gameObject.tag != "Usable" && !(hit.collider.tag == "Water"))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(HOLE, hit.point, Quaternion.FromToRotation(Vector3.forward, -hit.normal)) as GameObject;
			gameObject.GetComponent<BulletMarks>().GenerateDecal(8, hit.collider.gameObject);
			UnityEngine.Object.Destroy(gameObject.gameObject, 120f);
		}
	}

	[PunRPC]
	private void FlashLight(bool ok)
	{
	}

	[PunRPC]
	private void Ban(string _info)
	{
		string[] array = _info.Split('\n');
		int num = Convert.ToInt32(array[0]);
		int ban = Convert.ToInt32(array[1]);
		if (num == PhotonNetwork.player.ID)
		{
			if ((bool)hahaha)
			{
				AudioSource.PlayClipAtPoint(hahaha, base.transform.position);
			}
			BaseLogic.SetBan(ban);
			BaseLogic._player.SendMessage("SaveHero");
			if (PhotonNetwork.room != null)
			{
				PhotonNetwork.LeaveRoom();
			}
		}
	}

	[PunRPC]
	private void Kill(int _id)
	{
		if (_id == PhotonNetwork.player.ID)
		{
			BaseLogic._player.SendMessage("ApplyDamage", 200);
		}
	}

	[PunRPC]
	private void CreateAnimal(string _infoAnimal)
	{
		if (!_lodSys)
		{
			string[] array = _infoAnimal.Split('\n');
			float x = Convert.ToSingle(array[0]);
			float y = Convert.ToSingle(array[1]);
			float z = Convert.ToSingle(array[2]);
			int num = Convert.ToInt32(array[3]);
			Vector3 position = new Vector3(x, y, z);
			GameObject gameObject = null;
			if (num < 100)
			{
				gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_ANIMAL_BOAR"));
			}
			else if (num >= 100 && num < 200)
			{
				gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_ANIMAL_BEAR"));
			}
			else if (num >= 200 && num < 300)
			{
				gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_ANIMAL_WOLF2"));
			}
			else if (num >= 300 && num < 400)
			{
				gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_ANIMAL_WOLF3"));
			}
			else if (num >= 400 && num < 500)
			{
				gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_ANIMAL_WOLF"));
			}
			if ((bool)gameObject)
			{
				gameObject.transform.position = position;
				gameObject.transform.eulerAngles = new Vector3(0f, UnityEngine.Random.Range(0, 360), 0f);
				gameObject.name = "ANIMAL_" + photonView.owner.name + ". TYPE: " + num;
			}
			Debug.Log("CreateAnimal: " + gameObject.name);
		}
	}

	[PunRPC]
	private void DeadAnimal(string namez)
	{
		if (_lodSys)
		{
			return;
		}
		GameObject gameObject = GameObject.Find(namez);
		if (!gameObject)
		{
			return;
		}
		if ((bool)gameObject.GetComponent<AnimalAI>())
		{
			if (!gameObject.GetComponent<AnimalAI>().isDead)
			{
				gameObject.SendMessage("Dead");
			}
		}
		else if ((bool)gameObject.GetComponent<Bird>())
		{
			if (!gameObject.GetComponent<Bird>().dead)
			{
				gameObject.SendMessage("Death2");
			}
		}
		else if ((bool)gameObject.GetComponent<HeroBase>() && !gameObject.GetComponent<HeroBase>()._syncDead && !gameObject.GetComponent<SoldierAI>().dead)
		{
			gameObject.SendMessage("Dead");
		}
	}

	[PunRPC]
	private void SetTargetAnimal(string namez)
	{
		if (_lodSys)
		{
			return;
		}
		string[] array = namez.Split(':');
		string text = array[0];
		float x = Convert.ToSingle(array[1]);
		float y = Convert.ToSingle(array[2]);
		float z = Convert.ToSingle(array[3]);
		int num = Convert.ToInt32(array[4]);
		GameObject gameObject = GameObject.Find(text);
		if ((bool)gameObject)
		{
			if ((bool)gameObject.GetComponent<HeroBase>())
			{
				if (!gameObject.GetComponent<HeroBase>()._syncDead && !gameObject.GetComponent<SoldierAI>().dead)
				{
					gameObject.transform.position = new Vector3(x, y, z);
					gameObject.GetComponent<SoldierAI>().fireTarget = base.transform;
					gameObject.GetComponent<SoldierAI>().Attack();
					return;
				}
				gameObject.GetComponent<HeroBase>().SetHitPoints(50);
				gameObject.GetComponent<HeroBase>()._syncDead = false;
				gameObject.GetComponent<HeroBase>().animator.enabled = true;
				gameObject.SendMessage("RagdollOff");
				gameObject.GetComponent<SoldierAI>().enabled = true;
				gameObject.GetComponent<SoldierAI>().dead = false;
				gameObject.GetComponent<HeroBase>().WEAPONS_HANDS_OBJ.GetComponent<WeaponsBack>()._slotWeapon1 = gameObject.GetComponent<SoldierAI>().startWeapon;
				gameObject.GetComponent<CharacterController>().enabled = true;
			}
			else if (!gameObject.GetComponent<AnimalAI>().isDead && gameObject.GetComponent<AnimalAI>().target == null)
			{
				gameObject.GetComponent<AnimalAI>().controller.enabled = true;
				gameObject.transform.position = new Vector3(x, y, z);
				gameObject.GetComponent<AnimalAI>().target = base.gameObject;
				gameObject.GetComponent<AnimalAI>().Jump();
				gameObject.GetComponent<AudioSource>().maxDistance = 35f;
				gameObject.GetComponent<AudioSource>().clip = gameObject.GetComponent<AnimalAI>().snd_attack;
				gameObject.GetComponent<AudioSource>().volume = 0.6f;
				gameObject.GetComponent<AudioSource>().Play();
			}
		}
		else
		{
			if (num == 100)
			{
				return;
			}
			GameObject gameObject2 = GameObject.Find("_HORROR_SYSTEM(Clone)");
			GameObject gameObject3 = null;
			if (num == 1)
			{
				gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_ANIMAL_WOLF"));
			}
			if (num == 2)
			{
				gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_ANIMAL_BOAR"));
			}
			if (num == 3)
			{
				gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_ANIMAL_BEAR"));
			}
			if (num == 5)
			{
				gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_ANIMAL_VAMPIRE"));
			}
			if (num == 6)
			{
				gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_ANIMAL_GHOUL"));
			}
			if ((bool)gameObject3)
			{
				gameObject3.GetComponent<AnimalAI>().controller.enabled = true;
				gameObject3.name = text;
				gameObject3.transform.position = new Vector3(x, y, z);
				if ((bool)gameObject2)
				{
					gameObject3.transform.parent = gameObject2.transform;
				}
				gameObject3.GetComponent<AnimalAI>().target = base.gameObject;
				gameObject3.GetComponent<AnimalAI>().Jump();
				gameObject3.GetComponent<AudioSource>().maxDistance = 35f;
				gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<AnimalAI>().snd_attack;
				gameObject3.GetComponent<AudioSource>().volume = 0.6f;
				gameObject3.GetComponent<AudioSource>().Play();
			}
		}
	}

	[PunRPC]
	private void SetTargetAnimalSearch(string namez)
	{
		if (!_lodSys)
		{
			GameObject gameObject = GameObject.Find(namez);
			if ((bool)gameObject && !gameObject.GetComponent<AnimalAI>().isDead)
			{
				gameObject.GetComponent<AnimalAI>().MovedTarget = base.transform;
				gameObject.GetComponent<AnimalAI>().status = "Walk";
				gameObject.GetComponent<AnimalAI>().Moved = true;
			}
		}
	}

	[PunRPC]
	private void SetTargetAnimalNull(string namez)
	{
		if (_lodSys)
		{
			return;
		}
		GameObject gameObject = GameObject.Find(namez);
		if (!gameObject)
		{
			return;
		}
		if ((bool)gameObject.GetComponent<AnimalAI>())
		{
			if (!gameObject.GetComponent<AnimalAI>().isDead)
			{
				gameObject.GetComponent<AnimalAI>().controller.enabled = false;
				gameObject.GetComponent<AnimalAI>().target = null;
				gameObject.GetComponent<AnimalAI>().status = "Idle";
				gameObject.GetComponent<AnimalAI>()._model.GetComponent<Animation>().CrossFade(gameObject.GetComponent<AnimalAI>()._animIdle2, 0.2f);
			}
		}
		else if ((bool)gameObject.GetComponent<SoldierAI>())
		{
			gameObject.GetComponent<SoldierAI>().fireTarget = null;
		}
	}

	[PunRPC]
	private void SetFootType(int foot)
	{
		GetComponent<Heroserialize>().steps_type = foot;
	}

	[PunRPC]
	private void InCar(string carname)
	{
		string[] array = carname.Split('\n');
		GameObject gameObject = GameObject.Find(array[0]);
		int num = Convert.ToInt32(array[1]);
		if (!gameObject)
		{
			return;
		}
		VEHICLE = gameObject;
		if ((bool)gameObject.GetComponent<CarSystem>())
		{
			if (num == 1 && (bool)gameObject.GetComponent<CarSystem>().CAR_MORTAR)
			{
				animator.SetBool("Crouched", true);
				animator.SetFloat("Speed", 0f);
			}
			else
			{
				animator.SetBool("InCar", true);
			}
			gameObject.GetComponent<CarSystem>().allow_slot[num] = true;
			base.transform.parent = gameObject.transform;
			base.transform.localPosition = gameObject.GetComponent<CarSystem>().pos_slot[num].transform.localPosition;
			base.transform.localRotation = gameObject.GetComponent<CarSystem>().pos_slot[num].transform.localRotation;
		}
		else if ((bool)gameObject.GetComponent<TrackSystem>())
		{
			gameObject.GetComponent<TrackSystem>().allow_slot[num] = true;
			base.transform.parent = gameObject.transform;
			base.transform.localPosition = gameObject.GetComponent<TrackSystem>().pos_slot[num].transform.localPosition;
			base.transform.localRotation = gameObject.GetComponent<TrackSystem>().pos_slot[num].transform.localRotation;
			animator.SetBool("InCar", true);
		}
		else if ((bool)gameObject.GetComponent<HeliSystem>())
		{
			gameObject.GetComponent<HeliSystem>().allow_slot[num] = true;
			base.transform.parent = gameObject.transform;
			base.transform.localPosition = gameObject.GetComponent<HeliSystem>().pos_slot[num].transform.localPosition;
			base.transform.localRotation = gameObject.GetComponent<HeliSystem>().pos_slot[num].transform.localRotation;
			animator.SetBool("InCar", true);
		}
		if (num == 0)
		{
			if (!GetComponent<Heroserialize>().inVehicle)
			{
				GetComponent<Heroserialize>()._VEHICLE = gameObject;
				GetComponent<Heroserialize>().inVehicle = true;
				GetComponent<Heroserialize>()._noSlowPolet = true;
				GetComponent<Heroserialize>()._noSlowPoletTimer = Time.time;
			}
		}
		else
		{
			GetComponent<Heroserialize>().enabled = false;
		}
	}

	[PunRPC]
	private void InTurret(string carname)
	{
		GameObject gameObject = GameObject.Find(carname);
		if (!gameObject)
		{
			MortarCreateLocal(carname);
			gameObject = GameObject.Find(carname);
		}
		if ((bool)gameObject)
		{
			VEHICLE = gameObject;
			if ((bool)gameObject.GetComponent<MortarSystem>())
			{
				gameObject.GetComponent<MortarSystem>().allow_slot[0] = true;
				base.transform.parent = gameObject.transform;
				base.transform.localPosition = gameObject.GetComponent<MortarSystem>().pos_slot[0].transform.localPosition;
				base.transform.localRotation = gameObject.GetComponent<MortarSystem>().pos_slot[0].transform.localRotation;
				animator.SetBool("Crouched", true);
				animator.SetFloat("Speed", 0f);
				GetComponent<Heroserialize>()._VEHICLE = gameObject;
				GetComponent<Heroserialize>()._VEHICLE_GUN = gameObject.GetComponent<MortarSystem>()._stvol.gameObject;
				GetComponent<Heroserialize>().inTurret = true;
			}
		}
	}

	[PunRPC]
	private void InGunner(string carname)
	{
		string[] array = carname.Split('\n');
		GameObject gameObject = GameObject.Find(array[0]);
		int num = Convert.ToInt32(array[1]);
		if ((bool)gameObject)
		{
			VEHICLE = gameObject;
			animator.SetBool("Crouched", true);
			animator.SetFloat("Speed", 0f);
			base.transform.parent = gameObject.transform;
			if ((bool)gameObject.GetComponent<CarSystem>())
			{
				gameObject.GetComponent<CarSystem>().allow_slot[num] = true;
				base.transform.localPosition = gameObject.GetComponent<CarSystem>().pos_slot[num].transform.localPosition;
			}
			else if ((bool)gameObject.GetComponent<TrackSystem>())
			{
				gameObject.GetComponent<TrackSystem>().allow_slot[num] = true;
				base.transform.localPosition = gameObject.GetComponent<TrackSystem>().pos_slot[num].transform.localPosition;
			}
			else
			{
				gameObject.GetComponent<HeliSystem>().allow_slot[num] = true;
				base.transform.localPosition = gameObject.GetComponent<HeliSystem>().pos_slot[num].transform.localPosition;
			}
			if (photonView.isMine)
			{
				base.transform.parent = BaseLogic._player.transform;
			}
			GetComponent<Heroserialize>().GUNNER = true;
		}
	}

	[PunRPC]
	private void ExitCar(string carname)
	{
		string[] array = carname.Split('\n');
		GameObject gameObject = GameObject.Find(array[0]);
		int num = Convert.ToInt32(array[1]);
		if (!gameObject)
		{
			return;
		}
		VEHICLE = null;
		animator.SetBool("InCar", false);
		animator.SetBool("Crouched", false);
		if ((bool)gameObject.GetComponent<CarSystem>())
		{
			if (num == 0)
			{
				gameObject.GetComponent<CarSystem>().engine_start = false;
			}
			gameObject.GetComponent<CarSystem>().allow_slot[num] = false;
		}
		else if ((bool)gameObject.GetComponent<TrackSystem>())
		{
			gameObject.GetComponent<TrackSystem>().allow_slot[num] = false;
		}
		else if ((bool)gameObject.GetComponent<MortarSystem>())
		{
			gameObject.GetComponent<MortarSystem>().allow_slot[num] = false;
			GetComponent<Heroserialize>()._VEHICLE = null;
			GetComponent<Heroserialize>()._VEHICLE_GUN = null;
			GetComponent<Heroserialize>().inTurret = false;
			GetComponent<Heroserialize>().correct1 = Vector3.zero;
			GetComponent<Heroserialize>().correct2 = Quaternion.identity;
			GetComponent<Heroserialize>().correct3 = Vector3.zero;
		}
		else
		{
			if (num == 0)
			{
				gameObject.GetComponent<HeliSystem>().engine_start = false;
			}
			gameObject.GetComponent<HeliSystem>().allow_slot[num] = false;
		}
		if (num == 0)
		{
			if (!gameObject.GetComponent<HeliSystem>())
			{
				gameObject.GetComponent<AudioSource>().Stop();
			}
			GetComponent<Heroserialize>()._VEHICLE = null;
			GetComponent<Heroserialize>().inVehicle = false;
			GetComponent<Heroserialize>().correct1 = Vector3.zero;
			GetComponent<Heroserialize>().correct2 = Quaternion.identity;
			GetComponent<Heroserialize>().correct3 = Vector3.zero;
		}
		else
		{
			GetComponent<Heroserialize>()._VEHICLE = null;
			GetComponent<Heroserialize>().inVehicle = false;
			GetComponent<Heroserialize>().isGUNNER = false;
			GetComponent<Heroserialize>().enabled = true;
		}
		if (!photonView.isMine)
		{
			SetDefaultParent();
		}
		GetComponent<Heroserialize>().GUNNER = false;
		GetComponent<Heroserialize>().isGUNNER = false;
	}

	[PunRPC]
	private void SetVehicleLight(bool fars)
	{
		if ((bool)VEHICLE)
		{
			VEHICLE.SendMessage("SetLight", fars);
		}
	}

	[PunRPC]
	private void SetVehicleEngine()
	{
		if ((bool)VEHICLE)
		{
			VEHICLE.SendMessage("SetEngine");
			VEHICLE.GetComponent<AudioSource>().Play();
		}
	}

	[PunRPC]
	private void Signal()
	{
		if (!_lodSys && (bool)VEHICLE)
		{
			VEHICLE.GetComponent<CarSystem>()._signal.GetComponent<AudioSource>().volume = 0.4f;
			VEHICLE.GetComponent<CarSystem>()._signal.GetComponent<AudioSource>().clip = VEHICLE.GetComponent<CarSystem>()._signalSnd;
			VEHICLE.GetComponent<CarSystem>()._signal.GetComponent<AudioSource>().pitch = 1f;
			VEHICLE.GetComponent<CarSystem>()._signal.GetComponent<AudioSource>().Play();
		}
	}

	[PunRPC]
	private void DestroyVehicle(string carname)
	{
		GameObject gameObject = GameObject.Find(carname);
		if ((bool)gameObject)
		{
			gameObject.SendMessage("DestroyVehicle");
		}
	}

	[PunRPC]
	private void DamagedVehicle(string carname)
	{
		GameObject gameObject = GameObject.Find(carname);
		if ((bool)gameObject && (bool)gameObject.GetComponent<HeliSystem>())
		{
			gameObject.GetComponent<HeliSystem>()._damaged = 1;
		}
	}

	[PunRPC]
	private void FireVehicle(Vector3 dir)
	{
		if ((bool)VEHICLE)
		{
			VEHICLE.SendMessage("Fire", dir);
		}
	}

	[PunRPC]
	private void FireVehicle2(Vector3 dir)
	{
		if ((bool)VEHICLE)
		{
			VEHICLE.SendMessage("Fire2", dir);
		}
	}

	[PunRPC]
	private void FireMortar(Vector3 dir)
	{
		if (!VEHICLE)
		{
			return;
		}
		if ((bool)VEHICLE.GetComponent<CarSystem>())
		{
			if ((bool)VEHICLE.GetComponent<CarSystem>().CAR_MORTAR)
			{
				VEHICLE.GetComponent<CarSystem>().CAR_MORTAR.SendMessage("FireOff", dir);
			}
		}
		else if ((bool)VEHICLE.GetComponent<MortarSystem>())
		{
			VEHICLE.GetComponent<MortarSystem>().ownerID = photonView.owner.ID;
			VEHICLE.SendMessage("FireOff", dir);
		}
	}

	[PunRPC]
	private void ExploseMine(string name)
	{
		GameObject gameObject = GameObject.Find(name);
		if ((bool)gameObject)
		{
			gameObject.SendMessage("ExplosionMine");
		}
	}

	[PunRPC]
	private void SetRain()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_WEATHER"));
		gameObject.transform.parent = Camera.main.transform;
		gameObject.transform.localPosition = Vector3.zero;
		UnityEngine.Object.Destroy(gameObject, 440f);
	}

	[PunRPC]
	private void ApplyVehicleDamage(string carname, int damage)
	{
		GameObject gameObject = GameObject.Find(carname);
		if (!gameObject || damage > 50)
		{
			return;
		}
		if ((bool)gameObject.GetComponent<CarSystem>())
		{
			if (gameObject.GetComponent<CarSystem>()._control)
			{
				gameObject.SendMessage("ApplyDamage", damage);
			}
		}
		else if ((bool)gameObject.GetComponent<HeliSystem>())
		{
			if (gameObject.GetComponent<HeliSystem>()._control)
			{
				gameObject.SendMessage("ApplyDamage", damage);
			}
		}
		else if ((bool)gameObject.GetComponent<TrackSystem>() && gameObject.GetComponent<TrackSystem>()._control)
		{
			gameObject.SendMessage("ApplyDamage", damage);
		}
	}

	[PunRPC]
	private void ApplyVehicleDamage2(string carname)
	{
		GameObject gameObject = GameObject.Find(carname);
		if ((bool)gameObject)
		{
			gameObject.SendMessage("ApplyDamage", 99000);
		}
	}

	[PunRPC]
	private void ActivateButton(string carname)
	{
		GameObject gameObject = GameObject.Find(carname);
		if ((bool)gameObject)
		{
			gameObject.SendMessage("ActivateObject2");
		}
	}

	[PunRPC]
	private void SetSide(int side)
	{
		SIDE = side;
	}

	[PunRPC]
	private void SetVehicleFuel(string carname, int fuel)
	{
		GameObject gameObject = GameObject.Find(carname);
		if ((bool)gameObject)
		{
			if ((bool)gameObject.GetComponent<CarSystem>())
			{
				gameObject.GetComponent<CarSystem>().FUEL = fuel;
			}
			else if ((bool)gameObject.GetComponent<HeliSystem>())
			{
				gameObject.GetComponent<HeliSystem>().FUEL = fuel;
			}
		}
	}

	[PunRPC]
	private void SetAccuma(string carname)
	{
		GameObject gameObject = GameObject.Find(carname);
		if ((bool)gameObject)
		{
			if ((bool)gameObject.GetComponent<CarSystem>())
			{
				gameObject.GetComponent<CarSystem>().detail_accuma = true;
			}
			else if ((bool)gameObject.GetComponent<HeliSystem>())
			{
				gameObject.GetComponent<HeliSystem>().detail_accuma = true;
			}
		}
	}

	private void MortarCreateLocal(string carname)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_STATIC_GUN_MORTAR"));
		gameObject.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.85f, base.transform.position.z);
		gameObject.transform.rotation = base.transform.rotation;
		gameObject.name = carname;
		SetBackpackLocal(0);
	}

	[PunRPC]
	private void MortarCreate(string carname)
	{
		MortarCreateLocal(carname);
	}

	[PunRPC]
	private void MortarDelete(string name)
	{
		SetBackpackLocal(15);
		GameObject gameObject = GameObject.Find(name);
		if ((bool)gameObject)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	[PunRPC]
	private void GameChat(string text)
	{
		InGameChat.RPCSubmit(text);
	}

	[PunRPC]
	private void DestroyTree(Vector3 pozition)
	{
		if (!_lodSys)
		{
			BaseLogic._playerWeapon.SendMessage("DestroyTree", pozition);
		}
	}

	[PunRPC]
	private void SetHeroItems(int helmet, int glasses, int backpack, int cloth, int bodyArmor)
	{
		SetHelmetLocal(helmet);
		SetGlassesLocal(glasses);
		SetBackpackLocal(backpack);
		SetClothLocal(cloth);
		SetBodyArmorLocal(bodyArmor);
	}
}
