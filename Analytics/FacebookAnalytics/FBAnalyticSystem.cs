using UnityEngine;
using System.Collections.Generic;
#if CORE_FB
using Facebook.Unity;
#endif

namespace SCore
{
    /// <summary>
    /// Analytic class for Facebook events https://developers.facebook.com/docs/unity/reference/current/FB.LogAppEvent
    /// </summary
    public class FBAnalyticSystem : IAnalyticSystem
    {

        #region Public variables
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

#if CORE_FB


        /// <summary>
        /// Constructor
        /// </summary
        override public void Init(Callback.EventHandler _callbackFunction)
        {
            Debug.Log("FBAnalytics init");
            initCallbackFunction = _callbackFunction;

            //Init FB

            //User info
            /*
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
            */
            InitComplete();
        }

        /// <summary>
        /// Return static id of platform
        /// </summary>
        public void InitComplete()
        {
            Debug.Log("FBAnalytics InitComplete");
            if (initCallbackFunction != null)
                initCallbackFunction();
        }

        //// <summary>
        /// Track when mission/level/quest open and view
        /// </summary>
        override public void OpenLevel(int _level)
        {
            string _event = PrepareEventValue("Level_Open");
            Debug.Log("FBAnalytics.OpenLevel " + _level);
            FB.LogAppEvent(_event, _level);
        }

        //// <summary>
        /// Track when mission/level/quest started
        /// </summary>
        override public void StartLevel(int _level)
        {
            string _event = PrepareEventValue("Level_Start");
            Debug.Log("FBAnalytics.StartMission " + _level);
            FB.LogAppEvent(_event, _level);
        }

        //// <summary>
        /// Track when mission/level/quest failed
        /// </summary>
        override public void FailLevel(int _level)
        {
            string _event = PrepareEventValue("Level_Fail");
            Debug.Log("FBAnalytics.FailMission " + _level);
            FB.LogAppEvent(_event, _level);
        }

        //// <summary>
        /// Track when mission/level/quest completed
        /// </summary>
        override public void CompleteLevel(int _level)
        {
            string _event = PrepareEventValue("Level_Сomplete");
            Debug.Log("FBAnalytics.CompleteLevel " + _level);
            FB.LogAppEvent(_event, _level);
        }

        public override void TutorialStart()
        {
            Debug.Log("FBAnalytics.TutorialStart");
            string _event = PrepareEventValue("fb_mobile_tutorial_started");
            FB.LogAppEvent(_event);
        }

        public override void TutorialCompleted()
        {
            Debug.Log("FBAnalytics.TutorialCompleted");
            FB.LogAppEvent(AppEventName.CompletedTutorial);
        }

        //// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about try real payment
        /// </summary>
        override public void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("FBAnalytics.PaymentInfoTry " + _currency + " " + _amount / 100.0F + " " + _itemID + " " + _itemType);
            FB.LogAppEvent(AppEventName.InitiatedCheckout, _amount / 100.0F, new Dictionary<string, object>() { { AppEventParameterName.Currency, _currency } });
        }


        //// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about sucess real payment
        /// </summary>
        override public void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //Facebook platform already has this info
        }


        //// <summary>
        /// Track business real payment with currency and value
        /// </summary>
        override public void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            FB.LogPurchase(_amount / 100.0F, _currency);
            Debug.Log("FBAnalytics.PaymentReal " + _currency + " " + _amount / 100.0F + " " + _itemID + " " + _itemType);
        }


        //// <summary>
        /// Track resource event
        /// </summary>
        override public void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            string _event = PrepareEventValue("Add_" + _currency + "_" + _itemID);
            Debug.Log("FBAnalytics.ResourceAdd " + _currency + " " + _amount + " " + _itemID + " " + _itemType);
            FB.LogAppEvent(_event, _amount);
        }

        //// <summary>
        /// Track resource event
        /// </summary>
        override public void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            string _event = PrepareEventValue("Rem_" + _currency + "_" + _itemID);
            Debug.Log("FBAnalytics.ResourceRemove " + _currency + " " + _amount + " " + _itemID + " " + _itemType);
            FB.LogAppEvent(_event, _amount);
        }


        //// <summary>
        /// Track open invite window
        /// </summary>
        override public void InviteTry(string _area)
        {
            //Facebook platform already has this info
        }

        //// <summary>
        /// Track open share window
        /// </summary>
        override public void ShareTry(string _id, string _area)
        {
            //Facebook platform already has this info
        }

        //// <summary>
        /// Track successfull share
        /// </summary>
        override public void ShareSuccess(string _id, string _area)
        {
            //Facebook platform already has this info
        }

        //// <summary>
        /// Track open Request window
        /// </summary>
        override public void RequestTry(string _type, string _area)
        {
            //Facebook platform already has this info
        }

        //// <summary>
        /// Track successfull Request
        /// </summary>
        override public void RequestSuccess(string _type, string _area)
        {
            //Facebook platform already has this info
        }

        //// <summary>
        /// Track optional game design event
        /// </summary>
        override public void DesignEvent(string _id, int _amount)
        {
            string _event = PrepareEventValue(_id);
            FB.LogAppEvent(_event, _amount);
            Debug.Log("FBAnalytics.DesignEvent " + _event + " " + _amount);
        }

        private string PrepareEventValue(string _event)
        {
            _event.Replace(':', '_');
            _event.Replace(' ', '_');
            _event.Replace('.', '_');
            return _event;
        }

        public override void SocialSignUp()
        {
            //Facebook platform already has this info
        }

#endif
    }

}