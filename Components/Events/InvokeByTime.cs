using UnityEngine;
using UnityEngine.Events;

namespace SCore.Components
{
    public class InvokeByTime : MonoBehaviour
    {
        public float TimeToInvoke;
        public UnityEvent EventsToInvoke;

        // Use this for initialization
        private void Start()
        {
            if (TimeToInvoke == 0f)
                InvokeEvents();
        }

        // Update is called once per frame
        private void Update()
        {
            if (TimeToInvoke > 0f)
            {
                TimeToInvoke -= Time.unscaledDeltaTime;
                if (TimeToInvoke <= 0f)
                    InvokeEvents();
            }
        }

        private void InvokeEvents()
        {
            EventsToInvoke?.Invoke();
        }
    }
}