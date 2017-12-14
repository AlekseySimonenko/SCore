﻿using System;
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

        public abstract void SocialSignUp();
        public abstract void OpenLevel(int _level);
        public abstract void StartLevel(int _level);
        public abstract void FailLevel(int _level);
        public abstract void CompleteLevel(int _level);
        public abstract void NewScore(int _level, int _score);
        public abstract void AchievenemntUnlocked(string _achievementID);
        public abstract void TutorialStart();
        public abstract void TutorialCompleted();
        public abstract void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area);
        public abstract void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area);
        public abstract void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area);
        public abstract void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area);
        public abstract void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area);
        public abstract void InviteTry(string _area);
        public abstract void ShareTry(string _id, string _area);
        public abstract void ShareSuccess(string _id, string _area);
        public abstract void RequestTry(string _type, string _area);
        public abstract void RequestSuccess(string _type, string _area);
        public abstract void DesignEvent(string _id, int _amount);

    }
}