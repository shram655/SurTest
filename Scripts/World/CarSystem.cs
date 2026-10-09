using Photon;
using UnityEngine;

public class CarSystem : Photon.MonoBehaviour
{
	public class WheelData
	{
		public Transform wheelTransform;

		public WheelCollider col;

		public Vector3 wheelStartPos;

		public float rotation;
	}

	public int _VEH_ID;

	public GameObject StandartPrefab;

	public GameObject NormalModel;

	public GameObject DestroyModel;

	public int SIDE;

	public Light fars_light;

	public Material base_material;

	public Material fars_material;

	public Renderer base_model;

	public bool detail_accuma;

	public int FUEL;

	public bool engine_start;

	public bool GUNNER_CAR;

	public bool GUNNER_CAR_KUZOV;

	public Transform EXIT_POS;

	private float _fuelTimer;

	private Vector3 startPos;

	private Quaternion startRot;

	private int _zanos;

	public float accel;

	public float steer;

	public float _driftSpeedMin;

	public float _driftSpeedMax;

	public int _respawnTime;

	private float oldmaxAccel;

	public Vector3 _defaultPos;

	public Quaternion _defaultRot;

	public bool _lodSys;

	public float _lodSysTimer;

	private float _backStiff;

	private float _frontStiff;

	public int _choiceSlot;

	public AudioClip _signalSnd;

	public AudioClip _driftWheels;

	public AudioClip _roadWheels;

	public GameObject _signal;

	private float _oldTimeSignal;

	public float _oldTimeEnterCar;

	public bool _control;

	public bool _passenger;

	public bool MOTO;

	public GameObject PRODUCTS;

	public bool CAR_PRODS;

	public bool CAR_GUN;

	public GameObject CAR_MORTAR;

	public GameObject TURREL;

	public GameObject RUL;

	public GameObject RUL2;

	public bool FWD;

	public bool WD4;

	public WheelCollider[] WColForward;

	public WheelCollider[] WColBack;

	public bool[] WDead;

	public Transform[] wheelsF;

	public Transform[] wheelsB;

	public float wheelOffset = 0.1f;

	public float wheelRadius = 0.4f;

	public float maxSpeed;

	public float maxSteer = 30f;

	public float maxAccel = 25f;

	public float speedcar;

	public Transform COM;

	public bool[] allow_slot = new bool[8];

	public Transform[] pos_slot = new Transform[8];

	private float RPM;

	private int TRANS;

	private GameObject basecam;

	private bool BRAKE;

	private bool BRAKE2;

	private int timer;

	private WheelFrictionCurve BL_SdwCurve;

	private WheelFrictionCurve BR_SdwCurve;

	private WheelFrictionCurve FL_SdwCurve;

	private WheelFrictionCurve FR_SdwCurve;

	private GameObject _player;

	private GameObject _fpsWeapons;

	public float _gtaStyleTimer;

	protected WheelData[] wheels;

	private GameObject _fpsCamera;

	private GameObject _uiBase;

	public bool _RENT;

	public int _RENT_PRICE;

	public int _oldRENT_PRICE;

	public float _respawnCarTimer;

	private float _checkSlotsTimer;

	public int hitPoints = 2500;

	private int _damaged;

	private float _alarm_timer;

	private void ApplyDamage(int damage)
	{
		hitPoints -= damage;
		if (hitPoints <= 1000 && _damaged != 1)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("ApplyVehicleDamage2", PhotonTargets.Others, base.name);
			_alarm_timer = Time.time;
			_damaged = 1;
		}
	}

	private void RespawnVehicle()
	{
		if ((bool)DestroyModel)
		{
			NormalModel.SetActive(false);
			DestroyModel.SetActive(true);
		}
		Component[] componentsInChildren = GetComponentsInChildren<WheelCollider>();
		Component[] array = componentsInChildren;
		Component[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			WheelCollider wheelCollider = (WheelCollider)array2[i];
			Object.Destroy(wheelCollider.gameObject);
		}
		Object.Destroy(base.GetComponent<Rigidbody>());
		Object.Destroy(base.GetComponent<Collider>());
		base.enabled = false;
		base.GetComponent<AudioSource>().enabled = false;
		Object.Destroy(base.gameObject, 60f);
		GameObject gameObject = Object.Instantiate(StandartPrefab.GetComponent<AllVehicles>()._VEHICLES[_VEH_ID], startPos, startRot) as GameObject;
		gameObject.SendMessage("CheckStart");
		gameObject.name = base.name;
		gameObject.GetComponent<CarSystem>().StandartPrefab = StandartPrefab;
		base.name = "DESTROY_VEH";
		Debug.Log("Vehicle: " + base.name + ", RespawnVehicle");
	}

	private void DestroyVehicle()
	{
		if (_control || _passenger)
		{
			ExitCar();
		}
		RespawnVehicle();
	}

	private void SetCameraView(bool view)
	{
		if ((GUNNER_CAR && _choiceSlot == 1) || (GUNNER_CAR_KUZOV && _choiceSlot == 2) || (GUNNER_CAR_KUZOV && _choiceSlot == 3) || (GUNNER_CAR_KUZOV && _choiceSlot == 4) || (GUNNER_CAR_KUZOV && _choiceSlot == 5) || (GUNNER_CAR_KUZOV && _choiceSlot == 6) || (GUNNER_CAR_KUZOV && _choiceSlot == 7))
		{
			return;
		}
		if (view)
		{
			Camera.main.GetComponent<CameraKick>().enabled = false;
			Camera.main.GetComponent<MouseOrb>().enabled = true;
			Camera.main.GetComponent<MouseOrb>().target = base.transform;
			Camera.main.GetComponent<MouseOrb>().maxDistance = 12f;
			Camera.main.GetComponent<MouseOrb>().distance = 8f;
			Camera.main.GetComponent<MouseOrb>().targetHeight = 2f;
			_fpsCamera.GetComponent<CameraInVehicle>().enabled = false;
			if ((bool)Network.PLAYER)
			{
				if (_choiceSlot == 0)
				{
					Network.PLAYER.GetComponent<Animator>().SetBool("InCar", true);
				}
				Network.PLAYER.SendMessage("SetCustomizeView", true);
			}
		}
		else
		{
			Camera.main.GetComponent<CameraKick>().enabled = true;
			Camera.main.GetComponent<MouseOrb>().enabled = false;
			Camera.main.GetComponent<MouseOrb>().target = null;
			Camera.main.GetComponent<MouseOrb>().maxDistance = 10f;
			Camera.main.GetComponent<MouseOrb>().distance = 8f;
			Camera.main.GetComponent<MouseOrb>().targetHeight = 2f;
			_fpsCamera.GetComponent<CameraInVehicle>().enabled = true;
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.SendMessage("SetCustomizeView", false);
			}
		}
	}

	private void ActivateObject()
	{
		if (!(Time.time > _oldTimeEnterCar + 3f) || _player.GetComponent<FPSRigidBodyWalker>().crouched)
		{
			return;
		}
		CheckStart();
		_choiceSlot = 0;
		if (MOTO)
		{
			if (!allow_slot[0])
			{
				base.GetComponent<AudioSource>().Play();
				_control = true;
				_choiceSlot = 0;
			}
			else
			{
				if (allow_slot[1])
				{
					return;
				}
				_passenger = true;
				_choiceSlot = 1;
			}
		}
		else if (!allow_slot[0])
		{
			base.GetComponent<AudioSource>().Play();
			_control = true;
			_choiceSlot = 0;
			if (BaseLogic.GAME_MODE == 6)
			{
				detail_accuma = true;
				FUEL = 100;
				engine_start = true;
			}
		}
		else if (!allow_slot[1])
		{
			_passenger = true;
			_choiceSlot = 1;
			if ((bool)CAR_MORTAR)
			{
				CAR_MORTAR.GetComponent<MortarSystem>()._control = true;
				SetCameraView(true);
			}
		}
		else if (!allow_slot[2])
		{
			_passenger = true;
			_choiceSlot = 2;
		}
		else if (!allow_slot[3])
		{
			_passenger = true;
			_choiceSlot = 3;
		}
		else if (allow_slot.Length > 6)
		{
			if (!allow_slot[4])
			{
				_passenger = true;
				_choiceSlot = 4;
			}
			else if (!allow_slot[5])
			{
				_passenger = true;
				_choiceSlot = 5;
			}
			else if (!allow_slot[6])
			{
				_passenger = true;
				_choiceSlot = 6;
			}
			else
			{
				if (allow_slot[7])
				{
					return;
				}
				_passenger = true;
				_choiceSlot = 7;
			}
		}
		else
		{
			if (allow_slot.Length <= 4)
			{
				return;
			}
			if (!allow_slot[4])
			{
				_passenger = true;
				_choiceSlot = 4;
			}
			else
			{
				if (allow_slot[5])
				{
					return;
				}
				_passenger = true;
				_choiceSlot = 5;
			}
		}
		if ((GUNNER_CAR && _choiceSlot == 1) || (GUNNER_CAR_KUZOV && (_choiceSlot == 2 || _choiceSlot == 3 || _choiceSlot == 5 || _choiceSlot == 6 || _choiceSlot == 7)))
		{
			_oldTimeEnterCar = Time.time;
			_player.GetComponent<Rigidbody>().isKinematic = true;
			_player.SendMessage("SetCrosshairType");
			_player.GetComponent<FPSPlayer>().INCAR = true;
			_player.GetComponent<FPSRigidBodyWalker>().isGUNNER = true;
			_player.GetComponent<CapsuleCollider>().enabled = false;
			_player.GetComponent<AudioSource>().enabled = false;
			_player.transform.parent = base.transform;
			_player.transform.localEulerAngles = Vector3.zero;
			_player.transform.localPosition = new Vector3(pos_slot[_choiceSlot].transform.localPosition.x, pos_slot[_choiceSlot].transform.localPosition.y + 0.4f, pos_slot[_choiceSlot].transform.localPosition.z);
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("InGunner", PhotonTargets.All, base.name + "\n" + _choiceSlot);
			}
			return;
		}
		if ((bool)Network.PLAYER)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("InCar", PhotonTargets.All, base.name + "\n" + _choiceSlot);
		}
		_lodSys = false;
		base.GetComponent<Rigidbody>().isKinematic = false;
		_oldTimeEnterCar = Time.time;
		_fpsWeapons.SetActive(false);
		_fpsCamera.transform.parent = base.transform;
		_fpsCamera.transform.localEulerAngles = Vector3.zero;
		_fpsCamera.GetComponent<SmoothMouseLook>().enabled = false;
		_fpsCamera.GetComponent<CameraInVehicle>().enabled = true;
		_player.GetComponent<Rigidbody>().isKinematic = true;
		_player.GetComponent<FPSPlayer>().INCAR = true;
		_player.GetComponent<FPSRigidBodyWalker>().crouched = false;
		_player.GetComponent<FPSPlayer>().crosshairEnabled = false;
		_player.GetComponent<FPSPlayer>().DisableKeys(true);
		_player.GetComponent<FPSRigidBodyWalker>().enabled = false;
		_player.GetComponent<Footsteps>().enabled = false;
		_player.GetComponent<HorizontalBob>().enabled = false;
		_player.GetComponent<VerticalBob>().enabled = false;
		_player.GetComponent<Ironsights>().enabled = false;
		_player.GetComponent<CapsuleCollider>().enabled = false;
		_player.GetComponent<DragRigidbody>().enabled = false;
		_player.GetComponent<AudioSource>().enabled = false;
		_player.transform.parent = base.transform;
		_player.transform.localEulerAngles = Vector3.zero;
		_player.transform.localPosition = new Vector3(pos_slot[_choiceSlot].transform.localPosition.x, pos_slot[_choiceSlot].transform.localPosition.y + 0.2f, pos_slot[_choiceSlot].transform.localPosition.z);
		WColBack[0].GetComponent<FCarWheel>().enabled = true;
		WColBack[1].GetComponent<FCarWheel>().enabled = true;
		WColForward[0].GetComponent<FCarWheel>().enabled = true;
		if (!MOTO)
		{
			WColForward[1].GetComponent<FCarWheel>().enabled = true;
		}
		Camera.main.GetComponent<CameraKick>().inVeh = true;
		Camera.main.transform.parent = null;
	}

	public void ExitCar()
	{
		if (Time.time > _oldTimeEnterCar + 3f)
		{
			if ((bool)CAR_MORTAR)
			{
				CAR_MORTAR.GetComponent<MortarSystem>()._control = false;
			}
			SetCameraView(false);
			accel = 0f;
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("ExitCar", PhotonTargets.All, base.name + "\n" + _choiceSlot);
			}
			if (_choiceSlot == 0)
			{
				engine_start = false;
				base.GetComponent<AudioSource>().Stop();
			}
			_oldTimeEnterCar = Time.time;
			_fpsWeapons.SetActive(true);
			_fpsCamera.transform.parent = _fpsWeapons.transform.parent;
			_fpsCamera.transform.localEulerAngles = Vector3.zero;
			_fpsCamera.GetComponent<SmoothMouseLook>().enabled = true;
			_fpsCamera.GetComponent<CameraInVehicle>().enabled = false;
			_player.GetComponent<Rigidbody>().isKinematic = false;
			_player.GetComponent<FPSPlayer>().INCAR = false;
			_player.GetComponent<FPSPlayer>().crosshairEnabled = true;
			_player.GetComponent<FPSPlayer>().DisableKeys(false);
			_player.GetComponent<FPSRigidBodyWalker>().enabled = true;
			_player.GetComponent<FPSRigidBodyWalker>().isGUNNER = false;
			_player.GetComponent<Footsteps>().enabled = true;
			_player.GetComponent<HorizontalBob>().enabled = true;
			_player.GetComponent<VerticalBob>().enabled = true;
			_player.GetComponent<Ironsights>().enabled = true;
			_player.GetComponent<CapsuleCollider>().enabled = true;
			_player.GetComponent<DragRigidbody>().enabled = true;
			_player.GetComponent<AudioSource>().enabled = true;
			Network.PLAYER.transform.parent = BaseLogic._player.transform;
			GameObject gameObject = GameObject.Find("!!!FPS Player Main");
			BaseLogic._player.transform.parent = gameObject.transform;
			Network.PLAYER.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			Network.PLAYER.transform.localPosition = new Vector3(0f, -1.088882f, 0f);
			_player.transform.localEulerAngles = Vector3.zero;
			_player.transform.position = EXIT_POS.position;
			_control = false;
			_passenger = false;
			allow_slot[_choiceSlot] = false;
			WColBack[0].GetComponent<FCarWheel>().enabled = false;
			WColBack[1].GetComponent<FCarWheel>().enabled = false;
			WColForward[0].GetComponent<FCarWheel>().enabled = false;
			if (!MOTO)
			{
				WColForward[1].GetComponent<FCarWheel>().enabled = false;
			}
			Camera.main.GetComponent<CameraKick>().inVeh = false;
			Camera.main.transform.parent = _fpsCamera.transform;
		}
	}

	private void Awake()
	{
		GameObject gameObject = GameObject.Find("_BASEPARENTSYNC");
		base.transform.parent = gameObject.transform;
		_oldRENT_PRICE = _RENT_PRICE * 89;
		_lodSys = true;
		base.GetComponent<Rigidbody>().isKinematic = true;
		FUEL = 0;
		WDead = new bool[wheelsF.Length + wheelsB.Length];
		startPos = base.transform.position;
		startRot = base.transform.rotation;
		oldmaxAccel = maxAccel;
		_frontStiff = 0.064f;
		_backStiff = 0.064f;
		WColBack[0].GetComponent<FCarWheel>().enabled = false;
		WColBack[1].GetComponent<FCarWheel>().enabled = false;
		WColForward[0].GetComponent<FCarWheel>().enabled = false;
		if (!MOTO)
		{
			WColForward[1].GetComponent<FCarWheel>().enabled = false;
		}
	}

	private void Start()
	{
		CheckStart();
		_respawnCarTimer = Time.time;
		_defaultPos = base.transform.position;
		_defaultRot = base.transform.rotation;
		RPM = 100f;
		base.GetComponent<Rigidbody>().centerOfMass = COM.localPosition;
		TRANS = 1;
		BL_SdwCurve = WColBack[0].sidewaysFriction;
		BR_SdwCurve = WColBack[1].sidewaysFriction;
		FL_SdwCurve = WColForward[0].sidewaysFriction;
		if (!MOTO)
		{
			FR_SdwCurve = WColForward[1].sidewaysFriction;
		}
		wheels = new WheelData[WColForward.Length + WColBack.Length];
		for (int i = 0; i < WColForward.Length; i++)
		{
			wheels[i] = SetupWheels(wheelsF[i], WColForward[i]);
		}
		for (int j = 0; j < WColBack.Length; j++)
		{
			wheels[j + WColForward.Length] = SetupWheels(wheelsB[j], WColBack[j]);
		}
	}

	private void CheckStart()
	{
		if (!_player)
		{
			_player = GameObject.Find("!!!FPS Player");
		}
		if (!_fpsWeapons)
		{
			_fpsWeapons = GameObject.Find("!!!FPS Weapons");
		}
		if (!_fpsCamera)
		{
			_fpsCamera = GameObject.Find("!!!FPS Camera");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (_control && (bool)Network.PLAYER && other.gameObject.layer == 21 && speedcar >= 15f)
		{
			GameObject heroBase = other.GetComponent<InputDamage>().HeroBase;
			if ((bool)heroBase.GetComponent<Heroserialize>() && heroBase.GetComponent<HeroBase>().SIDE != BaseLogic.SideReal)
			{
				heroBase.GetComponent<Heroserialize>().enabled = false;
				HeroBase component = heroBase.GetComponent<HeroBase>();
				component.animator.enabled = false;
				heroBase.SendMessage("RagdollOn");
				component.photonView.RPC("Death", PhotonTargets.All, PhotonNetwork.player.ID, component.photonView.ownerId);
				component._syncDead = true;
			}
			else
			{
				heroBase.SendMessage("ApplyDamage", 200);
			}
		}
	}

	private WheelData SetupWheels(Transform wheel, WheelCollider col)
	{
		WheelData wheelData = new WheelData();
		wheelData.wheelTransform = wheel;
		wheelData.col = col;
		wheelData.wheelStartPos = wheel.transform.localPosition;
		wheelData.col.radius = wheelRadius;
		return wheelData;
	}

	private void ResetBrakes()
	{
		BRAKE = false;
		WheelCollider[] wColBack = WColBack;
		WheelCollider[] array = wColBack;
		foreach (WheelCollider wheelCollider in array)
		{
			wheelCollider.brakeTorque = 0f;
		}
		WheelCollider[] wColForward = WColForward;
		WheelCollider[] array2 = wColForward;
		foreach (WheelCollider wheelCollider2 in array2)
		{
			wheelCollider2.brakeTorque = 0f;
		}
	}

	private void RepairWheel(int wheel)
	{
		WDead[wheel] = false;
		if (wheel > 1)
		{
			WColBack[wheel - 2].radius = wheelRadius;
			WColBack[wheel - 2].GetComponent<FCarWheel>().damaged = false;
		}
		else
		{
			WColForward[wheel].radius = wheelRadius;
			WColForward[wheel].GetComponent<FCarWheel>().damaged = false;
		}
	}

	private void DamageWheel(int wheel)
	{
		WDead[wheel] = true;
		if (wheel > 1)
		{
			WColBack[wheel - 2].radius = wheelRadius / 1.6f;
			WColBack[wheel - 2].GetComponent<FCarWheel>().damaged = true;
		}
		else
		{
			WColForward[wheel].radius = wheelRadius / 1.6f;
			WColForward[wheel].GetComponent<FCarWheel>().damaged = true;
		}
	}

	private void Update()
	{
		if (_damaged == 1 && Time.time > _alarm_timer + 8f)
		{
			DestroyVehicle();
		}
		if (Time.time > _lodSysTimer + 5f)
		{
			if ((bool)Camera.main)
			{
				float num = Vector3.Distance(Camera.main.transform.position, base.transform.position);
				if (num < 200f)
				{
					if (!NormalModel.activeSelf)
					{
						NormalModel.SetActive(true);
					}
				}
				else if (NormalModel.activeSelf)
				{
					NormalModel.SetActive(false);
				}
				if (num < 50f)
				{
					if (_lodSys)
					{
						base.GetComponent<Rigidbody>().isKinematic = false;
						_lodSys = false;
					}
				}
				else if (!_lodSys)
				{
					base.GetComponent<Rigidbody>().isKinematic = true;
					_lodSys = true;
				}
			}
			_lodSysTimer = Time.time;
		}
		if (Time.time > _checkSlotsTimer + 10f)
		{
			if ((bool)Camera.main)
			{
				Component[] componentsInChildren = GetComponentsInChildren<HeroBase>();
				Component[] array = componentsInChildren;
				if (array.Length == 0)
				{
					for (int i = 0; i < allow_slot.Length; i++)
					{
						allow_slot[i] = false;
					}
				}
			}
			if ((_control || _passenger) && (bool)Network.PLAYER)
			{
				if ((_choiceSlot == 1 && GUNNER_CAR) || (GUNNER_CAR_KUZOV && (_choiceSlot == 2 || _choiceSlot == 3 || _choiceSlot == 4 || _choiceSlot == 5 || _choiceSlot == 6 || _choiceSlot == 7)))
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("InGunner", PhotonTargets.Others, base.name + "\n" + _choiceSlot);
				}
				else
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("InCar", PhotonTargets.Others, base.name + "\n" + _choiceSlot);
				}
			}
			_checkSlotsTimer = Time.time;
		}
		if (_passenger && Input.GetKeyDown("e") && Time.time > _oldTimeEnterCar + 3f)
		{
			ExitCar();
		}
		if (!_control)
		{
			return;
		}
		if (Input.GetKeyDown("e") && speedcar < 10f && Time.time > _oldTimeEnterCar + 3f)
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(EXIT_POS.position, EXIT_POS.TransformDirection(Vector3.left), out hitInfo, 1f))
			{
				Debug.Log("rayhit: " + hitInfo.collider.name);
			}
			else
			{
				ExitCar();
			}
		}
		if (Input.GetKeyDown(KeyCode.Mouse0) && BaseLogic._control && Time.time > _oldTimeSignal + 1f && detail_accuma)
		{
			_signal.GetComponent<AudioSource>().volume = 0.4f;
			_signal.GetComponent<AudioSource>().pitch = 1f;
			_signal.GetComponent<AudioSource>().clip = _signalSnd;
			_signal.GetComponent<AudioSource>().Play();
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("Signal", PhotonTargets.Others);
			}
			_oldTimeSignal = Time.time;
		}
		if (Input.GetKeyDown(KeyCode.L) && BaseLogic._control && Time.time > _oldTimeSignal + 1f && (bool)fars_light && detail_accuma)
		{
			if (fars_light.enabled)
			{
				SetLight(false);
			}
			else
			{
				SetLight(true);
			}
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("SetVehicleLight", PhotonTargets.Others, fars_light.enabled);
			}
			_oldTimeSignal = Time.time;
		}
		if (Input.GetKey("space"))
		{
			if (speedcar > _driftSpeedMin)
			{
				if (speedcar > 20f)
				{
					_signal.GetComponent<AudioSource>().clip = _driftWheels;
					_signal.GetComponent<AudioSource>().volume = 0.1f;
					if (!_signal.GetComponent<AudioSource>().isPlaying)
					{
						_signal.GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1f);
						_signal.GetComponent<AudioSource>().Play();
					}
				}
				if (speedcar > 2f)
				{
					BRAKE = true;
					WheelCollider[] wColBack = WColBack;
					WheelCollider[] array2 = wColBack;
					foreach (WheelCollider wheelCollider in array2)
					{
						wheelCollider.brakeTorque = 50f;
						wheelCollider.motorTorque = 0f;
					}
					WheelCollider[] wColForward = WColForward;
					WheelCollider[] array3 = wColForward;
					foreach (WheelCollider wheelCollider2 in array3)
					{
						wheelCollider2.brakeTorque = 50f;
						wheelCollider2.motorTorque = 0f;
					}
					SetStiffnessBack(0.002f);
					SetStiffnessFront(0.004f);
					SetForwardStiffnessBack(0.001f);
					SetForwardStiffnessFront(0.002f);
				}
			}
			else
			{
				BRAKE2 = true;
				WheelCollider[] wColBack2 = WColBack;
				WheelCollider[] array4 = wColBack2;
				foreach (WheelCollider wheelCollider3 in array4)
				{
					wheelCollider3.brakeTorque = 50f;
					wheelCollider3.motorTorque = 0f;
				}
				WheelCollider[] wColForward2 = WColForward;
				WheelCollider[] array5 = wColForward2;
				foreach (WheelCollider wheelCollider4 in array5)
				{
					wheelCollider4.brakeTorque = 50f;
					wheelCollider4.motorTorque = 0f;
				}
			}
		}
		else if (Input.GetKeyUp("space"))
		{
			if (_signal.GetComponent<AudioSource>().isPlaying)
			{
				_signal.GetComponent<AudioSource>().Stop();
			}
			BRAKE = false;
			BRAKE2 = false;
			WheelCollider[] wColBack3 = WColBack;
			WheelCollider[] array6 = wColBack3;
			foreach (WheelCollider wheelCollider5 in array6)
			{
				wheelCollider5.brakeTorque = 0f;
			}
			WheelCollider[] wColForward3 = WColForward;
			WheelCollider[] array7 = wColForward3;
			foreach (WheelCollider wheelCollider6 in array7)
			{
				wheelCollider6.brakeTorque = 0f;
			}
			maxAccel = oldmaxAccel;
		}
	}

	private void SetLight(bool fars)
	{
		if (fars)
		{
			Material[] materials = base_model.GetComponent<Renderer>().materials;
			materials[0] = base_material;
			materials[1] = fars_material;
			base_model.GetComponent<Renderer>().materials = materials;
			fars_light.enabled = true;
		}
		else
		{
			Material[] materials2 = base_model.GetComponent<Renderer>().materials;
			materials2[0] = base_material;
			materials2[1] = base_material;
			base_model.GetComponent<Renderer>().materials = materials2;
			fars_light.enabled = false;
		}
	}

	private void SetEngine()
	{
		engine_start = true;
	}

	private void FixedUpdate()
	{
		if (_lodSys)
		{
			return;
		}
		speedcar = base.GetComponent<Rigidbody>().velocity.magnitude * 2.6f;
		if (allow_slot[0] && engine_start && !_control)
		{
			base.GetComponent<AudioSource>().pitch = 0.3f + speedcar / 70f;
		}
		if (_control)
		{
			accel = Input.GetAxis("Vertical");
			float num = 0f;
			num = 0.025f - (5f + speedcar) / 6000f;
			if (Input.GetAxis("Horizontal") > 0f)
			{
				if (steer < 1f)
				{
					if (steer < 0f)
					{
						steer += 0.025f;
					}
					else
					{
						steer += num;
					}
				}
			}
			else if (Input.GetAxis("Horizontal") < 0f && steer > -1f)
			{
				if (steer > 0f)
				{
					steer -= 0.025f;
				}
				else
				{
					steer -= num;
				}
			}
			if (Input.GetAxis("Horizontal") == 0f)
			{
				if (steer > 0.04f)
				{
					steer -= 0.025f;
				}
				else if (steer < -0.04f)
				{
					steer += 0.025f;
				}
			}
			if (accel != 0f && engine_start && Time.time > _fuelTimer + 10f)
			{
				_fuelTimer = Time.time;
				if (FUEL > 0)
				{
					FUEL--;
				}
			}
			if (FUEL == 0 || !engine_start)
			{
				engine_start = false;
				accel = 0f;
				if (base.GetComponent<AudioSource>().isPlaying)
				{
					base.GetComponent<AudioSource>().Stop();
				}
			}
			if (steer > 1f)
			{
				steer = 1f;
			}
			else if (steer < -1f)
			{
				steer = -1f;
			}
			if (speedcar < _driftSpeedMin && accel != 0f && !BRAKE && !BRAKE2)
			{
				_signal.GetComponent<AudioSource>().clip = _driftWheels;
				_signal.GetComponent<AudioSource>().volume = 0.1f;
				if (!_signal.GetComponent<AudioSource>().isPlaying)
				{
					_signal.GetComponent<AudioSource>().Play();
					_signal.GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1f);
				}
				maxAccel = oldmaxAccel * 90f;
				if (WD4)
				{
					SetForwardStiffnessBack(0.0005f);
					SetForwardStiffnessFront(0.0005f);
				}
				else if (FWD)
				{
					SetForwardStiffnessFront(0.0005f);
				}
				else
				{
					SetForwardStiffnessBack(0.0005f);
				}
				if (accel > 0f)
				{
					SetStiffnessBack(0.002f);
					SetStiffnessFront(0.004f);
				}
				else
				{
					SetStiffnessBack(0.004f);
					SetStiffnessFront(0.002f);
				}
			}
			else if (_zanos < 2)
			{
				if (_signal.GetComponent<AudioSource>().isPlaying && !BRAKE && _signal.GetComponent<AudioSource>().clip == _driftWheels)
				{
					_signal.GetComponent<AudioSource>().Stop();
				}
				maxAccel = oldmaxAccel;
				SetForwardStiffnessBack(0.092f);
				SetForwardStiffnessFront(0.092f);
			}
			if (speedcar > _driftSpeedMax && (steer > 0.9f || steer < -0.9f))
			{
				if (_zanos < 180)
				{
					_zanos += 2;
				}
				if (_zanos > 30 && !BRAKE)
				{
					SetForwardStiffnessBack(0.0005f);
					SetForwardStiffnessFront(0.0005f);
					SetStiffnessBack(0.0025f);
					SetStiffnessFront(0.005f);
					_signal.GetComponent<AudioSource>().clip = _driftWheels;
					_signal.GetComponent<AudioSource>().volume = 0.1f;
					if (!_signal.GetComponent<AudioSource>().isPlaying)
					{
						_signal.GetComponent<AudioSource>().Play();
						_signal.GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1f);
					}
					if (accel > 0f)
					{
						RPM += Input.GetAxis("Vertical") * 1.2f / (float)TRANS * 2f;
					}
				}
			}
			else if (_zanos >= 2)
			{
				_zanos -= 2;
			}
			if (!BRAKE && _zanos < 10)
			{
				if (_backStiff < 0.064f)
				{
					SetStiffnessBack(_backStiff += 0.00015f);
				}
				if (_frontStiff < 0.064f)
				{
					SetStiffnessFront(_frontStiff += 0.0001f);
				}
			}
			if (!MOTO && (bool)RUL)
			{
				RUL.transform.localRotation = Quaternion.Lerp(RUL.transform.localRotation, Quaternion.Euler(RUL.transform.localEulerAngles.x, RUL.transform.localEulerAngles.y, 70f * steer), Time.deltaTime * 3f);
			}
			if (accel > 0f)
			{
				RPM += Input.GetAxis("Vertical") * 5.2f / (float)TRANS / 4f;
			}
			else if (RPM > 100f)
			{
				RPM -= 3f;
			}
			if (RPM > 650f)
			{
				if (TRANS < 4)
				{
					RPM = 300f;
					TRANS++;
				}
				else
				{
					RPM = 650f;
				}
			}
			if (RPM < 300f && TRANS > 1)
			{
				RPM = 400f;
				TRANS--;
			}
			base.GetComponent<AudioSource>().pitch = 0.7f + RPM / 1400f;
			if (speedcar > maxSpeed && accel > 0f)
			{
				accel = 0f;
			}
			else if (speedcar > 30f && accel < 0f)
			{
				accel = 0f;
			}
			base.GetComponent<Rigidbody>().centerOfMass = COM.localPosition;
		}
		CarMove(accel, steer);
		UpdateWheels();
	}

	private void UpdateWheels()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		if (_lodSys)
		{
			return;
		}
		int num = 0;
		WheelData[] array = wheels;
		WheelData[] array2 = array;
		foreach (WheelData wheelData in array2)
		{
			Vector3 localPosition = wheelData.wheelTransform.localPosition;
			WheelHit hit;
			if (wheelData.col.GetGroundHit(out hit))
			{
				if (WDead[num])
				{
					localPosition.y -= Vector3.Dot(wheelData.wheelTransform.position - hit.point, base.transform.up) - wheelRadius / 1.6f;
				}
				else
				{
					localPosition.y -= Vector3.Dot(wheelData.wheelTransform.position - hit.point, base.transform.up) - wheelRadius;
				}
			}
			else
			{
				wheelData.col.motorTorque = 0f;
				localPosition.y = wheelData.wheelStartPos.y - wheelOffset;
			}
			wheelData.wheelTransform.localPosition = localPosition;
			wheelData.rotation = Mathf.Repeat(wheelData.rotation + fixedDeltaTime * wheelData.col.rpm * 360f / 60f, 360f);
			if (MOTO)
			{
				if (WDead[num])
				{
					wheelData.wheelTransform.localRotation = Quaternion.Euler(wheelData.rotation, 0f, 98f);
				}
				else
				{
					wheelData.wheelTransform.localRotation = Quaternion.Euler(wheelData.rotation, 0f, 90f);
				}
			}
			else if (WDead[num])
			{
				wheelData.wheelTransform.localRotation = Quaternion.Euler(wheelData.rotation, wheelData.col.steerAngle, 98f);
			}
			else
			{
				wheelData.wheelTransform.localRotation = Quaternion.Euler(wheelData.rotation, wheelData.col.steerAngle, 90f);
			}
			if (!BRAKE && !BRAKE2 && allow_slot[0])
			{
				if (WDead[num])
				{
					wheelData.col.brakeTorque = 25f;
				}
				else if (!BRAKE2)
				{
					wheelData.col.brakeTorque = 0f;
				}
				num++;
			}
			else if (!allow_slot[0])
			{
				wheelData.col.brakeTorque = 125f;
			}
		}
	}

	private void CarMove(float accel, float steer)
	{
		int num = 0;
		WheelCollider[] wColForward = WColForward;
		WheelCollider[] array = wColForward;
		foreach (WheelCollider wheelCollider in array)
		{
			if (WDead[num])
			{
				wheelCollider.steerAngle = steer * 10f / (speedcar * 2.2f / maxSteer + 1f);
			}
			else if (speedcar < _driftSpeedMin || speedcar < 20f)
			{
				wheelCollider.steerAngle = steer * 40f / (speedcar * 2.6f / maxSteer + 1f);
			}
			else
			{
				wheelCollider.steerAngle = steer * 40f / (speedcar * 4.5f / maxSteer + 1f);
			}
			if (MOTO && (bool)RUL)
			{
				RUL.transform.localEulerAngles = new Vector3(RUL.transform.localEulerAngles.x, wheelCollider.steerAngle, RUL.transform.localEulerAngles.z);
				RUL2.transform.localEulerAngles = new Vector3(RUL2.transform.localEulerAngles.x, wheelCollider.steerAngle, RUL2.transform.localEulerAngles.z);
			}
			num++;
		}
		if (accel == 0f && !BRAKE && !BRAKE2)
		{
			if (FWD)
			{
				WheelCollider[] wColForward2 = WColForward;
				WheelCollider[] array2 = wColForward2;
				foreach (WheelCollider wheelCollider2 in array2)
				{
					wheelCollider2.brakeTorque = 20f;
					wheelCollider2.motorTorque = 0f;
				}
			}
			else
			{
				WheelCollider[] wColBack = WColBack;
				WheelCollider[] array3 = wColBack;
				foreach (WheelCollider wheelCollider3 in array3)
				{
					wheelCollider3.brakeTorque = 20f;
					wheelCollider3.motorTorque = 0f;
				}
			}
			if (WD4)
			{
				WheelCollider[] wColForward3 = WColForward;
				WheelCollider[] array4 = wColForward3;
				foreach (WheelCollider wheelCollider4 in array4)
				{
					wheelCollider4.brakeTorque = 20f;
					wheelCollider4.motorTorque = 0f;
				}
			}
		}
		else
		{
			if (accel == 0f || BRAKE || BRAKE2)
			{
				return;
			}
			if (FWD)
			{
				WheelCollider[] wColForward4 = WColForward;
				WheelCollider[] array5 = wColForward4;
				foreach (WheelCollider wheelCollider5 in array5)
				{
					wheelCollider5.brakeTorque = 0f;
					wheelCollider5.motorTorque = accel * maxAccel;
				}
			}
			else
			{
				WheelCollider[] wColBack2 = WColBack;
				WheelCollider[] array6 = wColBack2;
				foreach (WheelCollider wheelCollider6 in array6)
				{
					wheelCollider6.brakeTorque = 0f;
					wheelCollider6.motorTorque = accel * maxAccel;
				}
			}
			if (WD4)
			{
				WheelCollider[] wColForward5 = WColForward;
				WheelCollider[] array7 = wColForward5;
				foreach (WheelCollider wheelCollider7 in array7)
				{
					wheelCollider7.brakeTorque = 0f;
					wheelCollider7.motorTorque = accel * maxAccel;
				}
			}
		}
	}

	private void SetStiffness(float stiff)
	{
		FL_SdwCurve.stiffness = stiff;
		WColForward[0].sidewaysFriction = FL_SdwCurve;
		FR_SdwCurve.stiffness = stiff;
		if (!MOTO)
		{
			WColForward[1].sidewaysFriction = FR_SdwCurve;
		}
		BL_SdwCurve.stiffness = stiff;
		WColBack[0].sidewaysFriction = BL_SdwCurve;
		BR_SdwCurve.stiffness = stiff;
		WColBack[1].sidewaysFriction = BR_SdwCurve;
	}

	private void SetStiffnessFront(float stiff)
	{
		_frontStiff = stiff;
		FL_SdwCurve.stiffness = stiff;
		WColForward[0].sidewaysFriction = FL_SdwCurve;
		FR_SdwCurve.stiffness = stiff;
		if (!MOTO)
		{
			WColForward[1].sidewaysFriction = FR_SdwCurve;
		}
	}

	private void SetStiffnessBack(float stiff)
	{
		_backStiff = stiff;
		BL_SdwCurve.stiffness = stiff;
		WColBack[0].sidewaysFriction = BL_SdwCurve;
		BR_SdwCurve.stiffness = stiff;
		WColBack[1].sidewaysFriction = BR_SdwCurve;
	}

	private void SetForwardStiffnessFront(float stiff)
	{
		FL_SdwCurve.stiffness = stiff;
		WColForward[0].forwardFriction = FL_SdwCurve;
		FR_SdwCurve.stiffness = stiff;
		if (!MOTO)
		{
			WColForward[1].forwardFriction = FR_SdwCurve;
		}
	}

	private void SetForwardStiffnessBack(float stiff)
	{
		BL_SdwCurve.stiffness = stiff;
		WColBack[0].forwardFriction = BL_SdwCurve;
		BR_SdwCurve.stiffness = stiff;
		WColBack[1].forwardFriction = BR_SdwCurve;
	}
}
