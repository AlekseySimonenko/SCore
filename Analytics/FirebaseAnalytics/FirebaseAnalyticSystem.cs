using UnityEngine;
using System.Collections.Generic;
using System;
using SCore.Utils;

#if CORE_FIREBASE
using Firebase.Analytics;
#endif

namespace SCore.Analytics
{
    /// <summary>
    /// Analytic class for Firebase https://firebase.google.com/docs/analytics/
    /// </summary
    public class FirebaseAnalyticSystem : IAnalyticSystem
    {
        //PUBLIC STATIC

        //PUBLIC EVENTS
        public override event Action<IAnalyticSystem> InitCompletedEvent;
        public override event Action<IAnalyticSystem, string> InitErrorEvent;

        //PUBLIC VARIABLES
        public int sessionTimeoutDurationMS = 180000;

        //PRIVATE STATIC
        private static Action initCallbackFunction;

        //PRIVATE VARIABLES
        private string targetGameKey;
        private string targetSecretKey;

#if CORE_FIREBASE


        /// <summary>
        /// Constructor
        /// </summary
        override public void Init()
        {
            Debug.Log("FirebaseAnalyticSystem init");
            try
            {
#if UNITY_ANDROID 
                if (!GooglePlayServicesState.IsAvaliable())
                {
                    InitError();
                    return;
                }
#endif
                //Configuration
                TimeSpan sessionTimeout = new TimeSpan(sessionTimeoutDurationMS * TimeSpan.TicksPerMillisecond);
                FirebaseAnalytics.SetSessionTimeoutDuration(sessionTimeout);

                //Init events
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
                Debug.Log("FirebaseAnalyticSystem InitComplete");
                InitCompletedEvent?.Invoke(this);
            }
            catch (Exception e)
            {
                InitErrorEvent?.Invoke(this, e.Message);
            }

        }

        private void InitError()
        {
            InitErrorEvent?.Invoke(this, "FirebaseAnalyticSystem failed to load. Play servises is not avaliable");
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
            }
        }

        public override void SocialSignUp()
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSignUp);
        }

        override public void OpenLevel(int _level, string _type)
        {
            string _event = PrepareEventValue("Level_Open");
            FirebaseAnalytics.LogEvent(_event,
                new Parameter[]{
                    new Parameter(FirebaseAnalytics.ParameterLevel, _level),
                    new Parameter("level_type", _type)
                }
            );

            Debug.Log("FirebaseAnalyticSystem.OpenLevel " + _level);
        }

        override public void StartLevel(int _level, string _type)
        {
            string _event = PrepareEventValue("Level_Start");
            FirebaseAnalytics.LogEvent(_event,
                new Parameter[]{
                    new Parameter(FirebaseAnalytics.ParameterLevel, _level),
                    new Parameter("level_type", _type)
                }
            );
            Debug.Log("FirebaseAnalyticSystem.StartMission " + _level);
        }

        override public void FailLevel(int _level, string _type, int _score)
        {
            string _event = PrepareEventValue("Level_Fail");
            FirebaseAnalytics.LogEvent(_event,
                new Parameter[]{
                    new Parameter(FirebaseAnalytics.ParameterLevel, _level),
                    new Parameter("level_type", _type)
                }
            );
            Debug.Log("FirebaseAnalyticSystem.FailMission " + _level);
        }

        override public void CompleteLevel(int _level, string _type, int _score)
        {
            string _event = PrepareEventValue("Level_completed");
            FirebaseAnalytics.LogEvent(_event,
                new Parameter[]{
                    new Parameter(FirebaseAnalytics.ParameterLevel, _level),
                    new Parameter("level_type", _type),
                    new Parameter("score", _score)
                }
            );
            //Additional event
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelUp, FirebaseAnalytics.ParameterLevel, _level);
            Debug.Log("FirebaseAnalyticSystem.CompleteLevel " + _level);
        }

        public override void NewScore(int _level, int _score)
        {
            Debug.Log("FirebaseAnalyticSystem.PostScore " + _level + " " + _score);
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPostScore,
                new Parameter[]{
                    new Parameter(FirebaseAnalytics.ParameterLevel, _level),
                    new Parameter(FirebaseAnalytics.ParameterValue, _score)
                }
            );
        }

        public override void AchievenemntUnlocked(string _achievementID)
        {
            Debug.Log("FirebaseAnalyticSystem.AchievenemntUnlocked " + _achievementID);
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventUnlockAchievement, FirebaseAnalytics.ParameterAchievementId, _achievementID);
        }


        public override void TutorialStart()
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventTutorialBegin);
        }

        public override void TutorialCompleted()
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventTutorialComplete);
        }

        override public void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventBeginCheckout,
                new Parameter[]{
                    new Parameter(FirebaseAnalytics.ParameterCurrency, _currency),
                    new Parameter(FirebaseAnalytics.ParameterValue, _amount)
                }
            );

            Debug.Log("FirebaseAnalyticSystem.PaymentInfoTry " + _currency + " " + _amount / 100.0F + " " + _itemID + " " + _itemType);

        }

        override public void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //Firebase platform already has this info
        }

        override public void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //Firebase platform already has this info
        }

        override public void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency,
                new Parameter[]{
                    new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, _currency),
                    new Parameter(FirebaseAnalytics.ParameterValue, _amount),
                    new Parameter(FirebaseAnalytics.ParameterItemId, _itemID),
                    new Parameter(FirebaseAnalytics.ParameterItemCategory, _itemType),
                }
            );
            Debug.Log("FirebaseAnalyticSystem.ResourceAdd " + _currency + " " + _amount + " " + _itemID + " " + _itemType);
        }

        override public void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSpendVirtualCurrency,
                new Parameter[]{
                    new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, _currency),
                    new Parameter(FirebaseAnalytics.ParameterValue, _amount),
                    new Parameter(FirebaseAnalytics.ParameterItemId, _itemID),
                    new Parameter(FirebaseAnalytics.ParameterItemCategory, _itemType),
                }
            );
            Debug.Log("FirebaseAnalyticSystem.ResourceRemove " + _currency + " " + _amount + " " + _itemID + " " + _itemType);
        }

        override public void InviteTry(string _area)
        {
            Debug.Log("FirebaseAnalyticSystem:InviteTry not implemented event!");
        }

        override public void ShareTry(string _id, string _area)
        {
            Debug.Log("FirebaseAnalyticSystem:ShareTry not implemented event!");
        }

        override public void ShareSuccess(string _id, string _area)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventShare,
                new Parameter[]{
                    new Parameter(FirebaseAnalytics.ParameterContentType, _area),
                    new Parameter(FirebaseAnalytics.ParameterItemId, _id)
                }
            );

            Debug.Log("FirebaseAnalyticSystem.ShareSuccess " + _area);
        }

        override public void RequestTry(string _type, string _area)
        {
            Debug.Log("FirebaseAnalyticSystem:RequestTry not implemented event!");
        }

        override public void RequestSuccess(string _type, string _area)
        {
            Debug.Log("FirebaseAnalyticSystem:RequestSuccess not implemented event!");
        }

        override public void DesignEvent(string _id, int _amount, Dictionary<string, object> parameters)
        {
            string _event = PrepareEventValue(_id);

            if (parameters == null)
                parameters = new Dictionary<string, object>();

            if (!parameters.ContainsKey(FirebaseAnalytics.ParameterValue))
                parameters.Add(FirebaseAnalytics.ParameterValue, _amount);
            else
                parameters[FirebaseAnalytics.ParameterValue] = _amount;

            try
            {
                Parameter[] eventParameters = new Parameter[parameters.Count];
                int i = 0;
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    eventParameters[i] = new Parameter(kvp.Key, kvp.Value.ToString());
                    i++;
                }

                FirebaseAnalytics.LogEvent(_event, eventParameters);
            }
            catch (Exception e)
            {
                Debug.Log("FirebaseAnalyticSystem Exception");
                Debug.Log(e.ToString());
                throw;
            }


            Debug.Log("FirebaseAnalyticSystem.DesignEvent with Parameters " + _event);
        }


        private string PrepareEventValue(string _event)
        {
            _event.Replace(':', '_');
            _event.Replace(' ', '_');
            _event.Replace('.', '_');
            return _event;
        }


#else
        public override void Init()
        {
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
#endif
    }

}