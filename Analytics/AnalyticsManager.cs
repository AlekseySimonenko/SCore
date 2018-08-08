using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SCore
{
    /// <summary>
    /// Static class controlling analytic system choosing. 
    /// AnalyticsManager it's a bridge between private AnalyticSystem class and events from App.
    /// </summary>
    [RequireComponent(typeof(IServiceLoadingStep))]
    public class AnalyticsManager : MonoBehaviourSingleton<AnalyticsManager>
    {
        [Header("Android platform Analytics")]
        public IAnalyticSystem[] androidSystems;
        [Header("iOS platform Analytics")]
        public IAnalyticSystem[] iosSystems;
        [Header("WebGL platform Analytics")]
        public IAnalyticSystem[] webglSystems;
        [Header("Editor platform Analytics")]
        public IAnalyticSystem[] editorSystems;
        [Header("Default platform Analytics")]
        public IAnalyticSystem[] defaultSystems;

        public UnityEvent OnInitActions;
        
        private static IAnalyticSystem[] asystems = new IAnalyticSystem[ 0 ];
        private static int systemInitedCount = 0;
        private static bool isInitComplete = false;


        private void Start()
        {
            IAnalyticSystem[] initSystems = new IAnalyticSystem[ 0 ];

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    initSystems = editorSystems;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    initSystems = iosSystems;
                    break;
                case RuntimePlatform.Android:
                    initSystems = androidSystems;
                    break;
                case RuntimePlatform.WebGLPlayer:
                    initSystems = webglSystems;
                    break;
                default:
                    initSystems = defaultSystems;
                    break;
            }
            Init(initSystems);
        }


        /// <summary>
        /// Only one init can will be called
        /// </summary>
        private static void Init(IAnalyticSystem[] _asystems)
        {
            if (!isInitComplete)
            {
                Debug.Log("AnalyticsManager:init");
                if (_asystems != null && _asystems.Length > 0)
                {
                    asystems = _asystems;

                    foreach (IAnalyticSystem asystem in asystems)
                    {
                        asystem.InitCompletedEvent += OnSystemInitComleted;
                        asystem.InitErrorEvent += OnSystemInitErrorEvent;
                        asystem.Init();
                    }
                }
                else
                {
                    Debug.LogWarning("AnalyticsManager NOT ENABLED");
                    if (Instance.OnInitActions != null)
                        Instance.OnInitActions.Invoke();
                }

            }
            else
            {
                Debug.LogError("AnalyticsManager:Repeating static class Init!");
            }
        }


        static public void OnSystemInitComleted(IAnalyticSystem asystem)
        {
            Debug.Log("AnalyticsManager.OnSystemInitComleted");
            asystem.isInited = true;
            systemInitedCount++;
            CheckInitCompleted();
        }

        static public void OnSystemInitErrorEvent(IAnalyticSystem asystem, string message)
        {
            Debug.Log("AnalyticsManager.OnSystemInitErrorEvent: " + message);
            systemInitedCount++;
            CheckInitCompleted();
        }

        static private void CheckInitCompleted()
        {
            Debug.Log("AnalyticsManager.CheckInitCompleted");
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
                    Debug.Log("AnalyticsManager.InitCompleted");
                    isInitComplete = true;
                    if (Instance.OnInitActions != null)
                        Instance.OnInitActions.Invoke();
            }
        }






        /// <summary>
        /// Track when first login in social network
        /// </summary>
        static public void SocialSignUp()
        {
            Debug.Log("AnalyticsManager.SocialSignUp");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.SocialSignUp();
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.OpenMission");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.OpenLevel(_level, _type);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.StartMission");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.StartLevel(_level, _type);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.FailMission");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.FailLevel(_level, _type, _score);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.CompleteMission");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.CompleteLevel(_level, _type, _score);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.PostScore");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.NewScore(_level, _score);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.AchievenemntUnlocked");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.AchievenemntUnlocked(_achievementID);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.TutorialStart");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.TutorialStart();
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.TutorialCompleted");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.TutorialCompleted();
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.PaymentInfoTry");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.PaymentInfoTry(_currency, _amount, _itemID, _itemType, _area);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.PaymentInfoSuccess");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.PaymentInfoSuccess(_currency, _amount, _itemID, _itemType, _area);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.PaymentReal");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.PaymentReal(_currency, _amount, _itemID, _itemType, _area);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.ResourceAdd");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.ResourceAdd(_currency, _amount, _itemID, _itemType, _area);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.ResourceRemove");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.ResourceRemove(_currency, _amount, _itemID, _itemType, _area);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.InviteTry");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.InviteTry(_area);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.ShareTry");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.ShareTry(_id, _area);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.ShareSuccess");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.ShareSuccess(_id, _area);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.RequestTry");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.RequestTry(_type, _area);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
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
            Debug.Log("AnalyticsManager.RequestSuccess");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.RequestSuccess(_type, _area);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track optional game design event
        /// </summary>
        static public void DesignEvent(string _id, int _amount)
        {
            Debug.Log("AnalyticsManager.DesignEvent");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.DesignEvent(_id, _amount);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Track optional game design event
        /// </summary>
        static public void DesignEvent(string _id, Dictionary<string, object> parameters)
        {
            Debug.Log("AnalyticsManager.DesignEvent");
            foreach (IAnalyticSystem asystem in asystems)
            {
                try
                {
                    if (asystem.isInited)
                        asystem.DesignEvent(_id, parameters);
                }
                catch (Exception e)
                {
                    Debug.Log("AnalyticsManager Exception");
                    Debug.Log(e.ToString());
                    throw;
                }
            }
        }


    }

}