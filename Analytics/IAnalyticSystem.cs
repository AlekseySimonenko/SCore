
using System;
using UnityEngine;

namespace Core
{

    /// <summary>
    /// Base abstract class for analytics platforms. By standart most of the features are disabled.
    /// </summary>
    [Serializable]
    public abstract class IAnalyticSystem : MonoBehaviour
    {
        //// <summary>
        /// System must must call callback from this method when init will completed
        /// </summary>
        public virtual void Init(Callback.EventHandler _callbackFunction)
        {
            //By standart base abstract platform no need init.
            if (_callbackFunction != null)
                _callbackFunction();
        }

        //// <summary>
        /// Track when mission/level/quest started
        /// </summary>
        public virtual void OpenLevel(int _level)
        {
            //No to do by default
        }

        //// <summary>
        /// Track when mission/level/quest started
        /// </summary>
        public virtual void StartLevel(int _level)
        {
            //No to do by default
        }

        //// <summary>
        /// Track when mission/level/quest failed
        /// </summary>
        public virtual void FailLevel(int _level)
        {
            //No to do by default
        }

        //// <summary>
        /// Track when mission/level/quest completed
        /// </summary>
        public virtual void CompleteLevel(int _level)
        {
            //No to do by default
        }

        //// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about try real payment
        /// </summary>
        public virtual void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //No to do by default
        }

        //// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about sucess real payment
        /// </summary>
        public virtual void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //No to do by default
        }

        //// <summary>
        /// Track business real payment with currency and value
        /// </summary>
        public virtual void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //No to do by default
        }

        //// <summary>
        /// Track resource event
        /// </summary>
        public virtual void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //No to do by default
        }

        //// <summary>
        /// Track resource event
        /// </summary>
        public virtual void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            //No to do by default
        }

        //// <summary>
        /// Track info about invite try
        /// </summary>
        public virtual void InviteTry(string _area)
        {
            //No to do by default
        }

        //// <summary>
        /// Track info about share try
        /// </summary>
        public virtual void ShareTry(string _area)
        {
            //No to do by default
        }

        //// <summary>
        /// Track info about share success
        /// </summary>
        public virtual void ShareSuccess(string _area)
        {
            //No to do by default
        }

        //// <summary>
        /// Track info about request try
        /// </summary>
        public virtual void RequestTry(string _type, string _area)
        {
            //No to do by default
        }

        //// <summary>
        /// Track info about request success
        /// </summary>
        public virtual void RequestSuccess(string _type, string _area)
        {
            //No to do by default
        }

        //// <summary>
        /// Track optional game design event
        /// </summary>
        public virtual void DesignEvent(string _id, int _amount)
        {
            //No to do by default
        }
    }
}