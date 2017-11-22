using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class IServiceLoadingStep : MonoBehaviour
    {

        public event Core.Callback.EventHandler OnCompleted;
        public bool autoCompleteEventOnStart = false;
        private bool completed = false;

        // Use this for initialization
        void Start()
        {
            if (autoCompleteEventOnStart)
                CompleteStep();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CompleteStep()
        {
            completed = true;
            if (OnCompleted != null)
            {
                OnCompleted();
                OnCompleted = null;
            }
        }

        public bool IsCompleted()
        {
            return completed;
        }
    }
}
