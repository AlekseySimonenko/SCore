using UnityEngine;

namespace SCore
{
    /// <summary>
    /// Singletone language and texts manager
    /// </summary>
    public class WebRequestManager
    {
        #region Public variables

        #endregion

        #region Public constants
        #endregion

        #region Private constants
        #endregion

        #region Private variables

        #endregion

        public static void Request(string _url, Callback.EventHandlerObject successCallbackFunction = null, Callback.EventHandlerObject failCallbackFunction = null, float timeLimitSeconds = 10.0F)
        {
            Debug.Log("WebRequestManager.Request " + _url);

            GameObject requestObject = new GameObject();
            requestObject.AddComponent<WebRequest>();
            WebRequest request = requestObject.GetComponent<WebRequest>() as WebRequest;
            request.url = _url;
            request.callback = successCallbackFunction;
            request.callbackError = failCallbackFunction;
            request.SetTimeLimit(timeLimitSeconds);
        }

    }

}
