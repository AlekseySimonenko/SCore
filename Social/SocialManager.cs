using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SCore
{
    //// <summary>
    /// Static class choise and control social platforms
    /// Only one social platform can be inited at one time!
    /// </summary>
    [RequireComponent(typeof(IServiceLoadingStep))]
    public class SocialManager : MonoBehaviourSingleton<SocialManager>
    {
        #region Public var
        public ISocialPlatform platformAutoInit;
        public UnityEvent OnInitActions;
        static public ISocialPlatform platform { get; private set; }
        #endregion

        #region Public const
        #endregion

        #region Private const
        #endregion

        #region Private var
        #endregion


        #region Init
        private void Start()
        {
            if (platformAutoInit != null)
                Init(platformAutoInit, OnAutoInitCompleted, null);
        }

        public void OnAutoInitCompleted()
        {
            if (OnInitActions != null)
                OnInitActions.Invoke();
        }

        static public void Init(ISocialPlatform _platform, Callback.EventHandler callbackCompleted, Callback.EventHandler callbackError)
        {
            Debug.Log("SocialManager Init platform " + _platform.GetPlatformID());
            platform = _platform;
            platform.InitCompletedEvent += callbackCompleted;
            platform.InitCompletedEvent += callbackError;
            platform.Init();
        }

        void Update()
        {

        }
        #endregion

    }
}
