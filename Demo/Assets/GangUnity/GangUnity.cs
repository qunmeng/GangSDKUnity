
using AOT;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace GangUnity {

	public class GangSDK {

		private delegate void GangSDKLoginSuccessNativeCallback();

		private delegate void GangSDKLoginFailureNativeCallback(string errorDomain, int errorCode, string errorDescription);

		[MonoPInvokeCallback(typeof(GangSDKLoginSuccessNativeCallback))]  
		private static void gangSDKLoginSuccessNativeCallback() {
			isInited = true;
			if (loginSuccessCallBack != null) {
				loginSuccessCallBack ();
			}
		}
		[MonoPInvokeCallback(typeof(GangSDKLoginFailureNativeCallback))]  
		private static void gangSDKLoginFailureNativeCallback(string errorDomain, int errorCode, string errorDescription) {
			if (loginFailureCallBack != null) {
				loginFailureCallBack (errorDomain, errorCode, errorDescription);
			}
		}
		
		#if UNITY_ANDROID
		private const string LIBRARY_NAME = "gangunity";
		[DllImport (LIBRARY_NAME)]
		private static extern void loginGangSDK (string userid, string nickname, string headIconUrl, int gameLevel, string gameRole, string extParams, GangSDKLoginSuccessNativeCallback success, GangSDKLoginFailureNativeCallback failure);
		#elif UNITY_IPHONE
		private const string LIBRARY_NAME = "__Internal";
		[DllImport (LIBRARY_NAME)]
		private static extern void initGangSDK (string key);
		[DllImport (LIBRARY_NAME)]
		private static extern void loginGangSDK (string userid, string nickname, string headIconUrl, int gameLevel, string gameRole, string extParams, GangSDKLoginSuccessNativeCallback success, GangSDKLoginFailureNativeCallback failure);
		[DllImport (LIBRARY_NAME)]
		private static extern void jumpGangSDK ();
		#else

		#endif

		public delegate void GangSDKLoginSuccessCallback();

		public delegate void GangSDKLoginFailureCallback(string errorDomain, int errorCode, string errorDescription);

		public static GangSDKLoginSuccessCallback loginSuccessCallBack;
		public static GangSDKLoginFailureCallback loginFailureCallBack;

		private static bool isInited;

		public static void init(string key) {
			#if UNITY_ANDROID
			//注解1
			using (AndroidJavaClass gangSDKClass = new AndroidJavaClass("com.qm.gangunitybridge.GangUnityBridge"))
			{
				gangSDKClass.CallStatic("initGangSDK", "uzJkYFX6XSK4Ug4ggCy82g==");
			}
			#elif UNITY_IPHONE
			initGangSDK (key);
			#endif
		}

		public static void login(string userid, string nickname, string headIconUrl, int gameLevel, string gameRole, Dictionary<string, object> extParams, GangSDKLoginSuccessCallback success, GangSDKLoginFailureCallback failure) {
			loginSuccessCallBack = success;
			loginFailureCallBack = failure;
			#if UNITY_ANDROID || UNITY_IPHONE
			string json = dictionaryToJsonString (extParams);
			loginGangSDK (userid, nickname, headIconUrl, gameLevel, gameRole, json, gangSDKLoginSuccessNativeCallback, gangSDKLoginFailureNativeCallback);
			#endif
		}

		public static void start() {
			if (isInited) {
				#if UNITY_ANDROID
				//注解1
				using (AndroidJavaClass gangSDKClass = new AndroidJavaClass("com.qm.gangunitybridge.GangUnityBridge"))
				{
					gangSDKClass.CallStatic("startGangSDK");
				}
				#elif UNITY_IPHONE
				jumpGangSDK ();
				#endif
			}
		}

		private static string dictionaryToJsonString(Dictionary<string, object> dict) {
			string jsonString = null;
			List<string> keys = new List<string> (dict.Keys);
			for (int i = 0; i < keys.Count; i++)
			{
				string key = keys[i];
				object value = dict [key];
				string valueString = getValueString (value);
				if (valueString != null) {
					if (jsonString == null) {
						jsonString = "{";
					} else {
						jsonString += ",";
					}
					jsonString += "\"" + key + "\"" + ":" + valueString;
				}
			}
			if (jsonString != null) {
				jsonString += "}";
			}
			return jsonString;
		}

		private static string listToJsonString(List<object> list) {
			string jsonString = null;
			for (int i = 0; i < list.Count; i++) {
				object value = list [i];
				string valueString = getValueString (value);
				if (valueString != null) {
					if (jsonString == null) {
						jsonString = "[";
					} else {
						jsonString += ",";
					}
					jsonString += valueString;
				}
			}
			if (jsonString != null) {
				jsonString += "]";
			}
			return jsonString;
		}

		private static string getValueString(object value) {
			string valueString = null;
			if (value.GetType().IsPrimitive) {
				valueString = value + "";
			} else if (value.GetType() == typeof(string)) {
				valueString = "\"" + value + "\"";
			} else if (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>)) {
				Debug.Assert(value.GetType ().GetGenericArguments () [0] == typeof(string), "GangSDK extParams 内的 Dictionary 参数必须声明为 Dictionary<string, object>");
				Debug.Assert(value.GetType ().GetGenericArguments () [1] == typeof(object), "GangSDK extParams 内的 Dictionary 参数必须声明为 Dictionary<string, object>");
				Dictionary<string, object> valueDict = value as Dictionary<string, object>;
				if (valueDict != null) {
					valueString = dictionaryToJsonString(valueDict);
				}
			} else if (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(List<>)) {
				Debug.Assert(value.GetType ().GetGenericArguments () [0] == typeof(object), "GangSDK extParams 内的 List 参数必须声明为 List<object>");
				List<object> valueList = value as List<object>;
				if (valueList != null) {
					valueString = listToJsonString(valueList);
				}
			}
			return valueString;
		}
	}
}
