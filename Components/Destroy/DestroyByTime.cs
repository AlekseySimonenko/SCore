using SCore.ObjectPool;
using System;
using UnityEngine;

namespace SCore.Components
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
        private void Start()
        {
            Init();
        }

        /// <summary>
        /// Every object pool recreated or enable/disable time
        /// </summary>
        private void OnEnable()
        {
            Init();
        }

        /// <summary>
        /// Object like new!
        /// </summary>
        private void Init()
        {
            timer = lifetime;
        }

        private void Update()
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