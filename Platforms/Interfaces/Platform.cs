using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCore
{
    /// <summary>
    /// Base abstract class for platforms. By standart most of the features are disabled.
    /// </summary>
    [Serializable]
    public abstract class Platform : MonoBehaviour
    {

        #region Public variables
        public virtual event Callback.EventHandlerObject CurrencyAddedEvent;
        public virtual event Callback.EventHandlerObject DailyBonusEvent;
        public virtual event Callback.EventHandlerObject TournamentConfigEvent;
        public virtual event Callback.EventHandlerObject TournamentWinEvent;
        #endregion

        #region Public constants

        #endregion

        #region Private constants
        [HideInInspector]
        public string language;

        protected IAnalyticSystem[] analytics_systems;
        #endregion

        #region Private variables
        protected bool isWebPlatform = false;
        protected bool isMobilePlatform = false;
        #endregion

        /// <summary>
        /// Return static id of platform.
        /// </summary>
        public abstract string GetPlatformID();

        /// <summary>
        /// Return string currency of platform like "$".
        /// </summary>
        public abstract string GetCurrency();

        /// <summary>
        /// Return static id of user.
        /// </summary>
        public abstract string GetUserID();

        /// <summary>
        /// Return static id of application.
        /// </summary>
        public abstract string GetAppID();

        /// <summary>
        /// Init platfrom and call back.
        /// </summary>
        public virtual void Init(Callback.EventHandler callbackFunction)
        {
            //By standart base abstract platform no need init.
        }

        /// <summary>
        /// Configurate platform with json config.
        /// </summary>
        public virtual void Config(string _data)
        {
            //By standart base abstract platform no need configuration.
        }

        /// <summary>
        /// Get details info about platform
        /// </summary>
        public virtual Dictionary<string, string> GetPlatfornDetails()
        {
            return new Dictionary<string, string>();
        }


        /// <summary>
        /// Get id of analytics system
        /// </summary>
        public virtual IAnalyticSystem[] GetAnalyticsSystems()
        {
            return analytics_systems;
        }

        /// <summary>
        /// Get payment item's price
        /// </summary>
        public virtual float GetPriceOfItem(string _id)
        {
            return 0.0F;
        }

        /// <summary>
        /// Get player info like User
        /// </summary>
        public virtual User GetUserInfo()
        {
            //By standart base abstract platform not support player info
            return new User("0", "Player", "Guest", "1", 0, "", true, true);
        }

        /// <summary>
        /// Get refferal user inGame
        /// </summary>
        public virtual string GetUserReferral()
        {
            //By standart base abstract platform not support player info
            return "";
        }

        /// <summary>
        /// Get friend info
        /// </summary>
        public virtual User GetFriendInfo(string _uid)
        {
            //By standart base abstract platform not support friends.
            return null;
        }

        /// <summary>
        /// Get all friends
        /// </summary>
        public virtual List<User> GetAllFriends()
        {
            //By standart base abstract platform not support friends.
            return new List<User>();
        }

        /// <summary>
        /// Get inapp friends
        /// </summary>
        public virtual List<User> GetInAppFriends()
        {
            //By standart base abstract platform not support friends.
            return new List<User>();
        }

        /// <summary>
        /// Get not inapp friends
        /// </summary>
        public virtual List<User> GetNotInAppFriends()
        {
            //By standart base abstract platform not support friends.
            return new List<User>();
        }

        /// <summary>
        /// Run payment process with item ID.
        /// </summary>
        public virtual void Payment(string _itemID, Callback.EventHandler successCallbackFunction, Callback.EventHandler failCallbackFunction = null)
        {
            //By standart base abstract platform not support payments and always call sucess
            successCallbackFunction();
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public virtual void OnGetPaymentSuccessfull()
        {
            //By standart base abstract platform not support payments.
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public virtual void OnGetPaymentFail()
        {
            //By standart base abstract platform not support payments.
        }

        /// <summary>
        /// Crossplatform offer call
        /// </summary>
        public virtual void Offer()
        {
            //By standart base abstract platform not support offers.
        }

        //// <summary>
        /// We just get all friends from iframe
        /// </summary>
        public virtual void OnGetFriendsInfo(string _data)
        {
            //By standart base abstract platform not support friends.
        }

        /// <summary>
        /// Friends invite
        /// </summary>
        public virtual void FriendsInvite(List<string> _uids, string _text, Callback.EventHandler successCallbackFunction, Callback.EventHandler failCallbackFunction = null)
        {
            //By standart base abstract platform not support invites.
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public virtual void OnGetInviteSuccessfull()
        {
            //By standart base abstract platform not support invites.
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public virtual void OnGetInviteFail()
        {
            //By standart base abstract platform not support invites.
        }

        /// <summary>
        /// Friends request
        /// </summary>
        public virtual void FriendsRequest(List<string> _uids, string _text, Callback.EventHandler successCallbackFunction, Callback.EventHandler failCallbackFunction = null)
        {
            //By standart base abstract platform not support requests.
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public virtual void OnGetRequestSuccessfull()
        {
            //By standart base abstract platform not support requests.
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public virtual void OnGetRequestFail()
        {
            //By standart base abstract platform not support requests.
        }

        /// <summary>
        /// Sharing info
        /// </summary>
        public virtual void Share(string _postid, Callback.EventHandler successCallbackFunction = null, Callback.EventHandler failCallbackFunction = null)
        {
            //By standart base abstract platform not support sharing.
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public virtual void OnShareSuccessfull()
        {
            //By standart base abstract platform not support sharing.
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public virtual void OnShareFail()
        {
            //By standart base abstract platform not support sharing.
        }
            
        /// <summary>
        /// Request profile object from platform storage.
        /// </summary>
        public virtual void GameLoad(Callback.EventHandlerObject successCallbackFunction = null, Callback.EventHandlerObject failCallbackFunction = null)
        {
            //Nothing todo in abstract class
            if (successCallbackFunction != null)
                successCallbackFunction(null);
        }


        /// <summary>
        /// Push profile object from platform storage
        /// </summary>
        public virtual void GameSave(Dictionary<string, object> _saveVO, Dictionary<string, object> _meta = null, Callback.EventHandler successCallbackFunction = null, Callback.EventHandlerObject failCallbackFunction = null)
        {
            //Nothing todo in abstract class
            if (successCallbackFunction != null)
                successCallbackFunction();
        }

        /// <summary>
        /// Share activity with new level
        /// </summary>
        public virtual void ActivityLevel(int _level)
        {
            //By standart base abstract platform not support sharing.
        }

        /// <summary>
        /// Share activity with new level
        /// </summary>
        public virtual void ActivityAchievement(int _id, int _level)
        {
            //By standart base abstract platform not support sharing.
        }


        /// <summary>
        /// Write in leaderboard
        /// </summary>
        public virtual void LeaderboardResult(string _id, int _result)
        {
            //By standart base abstract platform not support sharing.
        }

        /// <summary>
        /// Send in leaderboard new result of raitings
        /// </summary>
        public virtual void RaitingNewResult(int _global_result, int _tournament_result, int _tournament_id, string _name)
        {
            //By standart base abstract platform not support leaderboard
        }

        /// <summary>
        /// Send in leaderboard new result of raitings
        /// </summary>
        public virtual void RaitingGetList(int _id, Callback.EventHandlerObject successCallbackFunction)
        {
            //By standart base abstract platform not support leaderboard
        }

        /// <summary>
        /// Send in leaderboard new result of raitings
        /// </summary>
        public virtual void RaitingGetFriendList(List<string> friends, Callback.EventHandlerObject successCallbackFunction)
        {
            //By standart base abstract platform not support leaderboard
        }

        /// <summary>
        /// GetUserInfo (Save) from server
        /// </summary>
        public virtual void RaitingGetUserInfo(string _uid, Callback.EventHandlerObject successCallbackFunction)
        {
            //By standart base abstract platform not support leaderboard
        }


        /// <summary>
        /// Get list of players of Arena
        /// </summary>
        public virtual void ArenaGetPlayers(int _level, Callback.EventHandlerObject successCallbackFunction)
        {
            //By standart base abstract platform not support leaderboard
        }

        /// <summary>
        /// Return true if platform is web.
        /// </summary>
        public bool IsWeb()
        {
            return isWebPlatform;
        }

        /// <summary>
        /// Return true if platform is mobile.
        /// </summary>
        public bool IsMobile()
        {
            return isMobilePlatform;
        } 
    }
}