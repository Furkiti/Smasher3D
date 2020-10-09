using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AndroidTenjin : BaseTenjin {

	private const string AndroidJavaTenjinClass = "com.tenjin.android.TenjinSDK";

#if UNITY_ANDROID && !UNITY_EDITOR
	private AndroidJavaObject tenjinJava = null;
	private AndroidJavaObject activity = null;

	public override void Init(string apiKey){
		if (Debug.isDebugBuild) {
			Debug.Log ("Android Initializing");
		}
		ApiKey = apiKey;
		initActivity();
		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}
		tenjinJava = sdk.CallStatic<AndroidJavaObject> ("getInstance", activity, apiKey);
	}

	public override void Init(string apiKey, string sharedSecret){
		if (Debug.isDebugBuild) {
			Debug.Log ("Android Initializing with Shared Secret");
		}
		ApiKey = apiKey;
		SharedSecret = sharedSecret;
		initActivity();
		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}
		tenjinJava = sdk.CallStatic<AndroidJavaObject> ("getInstance", activity, apiKey, sharedSecret);
	}

	private void initActivity(){
		AndroidJavaClass javaContext = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		activity = javaContext.GetStatic<AndroidJavaObject>("currentActivity");
	}

	public override void Connect() {
		string optInOut = null;
		if (optIn) {
			optInOut = "optin";
		}
		else if (optOut) {
			optInOut = "optout";
		}
		object[] args = new object[]{null, optInOut};
		tenjinJava.Call ("connect", args);
	}

	public override void Connect(string deferredDeeplink){
		string optInOut = null;
		if (optIn) {
			optInOut = "optin";
		}
		else if (optOut) {
			optInOut = "optout";
		}
		object[] args = new object[]{deferredDeeplink, optInOut};
		tenjinJava.Call ("connect", args);
	}

	//SendEvent accepts a single eventName as a String
	public override void SendEvent (string eventName){
		object[] args = new object[]{eventName};
		tenjinJava.Call ("eventWithName", args);
	}

	//SendEvent accepts eventName as a String and eventValue as a String
	public override void SendEvent (string eventName, string eventValue){
		object[] args = new object[]{eventName, eventValue};
		tenjinJava.Call ("eventWithNameAndValue", args);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature){

		transactionId = null;
		//if the receipt and signature have values then try to validate. if there are no values then manually log the transaction.
		if(receipt != null && signature != null){
			object[] receiptArgs = new object[]{productId, currencyCode, quantity, unitPrice, receipt, signature};
			if (Debug.isDebugBuild) {
				Debug.Log ("Android Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + receipt + ", " + signature);
			}		
			tenjinJava.Call ("transaction", receiptArgs);
		}
		else{
			object[] args = new object[]{productId, currencyCode, quantity, unitPrice};
			if (Debug.isDebugBuild) {
				Debug.Log ("Android Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice);
			}
			tenjinJava.Call ("transaction", args);
		}
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate) {
		DeferredDeeplinkListener onDeferredDeeplinkListener = new DeferredDeeplinkListener(deferredDeeplinkDelegate);
		tenjinJava.Call ("getDeeplink", onDeferredDeeplinkListener);
	}

	private class DeferredDeeplinkListener : AndroidJavaProxy {
		private Tenjin.DeferredDeeplinkDelegate callback;

		public DeferredDeeplinkListener(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkCallback) : base("com.tenjin.android.Callback") {
			this.callback = deferredDeeplinkCallback;
		}

		public void onSuccess(bool clickedTenjinLink, bool isFirstSession, AndroidJavaObject data) {
			Dictionary<string, string> deeplinkData = new Dictionary<string, string>();
			string adNetwork = data.Call<string>("get", "ad_network");
			string campaignId = data.Call<string>("get", "campaign_id");
			string advertisingId = data.Call<string>("get", "advertising_id");
			string deferredDeeplink = data.Call<string>("get", "deferred_deeplink_url");

			if (!string.IsNullOrEmpty(adNetwork)) {
				deeplinkData["ad_network"] = adNetwork;
			}
			if (!string.IsNullOrEmpty(campaignId)) {
				deeplinkData["campaign_id"] = campaignId;
			}
			if (!string.IsNullOrEmpty(advertisingId)) {
				deeplinkData["advertising_id"] = advertisingId;
			}
			if (!string.IsNullOrEmpty(deferredDeeplink)) {
				deeplinkData["deferred_deeplink_url"] = deferredDeeplink;
			}

			deeplinkData.Add("clicked_tenjin_link", Convert.ToString(clickedTenjinLink));
			deeplinkData.Add("is_first_session", Convert.ToString(isFirstSession));

			callback(deeplinkData);
		}
	}

	public override void OptIn(){
		optIn = true;
		tenjinJava.Call ("optIn");
	}

	public override void OptOut(){
		optOut = true;
		tenjinJava.Call ("optOut");
	}

	public override void OptInParams(List<string> parameters){
		tenjinJava.Call ("optInParams", new object[] {parameters.ToArray()});
	}

	public override void OptOutParams(List<string> parameters){
		tenjinJava.Call ("optOutParams", new object[] {parameters.ToArray()});
	}

    public override void AppendAppSubversion (int subversion){
        object[] args = new object[]{subversion};
        tenjinJava.Call ("appendAppSubversion", args);
    }

#else
	public override void Init(string apiKey){
		Debug.Log ("Android Initializing");
		ApiKey = apiKey;
	}

	public override void Init(string apiKey, string sharedSecret){
		Debug.Log ("Android Initializing with Shared Secret");
		ApiKey = apiKey;
		SharedSecret = sharedSecret;
	}

	public override void Connect(){
		Debug.Log ("Android Connecting");
	}

	public override void Connect(string deferredDeeplink){
		Debug.Log ("Android Connecting with deferredDeeplink " + deferredDeeplink);
	}

	public override void SendEvent (string eventName){
		Debug.Log ("Android Sending Event " + eventName);
	}

	public override void SendEvent (string eventName, string eventValue){
		Debug.Log ("Android Sending Event " + eventName + " : " + eventValue);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature){
		Debug.Log ("Android Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate) {
		Debug.Log ("Sending AndroidTenjin::GetDeeplink");
	}

	public override void OptIn(){
		Debug.Log ("Sending AndroidTenjin::OptIn");
	}

	public override void OptOut(){
		Debug.Log ("Sending AndroidTenjin::OptOut");
	}

	public override void OptInParams(List<string> parameters){
		Debug.Log ("Sending AndroidTenjin::OptInParams");
	}

	public override void OptOutParams(List<string> parameters){
		Debug.Log ("Sending AndroidTenjin::OptOutParams");
	}

	public override void AppendAppSubversion(int subversion)
	{
		Debug.Log("Sending AndroidTenjin::AppendAppSubversion :" + subversion);
	}
#endif
}
