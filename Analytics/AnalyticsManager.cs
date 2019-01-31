﻿using SCore.Framework;
using SCore.Loading;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SCore.Analytics
{
    /// <summary>
    /// Static class controlling analytic system choosing.
    /// AnalyticsManager it's a bridge between private AnalyticSystem class and events from App.
    /// </summary>
    [RequireComponent(typeof(IServiceLoadingStep))]
    public class AnalyticsManager : MonoBehaviourSingleton<AnalyticsManager>
    {
        //PUBLIC STATIC
        [Serializable]
        public struct PlatformConfig
        {
            public RuntimePlatform Platform;
            public IAnalyticSystem[] AnalyticsSystems;
        }

        //PUBLIC EVENTS
        public UnityEvent OnInitActions;

        //PUBLIC VARIABLES
        public PlatformConfig[] PlatformConfigs;

        //TODO remove legacy config
        [Header("LEGACY configs, please use platfromsConfigs instead")]
        public IAnalyticSystem[] androidSystems;

        public IAnalyticSystem[] iosSystems;
        public IAnalyticSystem[] webglSystems;
        public IAnalyticSystem[] editorSystems;
        public IAnalyticSystem[] defaultSystems;

        //PRIVATE STATIC
        private static IAnalyticSystem[] asystems = new IAnalyticSystem[0];

        private static int systemInitedCount = 0;
        private static bool isInitComplete = false;

        //PRIVATE VARIABLES

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

            //Legacy format warning
            if (androidSystems.Length + iosSystems.Length + webglSystems.Length + editorSystems.Length + defaultSystems.Length > 0)
            {
                Debug.LogWarning("Analytics Manager: LEGACY configs, please use platfromsConfigs instead");
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
                        Debug.Log("AnalyticsManager Exception", Instance.gameObject);
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
        private static void Init(IAnalyticSystem[] _asystems)
        {
            if (!isInitComplete)
            {
                Debug.Log("AnalyticsManager:init", Instance.gameObject);
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
                    Debug.LogWarning("AnalyticsManager NOT ENABLED", Instance.gameObject);
                    Instance.OnInitActions?.Invoke();
                }
            }
            else
            {
                Debug.LogError("AnalyticsManager:Repeating static class Init!", Instance.gameObject);
            }
        }

        static public void OnSystemInitComleted(IAnalyticSystem asystem)
        {
            Debug.Log("AnalyticsManager.OnSystemInitComleted");
            asystem.IsInited = true;
            systemInitedCount++;
            CheckInitCompleted();
        }

        static public void OnSystemInitErrorEvent(IAnalyticSystem asystem, string message)
        {
            Debug.Log("AnalyticsManager.OnSystemInitErrorEvent: " + message, Instance.gameObject);
            systemInitedCount++;
            CheckInitCompleted();
        }

        static private void CheckInitCompleted()
        {
            Debug.Log("AnalyticsManager.CheckInitCompleted", Instance.gameObject);
            if (!isInitComplete)
            {
                if (asystems == null || asystems.Length == 0 || systemInitedCount >= asystems.Length)
                {
                    InitCompleted();
                }
            }
        }

        static private void InitCompleted()
        {
            if (!isInitComplete)
            {
                Debug.Log("AnalyticsManager.InitCompleted", Instance.gameObject);
                isInitComplete = true;
                Instance.OnInitActions?.Invoke();
            }
        }

        /// <summary>
        /// Track when first login in social network
        /// </summary>
        static public void SocialSignUp()
        {
            Debug.Log("AnalyticsManager.SocialSignUp", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when mission/level/quest opened for player
        /// </summary>
        static public void OpenLevel(int _level, string _type)
        {
            Debug.Log("AnalyticsManager.OpenMission", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when mission/level/quest started by player
        /// </summary>
        static public void StartLevel(int _level, string _type)
        {
            Debug.Log("AnalyticsManager.StartMission", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when mission/level/quest failed
        /// </summary>
        static public void FailLevel(int _level, string _type, int _score)
        {
            Debug.Log("AnalyticsManager.FailMission", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when mission/level/quest completed
        /// </summary>
        static public void CompleteLevel(int _level, string _type, int _score)
        {
            Debug.Log("AnalyticsManager.CompleteMission", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when player get some record or score
        /// </summary>
        static public void NewScore(int _level, int _score)
        {
            Debug.Log("AnalyticsManager.PostScore", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when player get some record or score
        /// </summary>
        static public void AchievenemntUnlocked(string _achievementID)
        {
            Debug.Log("AnalyticsManager.AchievenemntUnlocked", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when game tutorial started
        /// </summary>
        static public void TutorialStart()
        {
            Debug.Log("AnalyticsManager.TutorialStart", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track when game tutorial ended (only one time!)
        /// </summary>
        static public void TutorialCompleted()
        {
            Debug.Log("AnalyticsManager.TutorialCompleted", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about try real payment
        /// </summary>
        static public void PaymentInfoTry(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AnalyticsManager.PaymentInfoTry", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info (NOT BUSINESS JUST INFO) about sucess real payment
        /// </summary>
        static public void PaymentInfoSuccess(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AnalyticsManager.PaymentInfoSuccess", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track business real payment with currency and value
        /// </summary>
        static public void PaymentReal(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AnalyticsManager.PaymentReal", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track resource event
        /// </summary>
        static public void ResourceAdd(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AnalyticsManager.ResourceAdd", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track resource event
        /// </summary>
        static public void ResourceRemove(string _currency, int _amount, string _itemID, string _itemType, string _area)
        {
            Debug.Log("AnalyticsManager.ResourceRemove", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info about invite try
        /// </summary>
        static public void InviteTry(string _area)
        {
            Debug.Log("AnalyticsManager.InviteTry", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info about share try
        /// </summary>
        static public void ShareTry(string _id, string _area)
        {
            Debug.Log("AnalyticsManager.ShareTry", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info about share success
        /// </summary>
        static public void ShareSuccess(string _id, string _area)
        {
            Debug.Log("AnalyticsManager.ShareSuccess", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info about request try
        /// </summary>
        static public void RequestTry(string _type, string _area)
        {
            Debug.Log("AnalyticsManager.RequestTry", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track info about request success
        /// </summary>
        static public void RequestSuccess(string _type, string _area)
        {
            Debug.Log("AnalyticsManager.RequestSuccess", Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track optional game design event
        /// </summary>
        static public void DesignEvent(string _id, int _amount, Dictionary<string, object> parameters = null)
        {
            Debug.Log("AnalyticsManager.DesignEvent " + _id + " " + _amount.ToString(), Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Set optional user property value
        /// </summary>
        static public void SetUserStringProperty(string _id, string _value)
        {
            Debug.Log("AnalyticsManager.SetUserStringProperty " + _id + " " + _value, Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Set optional user property value
        /// </summary>
        static public void SetUserIntProperty(string _id, int _value)
        {
            Debug.Log("AnalyticsManager.SetUserStringProperty " + _id + " " + _value, Instance.gameObject);
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
                    Debug.Log("AnalyticsManager Exception", Instance.gameObject);
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }
    }
}