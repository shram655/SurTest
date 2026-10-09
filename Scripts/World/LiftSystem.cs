using UnityEngine;

public class LiftSystem : MonoBehaviour
{
	public bool isLift;

	public bool isButton;

	public bool isButtonInLift;

	public bool isButtonInUnderground;

	public bool isLeftDoor;

	public bool isRightDoor;

	public bool moveLiftUp;

	public bool moveLiftDown;

	private int posLift;

	public GameObject Lift;

	public GameObject LiftDoorL;

	public GameObject LiftDoorR;

	public bool DoorMove;

	private int posDoor;

	private bool DoorMoveClosed = true;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		if (isLeftDoor)
		{
			if (DoorMove && posDoor != 125 && DoorMoveClosed)
			{
				if ((bool)base.GetComponent<AudioSource>() && !base.GetComponent<AudioSource>().isPlaying)
				{
					base.GetComponent<AudioSource>().Play();
				}
				base.transform.Translate(-0.008f, 0f, 0f);
				posDoor++;
				if (posDoor == 125)
				{
					DoorMove = false;
					DoorMoveClosed = false;
				}
			}
			else if (DoorMove && posDoor != 0 && !DoorMoveClosed)
			{
				if ((bool)base.GetComponent<AudioSource>() && !base.GetComponent<AudioSource>().isPlaying)
				{
					base.GetComponent<AudioSource>().Play();
				}
				base.transform.Translate(0.008f, 0f, 0f);
				posDoor--;
				if (posDoor == 0)
				{
					DoorMove = false;
					DoorMoveClosed = true;
				}
			}
		}
		else if (isRightDoor)
		{
			if (DoorMove && posDoor != 125 && DoorMoveClosed)
			{
				base.transform.Translate(0.008f, 0f, 0f);
				posDoor++;
				if (posDoor == 125)
				{
					DoorMove = false;
					DoorMoveClosed = false;
				}
			}
			else if (DoorMove && posDoor != 0 && !DoorMoveClosed)
			{
				base.transform.Translate(-0.008f, 0f, 0f);
				posDoor--;
				if (posDoor == 0)
				{
					DoorMove = false;
					DoorMoveClosed = true;
				}
			}
		}
		else
		{
			if (!isLift)
			{
				return;
			}
			if (moveLiftDown)
			{
				if ((bool)base.GetComponent<AudioSource>() && !base.GetComponent<AudioSource>().isPlaying)
				{
					base.GetComponent<AudioSource>().Play();
				}
				base.transform.Translate(-0.005f, 0f, 0f);
				posLift++;
				if (posLift < 125 && isLeftDoor)
				{
					base.transform.Translate(0f, -0.01f, 0f);
				}
				else if (posLift > 11511 && isRightDoor)
				{
					base.transform.Translate(0f, 0.01f, 0f);
				}
				if (posLift == 11636)
				{
					moveLiftDown = false;
					if (LiftDoorL.GetComponent<LiftSystem>().DoorMoveClosed)
					{
						LiftDoorL.SendMessage("MoveDoorFunc");
						LiftDoorR.SendMessage("MoveDoorFunc");
					}
				}
			}
			else
			{
				if (!moveLiftUp)
				{
					return;
				}
				if ((bool)base.GetComponent<AudioSource>() && !base.GetComponent<AudioSource>().isPlaying)
				{
					base.GetComponent<AudioSource>().Play();
				}
				base.transform.Translate(0.005f, 0f, 0f);
				posLift--;
				if (posLift > 11511 && isLeftDoor)
				{
					base.transform.Translate(0f, -0.01f, 0f);
				}
				else if (posLift > 11511 && isRightDoor)
				{
					base.transform.Translate(0f, 0.01f, 0f);
				}
				if (posLift == 0)
				{
					moveLiftUp = false;
					if (LiftDoorL.GetComponent<LiftSystem>().DoorMoveClosed)
					{
						LiftDoorL.SendMessage("MoveDoorFunc");
						LiftDoorR.SendMessage("MoveDoorFunc");
					}
				}
			}
		}
	}

	private void ActivateObject2()
	{
		if (!isButton)
		{
			return;
		}
		if ((bool)LiftDoorL && !Lift.GetComponent<LiftSystem>().moveLiftDown && !Lift.GetComponent<LiftSystem>().moveLiftUp)
		{
			if (LiftDoorL.GetComponent<LiftSystem>().DoorMoveClosed)
			{
				if (LiftDoorL.GetComponent<LiftSystem>().posLift == 0 && !LiftDoorL.GetComponent<LiftSystem>().isButtonInUnderground)
				{
					LiftDoorL.SendMessage("MoveDoorFunc");
					LiftDoorR.SendMessage("MoveDoorFunc");
				}
				if (LiftDoorL.GetComponent<LiftSystem>().posLift == 11636 && LiftDoorL.GetComponent<LiftSystem>().isButtonInUnderground)
				{
					LiftDoorL.SendMessage("MoveDoorFunc");
					LiftDoorR.SendMessage("MoveDoorFunc");
				}
			}
			else
			{
				LiftDoorL.SendMessage("MoveDoorFunc");
				LiftDoorR.SendMessage("MoveDoorFunc");
			}
		}
		if (isButtonInLift)
		{
			if (Lift.GetComponent<LiftSystem>().posLift == 11636)
			{
				Lift.SendMessage("MoveLiftUp");
				Lift.GetComponent<LiftSystem>().moveLiftDown = false;
			}
			else if (Lift.GetComponent<LiftSystem>().posLift == 0)
			{
				Lift.SendMessage("MoveLiftDown");
				Lift.GetComponent<LiftSystem>().moveLiftUp = false;
			}
		}
		else if (isButtonInUnderground && !Lift.GetComponent<LiftSystem>().moveLiftDown && !Lift.GetComponent<LiftSystem>().moveLiftUp)
		{
			if (Lift.GetComponent<LiftSystem>().posLift == 0)
			{
				Lift.SendMessage("MoveLiftDown");
				Lift.GetComponent<LiftSystem>().moveLiftUp = false;
			}
		}
		else if (!isButtonInUnderground && !Lift.GetComponent<LiftSystem>().moveLiftDown && !Lift.GetComponent<LiftSystem>().moveLiftUp && Lift.GetComponent<LiftSystem>().posLift == 11636)
		{
			Lift.SendMessage("MoveLiftUp");
			Lift.GetComponent<LiftSystem>().moveLiftDown = false;
		}
	}

	private void ActivateObject()
	{
		if (!isButton)
		{
			return;
		}
		if ((bool)Network.PLAYER)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("ActivateButton", PhotonTargets.Others, base.name);
		}
		if ((bool)LiftDoorL && !Lift.GetComponent<LiftSystem>().moveLiftDown && !Lift.GetComponent<LiftSystem>().moveLiftUp)
		{
			if (LiftDoorL.GetComponent<LiftSystem>().DoorMoveClosed)
			{
				if (LiftDoorL.GetComponent<LiftSystem>().posLift == 0 && !LiftDoorL.GetComponent<LiftSystem>().isButtonInUnderground)
				{
					LiftDoorL.SendMessage("MoveDoorFunc");
					LiftDoorR.SendMessage("MoveDoorFunc");
				}
				if (LiftDoorL.GetComponent<LiftSystem>().posLift == 11636 && LiftDoorL.GetComponent<LiftSystem>().isButtonInUnderground)
				{
					LiftDoorL.SendMessage("MoveDoorFunc");
					LiftDoorR.SendMessage("MoveDoorFunc");
				}
			}
			else
			{
				LiftDoorL.SendMessage("MoveDoorFunc");
				LiftDoorR.SendMessage("MoveDoorFunc");
			}
		}
		if (isButtonInLift)
		{
			if (Lift.GetComponent<LiftSystem>().posLift == 11636)
			{
				Lift.SendMessage("MoveLiftUp");
				Lift.GetComponent<LiftSystem>().moveLiftDown = false;
			}
			else if (Lift.GetComponent<LiftSystem>().posLift == 0)
			{
				Lift.SendMessage("MoveLiftDown");
				Lift.GetComponent<LiftSystem>().moveLiftUp = false;
			}
		}
		else if (isButtonInUnderground && !Lift.GetComponent<LiftSystem>().moveLiftDown && !Lift.GetComponent<LiftSystem>().moveLiftUp)
		{
			if (Lift.GetComponent<LiftSystem>().posLift == 0)
			{
				Lift.SendMessage("MoveLiftDown");
				Lift.GetComponent<LiftSystem>().moveLiftUp = false;
			}
		}
		else if (!isButtonInUnderground && !Lift.GetComponent<LiftSystem>().moveLiftDown && !Lift.GetComponent<LiftSystem>().moveLiftUp && Lift.GetComponent<LiftSystem>().posLift == 11636)
		{
			Lift.SendMessage("MoveLiftUp");
			Lift.GetComponent<LiftSystem>().moveLiftDown = false;
		}
	}

	private void MoveLiftDown()
	{
		moveLiftDown = true;
	}

	private void MoveLiftUp()
	{
		moveLiftUp = true;
	}

	private void MoveDoorFunc()
	{
		if (!Lift.GetComponent<LiftSystem>().moveLiftUp && !Lift.GetComponent<LiftSystem>().moveLiftDown && (posDoor == 125 || posDoor == 0))
		{
			DoorMove = true;
		}
	}
}
