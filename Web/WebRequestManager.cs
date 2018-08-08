using System;
using UnityEngine;

namespace SCore
{
    /// <summary>
    /// Singletone language and texts manager
    /// </summary>
    public class WebRequestManager
    {
        public static void Request(string _url, Action<object> successCallbackFunction = null, Action<object> failCallbackFunction = null, float timeLimitSeconds = 10.0F)
        {
            Debug.Log("WebRequestManager.Request " + _url);

            GameObject requestObject = new GameObject();
            requestObject.AddComponent<DontDestroy>();
            requestObject.AddComponent<WebRequest>();
            WebRequest request = requestObject.GetComponent<WebRequest>() as WebRequest;
            request.url = _url;
            request.callback = successCallbackFunction;
            request.callbackError = failCallbackFunction;
            request.SetTimeLimit(timeLimitSeconds);
        }
    }
}
