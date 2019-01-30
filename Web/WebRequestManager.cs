using System;
using UnityEngine;

namespace SCore.Web
{
    /// <summary>
    /// Singletone static web requests manager
    /// </summary>
    public class WebRequestManager
    {

        /// <summary>
        /// Static method for web request processing
        /// </summary>
        public static void Request(string _url, Action<object> successCallbackFunction = null, Action<object> failCallbackFunction = null, float timeLimitSeconds = 10.0F)
        {
            Debug.Log("WebRequestManager.Request " + _url);

            //Construct new gameobject for corountine usage
            GameObject requestObject = new GameObject();
            DontDestroyOnLoad(requestObject);

            //Construct new gameobject for corountine usage
            WebRequest request = requestObject.AddComponent<WebRequest>();
            request.url = _url;
            request.callback = successCallbackFunction;
            request.callbackError = failCallbackFunction;
            request.SetTimeLimit(timeLimitSeconds);
        }
    }
}
