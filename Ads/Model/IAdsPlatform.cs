﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore
{
    //// <summary>
    /// Interface of ads platforms
    /// </summary>
    [Serializable]
    public abstract class IAdsPlatform : MonoBehaviour
    {
        public abstract event Callback.EventHandler StartEvent;
        public abstract event Callback.EventHandler CompletedEvent;
        public abstract event Callback.EventHandler ErrorEvent;

        /// <summary>
        /// Show Interstitial ad that can be skiped
        /// </summary>
        public abstract void ShowInterstitial();

        /// <summary>
        /// Show Rewarded ads that can't be skiped
        /// </summary>
        public abstract void ShowRewarded();

        /// <summary>
        /// Show Rewarded ads that can't be skiped
        /// </summary>
        public abstract void ShowBanner();

    }

}