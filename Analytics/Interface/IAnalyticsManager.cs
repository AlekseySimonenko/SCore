using System.Collections.Generic;

namespace SCore.Analytics
{
    public interface IAnalyticsManager
    {
        void AchievenemntUnlocked(string _achievementID);
        void CompleteLevel(int _level, string _type, int _score);
        void DesignEvent(string _id, int _amount, Dictionary<string, object> parameters = null);
        void FailLevel(int _level, string _type, int _score);
        void InviteTry(string _area);
        void NewScore(int _level, int _score);
        void OnSystemInitComleted(IAnalyticSystem asystem);
        void OnSystemInitErrorEvent(IAnalyticSystem asystem, string message);
        void OpenLevel(int _level, string _type);
        void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area);
        void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area);
        void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area);
        void RequestSuccess(string _type, string _area);
        void RequestTry(string _type, string _area);
        void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area);
        void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area);
        void SetUserIntProperty(string _id, int _value);
        void SetUserStringProperty(string _id, string _value);
        void ShareSuccess(string _id, string _area);
        void ShareTry(string _id, string _area);
        void SocialSignUp();
        void StartLevel(int _level, string _type);
        void TutorialCompleted();
        void TutorialStart();
    }
}