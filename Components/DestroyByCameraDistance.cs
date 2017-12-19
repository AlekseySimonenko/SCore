using System.Collections;
using UnityEngine;

namespace SCore
{
    /// <summary>
    /// Destroy or recycle object by distance to camera
    /// </summary>
    public class DestroyByCameraDistance : MonoBehaviour
    {
        public float distance = 100.0F;
        public float checkInterval = 5.0F;
        public bool isRecycle;

        public event Callback.EventHandlerObject DestroyEvent;

        private float startTimer;

        /// <summary>
        /// Only one time
        /// </summary>
        void Start()
        {

        }

        /// <summary>
        /// Every object pool recreated or enable/disable time
        /// </summary>
        void OnEnable()
        {
            startTimer = checkInterval;

            StopAllCoroutines();
            StartCoroutine(UpdateStatus());
        }

        void Update()
        {
            if (startTimer > 0)
                startTimer -= Time.deltaTime;
        }

        IEnumerator UpdateStatus()
        {
            for (; ; )
            {
                if (gameObject.activeInHierarchy && startTimer <= 0)
                {
                    if (Vector3.Distance(transform.position, Camera.main.transform.position) > distance)
                        DestroyObject();
                }
                //Wait next step
                yield return new WaitForSeconds(checkInterval);
            }
        }

        /// <summary>
        /// Destroy or recycle object
        /// </summary>
        public void DestroyObject()
        {
            Debug.Log("DestroyObject");
            if (gameObject.activeInHierarchy)
            {
                StopAllCoroutines();

                if (DestroyEvent != null)
                    DestroyEvent(gameObject);

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
