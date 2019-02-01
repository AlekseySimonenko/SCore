using UnityEngine;
using UnityEngine.Events;

namespace SCore.Components
{
    /// <summary>
    /// Do something when collider is triggered
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TriggerEvent : MonoBehaviour
    {
        public UnityEvent EnterEvent;

        private void OnTriggerEnter(Collider collision)
        {
            EnterEvent?.Invoke();
        }
    }
}