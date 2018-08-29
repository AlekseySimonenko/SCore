﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if CORE_FIREBASE
using System.Threading.Tasks;
#endif

namespace SCore.FirebaseSDK
{
    public class FirebaseGPServicesResolver
    {
        static public event Action ServicesAvailableEvent;
        static public event Action ServicesErrorEvent;
        static private bool available;
        static private bool checkRunning;

        // Use this for initialization
        static public bool IsAvaliable()
        {
            if (!available)
            {
#if CORE_FIREBASE
                if (!checkRunning)
                {
                    checkRunning = true;
                    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                    {
                        var dependencyStatus = task.Result;
                        if (dependencyStatus == Firebase.DependencyStatus.Available)
                        {
                            available = true;
                            ServicesAvailableEvent?.Invoke();
                        // Create and hold a reference to your FirebaseApp, i.e.
                        //   app = Firebase.FirebaseApp.DefaultInstance;
                        // where app is a Firebase.FirebaseApp property of your application class.

                        // Set a flag here indicating that Firebase is ready to use by your
                        // application.
                    }
                        else
                        {
                            UnityEngine.Debug.LogError(System.String.Format(
                              "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                        // Firebase Unity SDK is not safe to use here.
                        ServicesErrorEvent?.Invoke();
                        }
                    });
                }
#endif
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}