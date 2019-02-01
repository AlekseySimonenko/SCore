using SCore.Framework;
using System;
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
        public event Action StartAnyAdEvent;

        public event Action CompletedAnyAdEvent;

        public event Action ErrorAnyAdEvent;

        public event Action CancelAnyAdEvent;

        //PUBLIC VARIABLES
        [SerializeField]
        private bool isEnabled = true;

        [SerializeField]
        public IAdsPlatform[] AdsPlatforms;

        //PRIVATE STATIC
        private int TargetAdsPlatformID;

        private ADSTYPES TargetAdsType;
        private float timeLimit;
        private Action callbackCompletedMain;
        private Action callbackErrorMain;
        private Action callbackCancelMain;

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
        private void Update()
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

        public bool IsEnabled()
        {
            return isEnabled;
        }

        public void ShowAd(ADSTYPES adstype = ADSTYPES.INTERSTITIAL, Action callbackCompleted = null, Action callbackError = null, Action callbackCancel = null, float _timeLimit = 0)
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

        private void TryShowAds()
        {
            Debug.Log("AdsManager: TryShowAds", Instance.gameObject);
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
                Debug.LogWarning("AdsManager: no more ads platforms", Instance.gameObject);
                callbackErrorMain?.Invoke();
            }
        }

        public bool IsAnyAdsReady(ADSTYPES adstype = ADSTYPES.INTERSTITIAL)
        {
            Debug.Log("AdsManager: IsAnyAdsReady " + adstype.ToString(), Instance.gameObject);

            for (int i = 0; i < Instance.AdsPlatforms.Length; i++)
            {
                IAdsPlatform AdsPlatform = Instance.AdsPlatforms[i];
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

        public void OnStarted()
        {
            Debug.Log("AdsManager: OnStarted", Instance.gameObject);

            StartAnyAdEvent?.Invoke();
        }

        public void OnCompleted()
        {
            Debug.Log("AdsManager: OnCompleted", Instance.gameObject);

            callbackCompletedMain?.Invoke();
            CompletedAnyAdEvent?.Invoke();
        }

        public void OnError()
        {
            Debug.Log("AdsManager: OnError", Instance.gameObject);

            callbackErrorMain?.Invoke();
            ErrorAnyAdEvent?.Invoke();
        }

        public void OnCancel()
        {
            Debug.Log("AdsManager: OnCancel", Instance.gameObject);

            callbackCancelMain?.Invoke();
            CancelAnyAdEvent?.Invoke();
        }
    }
}