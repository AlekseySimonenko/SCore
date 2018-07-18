using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore
{
    //// <summary>
    /// Static class controlling all ads platforms
    /// </summary>
    public class AdsManager : MonoBehaviourSingleton<AdsManager>
    {
        /// PUBLIC VARIABLES
        [SerializeField]
        private bool isEnabled = true;
        [SerializeField]
        public IAdsPlatform[] AdsPlatforms;

        static public event Callback.EventHandler StartAnyAdEvent;
        static public event Callback.EventHandler CompletedAnyAdEvent;
        static public event Callback.EventHandler ErrorAnyAdEvent;
        static public event Callback.EventHandler CancelAnyAdEvent;

        /// PUBLIC CONSTANTS
        public enum ADSTYPES { INTERSTITIAL, REWARDED };

        /// PRIVATE CONSTANTS
        private float reloadTimer;

        /// PRIVATE VARIABLES
        static private int TargetAdsPlatformID;
        static private ADSTYPES TargetAdsType;
        static private float timeLimit;
        static private Callback.EventHandler callbackCompletedMain;
        static private Callback.EventHandler callbackErrorMain;
        static private Callback.EventHandler callbackCancelMain;

        private void Start()
        {
            foreach (IAdsPlatform adsPlatform in AdsPlatforms)
            {
                if (adsPlatform != null)
                {
                    adsPlatform.StartEvent += OnStarted;
                    adsPlatform.CompletedEvent += OnCompleted;
                    adsPlatform.ErrorEvent += OnError;
                    adsPlatform.CancelEvent += OnCancel;

                    adsPlatform.Init();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            //Timelimit
            if (timeLimit > 0)
            {
                timeLimit -= Time.unscaledDeltaTime;
                if (timeLimit <= 0)
                {
                    Debug.Log("AdsManager: timeLimit");
                    callbackErrorMain();
                }
            }
        }

        static public bool IsEnabled()
        {
            return Instance.isEnabled;
        }


        static public void ShowAd(ADSTYPES adstype = ADSTYPES.INTERSTITIAL, Callback.EventHandler callbackCompleted = null, Callback.EventHandler callbackError = null, Callback.EventHandler callbackCancel = null, float _timeLimit = 0)
        {
            Debug.Log("AdsManager: ShowAds");
            TargetAdsPlatformID = -1;
            TargetAdsType = adstype;
            callbackCompletedMain = callbackCompleted;
            callbackErrorMain = callbackError;
            callbackCancelMain = callbackCancel;
            timeLimit = _timeLimit;
            TryShowAds();
        }

        static public void TryShowAds()
        {
            Debug.Log("AdsManager: TryShowAds");
            TargetAdsPlatformID++;
            if (TargetAdsPlatformID < Instance.AdsPlatforms.Length)
            {
                IAdsPlatform AdsPlatform = Instance.AdsPlatforms[ TargetAdsPlatformID ];
                switch (TargetAdsType)
                {
                    case ADSTYPES.INTERSTITIAL:
                        AdsPlatform.ShowInterstitial();
                        break;
                    case ADSTYPES.REWARDED:
#if !UNITY_IOS
                        AdsPlatform.ShowRewarded();
#endif
                        break;
                }
            }
            else
            {
                Debug.LogWarning("AdsManager: no more ads platforms");
                if (callbackErrorMain != null)
                    callbackErrorMain();
            }
        }

        static public bool IsAnyAdsReady(ADSTYPES adstype = ADSTYPES.INTERSTITIAL)
        {
            Debug.Log("AdsManager: IsAnyAdsReady " + adstype.ToString());

            for (int i = 0; i < Instance.AdsPlatforms.Length; i++)
            {
                IAdsPlatform AdsPlatform = Instance.AdsPlatforms[ i ];
                switch (TargetAdsType)
                {
                    case ADSTYPES.INTERSTITIAL:
                        if (AdsPlatform.IsInterstitialReady())
                            return true;
                        break;
#if !UNITY_IOS
                    case ADSTYPES.REWARDED:
                        if (AdsPlatform.IsRewardedReady())
                            return true;
                        break;
#endif
                }
            }

            return false;
        }


        static public void OnStarted()
        {
            Debug.Log("AdsManager OnStarted");
            if (StartAnyAdEvent != null)
                StartAnyAdEvent();
        }

        static public void OnCompleted()
        {
            Debug.Log("AdsManager OnCompleted");
            if (callbackCompletedMain != null)
                callbackCompletedMain();
            if (CompletedAnyAdEvent != null)
                CompletedAnyAdEvent();
        }

        static public void OnError()
        {
            Debug.Log("AdsManager OnError");
            if (callbackErrorMain != null)
                callbackErrorMain();
            if (ErrorAnyAdEvent != null)
                ErrorAnyAdEvent();
        }

        static public void OnCancel()
        {
            Debug.Log("AdsManager OnCancel");
            if (callbackCancelMain != null)
                callbackCancelMain();
            if (CancelAnyAdEvent != null)
                CancelAnyAdEvent();
        }


    }
}
