using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Tenjin  {

	public delegate void DeferredDeeplinkDelegate(Dictionary<string, string> deferredLinkData);

	//create dictionary of instances of tenjin with API keys
	private static Dictionary<string, BaseTenjin> _instances = new Dictionary<string, BaseTenjin>(); 

	//return instance with specific api key
	public static BaseTenjin getInstance(string apiKey){

		//if no api key exists then create a BaseTenjin object and store it into the dictionary of instances
		if (!_instances.ContainsKey (apiKey)) {
			_instances.Add(apiKey, createTenjin(apiKey, null));
		}
		return _instances [apiKey];
	}

	public static BaseTenjin getInstance(string apiKey, string sharedSecret){

		//if no api key exists then create a BaseTenjin object and store it into the dictionary of instances
		string instanceKey = apiKey + "." + sharedSecret;
		if (!_instances.ContainsKey (instanceKey)) {
			_instances.Add(instanceKey, createTenjin(apiKey, sharedSecret));
		}
		return _instances [instanceKey];
	}

	private static BaseTenjin createTenjin(string apiKey, string sharedSecret){
		GameObject tenjinGameObject = new GameObject("Tenjin");
		tenjinGameObject.hideFlags = HideFlags.HideAndDontSave;
		Object.DontDestroyOnLoad(tenjinGameObject);
		
#if UNITY_ANDROID && !UNITY_EDITOR
		BaseTenjin retTenjin = tenjinGameObject.AddComponent<AndroidTenjin>();
#elif UNITY_IPHONE && !UNITY_EDITOR
		BaseTenjin retTenjin = tenjinGameObject.AddComponent<IosTenjin>();
#else
		BaseTenjin retTenjin = tenjinGameObject.AddComponent<DebugTenjin>();
#endif
		if (sharedSecret != null) {
			retTenjin.Init (apiKey, sharedSecret);
		} else {
			retTenjin.Init (apiKey);
		}
		return retTenjin;
	}
}
