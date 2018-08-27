﻿using SCore.Loading;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SCore.Social
{
    /// <summary>
    /// Static class choise and control social platforms
    /// Only one social platform can be inited at one time!
    /// </summary>
    [RequireComponent(typeof(IServiceLoadingStep))]
    public class SocialManager : MonoBehaviourSingleton<SocialManager>
    {
        public ISocialPlatform platformAutoInit;
        public UnityEvent OnInitActions;

        static public event Action InitCompletedEvent;
        static public event Action InitErrorEvent;
        static public event Action LoginEvent;
        static public event Action LoginErrorEvent;
        static public event Action LogoutEvent;

        static public bool LoginCompleted { get; protected set; }
        static public bool LoginProcessed { get; protected set; }

        static private ISocialPlatform platform;


        #region Init
        private void Start()
        {
            Debug.Log("SocialManager.Start");
            if (platformAutoInit != null)
                Init(platformAutoInit, OnAutoInitCompleted, OnAutoInitCompleted);
        }

        public void OnAutoInitCompleted()
        {
            Debug.Log("SocialManager.OnAutoInitCompleted");
            OnInitActions?.Invoke();
        }

        static public void Init(ISocialPlatform _platform, Action callbackCompleted, Action callbackError)
        {
            Debug.Log("SocialManager Init platform " + _platform.GetPlatformID());
            platform = _platform;
            platform.InitCompletedEvent += callbackCompleted;
            platform.InitErrorEvent += callbackError;

            platform.InitCompletedEvent += OnInitComleted;
            platform.InitErrorEvent += OnInitErrorEvent;
            platform.LoginEvent += OnLogin;
            platform.LoginErrorEvent += OnLoginError;
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
            Debug.Log("SocialManager.OnInitComleted");
            //Remove listeners
            platform.InitCompletedEvent -= OnInitComleted;
            platform.InitErrorEvent -= OnInitErrorEvent;
            //Call completed event
            InitCompletedEvent?.Invoke();
        }

        static public void OnInitErrorEvent()
        {
            Debug.Log("SocialManager.OnInitErrorEvent");
            //Remove listeners
            platform.InitCompletedEvent -= OnInitComleted;
            platform.InitErrorEvent -= OnInitErrorEvent;
            //Call error event
            InitErrorEvent?.Invoke();
        }

        static public void OnLogin()
        {
            Debug.Log("SocialManager.OnLogin");
            LoginCompleted = true;
            LoginProcessed = false;
            LoginEvent?.Invoke();
        }

        static public void OnLoginError()
        {
            Debug.Log("SocialManager.OnLoginError");
            LoginProcessed = false;
            LoginErrorEvent?.Invoke();
        }

        static public void OnLogout()
        {
            Debug.Log("SocialManager.OnLogout");
            LoginCompleted = false;
            LogoutEvent?.Invoke();
        }


        static public string GetPlatformID()
        {
            return platform.GetPlatformID();
        }

        static public void Login(Dictionary<string, object> parameters = null)
        {
            Debug.Log("SocialManager.Login");
            if (!LoginCompleted)
            {
                LoginProcessed = true;
                platform.Login(parameters);
            }
            else
            {
                Debug.Log("SocialManager: Already logined!");
            }
        }

        static public void Logout()
        {
            Debug.Log("SocialManager.Logout");
            platform.Logout();
        }

        static public string GetUserID()
        {
            return platform.GetUserID();
        }

        static public SocialUser GetUserInfo()
        {
            return platform.GetUserInfo();
        }


        static public List<SocialUser> GetInAppFriends()
        {
            Debug.Log("SocialManager.GetInAppFriends");
            return platform.GetInAppFriends();
        }

        static public void InviteFriends(string inviteText = "", string area = "")
        {
            Debug.Log("SocialManager.InviteFriends");
            platform.InviteFriends(inviteText, area);
        }

        static public void Share(string title, string message, string url, string imageUrl, Action completedCallback, Action errorCallback, string shareID, string area = "")
        {
            Debug.Log("SocialManager.Share");
            platform.Share(title, message, url, imageUrl, completedCallback, errorCallback, shareID, area);
        }

        #endregion
    }
}
