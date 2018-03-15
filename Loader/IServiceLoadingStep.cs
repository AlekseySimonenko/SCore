using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore
{
    public class IServiceLoadingStep : MonoBehaviour
    {

        public event Callback.EventHandler OnCompleted;
        public bool autoCompleteEventOnStart = false;
        private bool completed = false;

        // Use this for initialization
        void Start()
        {
            Debug.Log("IServiceLoadingStep: Start");
        }

        // Update is called once per frame
        void Update()
        {
            if (autoCompleteEventOnStart && !completed)
            {
                Debug.Log("IServiceLoadingStep: autoCompleteEventOnStart");
                CompleteStep();
            }
        }

        public void CompleteStep()
        {
            Debug.Log("IServiceLoadingStep: CompleteStep");
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
