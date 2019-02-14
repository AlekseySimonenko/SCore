using SCore.ObjectPool;
using System;
using UnityEngine;
using Zenject;

namespace SCore.Components
{
    /// <summary>
    /// Destroy or recycle object by timer
    /// </summary>
    public class DestroyByTime : MonoBehaviour
    {
        //DEPENDENCIES

        [Inject] private DiContainer _container;

        //EVENTS

        public event Action<object> DestroyEvent;

        //EDITOR VARIABLES

        public float lifetime;
        public bool isRecycle;
        public GameObject destroyEffect;

        //PRIVATE VARIABLES

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
                _container.InstantiatePrefab(destroyEffect, transform.position, new Quaternion(), transform.parent);
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