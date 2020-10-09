using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

public class IosTenjin : BaseTenjin {

#if UNITY_IPHONE && !UNITY_EDITOR

	[DllImport ("__Internal")]
	private static extern void iosTenjinInit(string apiKey);

	[DllImport ("__Internal")]
	private static extern void iosTenjinInitWithSharedSecret(string apiKey, string sharedSecret);

	[DllImport ("__Internal")]
	private static extern void iosTenjinConnect();

	[DllImport ("__Internal")]
	private static extern void iosTenjinConnectWithDeferredDeeplink(string deferredDeeplink);

	[DllImport ("__Internal")]
	private static extern void iosTenjinOptIn();

	[DllImport ("__Internal")]
	private static extern void iosTenjinOptOut();

	[DllImport ("__Internal")]
	private static extern void iosTenjinOptInParams(String[] parameters, int size);

	[DllImport ("__Internal")]
	private static extern void iosTenjinOptOutParams(String[] parameters, int size);

    [DllImport ("__Internal")]
    private static extern void iosTenjinAppendAppSubversion(int subversion);

	[DllImport ("__Internal")]
	private static extern void iosTenjinSendEvent(string eventName);

	[DllImport ("__Internal")]
	private static extern void iosTenjinSendEventWithValue(string eventName, string eventValue);

	[DllImport ("__Internal")]
	private static extern void iosTenjinTransaction(string productId, string currencyCode, int quantity, double unitPrice);

	[DllImport ("__Internal")]
	private static extern void iosTenjinTransactionWithReceiptData(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt);

	[DllImport ("__Internal")]
 	private static extern void iosTenjinRegisterDeepLinkHandler(DeepLinkHandlerNativeDelegate deepLinkHandlerNativeDelegate);

	private delegate void DeepLinkHandlerNativeDelegate(IntPtr deepLinkDataPairArray, int deepLinkDataPairCount);
	
	private static readonly Stack<Dictionary<string, string>> deferredDeeplinkEvents = new Stack<Dictionary<string, string>>();
	private static Tenjin.DeferredDeeplinkDelegate registeredDeferredDeeplinkDelegate;

	public override void Init(string apiKey){
		if (Debug.isDebugBuild) {
			Debug.Log ("iOS Initializing");
		}
		ApiKey = apiKey;
		iosTenjinInit (ApiKey);
	}

	public override void Init(string apiKey, string sharedSecret){
		if (Debug.isDebugBuild) {
			Debug.Log ("iOS Initializing with Shared Secret");
		}
		ApiKey = apiKey;
		SharedSecret = sharedSecret;
		iosTenjinInitWithSharedSecret (ApiKey, SharedSecret);
	}

	public override void Connect(){
		if (Debug.isDebugBuild) {
			Debug.Log ("iOS Connecting");
		}
		iosTenjinConnect();
	}
	
	public override void Connect(string deferredDeeplink){
		if (Debug.isDebugBuild) {
			Debug.Log ("iOS Connecting with deferredDeeplink " + deferredDeeplink);
		}
		iosTenjinConnectWithDeferredDeeplink (deferredDeeplink);
	}

	public override void OptIn(){
		if (Debug.isDebugBuild) {
			Debug.Log ("iOS OptIn");
		}
		iosTenjinOptIn ();
	}

	public override void OptOut(){
		if (Debug.isDebugBuild) {
			Debug.Log ("iOS OptOut");
		}
		iosTenjinOptOut ();
	}

	public override void OptInParams(List<string> parameters){
		if (Debug.isDebugBuild) {
			Debug.Log ("iOS OptInParams" + parameters.ToString());
		}
		iosTenjinOptInParams (parameters.ToArray(), parameters.Count);
	}

	public override void OptOutParams(List<string> parameters){
		if (Debug.isDebugBuild) {
			Debug.Log ("iOS OptOutParams" + parameters.ToString());
		}
		iosTenjinOptOutParams (parameters.ToArray(), parameters.Count);
	}

	public override void AppendAppSubversion(int subversion){
		if (Debug.isDebugBuild) {
			Debug.Log ("iOS AppendAppSubversion " + subversion);
		}
		iosTenjinAppendAppSubversion (subversion);
	}

	public override void SendEvent(string eventName){
		if (Debug.isDebugBuild) {
			Debug.Log ("iOS Sending Event " + eventName);
		}
		iosTenjinSendEvent(eventName);
	}

	public override void SendEvent(string eventName, string eventValue){
		if (Debug.isDebugBuild) {
			Debug.Log ("iOS Sending Event " + eventName + " : " + eventValue);
		}
		iosTenjinSendEventWithValue(eventName, eventValue);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature){
		signature = null;

		//only if the receipt and transaction_id are not null, then try to validate the transaction. Otherwise manually record the transaction
		if(receipt != null && transactionId != null){
			if (Debug.isDebugBuild) {
				Debug.Log ("iOS Transaction with receipt " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt);
			}
			iosTenjinTransactionWithReceiptData(productId, currencyCode, quantity, unitPrice, transactionId, receipt);
		}
		else{
			if (Debug.isDebugBuild) {
				Debug.Log ("iOS Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice);
			}
			iosTenjinTransaction(productId, currencyCode, quantity, unitPrice);
		}
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate) {
		if (Debug.isDebugBuild) {
			Debug.Log ("Sending IosTenjin::GetDeeplink");
		}
		registeredDeferredDeeplinkDelegate = deferredDeeplinkDelegate;
		iosTenjinRegisterDeepLinkHandler(DeepLinkHandler);
	}

	private void Update() {
		lock (deferredDeeplinkEvents) {
			while (deferredDeeplinkEvents.Count > 0) {
				Dictionary<string, string> deepLinkData = deferredDeeplinkEvents.Pop();
				if (registeredDeferredDeeplinkDelegate != null) {
					registeredDeferredDeeplinkDelegate(deepLinkData);
				}
			}
		}
	}

	[MonoPInvokeCallback(typeof(DeepLinkHandlerNativeDelegate))]
	private static void DeepLinkHandler(IntPtr deepLinkDataPairArray, int deepLinkDataPairCount) {
		if (deepLinkDataPairArray == IntPtr.Zero)
			return;

		Dictionary<string, string> deepLinkData = 
			NativeUtility.MarshalStringStringDictionary(deepLinkDataPairArray, deepLinkDataPairCount);

		lock (deferredDeeplinkEvents) {
			deferredDeeplinkEvents.Push(deepLinkData);
		}
	}

	private static class NativeUtility {
		/// <summary>
		/// Marshals a native linear array of structs to the managed array.
		/// </summary>
		public static T[] MarshalNativeStructArray<T>(IntPtr nativeArrayPtr, int nativeArraySize) where T : struct {
			if (nativeArrayPtr == IntPtr.Zero)
				throw new ArgumentNullException("nativeArrayPtr");

			if (nativeArraySize < 0)
				throw new ArgumentOutOfRangeException("nativeArraySize");

			T[] managedArray = new T[nativeArraySize];
			IntPtr currentNativeArrayPtr = nativeArrayPtr;
			int structSize = Marshal.SizeOf(typeof(T));
			for (int i = 0; i < nativeArraySize; i++) {
				T marshaledStruct = (T) Marshal.PtrToStructure(currentNativeArrayPtr, typeof(T));
				managedArray[i] = marshaledStruct;
				currentNativeArrayPtr = (IntPtr) (currentNativeArrayPtr.ToInt64() + structSize);
			}

			return managedArray;
		}
		
		/// <summary>
		/// Marshals the native representation to a IDictionary&lt;string, string&gt;.
		/// </summary>
		public static Dictionary<string, string> MarshalStringStringDictionary(IntPtr nativePairArrayPtr, int nativePairArraySize) {
			if (nativePairArrayPtr == IntPtr.Zero)
				throw new ArgumentNullException("nativePairArrayPtr");

			if (nativePairArraySize < 0)
				throw new ArgumentOutOfRangeException("nativePairArraySize");

			Dictionary<string, string> dictionary = new Dictionary<string, string>(nativePairArraySize);
			StringStringKeyValuePair[] pairs = MarshalNativeStructArray<StringStringKeyValuePair>(nativePairArrayPtr, nativePairArraySize);
			foreach (StringStringKeyValuePair pair in pairs) {
				dictionary.Add(pair.Key, pair.Value);
			}
			return dictionary;
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public struct StringStringKeyValuePair {
			public string Key;
			public string Value;
		}
	}

#else
	public override void Init(string apiKey){
		Debug.Log ("iOS Initializing");
		ApiKey = apiKey;
	}

	public override void Init(string apiKey, string sharedSecret){
		Debug.Log ("iOS Initializing with Shared Secret");
		ApiKey = apiKey;
		SharedSecret = sharedSecret;
	}

	public override void Connect(){
		Debug.Log ("iOS Connecting");
	}

	public override void Connect(string deferredDeeplink){
		Debug.Log ("Connecting with deferredDeeplink " + deferredDeeplink);
	}

	public override void SendEvent(string eventName){
		Debug.Log ("iOS Sending Event " + eventName);
	}

	public override void SendEvent(string eventName, string eventValue){
		Debug.Log ("iOS Sending Event " + eventName + " : " + eventValue);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature){
		Debug.Log ("iOS Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate) {
		Debug.Log ("Sending IosTenjin::GetDeeplink");
	}

	public override void OptIn(){
		Debug.Log ("iOS OptIn");
	}

	public override void OptOut(){
		Debug.Log ("iOS OptOut");
	}

	public override void OptInParams(List<string> parameters){
		Debug.Log ("iOS OptInParams");
	}

	public override void OptOutParams(List<string> parameters){
		Debug.Log ("iOS OptOutParams");
	}

	public override void AppendAppSubversion(int subversion){
		Debug.Log("iOS AppendAppSubversion");
	}
#endif
}
