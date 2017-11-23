 using UnityEngine;

namespace SCore
{

    //// <summary>
    /// This behavior attached to dummy object on the scene and configurate Development platform vars
    /// </summary>
    public class PlatformDevConfig : MonoBehaviour
    {

        #region Public vars
        public string currency = "USD";
        public string language = "en";

        public IAnalyticSystem[] analytics_systems;
        #endregion


    }
}