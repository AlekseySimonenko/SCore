using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCore.Analytics
{
    /// <summary>
    /// Abstract class for Monobehavior fields polymorphism realisation
    /// Base abstract class for analytics platforms.
    /// </summary>
    [Serializable]
    public abstract class IAnalyticSystem : MonoBehaviour
    {
        public abstract event Action<IAnalyticSystem> InitCompletedEvent;
        public abstract event Action<IAnalyticSystem, string> InitErrorEvent;

        [HideInInspector]
        public bool IsInited;
        [HideInInspector]
        public List<Action> EventQueue;

        public abstract void Init();
        public abstract void SocialSignUp();
        public abstract void OpenLevel(int _level, string _type);
        public abstract void StartLevel(int _level, string _type);
        public abstract void FailLevel(int _level, string _type, int _score);
        public abstract void CompleteLevel(int _level, string _type, int _score);
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
        public abstract void DesignEvent(string _id, Dictionary<string, object> parameters);

    }



}