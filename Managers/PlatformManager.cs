using UnityEngine;
using System.Collections.Generic;
using System;

namespace Core
{
    //// <summary>
    /// Static class controlling platform choosing. 
    /// PlatformManager it's a bridge between private Platform class and public requests from App.
    /// This a entry point for work with multiplatform methods.
    /// </summary>
    public class PlatformManager : MonoBehaviour
    {
        #region Public var
        public static event Callback.EventHandler PaymentEvent;
        public static event Callback.EventHandlerObject CurrencyAddedEvent;
        public static event Callback.EventHandlerObject DailyBonusEvent;
        public static event Callback.EventHandlerObject TournamentConfigEvent;
        public static event Callback.EventHandlerObject TournamentWinEvent;
        #endregion
        
        #region Public const
        #endregion

        #region Private const
        #endregion

        #region Private var
        private static Platform platform = null;
        // Only one init calling protect variables
        private static bool isInitComplete = false;
        private static Callback.EventHandler initCallbackFunction;
        #endregion



        #region Init
        /// <summary>
        /// Only one init can will be called
        /// </summary>
        public static void Init(Callback.EventHandler _callbackFunction)
        {
            Debug.Log("PlatformManager:init");
            initCallbackFunction = _callbackFunction;

            if (!isInitComplete)
            {
                
                switch (Application.platform)
                {
                    case RuntimePlatform.WebGLPlayer:
#if FB_CANVAS
                        InitFBCanvasPlatform();
#else
                        // Wait response from iframe to OnGetWebPlatform function of iframeListener object in loader scene
                        Application.ExternalCall("GetWebPlatform");
#endif
                        break;
                    case RuntimePlatform.Android:
                        InitAndroidPlatform();
                        break;
                    default:
                        InitDevPlatform();
                        break;
                }

            }
            else
            {
                if(initCallbackFunction != null)
                initCallbackFunction();
            }
        }


        /// <summary>
        /// Android Platform init
        /// </summary>
        public static void InitAnyPlatform(Platform _platform, Callback.EventHandler _callbackFunction)
        {
            Debug.Log("PlatformManager.InitAnyPlatform");
            initCallbackFunction = _callbackFunction;

            if (_platform != null)
            {
                platform = _platform;
                platform.Init(InitComplete);
            }
        }


        /// <summary>
        /// Only one init can will be called
        /// </summary>
        private static void InitComplete()
        {
            if (!isInitComplete)
            {
                Debug.Log("PlatformManager.InitComplete");
                isInitComplete = true;

                platform.CurrencyAddedEvent += OnCurrencyAdded;
                platform.DailyBonusEvent += OnDailyBonus;
                platform.TournamentConfigEvent += OnTournamentConfig;
                platform.TournamentWinEvent += OnTournamentWin;

                initCallbackFunction();
            }
            else
            {
                Debug.LogError("PlatformManager:Repeating static class InitComplete!");
            }
        }

        /// <summary>
        /// Only one WEB init can will be called
        /// </summary>
        public static void OnGetWebPlatform(string _data)
        {
            PlatformConfigJSON data = JsonUtility.FromJson<PlatformConfigJSON>(_data);
            Debug.Log("PlatformManager.OnGetWebPlatform: " + data.platform + " " + data.language);
            InitWebPlatform(data.platform, data.language);
        }

        /// <summary>
        /// Web Platforms init
        /// </summary>
        private static void InitWebPlatform(string _platform, string _language)
        {
            Debug.Log("PlatformManager.InitWebPlatform");

            if (platform != null)
            {
                Debug.LogError("PlatformManager:Repeating InitWebPlatform!");
            }
            else
            {
                switch (_platform)
                {
                    case PlatformDev.ID:
                        InitDevPlatform();
                        break;
                    default:
                        Debug.LogError("Error: not found this Web platform id - " + _platform);
                        InitDevPlatform();
                        break;
                }

            }

        }


        /// <summary>
        /// Android Platform init
        /// </summary>
        private static void InitFBCanvasPlatform()
        {
            Debug.Log("PlatformManager.InitFBCanvasPlatform");

            if (platform != null)
            {
                Debug.LogError("PlatformManager:Repeating InitFBCanvasPlatform!");
            }
            else
            {
                platform = new PlatformFBcanvas();
                platform.Init(InitComplete);
            }

        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public static void OnGetPlatformConfig(string _data)
        {
            platform.Config(_data);
        }

        /// <summary>
        /// Development Platform init
        /// </summary>
        private static void InitDevPlatform()
        {
            Debug.Log("PlatformManager.InitDevPlatform");

            if (platform != null)
            {
                Debug.LogError("PlatformManager:Repeating InitDevPlatform!");
            }
            else
            {
                platform = new PlatformDev();
                platform.Init(InitComplete);
            }

        }


        /// <summary>
        /// Android Platform init
        /// </summary>
        private static void InitAndroidPlatform()
        {
            Debug.Log("PlatformManager.InitAndroidPlatform");

            if (platform != null)
            {
                Debug.LogError("PlatformManager:Repeating InitAndroidPlatform!");
            }
            else
            {
                platform = new PlatformMobileAdnroid();
                platform.Init(InitComplete);
            }

        }



     


        #endregion



        /// <summary>
        /// Get user language that configurated by platform.
        /// </summary>
        public static string GetLanguage()
        {
            return platform.language;
        }

        /// <summary>
        /// Crossplatform internet checker
        /// </summary>
        public static bool IsOnline()
        {
            //TODO check for stable internet connection
            return true;
        }

        /// <summary>
        /// Web platforms condition
        /// </summary>
        public static bool IsWeb()
        {
            return platform.IsWeb();
        }

        /// <summary>
        /// Mobile platforms condition
        /// </summary>
        public static bool IsMobile()
        {
            return platform.IsMobile();
        }

        /// <summary>
        /// Get platform currency
        /// </summary>
        public static string GetPlatformId()
        {
            return platform.GetPlatformID();
        }


        /// <summary>
        /// Get user or device unique identificator
        /// </summary>
        public static string GetUserID()
        {
            return platform.GetUserID();
        }

        /// <summary>
        /// Get application unique identificator
        /// </summary>
        public static string GetAppID()
        {
            return platform.GetAppID();
        }

        /// <summary>
        /// Get user or device unique identificator
        /// </summary>
        public static string GetUserReferral()
        {
            return platform.GetUserReferral();
        }

        /// <summary>
        /// Get details info about platform
        /// </summary>
        public static Dictionary<string, string> GetPlatfornDetails()
        {
            return platform.GetPlatfornDetails();
        }

        /// <summary>
        /// Get id of analytics system
        /// </summary>
        public static IAnalyticSystem[] GetAnalyticsSystems()
        {
            return platform.GetAnalyticsSystems();
        }


        /// <summary>
        /// Get platform currency
        /// </summary>
        public static string GetCurrency()
        {
            return platform.GetCurrency();
        }

        /// <summary>
        /// Get payment item's price
        /// </summary>
        public static float GetPriceOfItem(string _id)
        {
            return platform.GetPriceOfItem(_id);
        }

        /// <summary>
        /// Get player info like User
        /// </summary>
        public static User GetUserInfo()
        {
            return platform.GetUserInfo();
        }

        /// <summary>
        /// Get friend info
        /// </summary>
        public static User GetFriendInfo(string _uid)
        {
            return platform.GetFriendInfo(_uid);
        }

        /// <summary>
        /// Get all friends
        /// </summary>
        public static List<User> GetAllFriends()
        {
            return platform.GetAllFriends();
        }

        /// <summary>
        /// Get inapp friends
        /// </summary>
        public static List<User> GetInAppFriends()
        {
            return platform.GetInAppFriends();
        }

        /// <summary>
        /// Get not inapp friends
        /// </summary>
        public static List<User> GetNotInAppFriends()
        {
            return platform.GetNotInAppFriends();
        }



        /// <summary>
        /// Crossplatform payment call
        /// </summary>
        public static void Payment(string _itemID, string _itemType, string _area,  Callback.EventHandler successCallbackFunction, Callback.EventHandler failCallbackFunction = null)
        {
            //Manual properties
            lastPaymentItemID = _itemID;
            lastPaymentItemType = _itemType;
            lastPaymentArea = _area;

            //Auto properties
            lastPaymentAmount = Mathf.FloorToInt(GetPriceOfItem(_itemID) * 100);
            lastPaymentCurrency = platform.GetCurrency();

            AnalyticsManager.PaymentInfoTry(lastPaymentCurrency, lastPaymentAmount, lastPaymentItemID, lastPaymentItemType, lastPaymentArea);
            platform.Payment(_itemID, successCallbackFunction, failCallbackFunction);
            Screen.fullScreen = false;
        }

        private static string lastPaymentItemID;
        private static string lastPaymentItemType;
        private static string lastPaymentCurrency;
        private static int lastPaymentAmount;
        private static string lastPaymentArea;

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public static void OnGetPaymentSuccessfull()
        {
            AnalyticsManager.PaymentInfoSuccess(lastPaymentCurrency, lastPaymentAmount, lastPaymentItemID, lastPaymentItemType, lastPaymentArea);
            AnalyticsManager.PaymentReal(lastPaymentCurrency, lastPaymentAmount, lastPaymentItemID, lastPaymentItemType, lastPaymentArea);
            platform.OnGetPaymentSuccessfull();

            if (PaymentEvent != null)
                PaymentEvent();
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public static void OnGetPaymentFail()
        {
            platform.OnGetPaymentFail();
        }

        /// <summary>
        /// Crossplatform offer call
        /// </summary>
        public static void Offer()
        {
            //TODO analytics for offer events by platform
            //AnalyticsManager.PaymentInfoTry(lastPaymentCurrency, 0, "offer", "");
            platform.Offer();
        }


        //// <summary>
        /// We just get all friends from iframe
        /// </summary>
        public static void OnGetFriendsInfo(string _data)
        {
            platform.OnGetFriendsInfo(_data);
        }


        /// <summary>
        /// Crossplatform friends invite
        /// </summary>
        public static void FriendsInvite(List<string> _uids, string _text, string _area, Callback.EventHandler successCallbackFunction, Callback.EventHandler failCallbackFunction = null)
        {
            AnalyticsManager.InviteTry(_area);
            platform.FriendsInvite(_uids, _text, successCallbackFunction, failCallbackFunction);
            Screen.fullScreen = false;
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public static void OnGetInviteSuccessfull()
        {
            platform.OnGetInviteSuccessfull();
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public static void OnGetInviteFail()
        {
            platform.OnGetInviteFail();
        }

        /// <summary>
        /// Crossplatform friends request
        /// </summary>
        public static void FriendsRequest(List<string> _uids, string _text, string _type, string _area, Callback.EventHandler successCallbackFunction, Callback.EventHandler failCallbackFunction = null)
        {
            AnalyticsManager.RequestTry(_type, _area);
            lastRequestArea = _area;
            lastRequestType = _type;
            platform.FriendsRequest(_uids, _text, successCallbackFunction, failCallbackFunction);
            Screen.fullScreen = false;
        }

        private static string lastRequestArea;
        private static string lastRequestType;

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public static void OnGetRequestSuccessfull()
        {
            AnalyticsManager.RequestSuccess(lastRequestType, lastRequestArea);
            platform.OnGetRequestSuccessfull();
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public static void OnGetRequestFail()
        {
            platform.OnGetRequestFail();
        }


        /// <summary>
        /// Crossplatform sharing info
        /// </summary>
        public static void Share(string _postid, string _level, Callback.EventHandler successCallbackFunction = null, Callback.EventHandler failCallbackFunction = null)
        {
            AnalyticsManager.ShareTry(_level);
            lastShareLevel = _level;
            platform.Share(_postid, successCallbackFunction, failCallbackFunction);
            Screen.fullScreen = false;
        }

        private static string lastShareLevel;

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public static void OnShareSuccessfull()
        {
            AnalyticsManager.ShareSuccess(lastShareLevel);
            platform.OnShareSuccessfull();
        }

        /// <summary>
        /// iFrameListener calling this function
        /// </summary>
        public static void OnShareFail()
        {
            platform.OnShareFail();
        }


        /// <summary>
        /// Try to find any save data in platform storage.
        /// When callback return you can take saveVO from lastLoadedSaveVO var.
        /// </summary>
        public static void GameLoad(Callback.EventHandlerObject successCallbackFunction, Callback.EventHandlerObject failCallbackFunction = null)
        {
            //Remember gameLoadedCallback
            gameLoadSuccessCallbackFunction = successCallbackFunction;
            gameLoadFailCallbackFunction = failCallbackFunction;
            platform.GameLoad(GameLoadSuccess,GameLoadFail);
        }

        private static Callback.EventHandlerObject gameLoadSuccessCallbackFunction;
        private static Callback.EventHandlerObject gameLoadFailCallbackFunction;

        private static void GameLoadSuccess(object _saveVO)
        {
            //Call gameLoadedCallback
            if (gameLoadSuccessCallbackFunction != null)
                gameLoadSuccessCallbackFunction(_saveVO);
        }

        private static void GameLoadFail(object _saveVO)
        {
            //Call gameLoadedCallback
            if (gameLoadFailCallbackFunction != null)
                gameLoadFailCallbackFunction(_saveVO);
        }

        /// <summary>
        /// Try to save seriazible saveVO data in platform storage. 
        /// And callback on complete event.
        /// </summary>
        public static void GameSave(Dictionary<string, object> _saveVO, Dictionary<string, object> _meta,  Callback.EventHandler successCallbackFunction, Callback.EventHandlerObject failCallbackFunction = null)
        {
            //Remember gameSavedCallback
            gameSaveSuccessCallbackFunction = successCallbackFunction;
            gameSaveFailCallbackFunction = failCallbackFunction;
            platform.GameSave(_saveVO, _meta, GameSaveSuccess, GameSaveFail);
        }

        private static Callback.EventHandler gameSaveSuccessCallbackFunction;
        private static Callback.EventHandlerObject gameSaveFailCallbackFunction;

        private static void GameSaveSuccess()
        {
            //Call gameSavedCallback
            if (gameSaveSuccessCallbackFunction != null)
                gameSaveSuccessCallbackFunction();
        }

        private static void GameSaveFail(object _error)
        {
            //Call gameSavedCallback
            if (gameSaveFailCallbackFunction != null)
                gameSaveFailCallbackFunction(_error);
        }

        //// <summary>
        /// Platform give player currency bonus
        /// </summary>
        public static void OnCurrencyAdded(object _count)
        {
            if(CurrencyAddedEvent != null)
                CurrencyAddedEvent(_count);
        }

        //// <summary>
        /// Platform give player daily bonus
        /// </summary>
        public static void OnDailyBonus(object _count)
        {
            if (DailyBonusEvent != null)
                DailyBonusEvent(_count);
        }

        //// <summary>
        /// Platform give player daily bonus
        /// </summary>
        public static void OnTournamentConfig(object _count)
        {
            if (TournamentConfigEvent != null)
                TournamentConfigEvent(_count);
        }

        //// <summary>
        /// Platform give player daily bonus
        /// </summary>
        public static void OnTournamentWin(object _count)
        {
            if (TournamentWinEvent != null)
                TournamentWinEvent(_count);
        }

        /// <summary>
        /// Share activity with new level
        /// </summary>
        public static void ActivityLevel(object _level)
        {
            platform.ActivityLevel((int)_level);
        }


        /// <summary>
        /// Share activity with new level
        /// </summary>
        public static void ActivityAchievement(int _id, int _level)
        {
            platform.ActivityAchievement(_id, _level);
        }

        /// <summary>
        /// Share activity with new level
        /// </summary>
        public static void LeaderboardResult(string _id, int _result)
        {
            platform.LeaderboardResult(_id, _result);
        }

        /// <summary>
        /// Send in leaderboard new result of raitings
        /// </summary>
        public static void RaitingNewResult(int _global_result, int _tournament_result, int _tournament_id)
        {
            platform.RaitingNewResult(_global_result, _tournament_result, _tournament_id, GetUserInfo().first_name + " " + GetUserInfo().last_name);
        }

        /// <summary>
        /// Get leaderboard of server
        /// </summary>
        public static void RaitingGetList(int _id, Callback.EventHandlerObject successCallbackFunction)
        {
            platform.RaitingGetList(_id, successCallbackFunction);
        }

        /// <summary>
        /// Get leaderboard of friends
        /// </summary>
        public static void RaitingGetFriendList(Callback.EventHandlerObject successCallbackFunction)
        {
            List<string> friends = new List<string>();
            List<User> friendsInfo = GetInAppFriends();

            for (int i = 0; i < friendsInfo.Count; i++)
            {
                friends.Add(friendsInfo[i].uid);
            }

            //Player in list of friends too
            friends.Add(GetUserID());

            platform.RaitingGetFriendList(friends, successCallbackFunction);
        }


        /// <summary>
        /// Send in leaderboard new result of raitings
        /// </summary>
        public static void RaitingGetUserInfo(string _uid, Callback.EventHandlerObject successCallbackFunction)
        {
            platform.RaitingGetUserInfo(_uid, successCallbackFunction);
        }


        /// <summary>
        /// Get leaderboard of server
        /// </summary>
        public static void ArenaGetPlayers(int _level, Callback.EventHandlerObject successCallbackFunction)
        {
            platform.ArenaGetPlayers(_level, successCallbackFunction);
        }

    }

    [Serializable]
    public class PlatformConfigJSON
    {
        public string platform;
        public string language;
    }

}