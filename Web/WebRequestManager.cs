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
        public static void Request(string _url,
            Action<object> successCallbackFunction = null,
            Action<object> failCallbackFunction = null,
            float timeLimitSeconds = 10.0F,
            [System.Runtime.CompilerServices.CallerMemberName] string callerName = "")
        {
            Debug.LogFormat("WebRequestManager.Request url:{0} caller{1}", _url, callerName);

            //Construct new gameobject for corountine usage
            GameObject requestObject = new GameObject();
            GameObject.DontDestroyOnLoad(requestObject);

            //Construct new gameobject for corountine usage
            WebRequest request = requestObject.AddComponent<WebRequest>();
            request.url = _url;
            request.callback = successCallbackFunction;
            request.callbackError = failCallbackFunction;
            request.SetTimeLimit(timeLimitSeconds);
        }
    }
}