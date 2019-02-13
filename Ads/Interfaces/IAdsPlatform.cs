using System;

namespace SCore.Ads
{
    /// <summary>
    /// Interface for ads platform
    /// </summary>
    public interface IAdsPlatform
    {
        event Action AdStarted;

        event Action AdCompleted;

        event Action AdErrorShowed;

        event Action AdCanceled;

        /// <summary>
        /// Init ads platform
        /// </summary>
        void Init();

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
        void ShowInterstitial();

        /// <summary>
        /// Show Rewarded ads that can't be skiped
        /// </summary>
        void ShowRewarded();
    }
}