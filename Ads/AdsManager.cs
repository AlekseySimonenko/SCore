using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCore.Ads
{
    /// <summary>
    /// Controlling all ads platforms mediation
    /// </summary>
    public class AdsManager : MonoBehaviour, IAdsManager
    {
        //EVENTS

        public event Action AdStarted;

        public event Action AdCompleted;

        public event Action AdErrorShowed;

        public event Action AdCanceled;

        //EDITOR VARIABLES
        [SerializeField]
        private bool _isEnabled = true;

        [SerializeField]
        private GameObject[] _adsPlatformsObjects;

        //PRIVATE VARIABLES
        protected List<IAdsPlatform> _adsPlatforms = new List<IAdsPlatform>();

        protected float _timeLimit;

        private void Start()
        {
            foreach (GameObject adsPlatformObject in _adsPlatformsObjects)
            {
                IAdsPlatform adsPlatform = adsPlatformObject.GetComponent<IAdsPlatform>();

                if (adsPlatform != null)
                {
                    //Manager public events
                    adsPlatform.AdStarted += OnAdStarted;
                    adsPlatform.AdCompleted += OnAdCompleted;
                    adsPlatform.AdErrorShowed += OnAdErrorShowed;
                    adsPlatform.AdCanceled += OnAdCanceled;

                    //Manager private delegates
                    adsPlatform.AdStarted += OnPlatformAdStarted;

                    //Registrate platform for manager
                    _adsPlatforms.Add(adsPlatform);

                    //Init platform
                    adsPlatform.Init();
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {
            //Timelimit
            if (_timeLimit > 0)
            {
                _timeLimit -= Time.unscaledDeltaTime;
                if (_timeLimit <= 0)
                {
                    Debug.Log("AdsManager: timeLimit", gameObject);
                    OnAdErrorShowed();
                }
            }
        }

        public virtual bool IsEnabled()
        {
            return _isEnabled;
        }

        public virtual bool IsInterstitialReady()
        {
            Debug.Log("AdsManager: IsInterstitialReady ", gameObject);

            for (int i = 0; i < _adsPlatforms.Count; i++)
            {
                IAdsPlatform AdsPlatform = _adsPlatforms[i];
                if (AdsPlatform.IsInterstitialReady())
                    return true;
            }
            return false;
        }

        public virtual bool IsRewardedReady()
        {
            Debug.Log("AdsManager: IsRewardedReady ", gameObject);

            for (int i = 0; i < _adsPlatforms.Count; i++)
            {
                IAdsPlatform AdsPlatform = _adsPlatforms[i];
                if (AdsPlatform.IsRewardedReady())
                    return true;
            }
            return false;
        }

        public virtual void ShowInterstitial(float timeLimit = 0)
        {
            Debug.Log("AdsManager: ShowInterstitial", gameObject);

            for (int i = 0; i < _adsPlatforms.Count; i++)
            {
                IAdsPlatform AdsPlatform = _adsPlatforms[i];
                if (AdsPlatform.IsInterstitialReady())
                {
                    AdsPlatform.ShowInterstitial();
                    _timeLimit = timeLimit;
                    return;
                }
            }

            //If no any ad showed
            Debug.Log("AdsManager: no any interstitial ads ready", gameObject);
            OnAdErrorShowed();
        }

        public virtual void ShowRewarded(float timeLimit = 0)
        {
            Debug.Log("AdsManager: ShowRewarded", gameObject);

            for (int i = 0; i < _adsPlatforms.Count; i++)
            {
                IAdsPlatform AdsPlatform = _adsPlatforms[i];
                if (AdsPlatform.IsRewardedReady())
                {
                    AdsPlatform.ShowRewarded();
                    _timeLimit = timeLimit;
                    return;
                }
            }

            //If no any ad showed
            Debug.Log("AdsManager: no any reward ads ready", gameObject);
            OnAdErrorShowed();
        }

        private void OnPlatformAdStarted()
        {
            Debug.Log("AdsManager: OnAdStarted", gameObject);
            _timeLimit = 0;
        }

        /// <summary>
        /// The event-invoking method that derived classes can use.
        /// </summary>
        protected void OnAdStarted()
        {
            Debug.Log("AdsManager: OnAdStarted", gameObject);
            AdStarted?.Invoke();
        }

        /// <summary>
        /// The event-invoking method that derived classes can use.
        /// </summary>
        protected void OnAdCompleted()
        {
            Debug.Log("AdsManager: OnAdCompleted", gameObject);
            AdCompleted?.Invoke();
        }

        /// <summary>
        /// The event-invoking method that derived classes can use.
        /// </summary>
        protected void OnAdErrorShowed()
        {
            Debug.Log("AdsManager: OnAdErrorShowed", gameObject);
            AdErrorShowed?.Invoke();
        }

        /// <summary>
        /// The event-invoking method that derived classes can use.
        /// </summary>
        protected void OnAdCanceled()
        {
            Debug.Log("AdsManager: OnAdCanceled", gameObject);
            AdCanceled?.Invoke();
        }
    }
}