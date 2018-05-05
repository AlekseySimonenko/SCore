using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore
{
    public class IServiceLoadingStep : MonoBehaviour
    {

        public event Callback.EventHandler OnCompleted;
        public bool autoCompleteEventOnStart = false;
        public float InitTimelimit = 5.0F;
        private float initTimerlimit = 0.0F;

        private bool completed = false;

        // Use this for initialization
        void Start()
        {
            Debug.Log("IServiceLoadingStep: Start");
            initTimerlimit = InitTimelimit;
        }

        // Update is called once per frame
        void Update()
        {
            if (autoCompleteEventOnStart && !completed)
            {
                Debug.Log("IServiceLoadingStep: autoCompleteEventOnStart");
                CompleteStep();
            }
            if (initTimerlimit > 0 && !completed)
            {
                initTimerlimit -= Time.deltaTime;
                if (initTimerlimit <= 0)
                {
                    CompleteStep();
                }
            }
        }

        public void CompleteStep()
        {
            Debug.Log("IServiceLoadingStep: CompleteStep");
            if (!completed)
            {
                completed = true;
                if (OnCompleted != null)
                {
                    OnCompleted();
                    OnCompleted = null;
                }
            }
        }

        public bool IsCompleted()
        {
            return completed;
        }
    }
}
