using UnityEngine;
using System.Collections.Generic;
using System;
#if CORE_FB
using Facebook.Unity;
#endif

namespace SCore.Analytics
{
    /// <summary>
    /// Analytic class for Facebook events https://developers.facebook.com/docs/unity/reference/current/FB.LogAppEvent
    /// </summary
    public class FBAnalyticSystem : IAnalyticSystem
    {
        //PUBLIC STATIC

        //PUBLIC EVENTS
        public override event Action<IAnalyticSystem> InitCompletedEvent;
        public override event Action<IAnalyticSystem, string> InitErrorEvent;

        //PUBLIC VARIABLES

        //PRIVATE STATIC

        //PRIVATE VARIABLES
        private string targetGameKey;
        private string targetSecretKey;

#if CORE_FB

        /// <summary>
        /// Constructor
        /// </summary
        override public void Init()
        {
            Debug.Log("FBAnalytics init");

            try
            {
                //Init events
                if (!FB.IsInitialized)
                    FB.Init();
                Debug.Log("FBAnalytics InitComplete");
                if (InitCompletedEvent != null)
                    InitCompletedEvent(this);
            }
            catch (Exception e)
            {
                if (InitErrorEvent != null)
                    InitErrorEvent(this, e.Message);
            }
        }


        /// <summary>
        /// Track when mission/level/quest open and view
        /// </summary>
        override public void OpenLevel(int _level, string _type)
        {
            string _event = PrepareEventValue("Lv_Open_" + _type);
            Debug.Log("FBAnalytics.OpenLevel " + _level);
            FB.LogAppEvent(_event, _level);
        }

        /// <summary>
        /// Track when mission/level/quest started
        /// </summary>
        override public void StartLevel(int _level, string _type)
        {
            string _event = PrepareEventValue("Lv_Start_" + _type);
            Debug.Log("FBAnalytics.StartMission " + _level);
            FB.LogAppEvent(_event, _level);
        }

        /// <summary>
        /// Track when mission/level/quest failed
        /// </summary>
        override public void FailLevel(int _level, string _type, int _score)
        {
            string _event = PrepareEventValue("Lv_Fail_" + _type);
            Debug.Log("FBAnalytics.FailMission " + _level);
            FB.LogAppEvent(_event, _level);
        }

        /// <summary>
        /// Track when mission/level/quest completed
        /// </summary>
        override public void CompleteLevel(int _level, string _type, int _score)
        {
            string _event = PrepareEventValue("Lv_Сomplete_" + _type);
            Debug.Log("FBAnalytics.CompleteLevel " + _level);
            FB.LogAppEvent(_event, _level);
            FB.LogAppEvent(AppEventName.AchievedLevel, 0, new Dictionary<string, object>() { { AppEventParameterName.Level, _level } });
        }

        public override void NewScore(int _level, int _score)
        {
            Debug.Log("FBAnalytics.PostScore " + _level + " " + _score);
            string _event = PrepareEventValue("score");
            FB.LogAppEvent(_event, _score, new Dictionary<string, object>() { { AppEventParameterName.Level, _level } });
        }

        public override void AchievenemntUnlocked(string _achievementID)
        {
            Debug.Log("FBAnalytics.AchievenemntUnlocked " + _achievementID);
            FB.LogAppEvent(AppEventName.UnlockedAchievement, 0, new Dictionary<string, object>() { { AppEventParameterName.Description, _achievementID } });
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

        /// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about try real payment
        /// </summary>
        override public void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("FBAnalytics.PaymentInfoTry " + _currency + " " + _amount / 100.0F + " " + _itemID + " " + _itemType);
            FB.LogAppEvent(AppEventName.InitiatedCheckout, _amount / 100.0F, new Dictionary<string, object>() { { AppEventParameterName.Currency, _currency } });
        }


        /// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about sucess real payment
        /// </summary>
        override public void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //Facebook platform already has this info
        }


        /// <summary>
        /// Track business real payment with currency and value
        /// </summary>
        override public void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            FB.LogPurchase(_amount / 100.0F, _currency);
            Debug.Log("FBAnalytics.PaymentReal " + _currency + " " + _amount / 100.0F + " " + _itemID + " " + _itemType);
        }


        /// <summary>
        /// Track resource event
        /// </summary>
        override public void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            string _event = PrepareEventValue("Add_" + _currency + "_" + _itemID);
            Debug.Log("FBAnalytics.ResourceAdd " + _currency + " " + _amount + " " + _itemID + " " + _itemType);
            FB.LogAppEvent(_event, _amount);
        }

        /// <summary>
        /// Track resource event
        /// </summary>
        override public void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            string _event = PrepareEventValue("Rem_" + _currency + "_" + _itemType);
            Debug.Log("FBAnalytics.ResourceRemove " + _currency + " " + _amount + " " + _itemID + " " + _itemType);
            FB.LogAppEvent(_event, _amount);
        }


        /// <summary>
        /// Track open invite window
        /// </summary>
        override public void InviteTry(string _area)
        {
            //Facebook platform already has this info
        }

        /// <summary>
        /// Track open share window
        /// </summary>
        override public void ShareTry(string _id, string _area)
        {
            //Facebook platform already has this info
        }

        /// <summary>
        /// Track successfull share
        /// </summary>
        override public void ShareSuccess(string _id, string _area)
        {
            //Facebook platform already has this info
        }

        /// <summary>
        /// Track open Request window
        /// </summary>
        override public void RequestTry(string _type, string _area)
        {
            //Facebook platform already has this info
        }

        /// <summary>
        /// Track successfull Request
        /// </summary>
        override public void RequestSuccess(string _type, string _area)
        {
            //Facebook platform already has this info
        }

        /// <summary>
        /// Track optional game design event
        /// </summary>
        override public void DesignEvent(string _id, int _amount)
        {
            string _event = PrepareEventValue(_id);
            FB.LogAppEvent(_event, _amount);
            Debug.Log("FBAnalytics.DesignEvent " + _event + " " + _amount);
        }

        override public void DesignEvent(string _id, Dictionary<string, object> parameters)
        {
            string _event = PrepareEventValue(_id);
            try
            {
                FB.LogAppEvent(_event, 0, parameters);
            }
            catch (Exception e)
            {
                Debug.Log("FBAnalyticSystem Exception");
                Debug.Log(e.ToString());
                throw;
            }
            Debug.Log("FBAnalytics.DesignEvent with Parameters" + _event);
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




#else

        public override void Init()
        {
            InitErrorEvent(this, "CORE_FB not added in compilation constants");
        }

        public override void SocialSignUp()
        {
            throw new System.NotImplementedException();
        }

        public override void OpenLevel(int _level, string _type)
        {
            throw new System.NotImplementedException();
        }

        public override void StartLevel(int _level, string _type)
        {
            throw new System.NotImplementedException();
        }

        override public void FailLevel(int _level, string _type, int _score)
        {
            throw new System.NotImplementedException();
        }

        override public void CompleteLevel(int _level, string _type, int _score)
        {
            throw new System.NotImplementedException();
        }

        public override void NewScore(int _level, int _score)
        {
            throw new System.NotImplementedException();
        }

        public override void AchievenemntUnlocked(string _achievementID)
        {
            throw new System.NotImplementedException();
        }

        public override void TutorialStart()
        {
            throw new System.NotImplementedException();
        }

        public override void TutorialCompleted()
        {
            throw new System.NotImplementedException();
        }

        public override void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            throw new System.NotImplementedException();
        }

        public override void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            throw new System.NotImplementedException();
        }

        public override void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            throw new System.NotImplementedException();
        }

        public override void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            throw new System.NotImplementedException();
        }

        public override void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            throw new System.NotImplementedException();
        }

        public override void InviteTry(string _area)
        {
            throw new System.NotImplementedException();
        }

        public override void ShareTry(string _id, string _area)
        {
            throw new System.NotImplementedException();
        }

        public override void ShareSuccess(string _id, string _area)
        {
            throw new System.NotImplementedException();
        }

        public override void RequestTry(string _type, string _area)
        {
            throw new System.NotImplementedException();
        }

        public override void RequestSuccess(string _type, string _area)
        {
            throw new System.NotImplementedException();
        }

        public override void DesignEvent(string _id, int _amount)
        {
            throw new System.NotImplementedException();
        }

        public override void DesignEvent(string _id, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
#endif
    }

}