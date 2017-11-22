using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CoreVR
{

    //// <summary>
    /// Interface of ads platforms
    /// </summary>
    public class IAdsPlatform : MonoBehaviour
    {

        public event Callback.EventHandler ShowAdsEvent;

        public event Callback.EventHandler StartEvent;
        public event Callback.EventHandler CompletedEvent;
        public event Callback.EventHandler ErrorEvent;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowAds(Callback.EventHandler callbackStarted = null, Callback.EventHandler callbackCompleted = null, Callback.EventHandler callbackError = null)
        {
            Debug.Log("IAdsPlatform: ShowAd");

            StartEvent = callbackStarted;
            CompletedEvent = callbackCompleted;
            ErrorEvent = callbackError;

            if (ShowAdsEvent != null)
                ShowAdsEvent();
        }

        public void OnStart()
        {
            Debug.Log("IAdsPlatform: OnStart");
            if (ShowAdsEvent != null)
                StartEvent();
        }

        public void OnCompleted()
        {
            Debug.Log("IAdsPlatform: OnCompleted");
            if (CompletedEvent != null)
                CompletedEvent();
        }

        public void OnError()
        {
            Debug.Log("IAdsPlatform: OnError");
            if (ErrorEvent != null)
                ErrorEvent();
        }

    }

}