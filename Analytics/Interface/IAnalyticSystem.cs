
using System;
using UnityEngine;

namespace SCore
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
        /// Track first login in social network
        /// </summary>
        public abstract void SocialSignUp();

        //// <summary>
        /// Track when mission/level/quest started
        /// </summary>
        public abstract void OpenLevel(int _level);

        //// <summary>
        /// Track when mission/level/quest started
        /// </summary>
        public abstract void StartLevel(int _level);

        //// <summary>
        /// Track when mission/level/quest failed
        /// </summary>
        public abstract void FailLevel(int _level);

        //// <summary>
        /// Track when mission/level/quest completed
        /// </summary>
        public abstract void CompleteLevel(int _level);

        //// <summary>
        /// Track when game tutorial started
        /// </summary>
        public abstract void TutorialStart();

        //// <summary>
        /// Track when game tutorial ended (only one time!)
        /// </summary>
        public abstract void TutorialCompleted();

        //// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about try real payment
        /// </summary>
        public abstract void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area);

        //// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about sucess real payment
        /// </summary>
        public abstract void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area);

        //// <summary>
        /// Track business real payment with currency and value
        /// </summary>
        public abstract void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area);

        //// <summary>
        /// Track resource event
        /// </summary>
        public abstract void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area);

        //// <summary>
        /// Track resource event
        /// </summary>
        public abstract void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area);

        //// <summary>
        /// Track info about invite try
        /// </summary>
        public abstract void InviteTry(string _area);


        //// <summary>
        /// Track info about share try
        /// </summary>
        public abstract void ShareTry(string _id, string _area);

        //// <summary>
        /// Track info about share success
        /// </summary>
        public abstract void ShareSuccess(string _id, string _area);

        //// <summary>
        /// Track info about request try
        /// </summary>
        public abstract void RequestTry(string _type, string _area);


        //// <summary>
        /// Track info about request success
        /// </summary>
        public abstract void RequestSuccess(string _type, string _area);


        //// <summary>
        /// Track optional game design event
        /// </summary>
        public abstract void DesignEvent(string _id, int _amount);

    }
}