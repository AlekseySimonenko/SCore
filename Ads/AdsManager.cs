using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore
{
    public class AdsManager : MonoBehaviour
    {

        public IAdsPlatform[] AdsPlatforms;
        public bool autoRun = false;
        public float autoRunTimer = 10.0F;

        private int TargetAdsPlatformID;
        private float timeLimit;

        static public AdsManager instance;

        // Use this for initialization
        void Start()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            //AutoRunner
            if (autoRun)
            {
                if (autoRunTimer > 0)
                {
                    autoRunTimer -= Time.deltaTime;
                    if (autoRunTimer < 0)
                        ShowAds();
                }
            }

            //Timelimit
            if (timeLimit > 0)
            {
                timeLimit -= Time.deltaTime;
                if (timeLimit <= 0)
                {
                    Debug.Log("AdsManager: timeLimit");
                    callbackCompletedMain();
                }
            }
        }

        Callback.EventHandler callbackCompletedMain;
        Callback.EventHandler callbackErrorMain;

        public void ShowAds(Callback.EventHandler callbackCompleted = null, Callback.EventHandler callbackError = null, float _timeLimit = 0)
        {
            Debug.Log("AdsManager: ShowAds");
            TargetAdsPlatformID = -1;
            callbackCompletedMain = callbackCompleted;
            callbackErrorMain = callbackError;
            timeLimit = _timeLimit;
            TryShowAds();
        }


        public void TryShowAds()
        {
            Debug.Log("AdsManager: TryShowAds");
            TargetAdsPlatformID++;
            if (TargetAdsPlatformID < AdsPlatforms.Length)
            {
                IAdsPlatform AdsPlatform = AdsPlatforms[TargetAdsPlatformID];
                AdsPlatform.ShowAds(OnStarted, callbackCompletedMain, TryShowAds);
            }
            else
            {
                Debug.LogWarning("AdsManager: no more ads platforms");
                if (callbackErrorMain != null)
                    callbackErrorMain();
            }
        }

        public void OnStarted()
        {
            Debug.Log("AdsManager OnStarted");
            timeLimit = 0;
        }


    }
}
