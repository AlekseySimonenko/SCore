using SCore.ObjectPool;
using System;
using System.Collections;
using UnityEngine;

namespace SCore.Components
{
    /// <summary>
    /// Destroy or recycle object by distance to camera
    /// </summary>
    public class DestroyByCameraDistance : MonoBehaviour
    {
        public float distance = 100.0F;
        public float checkInterval = 5.0F;
        public bool isRecycle;

        public event Action<object> DestroyEvent;

        private float startTimer;

        /// <summary>
        /// Only one time
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// Every object pool recreated or enable/disable time
        /// </summary>
        private void OnEnable()
        {
            startTimer = checkInterval;

            StopAllCoroutines();
            StartCoroutine(UpdateStatus());
        }

        private void Update()
        {
            if (startTimer > 0)
                startTimer -= Time.unscaledDeltaTime;
        }

        private IEnumerator UpdateStatus()
        {
            for (; ; )
            {
                if (gameObject.activeInHierarchy && startTimer <= 0)
                {
                    if (Vector3.Distance(transform.position, Camera.main.transform.position) > distance)
                        DestroyObject();
                }
                //Wait next step
                yield return new WaitForSecondsRealtime(checkInterval);
            }
        }

        /// <summary>
        /// Destroy or recycle object
        /// </summary>
        public void DestroyObject()
        {
            //Debug.Log("DestroyObject");
            if (gameObject.activeInHierarchy)
            {
                StopAllCoroutines();

                DestroyEvent?.Invoke(gameObject);

                if (isRecycle)
                {
                    gameObject.Recycle();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}