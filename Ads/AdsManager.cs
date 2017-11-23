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
        #region Public var
        public bool isEnabled = true;
        public IAdsPlatform[] AdsPlatforms;
        #endregion

        #region Public const
        public enum ADSTYPES { INTERSTITIAL, REWARDED };
        #endregion

        #region Private const
        #endregion

        #region Private var
        static private int TargetAdsPlatformID;
        static private ADSTYPES TargetAdsType;
        static private float timeLimit;
        static private Callback.EventHandler callbackCompletedMain;
        static private Callback.EventHandler callbackErrorMain;
        #endregion

        private void Start()
        {
            foreach (IAdsPlatform adsPlatform in AdsPlatforms)
            {
                if(adsPlatform != null)
                {
                    adsPlatform.StartEvent += OnStarted;
                    adsPlatform.CompletedEvent += OnCompleted;
                    adsPlatform.ErrorEvent += OnError;
                }
            }
        }



        // Update is called once per frame
        void Update()
        {
            //Timelimit
            if (timeLimit > 0)
            {
                timeLimit -= Time.deltaTime;
                if (timeLimit <= 0)
                {
                    Debug.Log("AdsManager: timeLimit");
                    callbackErrorMain();
                }
            }
        }


        static public void ShowAd(ADSTYPES adstype = ADSTYPES.INTERSTITIAL, Callback.EventHandler callbackCompleted = null, Callback.EventHandler callbackError = null, float _timeLimit = 0)
        {
            Debug.Log("AdsManager: ShowAds");
            TargetAdsPlatformID = -1;
            TargetAdsType = adstype;
            callbackCompletedMain = callbackCompleted;
            callbackErrorMain = callbackError;
            timeLimit = _timeLimit;
            TryShowAds();
        }

        static public void TryShowAds()
        {
            Debug.Log("AdsManager: TryShowAds");
            TargetAdsPlatformID++;
            if (TargetAdsPlatformID < Instance.AdsPlatforms.Length)
            {
                IAdsPlatform AdsPlatform = Instance.AdsPlatforms[TargetAdsPlatformID];
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
                Debug.LogWarning("AdsManager: no more ads platforms");
                if (callbackErrorMain != null)
                    callbackErrorMain();
            }
        }

        static public void OnStarted()
        {
            Debug.Log("AdsManager OnStarted");
        }

        static public void OnCompleted()
        {
            Debug.Log("AdsManager OnCompleted");
            if (callbackCompletedMain != null)
                callbackCompletedMain();
        }

        static public void OnError()
        {
            Debug.Log("AdsManager OnError");
            if (callbackErrorMain != null)
                callbackErrorMain();
        }


    }
}
