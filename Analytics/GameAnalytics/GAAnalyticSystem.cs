using UnityEngine;
#if CORE_GA
using GameAnalyticsSDK;
using GameAnalyticsSDK.Wrapper;
#endif

namespace Core
{
    /// <summary>
    /// Analytic class for Game Analytics (GA) https://go.gameanalytics.com
    /// </summary
    public class GAAnalyticSystem : IAnalyticSystem
    {

        #region Public variables

        public bool useCustomInit = true;

        [Header("InEditor")]
        public string EditorGame;
        public string EditorKey;

        [Header("Android")]
        public string AndroidDevGame;
        public string AndroidDevKey;
        public string AndroidProdGame;
        public string AndroidProdKey;

        [Header("IOS")]
        public string IosDevGame;
        public string IosDevKey;
        public string IosProdGame;
        public string IosProdKey;

        [Header("Steam")]
        public string SteamDevGame;
        public string SteamDevKey;
        public string SteamProdGame;
        public string SteamProdKey;

        [Header("Gear")]
        public string GearDevGame;
        public string GearDevKey;
        public string GearProdGame;
        public string GearProdKey;
        #endregion

        #region Public constants
        #endregion

        #region Private constants

        #endregion

        #region Private variables
        private static Callback.EventHandler initCallbackFunction;
        private string targetGameKey;
        private string targetSecretKey;
        #endregion

#if CORE_GA


        /// <summary>
        /// Constructor
        /// </summary
        override public void Init(Callback.EventHandler _callbackFunction)
        {
            Debug.Log("GameAnalytics init");
            initCallbackFunction = _callbackFunction;

            if (useCustomInit)
            {

                //Init GA
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        targetGameKey = EditorGame;
                        targetSecretKey = EditorKey;
                        break;

                    case RuntimePlatform.WindowsPlayer:
                    case RuntimePlatform.OSXPlayer:
                    case RuntimePlatform.LinuxPlayer:
                        if (Debug.isDebugBuild)
                        {
                            targetGameKey = SteamDevGame;
                            targetSecretKey = SteamDevKey;
                        }
                        else
                        {
                            targetGameKey = SteamProdGame;
                            targetSecretKey = SteamProdKey;
                        }
                        break;

                    case RuntimePlatform.IPhonePlayer:
                        if (Debug.isDebugBuild)
                        {
                            targetGameKey = IosDevGame;
                            targetSecretKey = IosDevKey;
                        }
                        else
                        {
                            targetGameKey = IosProdGame;
                            targetSecretKey = IosProdKey;
                        }
                        break;
                    case RuntimePlatform.Android:

#if COREVR_GEAR
                    if (Debug.isDebugBuild)
                    {
                        targetGameKey = GearDevGame;
                        targetSecretKey = GearDevKey;
                    }
                    else
                    {
                        targetGameKey = GearProdGame;
                        targetSecretKey = GearProdKey;
                    }
#else
                        if (Debug.isDebugBuild)
                        {
                            targetGameKey = AndroidDevGame;
                            targetSecretKey = AndroidDevKey;
                        }
                        else
                        {
                            targetGameKey = AndroidProdGame;
                            targetSecretKey = AndroidProdKey;
                        }
#endif
                        break;

                    default:
                        break;
                }

                GA_Wrapper.Initialize(targetGameKey, targetSecretKey);

                //User info
                User _userInfo = PlatformManager.GetUserInfo();
                if (_userInfo != null)
                {
                    GAGender _gender = _userInfo.sex == "" || _userInfo.sex == "0" ? GAGender.Undefined : _userInfo.sex == "2" ? GAGender.male : GAGender.female;
                    GameAnalytics.SetGender(_gender);

                    int _birthYear = _userInfo.byear;
                    GameAnalytics.SetBirthYear(_birthYear);

                    string _referral = PlatformManager.GetUserReferral() == "" ? null : PlatformManager.GetUserReferral();
                }

                GameAnalytics.SettingsGA.SetCustomUserID(PlatformManager.GetUserID());

            }

            InitComplete();
        }

        /// <summary>
        /// Return static id of platform
        /// </summary>
        public void InitComplete()
        {
            Debug.Log("GameAnalytics InitComplete");
            if (initCallbackFunction != null)
                initCallbackFunction();
        }

        //// <summary>
        /// Track when mission/level/quest open and view
        /// </summary>
        override public void OpenLevel(int _level)
        {
            Debug.Log("GameAnalytics.OpenLevel " + _level);
            GameAnalytics.NewDesignEvent("Level:Open", _level);
        }

        //// <summary>
        /// Track when mission/level/quest started
        /// </summary>
        override public void StartLevel(int _level)
        {
            Debug.Log("GameAnalytics.StartLevel " + _level);
            GameAnalytics.NewDesignEvent("Level:Start", _level);
        }

        //// <summary>
        /// Track when mission/level/quest failed
        /// </summary>
        override public void FailLevel(int _level)
        {
            Debug.Log("GameAnalytics.FailLevel " + _level);
            GameAnalytics.NewDesignEvent("Level:Fail", _level);
        }

        //// <summary>
        /// Track when mission/level/quest completed
        /// </summary>
        override public void CompleteLevel(int _level)
        {
            Debug.Log("GameAnalytics.CompleteLevel " + _level);
            GameAnalytics.NewDesignEvent("Level:Complete", _level);
        }

        //// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about try real payment
        /// </summary>
        override public void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("GameAnalytics.PaymentInfoTry " + _itemID + " " + _amount + " " + _currency);
            GameAnalytics.NewDesignEvent("Payment:Try:" + _itemID + ":" + _area, _amount);
        }


        //// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about sucess real payment
        /// </summary>
        override public void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("GameAnalytics.PaymentInfoSuccess " + _itemID + " " + _amount + " " + _currency);
            GameAnalytics.NewDesignEvent("Payment:Success:" + _itemID + ":" + _area, _amount);
        }


        //// <summary>
        /// Track business real payment with currency and value
        /// </summary>
        override public void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("GameAnalytics.PaymentReal " + _itemID + " " + _amount + " " + _currency);
            GameAnalytics.NewBusinessEvent(_currency, _amount, _itemType, _itemID, _area);
        }


        //// <summary>
        /// Track resource event
        /// </summary>
        override public void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("GameAnalytics.ResourceAdd " + _itemID + " " + _amount + " " + _currency);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, _currency, _amount, _itemType, _itemID);
        }

        //// <summary>
        /// Track resource event
        /// </summary>
        override public void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("GameAnalytics.ResourceRemove " + _itemID + " " + _amount + " " + _currency);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, _currency, _amount, _itemType, _itemID);
        }


        //// <summary>
        /// Track open invite window
        /// </summary>
        override public void InviteTry(string _area)
        {
            Debug.Log("GameAnalytics.InviteTry " );
            GameAnalytics.NewDesignEvent("Invite:OpenWindow" + ":" + _area);
        }

        //// <summary>
        /// Track open share window
        /// </summary>
        override public void ShareTry(string _area)
        {
            Debug.Log("GameAnalytics.ShareTry ");
            GameAnalytics.NewDesignEvent("Share:OpenWindow" + ":" + _area);
        }

        //// <summary>
        /// Track successfull share
        /// </summary>
        override public void ShareSuccess(string _area)
        {
            Debug.Log("GameAnalytics.ShareSuccess ");
            GameAnalytics.NewDesignEvent("Share:Success" + ":" + _area);
        }

        //// <summary>
        /// Track open Request window
        /// </summary>
        override public void RequestTry(string _type, string _area)
        {
            Debug.Log("GameAnalytics.RequestTry " + _type);
            GameAnalytics.NewDesignEvent("Request:OpenWindow:" + _type + ":" + _area);
        }

        //// <summary>
        /// Track successfull Request
        /// </summary>
        override public void RequestSuccess(string _type, string _area)
        {
            Debug.Log("GameAnalytics.RequestSuccess " + _type);
            GameAnalytics.NewDesignEvent("Request:Success:" + _type + ":" + _area);
        }

        //// <summary>
        /// Track optional game design event
        /// </summary>
        override public void DesignEvent(string _id, int _amount)
        {
            Debug.Log("GameAnalytics.NewDesignEvent " + _id + " " + _amount);
            GameAnalytics.NewDesignEvent(_id, _amount);
        }


#endif
    }

}