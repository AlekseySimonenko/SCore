using System;
using UnityEngine;

namespace SCore
{
    /// <summary>
    /// Destroy or recycle object by timer
    /// </summary>
    public class DestroyByTime : MonoBehaviour
    {
        public float lifetime;
        public bool isRecycle;
        public GameObject destroyEffect;

        public event Action<object> DestroyEvent;

        [HideInInspector]
        [SerializeField]
        private float timer = 0.0f;

        /// <summary>
        /// Only one time
        /// </summary>
        void Start()
        {
            Init();
        }

        /// <summary>
        /// Every object pool recreated or enable/disable time
        /// </summary>
        void OnEnable()
        {
            Init();
        }

        /// <summary>
        /// Object like new!
        /// </summary>
        void Init()
        {
            timer = lifetime;
        }

        void Update()
        {
            if (timer > 0.0F)
            {
                timer -= Time.deltaTime;
                if (timer <= 0.0f)
                {
                    DestroyObject();
                }
            }

        }

        /// <summary>
        /// Destroy or recycle object
        /// </summary>
        public void DestroyObject()
        {
            if (destroyEffect != null)
            {
                Instantiate(destroyEffect, transform.position, new Quaternion());
            }

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
