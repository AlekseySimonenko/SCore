 
using UnityEngine;

namespace SCore
{

    //// <summary>
    /// This behavior attached to dummy object on the scene and configurate Development platform vars
    /// </summary>
    public class PlatformMobileAdnroidConfig : MonoBehaviour
    {

        #region Public vars
        public string currency = "USD";
        public string language = "";

        public IAnalyticSystem[] analytics_systems;

        public string app_id;
        public string server_key;
        public string auth_key;
        public string server_request_url;
        #endregion

    }
}