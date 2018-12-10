using UnityEngine;
using System.Collections.Generic;
using System;
using SCore.Utils;

#if CORE_ADJUST
using com.adjust.sdk;
#endif

namespace SCore.Analytics
{

    /// <summary>
    /// Analytic class for Adjust Analytics https://github.com/adjust/unity_sdk
    /// </summary
    public class AdjustAnalyticSystem : IAnalyticSystem
    {
        //PUBLIC STATIC

        //PUBLIC EVENTS
        public override event Action<IAnalyticSystem> InitCompletedEvent;
        public override event Action<IAnalyticSystem, string> InitErrorEvent;

        //PUBLIC VARIABLES
        public string AppToken = "{Your App Token}";
        public bool ShowWarningsOnMissingEvents = false;
        public bool EventBuffering = false;
        public bool SendInBackground = false;
        public bool LaunchDeferredDeeplink = true;
#if CORE_ADJUST
        public AdjustAnalyticEvent[] EventsConfig;
#endif

        //PRIVATE STATIC
        private static Action initCallbackFunction;
        private const string OPEN_EVENT = "app_open";

        //PRIVATE VARIABLES

#if CORE_ADJUST
        /// <summary>
        /// Constructor
        /// </summary
        override public void Init()
        {
            Debug.Log("AdjustAnalyticSystem init");
            try
            {
                //Configuration
                AdjustConfig adjustConfig = new AdjustConfig(this.AppToken, Debug.isDebugBuild ? AdjustEnvironment.Sandbox : AdjustEnvironment.Production, false);
                adjustConfig.setSendInBackground(SendInBackground);
                adjustConfig.setEventBufferingEnabled(EventBuffering);
                adjustConfig.setLaunchDeferredDeeplink(LaunchDeferredDeeplink);
                Adjust.start(adjustConfig);

                //Init events
                DesignEvent(OPEN_EVENT, 0, null);
                Debug.Log("AdjustAnalyticSystem InitComplete");
                InitCompletedEvent?.Invoke(this);
            }
            catch (Exception e)
            {
                InitErrorEvent?.Invoke(this, e.Message);
            }

        }

        private void InitError()
        {
            InitErrorEvent?.Invoke(this, "AdjustAnalyticSystem failed to load. Play servises is not avaliable");
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                DesignEvent(OPEN_EVENT, 0, null);
            }
        }

        public override void SocialSignUp()
        {
            Debug.Log("AdjustAnalyticSystem:SocialSignUp not implemented event!");
        }

        override public void OpenLevel(int _level, string _type)
        {
            Debug.Log("AdjustAnalyticSystem:OpenLevel not implemented event!");
        }

        override public void StartLevel(int _level, string _type)
        {
            Debug.Log("AdjustAnalyticSystem:StartLevel not implemented event!");
        }

        override public void FailLevel(int _level, string _type, int _score)
        {
            Debug.Log("AdjustAnalyticSystem:FailLevel not implemented event!");
        }

        override public void CompleteLevel(int _level, string _type, int _score)
        {
            Debug.Log("AdjustAnalyticSystem:CompleteLevel not implemented event!");
        }

        public override void NewScore(int _level, int _score)
        {
            Debug.Log("AdjustAnalyticSystem:NewScore not implemented event!");
        }

        public override void AchievenemntUnlocked(string _achievementID)
        {
            Debug.Log("AdjustAnalyticSystem:AchievenemntUnlocked not implemented event!");
        }


        public override void TutorialStart()
        {
            Debug.Log("AdjustAnalyticSystem:TutorialStart not implemented event!");
        }

        public override void TutorialCompleted()
        {
            Debug.Log("AdjustAnalyticSystem:TutorialCompleted not implemented event!");
        }

        override public void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AdjustAnalyticSystem:PaymentInfoTry not implemented event!");
        }

        override public void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //Backend already has this info
        }

        override public void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //Backend already has this info
        }

        override public void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AdjustAnalyticSystem:ResourceAdd not implemented event!");
        }

        override public void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AdjustAnalyticSystem:ResourceRemove not implemented event!");
        }

        override public void InviteTry(string _area)
        {
            Debug.Log("AdjustAnalyticSystem:InviteTry not implemented event!");
        }

        override public void ShareTry(string _id, string _area)
        {
            Debug.Log("AdjustAnalyticSystem:ShareTry not implemented event!");
        }

        override public void ShareSuccess(string _id, string _area)
        {
            Debug.Log("AdjustAnalyticSystem:ShareSuccess not implemented event!");
        }

        override public void RequestTry(string _type, string _area)
        {
            Debug.Log("AdjustAnalyticSystem:RequestTry not implemented event!");
        }

        override public void RequestSuccess(string _type, string _area)
        {
            Debug.Log("AdjustAnalyticSystem:RequestSuccess not implemented event!");
        }

        override public void DesignEvent(string _id, int _amount, Dictionary<string, object> parameters)
        {
            Debug.Log("AdjustAnalyticSystem.DesignEvent with Parameters ");

            AdjustAnalyticEvent targetEvent = FindEventByName(_id);
            if (targetEvent != null && !string.IsNullOrEmpty(targetEvent.Token))
            {
                AdjustEvent adjustEvent = new AdjustEvent(targetEvent.Token);
                Adjust.trackEvent(adjustEvent);
            }
        }

        public override void SetUserStringProperty(string _id, string _value)
        {
            Debug.Log("AdjustAnalyticSystem:SetUserStringProperty not implemented event!");
        }

        public override void SetUserIntProperty(string _id, int _value)
        {
            Debug.Log("AdjustAnalyticSystem:SetUserIntProperty not implemented event!");
        }

        public AdjustAnalyticEvent FindEventByName(string _name)
        {
            AdjustAnalyticEvent returned = null;
            for (int i = 0; i < EventsConfig.Length; i++)
            {
                if (EventsConfig[i].Name == _name)
                {
                    returned = EventsConfig[i];
                }
            }
            return returned;
        }

#else
        public override void Init()
        {
            InitErrorEvent(this, "CORE_ADJUST not added in compilation constants");
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

        public override void DesignEvent(string _id, int _amount, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public override void SetUserStringProperty(string _id, string _value)
        {
            throw new NotImplementedException();
        }

        public override void SetUserIntProperty(string _id, int _value)
        {
            throw new NotImplementedException();
        }
#endif
    }

#if CORE_ADJUST
    [Serializable]
    public class AdjustAnalyticEvent
    {
        public string Name;
        public string Token;
    }
#endif
}