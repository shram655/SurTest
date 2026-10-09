using System.Text;
using UnityEngine;

public class GameConsoleWithScrollView : MonoBehaviour
{
	public GUISkin guiSkin;

	public bool show_output = true;

	public bool show_stack;

	public static GameConsoleWithScrollView I;

	private int error_count;

	private StringBuilder strb = new StringBuilder();

	public Rect pos_rect = new Rect(50f, 75f, 400f, 400f);

	public Rect view_rect = new Rect(0f, 0f, 400f, 40000f);

	private Vector2 scroll_pos;

	public bool show;

	private void Awake()
	{
		I = this;
		strb.AppendLine("CONSOLE:");
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			show = !show;
			Debug.Log("~");
		}
	}

	private void OnEnable()
	{
		Application.RegisterLogCallback(HandleLog);
	}

	private void OnDisable()
	{
		Application.RegisterLogCallback(null);
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (type == LogType.Exception || type == LogType.Error)
		{
			error_count++;
		}
		if (show_output || show_stack)
		{
			if (show_output)
			{
				strb.AppendLine(logString);
			}
			if (show_stack || type == LogType.Exception || type == LogType.Error)
			{
				strb.AppendLine(stackTrace);
			}
		}
	}

	public void OnGUI()
	{
		GUI.skin = guiSkin;
		if (show)
		{
			GUI.Label(new Rect(pos_rect.x, pos_rect.y - 20f, 200f, 20f), "[errors " + error_count + "] length: " + strb.Length, "box");
			GUI.TextField(new Rect(pos_rect.x, pos_rect.y - 220f, 200f, 20f), string.Empty, "box");
			scroll_pos = GUI.BeginScrollView(pos_rect, scroll_pos, view_rect);
			GUI.TextArea(new Rect(0f, 0f, view_rect.width - 50f, view_rect.height), strb.ToString());
			GUI.EndScrollView();
		}
	}
}
