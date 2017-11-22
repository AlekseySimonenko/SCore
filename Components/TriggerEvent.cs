using UnityEngine;
using UnityEngine.Events;

namespace SCore
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEvent : MonoBehaviour
    {
        public UnityEvent EnterEvent;

        void OnTriggerEnter(Collider collision)
        {
            if (EnterEvent != null)
                EnterEvent.Invoke();
        }
    }
}