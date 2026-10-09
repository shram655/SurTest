using System.Collections.Generic;
using UnityEngine;

public class TrackSystem : MonoBehaviour
{
	public class WheelData
	{
		public Transform wheelTransform;

		public Transform boneTransform;

		public WheelCollider col;

		public Vector3 wheelStartPos;

		public Vector3 boneStartPos;

		public float rotation;

		public Quaternion startWheelAngle;
	}

	public int _VEH_ID;

	public GameObject StandartPrefab;

	public GameObject NormalModel;

	public GameObject DestroyModel;

	public int SIDE;

	public int hitPoints = 5000;

	public bool GUNNER_CAR;

	public Transform EXIT_POS;

	public GameObject _explode;

	private int _choiceSlot;

	public int _SIDE;

	private float _checkSlotsTimer;

	private bool _lodSys;

	private float _lodSysTimer;

	public GameObject _TURRET;

	public Renderer _GUN_FLASH;

	public Light FLASH_LIGHT;

	private int FLASH_TIME;

	public int GUN_MM;

	public GameObject _GUN;

	public float fireRateReal = 0.16f;

	public float fireRateReal2 = 2f;

	public int gunDamage1 = 100;

	public int gunDamage2 = 30;

	public bool CAR_GUN;

	public GameObject _destroyPrefab;

	public bool _control;

	public bool _GunControl;

	public bool _passenger;

	public float speedmultiplayer = 10f;

	public Transform COM;

	public GameObject wheelCollider;

	public float wheelRadius = 0.15f;

	public float suspensionOffset = 0.05f;

	public float trackTextureSpeed = 2.5f;

	public bool MirrorTracks;

	public GameObject leftTrack;

	public Transform[] leftTrackUpperWheels;

	public Transform[] leftTrackWheels;

	public Transform[] leftTrackBones;

	public GameObject rightTrack;

	public Transform[] rightTrackUpperWheels;

	public Transform[] rightTrackWheels;

	public Transform[] rightTrackBones;

	public float speedcar;

	public bool[] allow_slot = new bool[8];

	public Transform[] pos_slot = new Transform[8];

	public int _fireTimer;

	private int _checkWheel;

	protected WheelData[] leftTrackWheelData;

	protected WheelData[] rightTrackWheelData;

	protected float leftTrackTextureOffset;

	protected float rightTrackTextureOffset;

	private GameObject _mainCamera;

	private GameObject _fpsCamera;

	private GameObject _player;

	private GameObject _fpsWeapons;

	public int _choicePassSlot;

	private float accelerate;

	private float steer;

	private float _oldTime;

	public float _oldTimeEnterCar;

	private Vector3 startPos;

	private Quaternion startRot;

	public float rotateOnStandTorque = 1500f;

	public float rotateOnStandBrakeTorque = 500f;

	public float maxBrakeTorque = 1000f;

	public float forwardTorque = 500f;

	public float rotateOnMoveBrakeTorque = 400f;

	public float minBrakeTorque;

	public float minOnStayStiffness = 0.06f;

	public float minOnMoveStiffness = 0.05f;

	public float rotateOnMoveMultiply = 2f;

	public float _respawnCarTimer;

	private int ammo1 = 750;

	public GameObject Bullet1;

	public GameObject Bullet2;

	public Transform[] RocketPos;

	private float fireRate;

	public int _damaged;

	private float _alarm_timer;

	private Vector3 gunPositionStartFull;

	private int wheelid;

	private void SetAmmo()
	{
		if (GUN_MM == 25)
		{
			ammo1 = 2000;
		}
		else if (GUN_MM == 30)
		{
			ammo1 = 1500;
		}
		else if (GUN_MM == 73)
		{
			ammo1 = 60;
		}
	}

	private void ApplyDamage(int damage)
	{
		hitPoints -= damage;
		if (hitPoints <= 1000 && _damaged != 1)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("ApplyVehicleDamage2", PhotonTargets.Others, base.name);
			_alarm_timer = Time.time;
			_damaged = 1;
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("_DAMAGE_EFFECT"));
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
		}
	}

	private void RespawnVehicle()
	{
		NormalModel.SetActive(false);
		DestroyModel.SetActive(true);
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
		gameObject.GetComponent<TrackSystem>().StandartPrefab = StandartPrefab;
		base.name = "DESTROY_VEH";
		Debug.Log("Vehicle: " + base.name + ", RespawnVehicle");
	}

	private void DestroyVehicle()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("_EXPLOSION"));
		gameObject.GetComponent<Explosion>()._carBoom = true;
		gameObject.transform.position = base.transform.position;
		gameObject.transform.rotation = Quaternion.identity;
		if (_control || _passenger)
		{
			ExitCar();
		}
		RespawnVehicle();
		_damaged = 0;
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

	private void SetCameraView(bool view)
	{
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
				Network.PLAYER.SendMessage("SetCustomizeView", true);
			}
		}
		else
		{
			Camera.main.GetComponent<CameraKick>().enabled = true;
			Camera.main.GetComponent<MouseOrb>().enabled = false;
			Camera.main.GetComponent<MouseOrb>().target = null;
			Camera.main.GetComponent<MouseOrb>().maxDistance = 15f;
			Camera.main.GetComponent<MouseOrb>().distance = 15f;
			Camera.main.GetComponent<MouseOrb>().targetHeight = 2f;
			_fpsCamera.GetComponent<CameraInVehicle>().enabled = true;
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.SendMessage("SetCustomizeView", false);
			}
		}
	}

	private void NextPass()
	{
		if (allow_slot[0] && _control && !allow_slot[1])
		{
			allow_slot[0] = false;
			_control = false;
			_GunControl = true;
			_passenger = true;
			_choiceSlot = 1;
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("InTurret", PhotonTargets.All, base.name);
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
		if (!allow_slot[0])
		{
			base.GetComponent<AudioSource>().Play();
			_control = true;
			_choiceSlot = 0;
		}
		else if (!allow_slot[1])
		{
			_GunControl = true;
			_passenger = true;
			_choiceSlot = 1;
		}
		else
		{
			if (allow_slot[2])
			{
				return;
			}
			_passenger = true;
			_choiceSlot = 2;
		}
		_lodSys = false;
		base.GetComponent<Rigidbody>().isKinematic = false;
		_oldTimeEnterCar = Time.time;
		_fpsWeapons.SetActive(false);
		if (GUNNER_CAR && _choiceSlot == 1)
		{
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("InTurret", PhotonTargets.All, base.name);
			}
		}
		else if ((bool)Network.PLAYER)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("InCar", PhotonTargets.All, base.name + "\n" + _choiceSlot);
		}
		_fpsCamera.transform.parent = base.transform;
		_player.GetComponent<FPSPlayer>().crosshairEnabled = false;
		_fpsCamera.GetComponent<SmoothMouseLook>().enabled = false;
		_fpsCamera.GetComponent<CameraInVehicle>().enabled = true;
		_fpsCamera.transform.localEulerAngles = Vector3.zero;
		Camera.main.GetComponent<CameraKick>().inVeh = true;
		_player.GetComponent<Rigidbody>().isKinematic = true;
		_player.SendMessage("SetCrosshairType");
		_player.GetComponent<FPSPlayer>().INCAR = true;
		_player.GetComponent<FPSRigidBodyWalker>().crouched = false;
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
		_player.transform.localPosition = new Vector3(pos_slot[_choiceSlot].transform.localPosition.x - 0.4f, pos_slot[_choiceSlot].transform.localPosition.y - 0.3f, pos_slot[_choiceSlot].transform.localPosition.z);
		Camera.main.GetComponent<CameraKick>().inVeh = true;
		Camera.main.transform.parent = null;
	}

	public void ExitCar()
	{
		Debug.Log("Exit Car Func!");
		if (Time.time > _oldTimeEnterCar + 3f)
		{
			SetCameraView(false);
			accelerate = 0f;
			steer = 0f;
			Camera.main.GetComponent<CameraKick>().inVeh = false;
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("ExitCar", PhotonTargets.All, base.name + "\n" + _choiceSlot);
			}
			if (_choiceSlot == 0)
			{
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
			_GunControl = false;
			allow_slot[_choiceSlot] = false;
			Camera.main.GetComponent<CameraKick>().inVeh = false;
			Camera.main.transform.parent = _fpsCamera.transform;
		}
	}

	private void Awake()
	{
		GameObject gameObject = GameObject.Find("_BASEPARENTSYNC");
		base.transform.parent = gameObject.transform;
		gunPositionStartFull = _GUN.transform.localPosition;
		_GUN_FLASH.enabled = false;
		FLASH_LIGHT.GetComponent<Light>().enabled = false;
		SetAmmo();
		startPos = base.transform.position;
		startRot = base.transform.rotation;
		leftTrackWheelData = new WheelData[leftTrackWheels.Length];
		rightTrackWheelData = new WheelData[rightTrackWheels.Length];
		for (int i = 0; i < leftTrackWheels.Length; i++)
		{
			leftTrackWheelData[i] = SetupWheels(leftTrackWheels[i], leftTrackBones[i]);
		}
		for (int j = 0; j < rightTrackWheels.Length; j++)
		{
			rightTrackWheelData[j] = SetupWheels(rightTrackWheels[j], rightTrackBones[j]);
		}
		Vector3 position = base.transform.position;
		position.z += 0.01f;
		base.transform.position = position;
	}

	private void Start()
	{
		CheckStart();
		base.GetComponent<Rigidbody>().centerOfMass = COM.localPosition;
	}

	private WheelData SetupWheels(Transform wheel, Transform bone)
	{
		_checkWheel++;
		WheelData wheelData = new WheelData();
		GameObject gameObject = new GameObject("Collider_" + wheel.name);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = wheel.position;
		gameObject.transform.localRotation = Quaternion.Euler(0f, wheel.localRotation.y, 0f);
		WheelCollider wheelCollider = (WheelCollider)gameObject.AddComponent(typeof(WheelCollider));
		WheelCollider component = this.wheelCollider.GetComponent<WheelCollider>();
		if ((_checkWheel == 1 || _checkWheel == 7) && rightTrackWheels.Length == 6)
		{
			FCarWheel fCarWheel = (FCarWheel)gameObject.AddComponent(typeof(FCarWheel));
			fCarWheel.TANK = true;
			fCarWheel.wdWheel = true;
			fCarWheel.enabled = true;
			fCarWheel.carRigidbody = base.GetComponent<Rigidbody>();
			fCarWheel.wheelTransform = wheel;
		}
		else if (_checkWheel == 1 || _checkWheel == 8)
		{
			FCarWheel fCarWheel2 = (FCarWheel)gameObject.AddComponent(typeof(FCarWheel));
			fCarWheel2.TANK = true;
			fCarWheel2.wdWheel = true;
			fCarWheel2.enabled = true;
			fCarWheel2.carRigidbody = base.GetComponent<Rigidbody>();
			fCarWheel2.wheelTransform = wheel;
		}
		wheelCollider.gameObject.layer = 0;
		wheelCollider.mass = component.mass;
		wheelCollider.center = component.center;
		wheelCollider.radius = component.radius;
		wheelCollider.suspensionDistance = component.suspensionDistance;
		wheelCollider.suspensionSpring = component.suspensionSpring;
		wheelCollider.forwardFriction = component.forwardFriction;
		wheelCollider.sidewaysFriction = component.sidewaysFriction;
		wheelData.wheelTransform = wheel;
		wheelData.boneTransform = bone;
		wheelData.col = wheelCollider;
		wheelData.wheelStartPos = wheel.transform.localPosition;
		wheelData.boneStartPos = bone.transform.localPosition;
		wheelData.startWheelAngle = wheel.transform.localRotation;
		return wheelData;
	}

	private void Fire(Vector3 direction)
	{
		if ((bool)Bullet1)
		{
			base.GetComponent<Rigidbody>().AddForceAtPosition(595000f * _GUN.transform.TransformDirection(Vector3.back), base.transform.position);
			GameObject gameObject = Object.Instantiate(Bullet1, _GUN_FLASH.transform.position, _GUN_FLASH.transform.rotation) as GameObject;
			if (!_GunControl)
			{
				gameObject.GetComponent<Ballistic>().isOthers = true;
			}
			else
			{
				gameObject.GetComponent<Ballistic>().weaponObj = BaseLogic._playerWeapon;
			}
			gameObject.GetComponent<Ballistic>().SetDamage(gunDamage1);
			gameObject.GetComponent<Ballistic>().SIDE = SIDE;
			gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 70f);
			gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.forward, direction);
		}
		if ((bool)_TURRET.GetComponent<AudioSource>())
		{
			_TURRET.GetComponent<AudioSource>().Play();
		}
		ammo1--;
		_GUN_FLASH.GetComponent<Renderer>().enabled = true;
		FLASH_LIGHT.GetComponent<Light>().enabled = true;
		_fireTimer = 3;
		_GUN.transform.Translate(new Vector3(0f, 0f, -0.5f));
	}

	private void Update()
	{
		if (_damaged == 1 && Time.time > _alarm_timer + 8f)
		{
			DestroyVehicle();
		}
		if (_GunControl && BaseLogic._control)
		{
			if (Input.GetKey(KeyCode.Mouse0) && Time.time > fireRate + fireRateReal)
			{
				if (ammo1 > 0)
				{
					Vector3 vector = _GUN_FLASH.transform.TransformDirection(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 1f);
					Fire(vector);
					if ((bool)Network.PLAYER)
					{
						Network.PLAYER.GetComponent<PhotonView>().RPC("FireVehicle", PhotonTargets.Others, vector);
					}
				}
				fireRate = Time.time;
			}
			if (Input.GetKey(KeyCode.D))
			{
				_TURRET.transform.Rotate(0f, 0.1f, 0f);
			}
			if (Input.GetKey(KeyCode.A))
			{
				_TURRET.transform.Rotate(0f, -0.1f, 0f);
			}
			if (Input.GetKey(KeyCode.W))
			{
				_GUN.transform.Rotate(-0.1f, 0f, 0f);
			}
			if (Input.GetKey(KeyCode.S))
			{
				_GUN.transform.Rotate(0.1f, 0f, 0f);
			}
		}
		if (Time.time > _lodSysTimer + 10f && (bool)Camera.main)
		{
			float num = Vector3.Distance(Camera.main.transform.position, base.transform.position);
			if (num < 350f)
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
				if (_choiceSlot == 1)
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("InTurret", PhotonTargets.Others, base.name);
				}
				else
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("InCar", PhotonTargets.Others, base.name + "\n" + _choiceSlot);
				}
			}
			_checkSlotsTimer = Time.time;
		}
		if (_passenger && Input.GetKeyDown("e") && speedcar < 10f && Time.time > _oldTimeEnterCar + 3f)
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(EXIT_POS.position, EXIT_POS.TransformDirection(Vector3.left), out hitInfo, 2f))
			{
				Debug.Log("rayhit: " + hitInfo.collider.name);
			}
			else
			{
				ExitCar();
			}
		}
		if (_control && Input.GetKeyDown("e") && accelerate == 0f && steer == 0f && speedcar < 10f && Time.time > _oldTimeEnterCar + 3f)
		{
			ExitCar();
		}
	}

	private void FixedUpdate()
	{
		if (_lodSys)
		{
			return;
		}
		if (_GUN.transform.localPosition != gunPositionStartFull)
		{
			_GUN.transform.Translate(new Vector3(0f, 0f, 0.05f));
			FLASH_TIME++;
			if (FLASH_TIME == 4)
			{
				_GUN_FLASH.enabled = false;
				FLASH_LIGHT.GetComponent<Light>().enabled = false;
			}
			else if (FLASH_TIME == 8)
			{
				_GUN.transform.localPosition = gunPositionStartFull;
				FLASH_TIME = 0;
			}
		}
		base.GetComponent<Rigidbody>().centerOfMass = COM.localPosition;
		speedcar = base.GetComponent<Rigidbody>().velocity.magnitude * 2.6f;
		accelerate = 0f;
		steer = 0f;
		if (_control && BaseLogic._control)
		{
			if (!base.GetComponent<AudioSource>().enabled)
			{
				base.GetComponent<AudioSource>().enabled = true;
			}
			accelerate = Input.GetAxis("Vertical");
			steer = Input.GetAxis("Horizontal");
			if (accelerate != 0f)
			{
				if (accelerate > 0f)
				{
					base.GetComponent<AudioSource>().pitch = 0.5f + accelerate / 1.5f;
				}
				else
				{
					base.GetComponent<AudioSource>().pitch = 0.5f + (0f - accelerate) / 1.5f;
				}
			}
			else if (steer != 0f)
			{
				if (steer > 0f)
				{
					base.GetComponent<AudioSource>().pitch = 0.5f + steer / 1.5f;
				}
				else
				{
					base.GetComponent<AudioSource>().pitch = 0.5f + (0f - steer) / 1.5f;
				}
			}
			else
			{
				base.GetComponent<AudioSource>().pitch = 0.5f;
			}
			if (speedcar > speedmultiplayer)
			{
				if (accelerate > 0f)
				{
					accelerate = 0.8f;
				}
				else if (accelerate < 0f)
				{
					accelerate = -0.8f;
				}
			}
		}
		else if (base.GetComponent<AudioSource>().isPlaying)
		{
			base.GetComponent<AudioSource>().pitch = 0.5f + speedcar / 30f;
			if (base.GetComponent<AudioSource>().pitch > 1.2f)
			{
				base.GetComponent<AudioSource>().pitch = 1.2f;
			}
		}
		UpdateWheels(accelerate, steer);
	}

	public void UpdateWheels(float accel, float steer)
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		float num = CalculateSmoothRpm(leftTrackWheelData);
		WheelData[] array = leftTrackWheelData;
		WheelData[] array2 = array;
		foreach (WheelData wheelData in array2)
		{
			wheelData.wheelTransform.localPosition = CalculateWheelPosition(wheelData.wheelTransform, wheelData.col, wheelData.wheelStartPos);
			wheelData.boneTransform.localPosition = CalculateWheelPosition(wheelData.boneTransform, wheelData.col, wheelData.boneStartPos);
			if (MirrorTracks)
			{
				wheelData.rotation = Mathf.Repeat(wheelData.rotation + fixedDeltaTime * (0f - num) * 360f / 60f, 360f);
			}
			else
			{
				wheelData.rotation = Mathf.Repeat(wheelData.rotation + fixedDeltaTime * num * 360f / 60f, 360f);
			}
			wheelData.wheelTransform.localRotation = Quaternion.Euler(wheelData.rotation, wheelData.startWheelAngle.y, wheelData.startWheelAngle.z);
			CalculateMotorForce(wheelData.col, accel, steer);
		}
		leftTrackTextureOffset = Mathf.Repeat(leftTrackTextureOffset + fixedDeltaTime * num * trackTextureSpeed / 60f, 1f);
		leftTrack.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0f, leftTrackTextureOffset));
		num = CalculateSmoothRpm(rightTrackWheelData);
		WheelData[] array3 = rightTrackWheelData;
		WheelData[] array4 = array3;
		foreach (WheelData wheelData2 in array4)
		{
			wheelData2.wheelTransform.localPosition = CalculateWheelPosition(wheelData2.wheelTransform, wheelData2.col, wheelData2.wheelStartPos);
			wheelData2.boneTransform.localPosition = CalculateWheelPosition(wheelData2.boneTransform, wheelData2.col, wheelData2.boneStartPos);
			if (MirrorTracks)
			{
				wheelData2.rotation = Mathf.Repeat(wheelData2.rotation + fixedDeltaTime * (0f - num) * 360f / 60f, 360f);
			}
			else
			{
				wheelData2.rotation = Mathf.Repeat(wheelData2.rotation + fixedDeltaTime * num * 360f / 60f, 360f);
			}
			wheelData2.wheelTransform.localRotation = Quaternion.Euler(wheelData2.rotation, wheelData2.startWheelAngle.y, wheelData2.startWheelAngle.z);
			CalculateMotorForce(wheelData2.col, accel, 0f - steer);
		}
		rightTrackTextureOffset = Mathf.Repeat(rightTrackTextureOffset + fixedDeltaTime * num * trackTextureSpeed / 60f, 1f);
		rightTrack.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0f, rightTrackTextureOffset));
		for (int k = 0; k < leftTrackUpperWheels.Length; k++)
		{
			leftTrackUpperWheels[k].localRotation = Quaternion.Euler(leftTrackWheelData[0].rotation, leftTrackWheelData[0].startWheelAngle.y, leftTrackWheelData[0].startWheelAngle.z);
		}
		for (int l = 0; l < rightTrackUpperWheels.Length; l++)
		{
			rightTrackUpperWheels[l].localRotation = Quaternion.Euler(rightTrackWheelData[0].rotation, rightTrackWheelData[0].startWheelAngle.y, rightTrackWheelData[0].startWheelAngle.z);
		}
	}

	public void CalculateMotorForce(WheelCollider col, float accel, float steer)
	{
		WheelFrictionCurve sidewaysFriction = col.sidewaysFriction;
		if (accel == 0f && steer == 0f)
		{
			col.brakeTorque = maxBrakeTorque;
			sidewaysFriction.stiffness = 0.015f;
		}
		else if (accel == 0f)
		{
			sidewaysFriction.stiffness = 0.01f;
			col.brakeTorque = rotateOnStandBrakeTorque;
			col.motorTorque = steer * rotateOnStandTorque;
		}
		else if (steer == 0f)
		{
			sidewaysFriction.stiffness = 0.015f;
			col.brakeTorque = minBrakeTorque;
			col.motorTorque = accel * forwardTorque;
		}
		else
		{
			sidewaysFriction.stiffness = 0.0015f;
			wheelid++;
			if (wheelid > rightTrackUpperWheels.Length)
			{
				wheelid = 0;
			}
			col.brakeTorque = minBrakeTorque;
			if (steer < 0f)
			{
				if (wheelid > rightTrackUpperWheels.Length / 2)
				{
					col.motorTorque = accel * forwardTorque * 1.2f;
				}
				else
				{
					col.motorTorque = accel * forwardTorque / 4f;
				}
			}
			if (steer > 0f)
			{
				if (wheelid <= rightTrackUpperWheels.Length / 2)
				{
					col.motorTorque = accel * forwardTorque * 1.2f;
				}
				else
				{
					col.motorTorque = accel * forwardTorque / 4f;
				}
			}
		}
		col.sidewaysFriction = sidewaysFriction;
	}

	private float CalculateSmoothRpm(WheelData[] w)
	{
		float num = 0f;
		List<int> list = new List<int>();
		for (int i = 0; i < w.Length; i++)
		{
			if (w[i].col.isGrounded)
			{
				list.Add(i);
			}
		}
		if (list.Count == 0)
		{
			foreach (WheelData wheelData in w)
			{
				num += wheelData.col.rpm;
			}
			return num / (float)w.Length;
		}
		for (int k = 0; k < list.Count; k++)
		{
			num += w[list[k]].col.rpm;
		}
		return num / (float)list.Count;
	}

	private Vector3 CalculateWheelPosition(Transform w, WheelCollider col, Vector3 startPos)
	{
		Vector3 localPosition = w.localPosition;
		WheelHit hit;
		if (col.GetGroundHit(out hit))
		{
			localPosition.y -= Vector3.Dot(w.position - hit.point, base.transform.up) - wheelRadius;
		}
		else
		{
			localPosition.y = startPos.y - suspensionOffset;
		}
		return localPosition;
	}
}
