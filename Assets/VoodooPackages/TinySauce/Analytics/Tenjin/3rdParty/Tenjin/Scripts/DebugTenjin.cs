using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Voodoo.Sauce.Internal;

public class DebugTenjin : BaseTenjin
{

	private const string TAG = "DebugTenjin";

	public override void Connect(){
		VoodooLog.Log(TAG, "Connecting " + ApiKey);
	}

	public override void Connect(string deferredDeeplink){
		VoodooLog.Log(TAG, "Connecting with deferredDeeplink " + deferredDeeplink);
	}

	public override void Init(string apiKey){
		VoodooLog.Log(TAG, "Initializing " + apiKey);
		ApiKey = apiKey;
	}

	public override void Init(string apiKey, string sharedSecret){
		VoodooLog.Log(TAG, "Initializing with secret " + apiKey);
		ApiKey = apiKey;
		SharedSecret = sharedSecret;
	}

	public override void SendEvent (string eventName){
		VoodooLog.Log(TAG, "Sending Event " + eventName);
	}

	public override void SendEvent (string eventName, string eventValue){
		VoodooLog.Log(TAG, "Sending Event " + eventName + " : " + eventValue);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature){
		VoodooLog.Log(TAG, "Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate) {
		VoodooLog.Log(TAG, "Sending DebugTenjin::GetDeeplink");
	}

	public override void OptIn(){
		VoodooLog.Log(TAG, "OptIn ");
	}

	public override void OptOut(){
		VoodooLog.Log(TAG, "OptOut ");
	}

	public override void OptInParams(List<string> parameters){
		VoodooLog.Log(TAG, "OptInParams");
	}

	public override void OptOutParams(List<string> parameters){
		VoodooLog.Log(TAG, "OptOutParams" );
	}

	public override void AppendAppSubversion(int subversion)
	{
		VoodooLog.Log(TAG, "AppendAppSubversion: " + subversion);
	}
}
