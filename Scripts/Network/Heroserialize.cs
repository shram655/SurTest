using Photon;
using UnityEngine;

public class Heroserialize : Photon.MonoBehaviour
{
	public bool GUNNER;

	public bool isGUNNER;

	public GameObject _VEHICLE;

	public GameObject _VEHICLE_GUN;

	public GameObject _GUN;

	public bool inVehicle;

	public bool inTurret;

	public string _animState = string.Empty;

	private string _oldanimState = string.Empty;

	private int _animID;

	public GameObject TARGET_CAMERA;

	private Vector3 correctPlayerPos = Vector3.zero;

	private Quaternion correctPlayerRot = Quaternion.identity;

	private Vector3 correctCamPos = Vector3.zero;

	public bool _proSync;

	public float _proSyncTime;

	public bool _optimizedSync;

	public bool _optimizedSync2;

	public float _optimizedSyncTime;

	public bool _noSlowPolet;

	public float _noSlowPoletTimer;

	protected Animator animator;

	private float _stepTimer;

	private float _jumpTimer;

	private int speedHero;

	public AudioClip[] steps;

	public AudioClip[] steps_wood;

	public AudioClip[] steps_beton;

	public AudioClip[] steps_metall;

	public int steps_type;

	public Vector3 correct1 = Vector3.zero;

	public Quaternion correct2 = Quaternion.identity;

	public Vector3 correct3 = Vector3.zero;

	public Quaternion correctG1 = Quaternion.identity;

	public Quaternion correctG2 = Quaternion.identity;

	private float updateTimer;

	private void Awake()
	{
		_stepTimer = Time.time;
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (inVehicle && (bool)_VEHICLE)
		{
			if (stream.isWriting)
			{
				if (isGUNNER)
				{
					stream.SendNext(_VEHICLE.transform.localRotation);
					stream.SendNext(_GUN.transform.localRotation);
				}
				else
				{
					stream.SendNext(_VEHICLE.transform.position);
					stream.SendNext(_VEHICLE.transform.rotation);
					stream.SendNext(_VEHICLE.GetComponent<Rigidbody>().velocity);
				}
			}
			else
			{
				if (!stream.isReading)
				{
					return;
				}
				try
				{
					if (isGUNNER)
					{
						correctG1 = (Quaternion)stream.ReceiveNext();
						correctG2 = (Quaternion)stream.ReceiveNext();
					}
					else
					{
						correct1 = (Vector3)stream.ReceiveNext();
						correct2 = (Quaternion)stream.ReceiveNext();
						correct3 = (Vector3)stream.ReceiveNext();
					}
				}
				catch
				{
				}
			}
		}
		else if (inTurret && (bool)_VEHICLE)
		{
			if (stream.isWriting)
			{
				stream.SendNext(_VEHICLE.transform.localRotation);
				stream.SendNext(_VEHICLE_GUN.transform.localRotation);
			}
			else if (stream.isReading)
			{
				try
				{
					correctG1 = (Quaternion)stream.ReceiveNext();
					correctG2 = (Quaternion)stream.ReceiveNext();
				}
				catch
				{
				}
			}
		}
		else if (stream.isWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
			stream.SendNext(TARGET_CAMERA.transform.position);
			if (_animState != _oldanimState)
			{
				if (_animState == "0_Idle")
				{
					_animID = 0;
				}
				else if (_animState == "1_Idle")
				{
					_animID = 1;
				}
				else if (_animState == "2_Idle")
				{
					_animID = 2;
				}
				else if (_animState == "3_Idle")
				{
					_animID = 3;
				}
				else if (_animState == "4_Idle")
				{
					_animID = 4;
				}
				else if (_animState == "5_Idle")
				{
					_animID = 5;
				}
				else if (_animState == "0_Walk")
				{
					_animID = 6;
				}
				else if (_animState == "1_Walk")
				{
					_animID = 7;
				}
				else if (_animState == "2_Walk")
				{
					_animID = 8;
				}
				else if (_animState == "3_Walk")
				{
					_animID = 9;
				}
				else if (_animState == "4_Walk")
				{
					_animID = 19;
				}
				else if (_animState == "5_Walk")
				{
					_animID = 20;
				}
				else if (_animState == "0_Run")
				{
					_animID = 10;
				}
				else if (_animState == "1_Run")
				{
					_animID = 11;
				}
				else if (_animState == "2_Run")
				{
					_animID = 12;
				}
				else if (_animState == "0_WalkB")
				{
					_animID = 13;
				}
				else if (_animState == "1_WalkB")
				{
					_animID = 14;
				}
				else if (_animState == "2_WalkB")
				{
					_animID = 15;
				}
				else if (_animState == "3_WalkB")
				{
					_animID = 16;
				}
				else if (_animState == "4_WalkB")
				{
					_animID = 17;
				}
				else if (_animState == "5_WalkB")
				{
					_animID = 18;
				}
				else if (_animState == "21_Climbing")
				{
					_animID = 21;
				}
				else if (_animState == "22_Jump")
				{
					_animID = 22;
				}
				_oldanimState = _animState;
			}
			stream.SendNext(_animID);
		}
		else if (stream.isReading)
		{
			try
			{
				correctPlayerPos = (Vector3)stream.ReceiveNext();
				correctPlayerRot = (Quaternion)stream.ReceiveNext();
				correctCamPos = (Vector3)stream.ReceiveNext();
				_animID = (int)stream.ReceiveNext();
			}
			catch
			{
			}
		}
	}

	private void PlayAudioClip(AudioClip clip, Vector3 position, float volume, float distance)
	{
		GameObject gameObject = new GameObject("One shot audio");
		gameObject.transform.position = position;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
		audioSource.clip = clip;
		audioSource.maxDistance = distance;
		audioSource.volume = volume;
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.Play();
		Object.Destroy(gameObject, clip.length);
	}

	private void Update()
	{
		if (base.photonView.isMine)
		{
			if (!(Time.time > updateTimer + 0.3f))
			{
				return;
			}
			if (!inVehicle && !inTurret)
			{
				if (BaseLogic._player.GetComponent<FPSRigidBodyWalker>().crouched)
				{
					if (BaseLogic._player.GetComponent<FPSRigidBodyWalker>().inputY != 0f)
					{
						TARGET_CAMERA.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 16f, Camera.main.transform.position.z) + Camera.main.transform.forward * 20f;
					}
					else
					{
						TARGET_CAMERA.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 4f, Camera.main.transform.position.z) + Camera.main.transform.forward * 20f;
					}
				}
				else
				{
					TARGET_CAMERA.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 6f, Camera.main.transform.position.z) + Camera.main.transform.forward * 20f;
				}
			}
			updateTimer = Time.time;
			return;
		}
		if (inVehicle)
		{
			if (isGUNNER)
			{
				if (_optimizedSync)
				{
					if (Time.time > _optimizedSyncTime + 1f)
					{
						_VEHICLE_GUN.transform.localRotation = correctG1;
						_GUN.transform.localRotation = correctG2;
						_optimizedSyncTime = Time.time;
					}
				}
				else
				{
					_VEHICLE_GUN.transform.localRotation = Quaternion.Slerp(_VEHICLE_GUN.transform.localRotation, correctG1, Time.deltaTime * 5f);
					_GUN.transform.localRotation = Quaternion.Slerp(_GUN.transform.localRotation, correctG2, Time.deltaTime * 5f);
				}
			}
			else if (_optimizedSync)
			{
				if (Time.time > _optimizedSyncTime + 1f)
				{
					_VEHICLE.transform.position = correct1;
					_VEHICLE.transform.rotation = correct2;
					_optimizedSyncTime = Time.time;
				}
			}
			else if (_noSlowPolet)
			{
				if (Time.time > _noSlowPoletTimer + 1f)
				{
					_VEHICLE.transform.position = correct1;
					_VEHICLE.transform.rotation = correct2;
					_noSlowPolet = false;
				}
			}
			else if ((bool)_VEHICLE.GetComponent<Rigidbody>())
			{
				if (!_VEHICLE.GetComponent<Rigidbody>().isKinematic)
				{
					_VEHICLE.transform.position = Vector3.Lerp(_VEHICLE.transform.localPosition, correct1, Time.deltaTime * 3.5f);
					_VEHICLE.transform.rotation = Quaternion.Lerp(_VEHICLE.transform.localRotation, correct2, Time.deltaTime * 5f);
					_VEHICLE.GetComponent<Rigidbody>().velocity = Vector3.Lerp(_VEHICLE.GetComponent<Rigidbody>().velocity, correct3, Time.deltaTime * 3.5f);
				}
				else
				{
					_VEHICLE.transform.position = Vector3.Lerp(_VEHICLE.transform.position, correct1, Time.deltaTime * 3.5f);
					_VEHICLE.transform.rotation = Quaternion.Lerp(_VEHICLE.transform.rotation, correct2, Time.deltaTime * 5f);
				}
			}
			return;
		}
		if (inTurret)
		{
			if (_optimizedSync)
			{
				if (Time.time > _optimizedSyncTime + 1f)
				{
					_VEHICLE.transform.localRotation = correctG1;
					_optimizedSyncTime = Time.time;
				}
			}
			else
			{
				_VEHICLE.transform.localRotation = Quaternion.Lerp(_VEHICLE.transform.localRotation, correctG1, Time.deltaTime * 3.5f);
				_VEHICLE_GUN.transform.localRotation = Quaternion.Lerp(_VEHICLE_GUN.transform.localRotation, correctG2, Time.deltaTime * 3.5f);
			}
			return;
		}
		if (_optimizedSync)
		{
			if (Time.time > _optimizedSyncTime + 1f)
			{
				if (!GUNNER)
				{
					base.transform.position = correctPlayerPos;
					base.transform.rotation = correctPlayerRot;
				}
				_optimizedSyncTime = Time.time;
			}
			return;
		}
		if (_noSlowPolet)
		{
			if (Time.time > _noSlowPoletTimer + 1f)
			{
				base.transform.position = correctPlayerPos;
				base.transform.rotation = correctPlayerRot;
				_noSlowPolet = false;
			}
			return;
		}
		if (_optimizedSync2)
		{
			if (speedHero == 1)
			{
				if ((double)Time.time > (double)_stepTimer + 0.8)
				{
					if (steps_type == 0)
					{
						PlayAudioClip(steps[Random.Range(0, steps.Length)], base.transform.position, 0.01f, 15f);
					}
					else if (steps_type == 1)
					{
						PlayAudioClip(steps_wood[Random.Range(0, steps_wood.Length)], base.transform.position, 0.07f, 15f);
					}
					else if (steps_type == 2)
					{
						PlayAudioClip(steps_beton[Random.Range(0, steps_beton.Length)], base.transform.position, 0.07f, 15f);
					}
					else if (steps_type == 3)
					{
						PlayAudioClip(steps_metall[Random.Range(0, steps_metall.Length)], base.transform.position, 0.03f, 15f);
					}
					_stepTimer = Time.time;
				}
			}
			else if (speedHero == 2)
			{
				if ((double)Time.time > (double)_stepTimer + 0.5)
				{
					if (steps_type == 0)
					{
						PlayAudioClip(steps[Random.Range(0, steps.Length)], base.transform.position, 0.015f, 40f);
					}
					else if (steps_type == 1)
					{
						PlayAudioClip(steps_wood[Random.Range(0, steps_wood.Length)], base.transform.position, 0.1f, 40f);
					}
					else if (steps_type == 2)
					{
						PlayAudioClip(steps_beton[Random.Range(0, steps_beton.Length)], base.transform.position, 0.1f, 40f);
					}
					else if (steps_type == 3)
					{
						PlayAudioClip(steps_metall[Random.Range(0, steps_metall.Length)], base.transform.position, 0.04f, 40f);
					}
					_stepTimer = Time.time;
				}
			}
			else if (speedHero == 3 && (double)Time.time > (double)_stepTimer + 0.35)
			{
				if (steps_type == 0)
				{
					PlayAudioClip(steps[Random.Range(0, steps.Length)], base.transform.position, 0.02f, 50f);
				}
				else if (steps_type == 1)
				{
					PlayAudioClip(steps_wood[Random.Range(0, steps_wood.Length)], base.transform.position, 0.15f, 50f);
				}
				else if (steps_type == 2)
				{
					PlayAudioClip(steps_beton[Random.Range(0, steps_beton.Length)], base.transform.position, 0.15f, 50f);
				}
				else if (steps_type == 3)
				{
					PlayAudioClip(steps_metall[Random.Range(0, steps_metall.Length)], base.transform.position, 0.05f, 50f);
				}
				_stepTimer = Time.time;
			}
		}
		if (!GUNNER)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, correctPlayerPos, Time.deltaTime * 5f);
		}
		if (_animID != 21)
		{
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * 5f);
			TARGET_CAMERA.transform.position = Vector3.Lerp(TARGET_CAMERA.transform.position, correctCamPos, Time.deltaTime * 12f);
			animator.SetLookAtWeight(1f, 1f, 1f);
			animator.SetLookAtPosition(TARGET_CAMERA.transform.position);
		}
		if (!GUNNER)
		{
			SetAnimation();
		}
	}

	private void SetAnimation()
	{
		float @float = animator.GetFloat("Speed");
		if (_animID == 0)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 0);
			animator.SetBool("Crouched", false);
			float value = Mathf.Lerp(@float, 0f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value);
			speedHero = 0;
		}
		else if (_animID == 1)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 1);
			animator.SetBool("Crouched", false);
			float value2 = Mathf.Lerp(@float, 0f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value2);
			speedHero = 0;
		}
		else if (_animID == 2)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 2);
			animator.SetBool("Crouched", false);
			float value3 = Mathf.Lerp(@float, 0f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value3);
			speedHero = 0;
		}
		else if (_animID == 3)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 0);
			animator.SetBool("Crouched", true);
			float value4 = Mathf.Lerp(@float, 0f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value4);
			speedHero = 0;
		}
		else if (_animID == 4)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 1);
			animator.SetBool("Crouched", true);
			float value5 = Mathf.Lerp(@float, 0f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value5);
			speedHero = 0;
		}
		else if (_animID == 5)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 2);
			animator.SetBool("Crouched", true);
			float value6 = Mathf.Lerp(@float, 0f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value6);
			speedHero = 0;
		}
		else if (_animID == 6)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 0);
			animator.SetBool("Crouched", false);
			float value7 = Mathf.Lerp(@float, 0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value7);
			speedHero = 2;
		}
		else if (_animID == 7)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 1);
			animator.SetBool("Crouched", false);
			float value8 = Mathf.Lerp(@float, 0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value8);
			speedHero = 2;
		}
		else if (_animID == 8)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 2);
			animator.SetBool("Crouched", false);
			float value9 = Mathf.Lerp(@float, 0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value9);
			speedHero = 2;
		}
		else if (_animID == 9)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 0);
			animator.SetBool("Crouched", true);
			float value10 = Mathf.Lerp(@float, 0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value10);
			speedHero = 1;
		}
		else if (_animID == 10)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 0);
			animator.SetBool("Crouched", false);
			float value11 = Mathf.Lerp(@float, 1f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value11);
			speedHero = 3;
		}
		else if (_animID == 11)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 1);
			animator.SetBool("Crouched", false);
			float value12 = Mathf.Lerp(@float, 1f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value12);
			speedHero = 3;
		}
		else if (_animID == 12)
		{
			animator.SetBool("Climbing", false);
			animator.SetInteger("AnimID", 2);
			animator.SetBool("Crouched", false);
			float value13 = Mathf.Lerp(@float, 1f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value13);
			speedHero = 3;
		}
		else if (_animID == 13)
		{
			animator.SetInteger("AnimID", 0);
			animator.SetBool("Crouched", false);
			float value14 = Mathf.Lerp(@float, -0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value14);
			speedHero = 2;
		}
		else if (_animID == 14)
		{
			animator.SetInteger("AnimID", 1);
			animator.SetBool("Crouched", false);
			float value15 = Mathf.Lerp(@float, -0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value15);
			speedHero = 1;
		}
		else if (_animID == 15)
		{
			animator.SetInteger("AnimID", 2);
			animator.SetBool("Crouched", false);
			float value16 = Mathf.Lerp(@float, -0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value16);
			speedHero = 2;
		}
		else if (_animID == 16)
		{
			animator.SetInteger("AnimID", 0);
			animator.SetBool("Crouched", true);
			float value17 = Mathf.Lerp(@float, -0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value17);
			speedHero = 2;
		}
		else if (_animID == 17)
		{
			animator.SetInteger("AnimID", 1);
			animator.SetBool("Crouched", true);
			float value18 = Mathf.Lerp(@float, -0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value18);
			speedHero = 2;
		}
		else if (_animID == 18)
		{
			animator.SetInteger("AnimID", 2);
			animator.SetBool("Crouched", true);
			float value19 = Mathf.Lerp(@float, -0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value19);
			speedHero = 2;
		}
		else if (_animID == 19)
		{
			animator.SetInteger("AnimID", 1);
			animator.SetBool("Crouched", true);
			float value20 = Mathf.Lerp(@float, -0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value20);
			speedHero = 2;
		}
		else if (_animID == 20)
		{
			animator.SetInteger("AnimID", 2);
			animator.SetBool("Crouched", true);
			float value21 = Mathf.Lerp(@float, 0.5f, Time.deltaTime * 4f);
			animator.SetFloat("Speed", value21);
			speedHero = 2;
		}
		else if (_animID == 21)
		{
			animator.SetBool("Climbing", true);
		}
		else if (_animID == 22)
		{
			animator.SetBool("Climbing", false);
			if (Time.time > _jumpTimer + 0.7f)
			{
				animator.SetTrigger("Jump");
				_jumpTimer = Time.time;
			}
		}
	}
}
