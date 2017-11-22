using UnityEngine;

using System.Collections.Generic;
using System;
using Facebook.Unity;

namespace Core
{
    /// <summary>
    /// Platform class for Facebook social network (canvas)
    /// </summary
    public class PlatformFBcanvas : Platform
    {

        #region Public variables
        #endregion

        #region Public constants
        public const string ID = "fb";
        public string currency = "USD";
        #endregion

        #region Private constants

        #endregion

        #region Private variables
        private string app_id;
        private string user_id;
        private string user_referral;
        private string user_browser;
        private string server_domain;
        private string server_key;
        private string server_protocol;
        private string auth_key;

        private string analytics_game = "";
        private string analytics_key = "";

        private User userInfo;
        private List<User> allFriends = new List<User>();
        private List<User> inAppFriends = new List<User>();
        private List<User> notInAppFriends = new List<User>();

        private Dictionary<string, object> itemsPrices;

        private static Callback.EventHandler initCallbackFunction;
        private static Callback.EventHandler paymentSuccessCallbackFunction;
        private static Callback.EventHandler paymentFailCallbackFunction;
        private static Callback.EventHandler inviteSuccessCallbackFunction;
        private static Callback.EventHandler inviteFailCallbackFunction;
        private static Callback.EventHandler requestSuccessCallbackFunction;
        private static Callback.EventHandler requestFailCallbackFunction;
        private static Callback.EventHandler shareSuccessCallbackFunction;
        private static Callback.EventHandler shareFailCallbackFunction;

        private string server_request_url;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary
        public PlatformFBcanvas()
        {
            isWebPlatform = true;
        }

        /// <summary>
        /// Return static id of platform
        /// </summary>
        override public string GetPlatformID()
        {
            return ID;
        }

        /// <summary>
        /// Init Facebook canvas
        /// </summary>
        override public void Init(Callback.EventHandler callbackFunction)
        {
            Debug.Log("PlatformFBcanvas init");

            GameObject configObject = GameObject.FindWithTag("PlatformFBcanvasConfig");
            if (configObject != null)
            {
                Debug.Log("PlatformFBcanvasConfig init");
                PlatformFBcanvasConfig config = configObject.GetComponent<PlatformFBcanvasConfig>();
                currency = config.currency;
                language = config.language;
                analytics_systems = config.analytics_systems;

                SaveBackendStorage.Init(config.app_id, user_id, config.server_key, config.auth_key, config.server_request_url);

                if (callbackFunction != null)
                    callbackFunction();

            }
            else
            {
                Debug.LogError("PlatformFBcanvas " + "Can't find PlatformFBcanvasConfig on scene");
            }
        }


        /// <summary>
        /// Return id of user
        /// </summary>
        override public string GetUserID()
        {
            return user_id;
        }

        /// <summary>
        /// Return id of application
        /// </summary>
        override public string GetAppID()
        {
            return app_id;
        }


        /// <summary>
        /// Get id of analytics system
        /// </summary>
        override public IAnalyticSystem[] GetAnalyticsSystems()
        {
            return analytics_systems;
        }


        /// <summary>
        /// Get payment item's price
        /// </summary>
        override public float GetPriceOfItem(string _id)
        {
            //To prices with point like 115 cents = 1.15$
            float price = itemsPrices.ContainsKey(_id) ? price = Convert.ToInt32(itemsPrices[_id]) / 100 : 0.0F;
            return price;
        }

        /// <summary>
        /// Return string currency of platform like "$".
        /// </summary>
        override public string GetCurrency()
        {
            return currency;
        }

        /// <summary>
        /// Get player info like User
        /// </summary>
        override public User GetUserInfo()
        {
            return userInfo;
        }

        /// <summary>
        /// Get player info like User
        /// </summary>
        override public string GetUserReferral()
        {
            return user_referral;
        }

        /// <summary>
        /// ExternalCall to iframe
        /// </summary>
        override public void Payment(string _itemID, Callback.EventHandler successCallbackFunction, Callback.EventHandler failCallbackFunction = null)
        {
            //Application.ExternalCall("Payment", _itemID);
            paymentSuccessCallbackFunction = successCallbackFunction;
            paymentFailCallbackFunction = failCallbackFunction;
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        override public void OnGetPaymentSuccessfull()
        {
            if (paymentSuccessCallbackFunction != null)
                paymentSuccessCallbackFunction();
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        override public void OnGetPaymentFail()
        {
            if (paymentFailCallbackFunction != null)
                paymentFailCallbackFunction();
        }



    }



}