using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore
{
    //// <summary>
    /// Interface of social platforms
    /// </summary>
    [Serializable]
    public abstract class ISocialPlatform : MonoBehaviour
    {
        #region Public variables
        public abstract event Callback.EventHandler InitCompletedEvent;
        public abstract event Callback.EventHandler InitErrorEvent;
        public abstract event Callback.EventHandler LoginEvent;
        public abstract event Callback.EventHandler LoginErrorEvent;
        public abstract event Callback.EventHandler LogoutEvent;
        #endregion

        /// <summary>
        /// Init social platform with custom parameters
        /// </summary>
        public abstract void Init(Dictionary<string,object> parameters = null);

        /// <summary>
        /// Get unique platform ID for platform dependency logic realisation
        /// </summary>
        public abstract string GetPlatformID();

        /// <summary>
        /// Login in social platform with custom parameters
        /// </summary>
        public abstract void Login(Dictionary<string, object> parameters = null);

        /// <summary>
        /// Logout from social account
        /// </summary>
        public abstract void Logout();

        /// <summary>
        /// Get unique user ID
        /// </summary>
        public abstract string GetUserID();

        /// <summary>
        /// Get in app friends
        /// Always must return 1 user (player self)
        /// </summary>
        public abstract List<SocialUser> GetInAppFriends();

        /// <summary>
        /// Invite friends by social channel
        /// </summary>
        public abstract void InviteFriends(string inviteText = "");

        /// <summary>
        /// Share post to social feed channel
        /// </summary>
        public abstract void Share(string title, string message, string url, string imageUrl, Callback.EventHandler completedCallback, Callback.EventHandler errorCallback);
    }

}
