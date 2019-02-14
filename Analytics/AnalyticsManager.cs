using SCore.Loading;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SCore.Analytics
{
    /// <summary>
    /// Controlling analytic system choosing.
    /// AnalyticsManager it's a bridge between Analytic Systems and events from App.
    /// </summary>
    [RequireComponent(typeof(IServiceLoadingStep))]
    public class AnalyticsManager : MonoBehaviour, IAnalyticsManager
    {
        //STATIC
        [Serializable]
        public struct PlatformConfig
        {
            public RuntimePlatform Platform;
            public IAnalyticSystem[] AnalyticsSystems;
        }

        //EVENTS
        public UnityEvent OnInitActions;

        //PUBLIC VARIABLES
        public PlatformConfig[] PlatformConfigs;

        //PRIVATE VARIABLES
        private IAnalyticSystem[] asystems = new IAnalyticSystem[0];

        private int systemInitedCount = 0;
        private bool isInitComplete = false;

        private void Start()
        {
            IAnalyticSystem[] initSystems = new IAnalyticSystem[0];

            foreach (var platformConfig in PlatformConfigs)
            {
                if (platformConfig.Platform == Application.platform)
                    initSystems = platformConfig.AnalyticsSystems;
            }

            if (initSystems.Length == 0)
            {
                Debug.LogWarning("Analytics Manager: not any analytics system setuped for platform : " + Application.platform.ToString());
            }

            Init(initSystems);
        }

        /// <summary>
        /// Async event queues for each system
        /// </summary>
        private void Update()
        {
            foreach (IAnalyticSystem asystem in asystems)
            {
                if (asystem.IsInited && asystem.EventQueue != null && asystem.EventQueue.Count > 0)
                {
                    try
                    {
                        asystem.EventQueue[0]();
                    }
                    catch (Exception e)
                    {
                        Debug.Log("AnalyticsManager Exception", gameObject);
                        Debug.Log(e.ToString());
                        throw;
                    }
                    asystem.EventQueue.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// Only one init can will be called
        /// </summary>
        private void Init(IAnalyticSystem[] _asystems)
        {
            if (!isInitComplete)
            {
                Debug.Log("AnalyticsManager:init", gameObject);
                if (_asystems != null && _asystems.Length > 0)
                {
                    asystems = _asystems;

                    //Pre init event queues
                    foreach (IAnalyticSystem asystem in asystems)
                    {
                        asystem.EventQueue = new List<Action>();
                    }
                    //Init each system
                    foreach (IAnalyticSystem asystem in asystems)
                    {
                        asystem.InitCompletedEvent += OnSystemInitComleted;
                        asystem.InitErrorEvent += OnSystemInitErrorEvent;
                        asystem.Init();
                    }
                }
                else
                {
                    Debug.LogWarning("AnalyticsManager NOT ENABLED", gameObject);
                    OnInitActions?.Invoke();
                }
            }
            else
            {
                Debug.LogError("AnalyticsManager:Repeating static class Init!", gameObject);
            }
        }

        public void OnSystemInitComleted(IAnalyticSystem asystem)
        {
            Debug.Log("AnalyticsManager.OnSystemInitComleted");
            asystem.IsInited = true;
            systemInitedCount++;
            CheckInitCompleted();
        }

        public void OnSystemInitErrorEvent(IAnalyticSystem asystem, string message)
        {
            Debug.Log("AnalyticsManager.OnSystemInitErrorEvent: " + message, gameObject);
            systemInitedCount++;
            CheckInitCompleted();
        }

        private void CheckInitCompleted()
        {
            Debug.Log("AnalyticsManager.CheckInitCompleted", gameObject);
            if (!isInitComplete)
            {
                if (asystems == null || asystems.Length == 0 || systemInitedCount >= asystems.Length)
                {
                    InitCompleted();
                }
            }
        }

        private void InitCompleted()
        {
            if (!isInitComplete)
            {
                Debug.Log("AnalyticsManager.InitCompleted", gameObject);
                isInitComplete = true;
                OnInitActions?.Invoke();
            }
        }

        /// <summary>
        /// Track when first login in social network
        /// </summary>
        public void SocialSignUp()
        {
            Debug.Log("AnalyticsManager.SocialSignUp", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.SocialSignUp();
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when mission/level/quest opened for player
        /// </summary>
        public void OpenLevel(int _level, string _type)
        {
            Debug.Log("AnalyticsManager.OpenMission", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.OpenLevel(_level, _type);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when mission/level/quest started by player
        /// </summary>
        public void StartLevel(int _level, string _type)
        {
            Debug.Log("AnalyticsManager.StartMission", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.StartLevel(_level, _type);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when mission/level/quest failed
        /// </summary>
        public void FailLevel(int _level, string _type, int _score)
        {
            Debug.Log("AnalyticsManager.FailMission", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.FailLevel(_level, _type, _score);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when mission/level/quest completed
        /// </summary>
        public void CompleteLevel(int _level, string _type, int _score)
        {
            Debug.Log("AnalyticsManager.CompleteMission", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.CompleteLevel(_level, _type, _score);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when player get some record or score
        /// </summary>
        public void NewScore(int _level, int _score)
        {
            Debug.Log("AnalyticsManager.PostScore", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.NewScore(_level, _score);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when player get some record or score
        /// </summary>
        public void AchievenemntUnlocked(string _achievementID)
        {
            Debug.Log("AnalyticsManager.AchievenemntUnlocked", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.AchievenemntUnlocked(_achievementID);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when game tutorial started
        /// </summary>
        public void TutorialStart()
        {
            Debug.Log("AnalyticsManager.TutorialStart", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.TutorialStart();
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when game tutorial ended (only one time!)
        /// </summary>
        public void TutorialCompleted()
        {
            Debug.Log("AnalyticsManager.TutorialCompleted", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.TutorialCompleted();
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about try real payment
        /// </summary>
        public void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AnalyticsManager.PaymentInfoTry", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.PaymentInfoTry(_currency, _amount, _itemID, _itemType, _area);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about sucess real payment
        /// </summary>
        public void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AnalyticsManager.PaymentInfoSuccess", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.PaymentInfoSuccess(_currency, _amount, _itemID, _itemType, _area);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track business real payment with currency and value
        /// </summary>
        public void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AnalyticsManager.PaymentReal", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.PaymentReal(_currency, _amount, _itemID, _itemType, _area);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track resource event
        /// </summary>
        public void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AnalyticsManager.ResourceAdd", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.ResourceAdd(_currency, _amount, _itemID, _itemType, _area);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track resource event
        /// </summary>
        public void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AnalyticsManager.ResourceRemove", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.ResourceRemove(_currency, _amount, _itemID, _itemType, _area);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info about invite try
        /// </summary>
        public void InviteTry(string _area)
        {
            Debug.Log("AnalyticsManager.InviteTry", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.InviteTry(_area);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info about share try
        /// </summary>
        public void ShareTry(string _id, string _area)
        {
            Debug.Log("AnalyticsManager.ShareTry", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.ShareTry(_id, _area);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info about share success
        /// </summary>
        public void ShareSuccess(string _id, string _area)
        {
            Debug.Log("AnalyticsManager.ShareSuccess", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.ShareSuccess(_id, _area);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info about request try
        /// </summary>
        public void RequestTry(string _type, string _area)
        {
            Debug.Log("AnalyticsManager.RequestTry", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.RequestTry(_type, _area);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info about request success
        /// </summary>
        public void RequestSuccess(string _type, string _area)
        {
            Debug.Log("AnalyticsManager.RequestSuccess", gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.RequestSuccess(_type, _area);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track optional game design event
        /// </summary>
        public void DesignEvent(string _id, int _amount, Dictionary<string, object> parameters = null)
        {
            Debug.Log("AnalyticsManager.DesignEvent " + _id + " " + _amount.ToString(), gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.DesignEvent(_id, _amount, parameters);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Set optional user property value
        /// </summary>
        public void SetUserStringProperty(string _id, string _value)
        {
            Debug.Log("AnalyticsManager.SetUserStringProperty " + _id + " " + _value, gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.SetUserStringProperty(_id, _value);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Set optional user property value
        /// </summary>
        public void SetUserIntProperty(string _id, int _value)
        {
            Debug.Log("AnalyticsManager.SetUserStringProperty " + _id + " " + _value, gameObject);
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    asystem.EventQueue.Add(() =>
                    {
                        asystem.SetUserIntProperty(_id, _value);
                    });
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception", gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }
    }
}