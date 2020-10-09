﻿using System.Collections.Generic;

 namespace Voodoo.Sauce.Internal.Analytics
{
    public static class TenjinWrapper
    {
        private const string TAG = "TenjinWrapper";

        private static bool _isInitialized;
        private static readonly Queue<string> _eventsQueue = new Queue<string>();

        internal static void Initialize(bool consentValue)
        {
            GetInstance().Init(TenjinConstants.ApiKey);

            Connect();
            SetConsent(consentValue);
            _isInitialized = true;
            while (_eventsQueue.Count > 0)
            {
                TrackEvent(_eventsQueue.Dequeue());
            }
        }

        internal static void TrackEvent(string eventName)
        {
            if (!_isInitialized)
            {
                _eventsQueue.Enqueue(eventName);
                return;
            }

            VoodooLog.Log(TAG, "Sending event " + eventName + " to Tenjin");
            GetInstance().SendEvent(eventName);
        }

        internal static void Connect()
        {
            GetInstance().Connect();
        }

        internal static void SetConsent(bool value)
        {
            if (value) GetInstance().OptIn();
            else GetInstance().OptOut();
        }

        private static BaseTenjin GetInstance()
        {
            return Tenjin.getInstance(TenjinConstants.ApiKey);
        }
    }
}