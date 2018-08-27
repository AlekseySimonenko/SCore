using UnityEngine;
using UnityEngine.Events;

namespace SCore
{
    /// <summary>
    /// Do something when collider is triggered
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TriggerEvent : MonoBehaviour
    {
        public UnityEvent EnterEvent;

        void OnTriggerEnter(Collider collision)
        {
            EnterEvent?.Invoke();
        }
    }
}