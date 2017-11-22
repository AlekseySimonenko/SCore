using UnityEngine;

using System.Collections.Generic;
using System;

namespace Core
{
    /// <summary>
    /// Platform class for Mobile - offline/online platform parent class
    /// </summary
    public class PlatformMobileAdnroid : Platform
    {

        #region Public variables
        #endregion

        #region Public constants
        [HideInInspector]
        public const string ID = "android";
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
        public PlatformMobileAdnroid()
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
            Debug.Log("PlatformMobileAdnroid init");

            GameObject configObject = GameObject.FindWithTag("PlatformMobileAdnroidConfig");
            if (configObject != null)
            {
                Debug.Log("PlatformMobileAdnroidConfig init");
                PlatformMobileAdnroidConfig config = configObject.GetComponent<PlatformMobileAdnroidConfig>();
                currency = config.currency;
                language = config.language;
                analytics_systems = config.analytics_systems;
#if CORE_GPGS
                GooglePlayGames.PlayGamesPlatform.Activate();
#endif
                Social.localUser.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        user_id = Social.localUser.id;
                    }
                    else
                    {
                        user_id = Convert.ToString(UnityEngine.Random.Range(1, 99999));
                    }
                    Debug.Log("Social.localUser.Authenticate " + success + " UserID:" + user_id);

                    SaveBackendStorage.Init(config.app_id, user_id, config.server_key, config.auth_key, config.server_request_url);

                    if (callbackFunction != null)
                        callbackFunction();
                });

            }
            else
            {
                Debug.LogError("PlatformMobileAdnroid " + "Can't find PlatformMobileAdnroidConfig on scene");
            }

        }


        /// <summary>
        /// Return static id of platform
        /// </summary>
        public virtual bool IsAuthenticated()
        {
            return Social.localUser.authenticated;
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
            return Application.identifier;
        }

        private Callback.EventHandlerObject gameLoadSuccessCallbackFunction;

        /// <summary>
        /// Game load profile from locale storage
        /// </summary>
        override public void GameLoad(Callback.EventHandlerObject successCallbackFunction = null, Callback.EventHandlerObject failCallbackFunction = null)
        {
            gameLoadSuccessCallbackFunction = successCallbackFunction;

            if (IsAuthenticated())
            {
                SaveBackendStorage.Load(gameLoadSuccessCallbackFunction, GameLoadLocal);
            }
            else
            {
                GameLoadLocal();
            }
        }


        protected void GameLoadLocal(object errorCode = null)
        {
            //OFFLINE realisation
            Dictionary<string, object> saveVO = SaveLocalStorage.Load();

            if (gameLoadSuccessCallbackFunction != null)
                gameLoadSuccessCallbackFunction(saveVO);
        }

        private static Callback.EventHandler gameSaveSuccessCallbackFunction;

        /// <summary>
        /// Game save profile to locale storage
        /// </summary>
        override public void GameSave(Dictionary<string, object> _saveVO, Dictionary<string, object> _meta = null, Callback.EventHandler successCallbackFunction = null, Callback.EventHandlerObject failCallbackFunction = null)
        {
            gameSaveSuccessCallbackFunction = successCallbackFunction;

            if (IsAuthenticated())
            {
                SaveBackendStorage.Save(_saveVO, _meta, successCallbackFunction, failCallbackFunction);
            }

            GameSaveLocal(_saveVO);
        }

        protected void GameSaveLocal(Dictionary<string, object> _saveVO)
        {
            //OFFLINE realisation
            SaveLocalStorage.Save(_saveVO);

            if (gameSaveSuccessCallbackFunction != null)
                gameSaveSuccessCallbackFunction();
        }


        /// <summary>
        /// Write in leaderboard
        /// </summary>
        override public void LeaderboardResult(string _id, int _result)
        {
            if (IsAuthenticated())
            {
#if CORE_GPGS
                PlayGamesPlatform.Instance.ReportScore(_result, _id, null);
#endif
            }
        }


        /// <summary>
        /// Get global leadboard
        /// </summary>
        override public void RaitingGetUserInfo(string _uid, Callback.EventHandlerObject successCallbackFunction)
        {
            Debug.Log("PlatformMobileAdnroid.RaitingGetUserInfo");
        }


        /// <summary>
        /// Get global leadboard
        /// </summary>
        override public void ArenaGetPlayers(int _level, Callback.EventHandlerObject successCallbackFunction)
        {
            Debug.Log("PlatformMobileAdnroid.ArenaGetPlayers");

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