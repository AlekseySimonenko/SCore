using UnityEngine;
using System.Collections.Generic;

#if CORE_FIREBASE
using Firebase.Analytics;
#endif

namespace SCore
{
    /// <summary>
    /// Analytic class for Firebase https://firebase.google.com/docs/analytics/
    /// </summary
    public class FirebaseAnalyticSystem : IAnalyticSystem
    {

        private static Callback.EventHandler initCallbackFunction;
        private string targetGameKey;
        private string targetSecretKey;

#if CORE_FIREBASE


        /// <summary>
        /// Constructor
        /// </summary
        override public void Init(Callback.EventHandler _callbackFunction)
        {
            Debug.Log("FirebaseAnalyticSystem init");
            initCallbackFunction = _callbackFunction;
            InitComplete();
        }

        /// <summary>
        /// Return static id of platform
        /// </summary>
        public void InitComplete()
        {
            Debug.Log("FirebaseAnalyticSystem InitComplete");
            if (initCallbackFunction != null)
                initCallbackFunction();
        }

        //// <summary>
        /// Track when mission/level/quest open and view
        /// </summary>
        override public void OpenLevel(int _level)
        {
            string _event = PrepareEventValue("Level_Open");
            FirebaseAnalytics.LogEvent(_event, FirebaseAnalytics.ParameterLevel, _level);
            Debug.Log("FirebaseAnalyticSystem.OpenLevel " + _level);
        }

        //// <summary>
        /// Track when mission/level/quest started
        /// </summary>
        override public void StartLevel(int _level)
        {
            string _event = PrepareEventValue("Level_Start");
            FirebaseAnalytics.LogEvent(_event, FirebaseAnalytics.ParameterLevel, _level);
            Debug.Log("FirebaseAnalyticSystem.StartMission " + _level);
        }

        //// <summary>
        /// Track when mission/level/quest failed
        /// </summary>
        override public void FailLevel(int _level)
        {
            string _event = PrepareEventValue("Level_Fail");
            FirebaseAnalytics.LogEvent(_event, FirebaseAnalytics.ParameterLevel, _level);
            Debug.Log("FirebaseAnalyticSystem.FailMission " + _level);
        }

        //// <summary>
        /// Track when mission/level/quest completed
        /// </summary>
        override public void CompleteLevel(int _level)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelUp, FirebaseAnalytics.ParameterLevel, _level);
            Debug.Log("FirebaseAnalyticSystem.CompleteLevel " + _level);
        }

        //// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about try real payment
        /// </summary>
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
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEcommercePurchase,
                new Parameter[]{
                    new Parameter(FirebaseAnalytics.ParameterCurrency, _currency),
                    new Parameter(FirebaseAnalytics.ParameterValue, _amount)
                }
            );
            Debug.Log("FirebaseAnalyticSystem.PaymentReal " + _currency + " " + _amount / 100.0F + " " + _itemID + " " + _itemType);
        }


        //// <summary>
        /// Track resource event
        /// </summary>
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

        //// <summary>
        /// Track resource event
        /// </summary>
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


        //// <summary>
        /// Track open invite window
        /// </summary>
        override public void InviteTry(string _area)
        {
            Debug.Log("FirebaseAnalyticSystem:InviteTry not implemented event!");
        }

        //// <summary>
        /// Track open share window
        /// </summary>
        override public void ShareTry(string _area)
        {
            Debug.Log("FirebaseAnalyticSystem:ShareTry not implemented event!");
        }

        //// <summary>
        /// Track successfull share
        /// </summary>
        override public void ShareSuccess(string _area)
        {
            Debug.Log("FirebaseAnalyticSystem:ShareSuccess not implemented event!");
        }

        //// <summary>
        /// Track open Request window
        /// </summary>
        override public void RequestTry(string _type, string _area)
        {
            Debug.Log("FirebaseAnalyticSystem:RequestTry not implemented event!");
        }

        //// <summary>
        /// Track successfull Request
        /// </summary>
        override public void RequestSuccess(string _type, string _area)
        {
            Debug.Log("FirebaseAnalyticSystem:RequestSuccess not implemented event!");
        }

        //// <summary>
        /// Track optional game design event
        /// </summary>
        override public void DesignEvent(string _id, int _amount)
        {
            string _event = PrepareEventValue(_id);

            FirebaseAnalytics.LogEvent(_event, FirebaseAnalytics.ParameterValue, _amount);
            Debug.Log("FirebaseAnalyticSystem.DesignEvent " + _event + " " + _amount);
        }

        private string PrepareEventValue(string _event)
        {
            _event.Replace(':', '_');
            _event.Replace(' ', '_');
            _event.Replace('.', '_');
            return _event;
        }

#endif
    }

}