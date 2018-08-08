using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore.Social
{
    /// <summary>
    /// Interface of social platforms
    /// </summary>
    [Serializable]
    public abstract class ISocialPlatform : MonoBehaviour
    {
        public abstract event Action InitCompletedEvent;
        public abstract event Action InitErrorEvent;
        public abstract event Action LoginEvent;
        public abstract event Action LoginErrorEvent;
        public abstract event Action LogoutEvent;

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
        /// Get user social info
        /// </summary>
        public abstract SocialUser GetUserInfo();

        /// <summary>
        /// Get in app friends
        /// Always must return 1 user (player self)
        /// </summary>
        public abstract List<SocialUser> GetInAppFriends();

        /// <summary>
        /// Invite friends by social channel
        /// </summary>
        public abstract void InviteFriends(string inviteText = "", string area = "");

        /// <summary>
        /// Share post to social feed channel
        /// </summary>
        public abstract void Share(string title, string message, string url, string imageUrl, Action completedCallback, Action errorCallback, string shareID, string area = "");
    }

}
