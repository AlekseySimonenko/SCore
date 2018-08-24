using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore.Ads
{
    /// <summary>
    /// Static class controlling all ads platforms
    /// </summary>
    public class AdsManager : MonoBehaviourSingleton<AdsManager>
    {
        //PUBLIC STATIC
        public enum ADSTYPES { INTERSTITIAL, REWARDED };

        //PUBLIC EVENTS
        static public event Action StartAnyAdEvent;
        static public event Action CompletedAnyAdEvent;
        static public event Action ErrorAnyAdEvent;
        static public event Action CancelAnyAdEvent;

        //PUBLIC VARIABLES
        [SerializeField]
        private bool isEnabled = true;
        [SerializeField]
        public IAdsPlatform[] AdsPlatforms;

        //PRIVATE STATIC
        static private int TargetAdsPlatformID;
        static private ADSTYPES TargetAdsType;
        static private float timeLimit;
        static private Action callbackCompletedMain;
        static private Action callbackErrorMain;
        static private Action callbackCancelMain;

        //PRIVATE VARIABLES


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
                    Debug.Log("AdsManager: timeLimit", Instance.gameObject);
                    callbackErrorMain();
                }
            }
        }

        static public bool IsEnabled()
        {
            return Instance.isEnabled;
        }


        static public void ShowAd(ADSTYPES adstype = ADSTYPES.INTERSTITIAL, Action callbackCompleted = null, Action callbackError = null, Action callbackCancel = null, float _timeLimit = 0)
        {
            Debug.Log("AdsManager: ShowAds", Instance.gameObject);
            TargetAdsPlatformID = -1;
            TargetAdsType = adstype;
            callbackCompletedMain = callbackCompleted;
            callbackErrorMain = callbackError;
            callbackCancelMain = callbackCancel;
            timeLimit = _timeLimit;
            TryShowAds();
        }

        static private void TryShowAds()
        {
            Debug.Log("AdsManager: TryShowAds", Instance.gameObject);
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
                        AdsPlatform.ShowRewarded();
                        break;
                }
            }
            else
            {
                Debug.LogWarning("AdsManager: no more ads platforms", Instance.gameObject);
                if (callbackErrorMain != null)
                    callbackErrorMain();
            }
        }

        static public bool IsAnyAdsReady(ADSTYPES adstype = ADSTYPES.INTERSTITIAL)
        {
            Debug.Log("AdsManager: IsAnyAdsReady " + adstype.ToString(), Instance.gameObject);

            for (int i = 0; i < Instance.AdsPlatforms.Length; i++)
            {
                IAdsPlatform AdsPlatform = Instance.AdsPlatforms[ i ];
                switch (adstype)
                {
                    case ADSTYPES.INTERSTITIAL:
                        if (AdsPlatform.IsInterstitialReady())
                            return true;
                        break;
                    case ADSTYPES.REWARDED:
                        if (AdsPlatform.IsRewardedReady())
                            return true;
                        break;
                }
            }

            return false;
        }


        static public void OnStarted()
        {
            Debug.Log("AdsManager: OnStarted", Instance.gameObject);

            if (StartAnyAdEvent != null)
                StartAnyAdEvent();
        }

        static public void OnCompleted()
        {
            Debug.Log("AdsManager: OnCompleted", Instance.gameObject);

            if (callbackCompletedMain != null)
                callbackCompletedMain();
            if (CompletedAnyAdEvent != null)
                CompletedAnyAdEvent();
        }

        static public void OnError()
        {
            Debug.Log("AdsManager: OnError", Instance.gameObject);

            if (callbackErrorMain != null)
                callbackErrorMain();
            if (ErrorAnyAdEvent != null)
                ErrorAnyAdEvent();
        }

        static public void OnCancel()
        {
            Debug.Log("AdsManager: OnCancel", Instance.gameObject);

            if (callbackCancelMain != null)
                callbackCancelMain();
            if (CancelAnyAdEvent != null)
                CancelAnyAdEvent();
        }


    }
}
