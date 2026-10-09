using System;
using UnityEngine;

public class HeliSystem : MonoBehaviour
{
	public bool cinematic;

	public int _VEH_ID;

	public GameObject StandartPrefab;

	public GameObject NormalModel;

	public GameObject DestroyModel;

	public int SIDE;

	private float _oldTimeSignal;

	public Light fars_light;

	public Material base_material;

	public Material fars_material;

	public Renderer base_model;

	public bool detail_accuma;

	public int FUEL;

	public bool engine_start;

	public bool GUNNER_CAR;

	public int CountGunners = 1;

	private int rockets = 64;

	public GameObject Rocketz;

	public Transform[] RocketPos;

	private float fireRate;

	public AudioClip fireSnd;

	public AudioClip alarmSnd;

	public Light ALARM;

	public GameObject FIRE;

	private float alarm_timer;

	private float fire_timer;

	public GameObject WindZoner;

	public Transform EXIT_POS;

	public Transform POWER;

	public int _choiceSlot;

	public bool _LOCK;

	public bool _control;

	public bool _passenger;

	public bool CAR_GUN;

	private bool _lodSys;

	private float _lodSysTimer;

	public int _damaged;

	public float speedcar;

	public int mass = 5000;

	public float engineForce = 90000f;

	public Vector3 turningTorques = new Vector3(40000f, 50000f, 20000f);

	public float pitchRecenterTorque = 0.8f;

	public float rollRecenterTorque = 0.8f;

	public Vector3 drag = new Vector3(4f, 3f, 2f);

	public float angularDrag = 0.5f;

	public float groundEffectHeight = 20f;

	public Transform mainRotor;

	public Transform mainRotor2;

	public Transform groundDustEffect;

	private bool queryUserInput = true;

	private float groundHeight;

	private float groundEffect;

	private float targetRotorPower;

	public float rotorPower;

	private float rotorPitch;

	private float rotorVolume;

	public Quaternion mainRotorOriginalRotation;

	public float mainRotorSpin;

	private float pitchInput;

	private float turnInput;

	public float thrustInput;

	private float groundHeightExtrapolated;

	private Vector3 dragDirection;

	private Vector3 dragForces;

	private float pitch;

	private float pitchDampingFactor;

	private float roll;

	private float propMotion;

	public bool[] allow_slot = new bool[8];

	public Transform[] pos_slot = new Transform[8];

	private int timerPropeller;

	private GameObject PLAYER;

	private int timer;

	private float _fuelTimer;

	public Transform COM;

	private GameObject _fpsCamera;

	private GameObject _player;

	private GameObject _fpsWeapons;

	private float _oldTime;

	public float _oldTimeEnterCar;

	private Vector3 startPos;

	private Quaternion startRot;

	public bool _RENT;

	public int _RENT_PRICE;

	public int _oldRENT_PRICE;

	public float _respawnCarTimer;

	private float _checkSlotsTimer;

	public int hitPoints = 3000;

	private void ApplyDamage(int damage)
	{
		hitPoints -= damage;
		if (hitPoints <= 1000 && _damaged != 1)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("ApplyVehicleDamage2", PhotonTargets.Others, base.name);
			fire_timer = Time.time;
			_damaged = 1;
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
			UnityEngine.Object.Destroy(wheelCollider.gameObject);
		}
		UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		UnityEngine.Object.Destroy(base.GetComponent<Collider>());
		base.enabled = false;
		base.GetComponent<AudioSource>().enabled = false;
		UnityEngine.Object.Destroy(base.gameObject, 60f);
		GameObject gameObject = UnityEngine.Object.Instantiate(StandartPrefab.GetComponent<AllVehicles>()._VEHICLES[_VEH_ID], startPos, startRot) as GameObject;
		gameObject.SendMessage("CheckStart");
		gameObject.name = base.name;
		gameObject.GetComponent<HeliSystem>().StandartPrefab = StandartPrefab;
		base.name = "DESTROY_VEH";
		Debug.Log("Vehicle: " + base.name + ", RespawnVehicle");
	}

	private void DestroyVehicle()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("_EXPLOSION"));
		gameObject.GetComponent<Explosion>()._carBoom = true;
		gameObject.transform.position = base.transform.position;
		gameObject.transform.rotation = Quaternion.identity;
		if (_control || _passenger)
		{
			ExitCar();
		}
		RespawnVehicle();
	}

	private void Awake()
	{
		GameObject gameObject = GameObject.Find("_BASEPARENTSYNC");
		base.transform.parent = gameObject.transform;
		FUEL = 0;
		startPos = base.transform.position;
		startRot = base.transform.rotation;
	}

	private void Start()
	{
		CheckStart();
		base.GetComponent<Rigidbody>().mass = mass;
		base.GetComponent<Rigidbody>().angularDrag = angularDrag;
		base.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
		base.GetComponent<AudioSource>().loop = true;
		if (mainRotor != null)
		{
			mainRotorOriginalRotation = mainRotor.localRotation;
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

	private void SetCameraView(bool view)
	{
		if (GUNNER_CAR && (_choiceSlot == 1 || (CountGunners == 2 && _choiceSlot == 2)))
		{
			return;
		}
		if (view)
		{
			Camera.main.GetComponent<CameraKick>().enabled = false;
			Camera.main.GetComponent<MouseOrb>().enabled = true;
			Camera.main.GetComponent<MouseOrb>().target = base.transform;
			Camera.main.GetComponent<MouseOrb>().maxDistance = 22f;
			Camera.main.GetComponent<MouseOrb>().distance = 22f;
			Camera.main.GetComponent<MouseOrb>().targetHeight = 3f;
			_fpsCamera.GetComponent<CameraInVehicle>().enabled = false;
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.SendMessage("SetCustomizeView", true);
			}
			return;
		}
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
		BaseLogic._3rdView = false;
	}

	private void GoToDriver()
	{
		_choiceSlot = 0;
	}

	private void GoToGunner(int _slot)
	{
		_choiceSlot = _slot;
		_oldTimeEnterCar = Time.time;
		_player.GetComponent<Rigidbody>().isKinematic = true;
		_player.SendMessage("SetCrosshairType");
		_player.GetComponent<FPSPlayer>().INCAR = true;
		_player.GetComponent<FPSRigidBodyWalker>().isGUNNER = true;
		_player.GetComponent<CapsuleCollider>().enabled = false;
		_player.GetComponent<AudioSource>().enabled = false;
		_player.transform.parent = base.transform;
		_player.transform.localEulerAngles = Vector3.zero;
		_player.transform.localPosition = new Vector3(pos_slot[_choiceSlot].transform.localPosition.x, pos_slot[_choiceSlot].transform.localPosition.y, pos_slot[_choiceSlot].transform.localPosition.z);
		if ((bool)Network.PLAYER)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("InGunner", PhotonTargets.All, base.name + "\n" + _choiceSlot);
		}
	}

	private void GoToPassenger(int _slot)
	{
		_choiceSlot = _slot;
		if ((bool)Network.PLAYER)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("InCar", PhotonTargets.All, base.name + "\n" + _choiceSlot);
		}
		Camera.main.GetComponent<CameraKick>().inVeh = true;
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
		_player.transform.localPosition = new Vector3(pos_slot[_choiceSlot].transform.localPosition.x, pos_slot[_choiceSlot].transform.localPosition.y - 0.25f, pos_slot[_choiceSlot].transform.localPosition.z);
		Camera.main.GetComponent<CameraKick>().inVeh = true;
		Camera.main.transform.parent = null;
	}

	private void ActivateObject()
	{
		if (!(Time.time > _oldTimeEnterCar + 3f) || _player.GetComponent<FPSRigidBodyWalker>().crouched)
		{
			return;
		}
		CheckStart();
		for (int i = 0; i < allow_slot.Length; i++)
		{
			if (!allow_slot[i])
			{
				if (i == 0)
				{
					_control = true;
					GoToDriver();
					GoToPassenger(i);
					break;
				}
				if (!GUNNER_CAR || i > CountGunners)
				{
					_passenger = true;
					GoToPassenger(i);
					break;
				}
				if (!allow_slot[i])
				{
					_passenger = true;
					GoToGunner(i);
					break;
				}
			}
		}
	}

	public void ExitCar()
	{
		if (Time.time > _oldTimeEnterCar + 3f)
		{
			SetCameraView(false);
			Camera.main.GetComponent<CameraKick>().inVeh = false;
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("ExitCar", PhotonTargets.All, base.name + "\n" + _choiceSlot);
			}
			if (_choiceSlot == 0)
			{
				engine_start = false;
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
			Camera.main.GetComponent<CameraKick>().inVeh = false;
			Camera.main.transform.parent = _fpsCamera.transform;
		}
	}

	private void SetEngine()
	{
		engine_start = true;
	}

	private void FixedUpdate()
	{
		if (base.GetComponent<Rigidbody>().isKinematic)
		{
			return;
		}
		if (!base.GetComponent<AudioSource>().isPlaying && thrustInput > 0.1f && engine_start)
		{
			base.GetComponent<AudioSource>().Play();
		}
		speedcar = base.GetComponent<Rigidbody>().velocity.magnitude * 2.6f;
		base.GetComponent<Rigidbody>().centerOfMass = COM.localPosition;
		pitchInput = 0f;
		turnInput = 0f;
		if (!_control)
		{
			return;
		}
		if (queryUserInput && engine_start)
		{
			pitchInput = Input.GetAxis("Vertical") * 2f;
			turnInput = Input.GetAxis("Horizontal");
			if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.LeftShift))
			{
				if (thrustInput < 0.47f)
				{
					thrustInput += 0.0005f;
					POWER.Rotate(0f, 0f, 0.35f);
				}
			}
			else if ((Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.LeftControl)) && thrustInput > 0.3f)
			{
				thrustInput -= 0.0005f;
				POWER.Rotate(0f, 0f, -0.35f);
			}
		}
		if (pitchInput > 0.15f && (turnInput != 0f || thrustInput != 0f) && Time.time > _fuelTimer + 10f)
		{
			_fuelTimer = Time.time;
			if (FUEL > 0)
			{
				FUEL--;
			}
		}
		if (FUEL == 0)
		{
			engine_start = false;
			pitchInput = 0f;
		}
		groundHeight = base.transform.position.y;
		RaycastHit hitInfo;
		if (Physics.Raycast(base.transform.position, Vector3.down, out hitInfo))
		{
			groundHeight = base.transform.position.y - hitInfo.point.y;
			groundEffect = Mathf.Clamp01(1f - groundHeight / groundEffectHeight);
			if (groundEffect > 0f && hitInfo.collider.tag == "Dusty" && groundDustEffect != null)
			{
			}
		}
		if (_damaged == 1)
		{
			rotorPower = thrustInput * 0.95f;
		}
		else
		{
			rotorPower = thrustInput * 1.2f;
		}
		dragDirection = base.transform.InverseTransformDirection(base.GetComponent<Rigidbody>().velocity);
		dragForces = -Vector3.Scale(dragDirection, drag) * base.GetComponent<Rigidbody>().velocity.magnitude;
		base.GetComponent<Rigidbody>().AddForce(base.transform.TransformDirection(dragForces));
		base.GetComponent<Rigidbody>().AddForce(base.transform.up * engineForce * rotorPower);
		pitch = Mathf.Asin(base.transform.forward.y);
		pitchDampingFactor = 1f - Mathf.Clamp01((0f - pitch) * Mathf.Sign(pitchInput) / ((float)Math.PI / 2f));
		base.GetComponent<Rigidbody>().AddTorque(base.transform.right * turningTorques.x * pitchInput * pitchDampingFactor);
		base.GetComponent<Rigidbody>().AddTorque(base.transform.right * pitchRecenterTorque * turningTorques.x * pitch);
		base.GetComponent<Rigidbody>().AddTorque(-base.transform.forward * turningTorques.z * turnInput * Mathf.Abs(pitch));
		roll = Mathf.Asin(base.transform.right.y);
		base.GetComponent<Rigidbody>().AddTorque(-base.transform.forward * rollRecenterTorque * turningTorques.z * roll);
		base.GetComponent<Rigidbody>().AddTorque(base.transform.up * turningTorques.y * turnInput);
		base.GetComponent<Rigidbody>().angularVelocity = new Vector3(base.GetComponent<Rigidbody>().angularVelocity.x, base.GetComponent<Rigidbody>().angularVelocity.y + turnInput / 300f, base.GetComponent<Rigidbody>().angularVelocity.z);
		rotorVolume = Mathf.Lerp(rotorVolume, rotorPower, Time.deltaTime);
		rotorPitch = Mathf.Lerp(rotorPitch, rotorVolume, Time.deltaTime * 0.5f);
		base.GetComponent<AudioSource>().pitch = 0.5f + rotorPitch / 2f;
		base.GetComponent<AudioSource>().volume = 0.3f + 1.2f * rotorVolume;
	}

	private void Update()
	{
		if (_damaged == 1)
		{
			if (Time.time > alarm_timer + 0.5f)
			{
				if (!ALARM.enabled)
				{
					ALARM.enabled = true;
					AudioSource.PlayClipAtPoint(alarmSnd, pos_slot[0].position, 1f);
				}
				else
				{
					ALARM.enabled = false;
				}
				if (Time.time > fire_timer + 8f)
				{
					DestroyVehicle();
				}
				alarm_timer = Time.time;
			}
			if (_control)
			{
				base.GetComponent<Rigidbody>().AddTorque(base.transform.right * turningTorques.x * 2.05f);
				base.GetComponent<Rigidbody>().AddTorque(-base.transform.forward * turningTorques.z * 1.15f * Mathf.Abs(pitch));
				base.GetComponent<Rigidbody>().angularVelocity = new Vector3(base.GetComponent<Rigidbody>().angularVelocity.x, base.GetComponent<Rigidbody>().angularVelocity.y + 0.205f * rotorPower, base.GetComponent<Rigidbody>().angularVelocity.z);
			}
		}
		if (Time.time > _lodSysTimer + 5f)
		{
			if ((bool)Camera.main)
			{
				float num = Vector3.Distance(Camera.main.transform.position, base.transform.position);
				if (num < 300f)
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
				if (num < 250f)
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
				if (GUNNER_CAR && (_choiceSlot == 1 || (CountGunners == 2 && _choiceSlot == 2)))
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
		if (!_control && allow_slot[0])
		{
			float num2 = speedcar / 30f;
			if (num2 > 0.4f)
			{
				num2 = 0.4f;
			}
			if (speedcar > 1f)
			{
				if (!base.GetComponent<AudioSource>().isPlaying)
				{
					base.GetComponent<AudioSource>().Play();
				}
				base.GetComponent<AudioSource>().pitch = 0.5f + num2;
				thrustInput = 0.5f + speedcar / 300f;
			}
		}
		else if (!allow_slot[0] || (_control && (FUEL == 0 || !engine_start)) || (!_control && speedcar < 1f))
		{
			if (thrustInput > 0f)
			{
				base.GetComponent<AudioSource>().pitch -= 0.001f;
				thrustInput -= 0.001f;
			}
			if (thrustInput < 0.1f)
			{
				engine_start = false;
				if (base.GetComponent<AudioSource>().isPlaying)
				{
					base.GetComponent<AudioSource>().Stop();
				}
			}
		}
		if (mainRotor != null)
		{
			if (cinematic)
			{
				thrustInput = 0.1f;
			}
			propMotion = thrustInput * 50f;
			if (propMotion > 18f)
			{
				propMotion = 18f;
			}
			if (propMotion > 10f)
			{
				WindZoner.SetActive(true);
			}
			else
			{
				WindZoner.SetActive(false);
			}
			mainRotorSpin = Mathf.Repeat(mainRotorSpin + propMotion, 360f);
			mainRotor.localRotation = Quaternion.Euler(0f, mainRotorSpin, 0f);
			if (_damaged != 1)
			{
				mainRotor2.localRotation = Quaternion.Euler(0f - mainRotorSpin, 0f, 0f);
			}
		}
		if (_passenger && Input.GetKeyDown("e") && Time.time > _oldTimeEnterCar + 3f)
		{
			ExitCar();
		}
		if (!_control)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > fireRate + 0.4f)
		{
			if (rockets > 0 && RocketPos.Length != 0)
			{
				int num3 = UnityEngine.Random.Range(0, RocketPos.Length);
				Vector3 eulerAngles = RocketPos[num3].eulerAngles;
				Firex(eulerAngles);
				if ((bool)Network.PLAYER)
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("FireVehicle", PhotonTargets.Others, eulerAngles);
				}
			}
			fireRate = Time.time;
		}
		if (Input.GetKeyDown("e") && Time.time > _oldTimeEnterCar + 3f)
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

	private void Firex(Vector3 direction)
	{
		if ((bool)Rocketz)
		{
			AudioSource.PlayClipAtPoint(fireSnd, base.transform.position, 1f);
			rockets--;
			int num = UnityEngine.Random.Range(0, RocketPos.Length);
			GameObject gameObject = UnityEngine.Object.Instantiate(Rocketz, RocketPos[num].position, RocketPos[num].rotation) as GameObject;
			gameObject.transform.eulerAngles = direction;
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		if (!_control || col.gameObject.layer == 12)
		{
			return;
		}
		if (col.relativeVelocity.magnitude > 12f)
		{
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("DamagedVehicle", PhotonTargets.Others, base.name);
			}
			_damaged = 1;
		}
		if (col.relativeVelocity.magnitude > 15f)
		{
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("DestroyVehicle", PhotonTargets.Others, base.name);
			}
			DestroyVehicle();
		}
	}
}
