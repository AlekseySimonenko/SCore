using UnityEngine;

using System.Collections.Generic;
using System;

namespace Core
{
    /// <summary>
    /// Platform class for DEV - offline platform
    /// </summary
    public class PlatformDev : Platform
    {

        #region Public variables
        #endregion

        #region Public constants
        public const string ID = "off";
        private string currency = "USD";
        #endregion

        #region Private constants

        #endregion

        #region Private variables
        private string user_id;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary
        public PlatformDev()
        {

        }

        /// <summary>
        /// Return static id of platform
        /// </summary>
        override public string GetPlatformID()
        {
            return ID;
        }

        /// <summary>
        /// Init Dev platform
        /// </summary>
        override public void Init(Callback.EventHandler callbackFunction)
        {
            Debug.Log("PlatformDev init");

            GameObject devConfigObject = GameObject.FindWithTag("PlatformDevConfig");
            if (devConfigObject != null)
            {
                Debug.Log("PlatformDevConfig init");
                PlatformDevConfig devConfig = devConfigObject.GetComponent<PlatformDevConfig>();
                currency = devConfig.currency;
                analytics_systems = devConfig.analytics_systems;
                language = devConfig.language;
            }

            user_id = Convert.ToString(UnityEngine.Random.Range(1, 99999));

            if (callbackFunction != null)
                callbackFunction();
        }

        /// <summary>
        /// Return string currency of platform like "$".
        /// </summary>
        override public string GetCurrency()
        {
            return currency;
        }


        /// <summary>
        /// Return id of development
        /// </summary>
        override public string GetUserID()
        {
            return user_id;
        }

        /// <summary>
        /// Get player info like User
        /// </summary>
        override public User GetUserInfo()
        {
            //By standart base abstract platform not support player info
            return new User(GetUserID(), "Player", "Guest", "1", 0, "", true, true);
        }

        /// <summary>
        /// Return id of development
        /// </summary>
        override public string GetAppID()
        {
            return "dev";
        }

        /// <summary>
        /// Game load profile from locale storage
        /// </summary>
        override public void GameLoad(Callback.EventHandlerObject successCallbackFunction = null, Callback.EventHandlerObject failCallbackFunction = null)
        {
            Dictionary<string, object> saveVO = SaveLocalStorage.Load();

            if (successCallbackFunction != null)
                successCallbackFunction(saveVO);
        }

        /// <summary>
        /// Game save profile to locale storage
        /// </summary>
        override public void GameSave(Dictionary<string, object> _saveVO, Dictionary<string, object> _meta = null, Callback.EventHandler successCallbackFunction = null, Callback.EventHandlerObject failCallbackFunction = null)
        {
            SaveLocalStorage.Save(_saveVO);

            if (successCallbackFunction != null)
                successCallbackFunction();
        }


        /// <summary>
        /// Get global leadboard
        /// </summary>
        override public void RaitingGetList(int _id, Callback.EventHandlerObject successCallbackFunction)
        {
            Debug.Log("PlatformDev.RaitingGetList");

            List<RaitingItemVO> _list = new List<RaitingItemVO>();
            RaitingItemVO _user;

            for (int i = 0; i < 8; i++)
            {
                _user = new RaitingItemVO();
                _user.name = "Player " + i;
                _user.place = i + 1;
                _user.result = 1000000 - i;
                _user.uid = (i + 2).ToString();
                _list.Add(_user);
            }

            _user = new RaitingItemVO();
            _user.name = "Me";
            _user.place = 9;
            _user.result = 100;
            _user.uid = GetUserID();
            _list.Add(_user);

            successCallbackFunction(_list);
        }


        /// <summary>
        /// Get global leadboard
        /// </summary>
        override public void RaitingGetFriendList(List<string> friends, Callback.EventHandlerObject successCallbackFunction)
        {
            Debug.Log("PlatformDev.RaitingGetFriendList");

            List<RaitingItemVO> _list = new List<RaitingItemVO>();
            RaitingItemVO _user;

            for (int i = 0; i < 4; i++)
            {
                _user = new RaitingItemVO();
                _user.name = "Friend " + i;
                _user.place = i + 1;
                _user.result = 1000000 - i;
                _user.uid = (i + 2).ToString();
                _list.Add(_user);
            }

            _user = new RaitingItemVO();
            _user.name = "Me";
            _user.place = 5;
            _user.result = 1;
            _user.uid = GetUserID();
            _list.Add(_user);

            successCallbackFunction(_list);
        }


        /// <summary>
        /// Get global leadboard
        /// </summary>
        override public void RaitingGetUserInfo(string _uid, Callback.EventHandlerObject successCallbackFunction)
        {
            Debug.Log("PlatformDev.RaitingGetUserInfo");
        }


        /// <summary>
        /// Get global leadboard
        /// </summary>
        override public void ArenaGetPlayers(int _level, Callback.EventHandlerObject successCallbackFunction)
        {
            Debug.Log("PlatformDev.ArenaGetPlayers");

            List<RaitingItemVO> _list = new List<RaitingItemVO>();
            RaitingItemVO _user;

            for (int i = 0; i < 50; i++)
            {
                _user = new RaitingItemVO();
                _user.name = "Player " + i;
                _user.place = 0;
                _user.result = 0;
                _user.uid = (i + 2).ToString();
                _list.Add(_user);
            }

            successCallbackFunction(_list);
        }
    }

}