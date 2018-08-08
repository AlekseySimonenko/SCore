using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore.Ads
{
    /// <summary>
    /// Interface of ads platforms
    /// </summary>
    [Serializable]
    public abstract class IAdsPlatform : MonoBehaviour
    {
        public abstract event Action StartEvent;
        public abstract event Action CompletedEvent;
        public abstract event Action ErrorEvent;
        public abstract event Action CancelEvent;

        /// <summary>
        /// Init ads platform
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Is Interstitial ad was loading and ready
        /// </summary>
        public abstract bool IsInterstitialReady();

        /// <summary>
        /// Show Interstitial ad that can be skiped
        /// </summary>
        public abstract void ShowInterstitial();


        /// <summary>
        /// Is Interstitial ad was loading and ready
        /// </summary>
        public abstract bool IsRewardedReady();

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
