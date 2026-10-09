using UnityEngine;

public class MortarSystem : MonoBehaviour
{
	public bool vehicleGun;

	public int ownerID;

	private float _checkSlotsTimer;

	public GameObject _mineHE;

	public Transform minePos;

	public Transform _stvol;

	public Transform _nogi;

	public Transform _stir;

	public Transform baseMortat;

	public Transform targetMortat;

	public Transform targetRay;

	public GameObject _effect;

	private GameObject _fpsCamera;

	private GameObject _fpsWeapons;

	private GameObject _player;

	public Renderer[] fugas;

	public bool _control;

	public bool[] allow_slot = new bool[2];

	public Transform[] pos_slot;

	private int _choiceSlot;

	public int ammo;

	private int rotate;

	private float fireTimer;

	private float _oldTimeEnterCar;

	private void Start()
	{
		if (vehicleGun)
		{
			base.name = base.transform.parent.transform.parent.name + "_GUN";
		}
		UpdateAmmo();
		targetMortat.GetComponent<Renderer>().enabled = false;
		CheckStart();
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
			Camera.main.GetComponent<MouseOrb>().maxDistance = 7f;
			Camera.main.GetComponent<MouseOrb>().distance = 7f;
			Camera.main.GetComponent<MouseOrb>().targetHeight = 3f;
			Camera.main.GetComponent<MouseOrb>().yMinLimit = 5f;
			_fpsCamera.GetComponent<CameraInVehicle>().enabled = false;
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<Animator>().SetBool("Crouched", true);
				Network.PLAYER.GetComponent<Animator>().SetFloat("Speed", 0f);
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
			Camera.main.GetComponent<MouseOrb>().yMinLimit = -40f;
			_fpsCamera.GetComponent<CameraInVehicle>().enabled = true;
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.SendMessage("SetCustomizeView", false);
			}
		}
	}

	private void ActivateObject()
	{
		if (Time.time > _oldTimeEnterCar + 3f && !allow_slot[0] && !_player.GetComponent<FPSRigidBodyWalker>().crouched)
		{
			targetMortat.GetComponent<Renderer>().enabled = true;
			_choiceSlot = 0;
			Network.PLAYER.GetComponent<HeroBase>().VEHICLE = base.gameObject;
			_control = true;
			SetCameraView(true);
			_oldTimeEnterCar = Time.time;
			_fpsWeapons.SetActive(false);
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("InTurret", PhotonTargets.All, base.name);
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
			_player.transform.localPosition = pos_slot[_choiceSlot].localPosition;
			Camera.main.GetComponent<CameraKick>().inVeh = true;
			Camera.main.transform.parent = null;
		}
	}

	public void ExitCar2()
	{
		ExitCar();
		Object.Destroy(base.gameObject);
	}

	public void ExitCar()
	{
		targetMortat.GetComponent<Renderer>().enabled = false;
		SetCameraView(false);
		Camera.main.GetComponent<CameraKick>().inVeh = false;
		if ((bool)Network.PLAYER)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("ExitCar", PhotonTargets.All, base.name + "\n" + _choiceSlot);
			if (ammo > 0 && ammo < 12)
			{
				for (int i = 0; i < ammo; i++)
				{
					BaseLogic.AddItem(102, 100);
				}
			}
			ammo = 0;
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
		_player.transform.position = base.transform.position;
		_control = false;
		allow_slot[_choiceSlot] = false;
		Camera.main.GetComponent<CameraKick>().inVeh = false;
		Camera.main.transform.parent = _fpsCamera.transform;
	}

	private void Fire()
	{
		ammo--;
		Object.Instantiate(_effect, minePos.position, minePos.rotation);
		Ballistic ballistic = null;
		for (int i = 0; i < BaseLogic._poolBulletsMine.Length; i++)
		{
			ballistic = BaseLogic._poolBulletsMine[i].GetComponent<Ballistic>();
			if (ballistic.isSleep)
			{
				ballistic.enabled = true;
				ballistic.transform.position = minePos.position;
				ballistic.transform.rotation = minePos.rotation;
				if (vehicleGun)
				{
					ballistic.transform.eulerAngles = new Vector3(ballistic.transform.eulerAngles.x + Random.Range(-0.7f, 0.7f), ballistic.transform.eulerAngles.y + Random.Range(-0.7f, 0.7f), ballistic.transform.eulerAngles.z);
				}
				else
				{
					ballistic.transform.eulerAngles = new Vector3(ballistic.transform.eulerAngles.x + Random.Range(-2f, 2f), ballistic.transform.eulerAngles.y + Random.Range(-2f, 2f), ballistic.transform.eulerAngles.z);
				}
				ballistic.StartBullet();
				break;
			}
		}
		Network.PLAYER.GetComponent<PhotonView>().RPC("FireMortar", PhotonTargets.Others, ballistic.transform.eulerAngles);
		if ((bool)base.transform.parent)
		{
			base.transform.parent.transform.parent.GetComponent<Rigidbody>().AddForceAtPosition(95000f * base.transform.TransformDirection(Vector3.down), base.transform.position);
		}
		fireTimer = Time.time;
		base.GetComponent<AudioSource>().Play();
		UpdateAmmo();
	}

	private void FireOff(Vector3 dir)
	{
		Object.Instantiate(_effect, minePos.position, minePos.rotation);
		Ballistic ballistic = null;
		for (int i = 0; i < BaseLogic._poolBulletsMine.Length; i++)
		{
			ballistic = BaseLogic._poolBulletsMine[i].GetComponent<Ballistic>();
			if (ballistic.isSleep)
			{
				ballistic.enabled = true;
				ballistic.transform.position = minePos.position;
				ballistic.transform.eulerAngles = dir;
				ballistic.ownerID = ownerID;
				ballistic.StartBullet();
				break;
			}
		}
		if ((bool)base.transform.parent)
		{
			base.transform.parent.transform.parent.GetComponent<Rigidbody>().AddForceAtPosition(95000f * base.transform.TransformDirection(Vector3.down), base.transform.position);
		}
		fireTimer = Time.time;
		base.GetComponent<AudioSource>().Play();
	}

	private void UpdateAmmo()
	{
		for (int i = 0; i < fugas.Length; i++)
		{
			if (i < ammo)
			{
				fugas[i].enabled = true;
			}
			else
			{
				fugas[i].enabled = false;
			}
		}
	}

	private void UpdateTarget()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(targetRay.transform.position, targetRay.transform.TransformDirection(Vector3.down), out hitInfo, 500f, BaseLogic.layerSet))
		{
			targetMortat.transform.position = hitInfo.point;
		}
	}

	private void Update()
	{
		if (Time.time > _checkSlotsTimer + 10f)
		{
			if (_control && (bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("InTurret", PhotonTargets.All, base.name);
			}
			_checkSlotsTimer = Time.time;
		}
		if (!_control || !BaseLogic._control)
		{
			return;
		}
		if (Input.GetKey(KeyCode.Mouse0))
		{
			if (vehicleGun)
			{
				if (Time.time > fireTimer + 5f && ammo > 0)
				{
					Fire();
				}
			}
			else if (Time.time > fireTimer + 2f && ammo > 0)
			{
				Fire();
			}
		}
		if (Input.GetKey(KeyCode.S))
		{
			if (_stvol.localEulerAngles.x < 44f)
			{
				if (!_stvol.GetComponent<AudioSource>().isPlaying)
				{
					_stvol.GetComponent<AudioSource>().Play();
				}
				_stvol.Rotate(0.08f, 0f, 0f);
				if ((bool)_nogi)
				{
					_nogi.Rotate(0.01f, 0f, 0f);
					targetRay.Translate(0f, 0f, 0.195f);
				}
				UpdateTarget();
			}
			else if (_stvol.GetComponent<AudioSource>().isPlaying)
			{
				_stvol.GetComponent<AudioSource>().Stop();
			}
		}
		else if (Input.GetKey(KeyCode.W))
		{
			if (_stvol.localEulerAngles.x > 8f)
			{
				if (!_stvol.GetComponent<AudioSource>().isPlaying)
				{
					_stvol.GetComponent<AudioSource>().Play();
				}
				_stvol.Rotate(-0.08f, 0f, 0f);
				if ((bool)_nogi)
				{
					_nogi.Rotate(-0.01f, 0f, 0f);
					targetRay.Translate(0f, 0f, -0.195f);
				}
				UpdateTarget();
			}
			else if (_stvol.GetComponent<AudioSource>().isPlaying)
			{
				_stvol.GetComponent<AudioSource>().Stop();
			}
		}
		else if (Input.GetKey(KeyCode.A))
		{
			if (rotate > -250 || rotate == 0 || vehicleGun)
			{
				if (!_stvol.GetComponent<AudioSource>().isPlaying)
				{
					_stvol.GetComponent<AudioSource>().Play();
				}
				rotate--;
				baseMortat.transform.Rotate(0f, -0.18f, 0f, Space.World);
				UpdateTarget();
			}
			else if (_stvol.GetComponent<AudioSource>().isPlaying)
			{
				_stvol.GetComponent<AudioSource>().Stop();
			}
		}
		else if (Input.GetKey(KeyCode.D))
		{
			if (rotate < 250 || rotate == 0 || vehicleGun)
			{
				if (!_stvol.GetComponent<AudioSource>().isPlaying)
				{
					_stvol.GetComponent<AudioSource>().Play();
				}
				rotate++;
				baseMortat.transform.Rotate(0f, 0.18f, 0f, Space.World);
				UpdateTarget();
			}
			else if (_stvol.GetComponent<AudioSource>().isPlaying)
			{
				_stvol.GetComponent<AudioSource>().Stop();
			}
		}
		else if (_stvol.GetComponent<AudioSource>().isPlaying)
		{
			_stvol.GetComponent<AudioSource>().Stop();
		}
		if (Input.GetKeyDown("e") && Time.time > _oldTimeEnterCar + 3f)
		{
			ExitCar();
		}
	}
}
