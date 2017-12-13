
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GangUnity;

public class MainScript : MonoBehaviour {



	InputField userid;
	InputField nickname;

	// Use this for initialization
	void Start () {
		userid = this.transform.Find ("userid").GetComponent<InputField>();
		nickname = this.transform.Find ("nickname").GetComponent<InputField>();
		Button btnInit = this.transform.Find ("btnInit").GetComponent<Button>();
		btnInit.onClick.AddListener(OnInitClick);
		Button btnJump = this.transform.Find ("btnJump").GetComponent<Button>();
		btnJump.onClick.AddListener(OnJumpClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnInitClick()
	{
		if (userid.text.Length == 0 || nickname.text.Length == 0) {
			return;
		}
		GangSDK.init ("uzJkYFX6XSK4Ug4ggCy82g==");
		Dictionary<string, object> extParams = new Dictionary<string, object>();
		extParams.Add ("test_params", "1");
		GangSDK.login (userid.text, nickname.text, null, 0, null, extParams, gangSDKLoginSuccessCallback, gangSDKLoginFailureCallback);
	}

	private void OnJumpClick()
	{
		GangSDK.start ();
	}
  
	static void gangSDKLoginSuccessCallback() {
		
	}

	static void gangSDKLoginFailureCallback(string errorDomain, int errorCode, string errorDescription) {
		
	}
}
