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
        
        static public event Callback.EventHandler InitCompletedEvent;
        static public event Callback.EventHandler InitErrorEvent;
        static public event Callback.EventHandler LoginEvent;
        static public event Callback.EventHandler LogoutEvent;

        static public bool loginCompleted { get { return platform.loginCompleted; } protected set { } }
        #endregion

        #region Public const
        #endregion

        #region Private const
        #endregion

        #region Private var
        static private ISocialPlatform platform;
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
            platform.InitErrorEvent += callbackError;

            platform.InitCompletedEvent += OnInitComleted;
            platform.InitErrorEvent += OnInitErrorEvent;
            platform.LoginEvent += OnLogin;
            platform.LogoutEvent += OnLogout;

        platform.Init();
        }

        void Update()
        {

        }
        #endregion


        #region Static realisation of ISocialPlatform interface

        static public void OnInitComleted()
        {
            if (InitCompletedEvent != null)
                InitCompletedEvent();
        }

        static public void OnInitErrorEvent()
        {
            if (InitErrorEvent != null)
                InitErrorEvent();
        }

        static public void OnLogin()
        {
            if (LoginEvent != null)
                LoginEvent();
        }

        static public void OnLogout()
        {
            if (LogoutEvent != null)
                LogoutEvent();
        }


        static public string GetPlatformID()
        {
            return platform.GetPlatformID();
        }

        static public void Login(Dictionary<string, object> parameters = null)
        {
            platform.Login(parameters);
        }

        static public void Logout()
        {
            platform.Logout();
        }

        static public string GetUserID()
        {
            return platform.GetUserID();
        }

        static public List<SocialUser> GetInAppFriends()
        {
            return platform.GetInAppFriends();
        }

        static public void InviteFriends(string inviteText = "")
        {
            platform.InviteFriends(inviteText);
        }

        static public void Share(string title, string message, string url, string imageUrl, Callback.EventHandler completedCallback, Callback.EventHandler errorCallback)
        {
            platform.Share(title, message, url, imageUrl, completedCallback, errorCallback);
        }

        #endregion
    }
}
