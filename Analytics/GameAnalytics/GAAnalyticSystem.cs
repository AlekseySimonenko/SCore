using UnityEngine;
#if CORE_GA
using GameAnalyticsSDK;
using GameAnalyticsSDK.Wrapper;
#endif

namespace SCore
{
    /// <summary>
    /// Analytic class for Game Analytics (GA) https://go.gameanalytics.com
    /// </summary
    public class GAAnalyticSystem : IAnalyticSystem
    {

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


        private static Callback.EventHandler initCallbackFunction;
        private string targetGameKey;
        private string targetSecretKey;

        
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
                //GameAnalytics.SettingsGA.SetCustomUserID(PlatformManager.GetUserID());
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


        public override void SocialSignUp()
        {
            Debug.Log("GameAnalytics.SocialSignUp");
            GameAnalytics.NewDesignEvent("Social:SignUp");
        }

        override public void OpenLevel(int _level)
        {
            Debug.Log("GameAnalytics.OpenLevel " + _level);
            GameAnalytics.NewDesignEvent("Level:Open", _level);
        }

        override public void StartLevel(int _level)
        {
            Debug.Log("GameAnalytics.StartLevel " + _level);
            GameAnalytics.NewDesignEvent("Level:Start", _level);
        }

        override public void FailLevel(int _level)
        {
            Debug.Log("GameAnalytics.FailLevel " + _level);
            GameAnalytics.NewDesignEvent("Level:Fail", _level);
        }

        override public void CompleteLevel(int _level)
        {
            Debug.Log("GameAnalytics.CompleteLevel " + _level);
            GameAnalytics.NewDesignEvent("Level:Complete", _level);
        }

        public override void NewScore(int _level, int _score)
        {
            Debug.Log("GameAnalytics.PostScore " + _level + " " + _score);
            GameAnalytics.NewDesignEvent("Score", _score);
        }

        public override void AchievenemntUnlocked(string _achievementID)
        {
            Debug.Log("GameAnalytics.AchievenemntUnlocked " + _achievementID);
            GameAnalytics.NewDesignEvent("Achievement:Unlocked:" + _achievementID, 0);
        }

        public override void TutorialStart()
        {
            Debug.Log("GameAnalytics.TutorialStart");
            GameAnalytics.NewDesignEvent("Tutorial:Start");
        }

        public override void TutorialCompleted()
        {
            Debug.Log("GameAnalytics.TutorialCompleted");
            GameAnalytics.NewDesignEvent("Tutorial:Completed");
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
            Debug.Log("GameAnalytics.InviteTry ");
            GameAnalytics.NewDesignEvent("Invite:OpenWindow" + ":" + _area);
        }

        //// <summary>
        /// Track open share window
        /// </summary>
        override public void ShareTry(string _id, string _area)
        {
            Debug.Log("GameAnalytics.ShareTry ");
            GameAnalytics.NewDesignEvent("ShareOpenWindow" + ":" + _id + ":" + _area);
        }

        //// <summary>
        /// Track successfull share
        /// </summary>
        override public void ShareSuccess(string _id, string _area)
        {
            Debug.Log("GameAnalytics.ShareSuccess ");
            GameAnalytics.NewDesignEvent("ShareSuccess" + ":" + _id + ":" + _area);
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



#else
        public override void SocialSignUp()
        {
            //throw new System.NotImplementedException();
        }

        public override void OpenLevel(int _level)
        {
            //throw new System.NotImplementedException();
        }

        public override void StartLevel(int _level)
        {
            //throw new System.NotImplementedException();
        }

        public override void FailLevel(int _level)
        {
            //throw new System.NotImplementedException();
        }

        public override void CompleteLevel(int _level)
        {
            //throw new System.NotImplementedException();
        }

        public override void NewScore(int _level, int _score)
        {
            //throw new System.NotImplementedException();
        }

        public override void AchievenemntUnlocked(string _achievementID)
        {
            //throw new System.NotImplementedException();
        }

        public override void TutorialStart()
        {
            //throw new System.NotImplementedException();
        }

        public override void TutorialCompleted()
        {
            //throw new System.NotImplementedException();
        }

        public override void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //throw new System.NotImplementedException();
        }

        public override void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //throw new System.NotImplementedException();
        }

        public override void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //throw new System.NotImplementedException();
        }

        public override void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //throw new System.NotImplementedException();
        }

        public override void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //throw new System.NotImplementedException();
        }

        public override void InviteTry(string _area)
        {
            //throw new System.NotImplementedException();
        }

        public override void ShareTry(string _id, string _area)
        {
            //throw new System.NotImplementedException();
        }

        public override void ShareSuccess(string _id, string _area)
        {
            //throw new System.NotImplementedException();
        }

        public override void RequestTry(string _type, string _area)
        {
            //throw new System.NotImplementedException();
        }

        public override void RequestSuccess(string _type, string _area)
        {
            //throw new System.NotImplementedException();
        }

        public override void DesignEvent(string _id, int _amount)
        {
            //throw new System.NotImplementedException();
        }

#endif
    }

}