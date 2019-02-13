using System;

namespace SCore.Ads
{
    /// <summary>
    /// AdsManager - controller and mediation between different ads platforms
    /// </summary>
    public interface IAdsManager
    {
        event Action AdStarted;

        event Action AdCompleted;

        event Action AdErrorShowed;

        event Action AdCanceled;

        bool IsEnabled();

        /// <summary>
        /// Is Interstitial ad was loading and ready
        /// </summary>
        bool IsInterstitialReady();

        /// <summary>
        /// Is Interstitial ad was loading and ready
        /// </summary>
        bool IsRewardedReady();

        /// <summary>
        /// Show Interstitial ad that can be skiped
        /// </summary>
        void ShowInterstitial(float timeLimit = 0);

        /// <summary>
        /// Show Rewarded ads that can't be skiped
        /// </summary>
        void ShowRewarded(float timeLimit = 0);
    }
}