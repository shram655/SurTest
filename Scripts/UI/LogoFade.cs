using UnityEngine;

public class LogoFade : MonoBehaviour
{
	public GUITexture LOGO_BACK;

	public GUITexture LOGO_01;

	public GUITexture LOGO_02;

	private int fadeTimer;

	public GameObject GUIMenu;

	public UILabel _viewer_id;

	private void Start()
	{
		GUIMenu.SetActive(false);
		LOGO_BACK.color = Color.black;
	}

	private void FixedUpdate()
	{
		fadeTimer++;
		if (fadeTimer >= 0 && fadeTimer < 200)
		{
			LOGO_01.color = new Color(1f, 1f, 1f, LOGO_01.color.a + 0.0015f);
		}
		else if (fadeTimer >= 200 && fadeTimer < 400)
		{
			LOGO_01.color = new Color(1f, 1f, 1f, LOGO_01.color.a - 0.0015f);
		}
		else if (fadeTimer >= 500 && fadeTimer < 700)
		{
			LOGO_02.color = new Color(1f, 1f, 1f, LOGO_02.color.a + 0.0015f);
		}
		else if (fadeTimer >= 700 && fadeTimer < 900)
		{
			LOGO_02.color = new Color(1f, 1f, 1f, LOGO_02.color.a - 0.0015f);
		}
		if (fadeTimer >= 600)
		{
			LOGO_BACK.color = new Color(1f, 1f, 1f, LOGO_BACK.color.a - 0.002f);
		}
		if (fadeTimer >= 1100)
		{
			GUIMenu.SetActive(true);
			base.enabled = false;
		}
		_viewer_id.text = "ID: " + BaseLogic.viewer_id + "  Online: " + PhotonNetwork.countOfPlayers;
	}

	private void Update()
	{
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && BaseLogic.LOADING_COMPLETE)
		{
			LOGO_01.color = new Color(1f, 1f, 1f, 0f);
			LOGO_02.color = new Color(1f, 1f, 1f, 0f);
			LOGO_BACK.color = new Color(1f, 1f, 1f, 0f);
			GUIMenu.SetActive(true);
			base.enabled = false;
		}
	}
}
