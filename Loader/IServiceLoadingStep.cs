using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore.Loading
{
    /// <summary>
    /// Every loading step gameobject may contain this component.
    /// IServiceLoadingStep have many options to setup loading behavior.
    /// </summary>
    public class IServiceLoadingStep : MonoBehaviour
    {
        //PUBLIC STATIC

        //PUBLIC EVENTS
        public event Action OnCompleted;

        //PUBLIC VARIABLES
        public bool autoCompleteEventOnStart = false;
        public float InitTimelimit = 5.0F;

        //PRIVATE STATIC

        //PRIVATE VARIABLES
        private float initTimerlimit = 0.0F;
        private bool completed = false;

        // Use this for initialization
        void Start()
        {
            initTimerlimit = InitTimelimit;
        }

        // Update is called once per frame
        void Update()
        {
            if (autoCompleteEventOnStart && !completed)
            {
                Debug.Log("IServiceLoadingStep: AutoComplete " + gameObject.name );
                CompleteStep();
            }
            if (initTimerlimit > 0 && !completed)
            {
                initTimerlimit -= Time.deltaTime;
                if (initTimerlimit <= 0)
                {
                    Debug.Log("IServiceLoadingStep: TimeLimit! " + gameObject.name);
                    CompleteStep();
                }
            }
        }

        public void CompleteStep()
        {
            Debug.Log("IServiceLoadingStep: CompleteStep " + gameObject.name);
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
