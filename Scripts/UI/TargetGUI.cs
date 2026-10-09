using UnityEngine;

public class TargetGUI : MonoBehaviour
{
	public GameObject target;

	public string TARGET_TEXT = "НАЙТИ И УНИЧТОЖИТЬ";

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		if (BaseLogic._control && (bool)target)
		{
			Vector3 vector = Camera.main.WorldToScreenPoint(target.transform.position);
			if (Camera.main.transform.InverseTransformPoint(target.transform.position).z > 0f)
			{
				GUI.color = new Color(base.GetComponent<GUITexture>().color.r, base.GetComponent<GUITexture>().color.g, base.GetComponent<GUITexture>().color.b, 1f);
				Rect position = new Rect(vector.x + 60f, (float)Screen.height - vector.y - 20f, 160f, 50f);
				base.GetComponent<GUITexture>().pixelInset = new Rect(vector.x - 32f, vector.y - 32f, 64f, 64f);
				float num = Vector3.Distance(Camera.main.transform.position, target.transform.position);
				GUI.Label(position, TARGET_TEXT + "\n" + (int)num + " М.");
			}
		}
		else
		{
			base.GetComponent<GUITexture>().pixelInset = new Rect(0f, 0f, 0f, 0f);
		}
	}
}
