using System;
using System.Collections;
using UnityEngine;

namespace SCore.Web
{

    /// <summary>
    /// This behavior attached to dummy object on the scene. 
    /// When  get response this object self-destroyed.
    /// </summary>
    public class WebRequest : MonoBehaviour
    {
        public string url;
        public Action<object> callback;
        public Action<object> callbackError;

        private WWW www;
        private float timeLimit;

        void Start()
        {
            Debug.Log("RequestWeb Start");
            www = new WWW(url);
            StartCoroutine(WaitForRequest(www));
        }

        private void Update()
        {
            if (timeLimit > 0.0F)
            {
                timeLimit -= Time.unscaledDeltaTime;
                if (timeLimit <= 0.0F)
                {
                    callbackError("WWW Request time limit");
                    DestroyRequest();
                }
            }
        }

        public void SetTimeLimit(float _timeLimitSeconds)
        {
            timeLimit = _timeLimitSeconds;
        }

        IEnumerator WaitForRequest(WWW www)
        {
            yield return www;

            timeLimit = 0.0F;
            if (www.error == null)
            {
                Debug.Log("WWW Response: " + www.text);
                callback?.Invoke(www);
            }
            else
            {
                Debug.LogError("WWW Error: " + www.error);
                callbackError?.Invoke(www.error);
            }

            DestroyRequest();
        }

        private void DestroyRequest()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}