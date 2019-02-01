using System;
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
        private void Start()
        {
            initTimerlimit = InitTimelimit;
        }

        // Update is called once per frame
        private void Update()
        {
            if (autoCompleteEventOnStart && !completed)
            {
                CompleteStep("AutoComplete");
            }
            if (initTimerlimit > 0 && !completed)
            {
                initTimerlimit -= Time.unscaledDeltaTime;
                if (initTimerlimit <= 0)
                {
                    CompleteStep("TimeLimit");
                }
            }
        }

        public void CompleteStep()
        {
            CompleteStep("");
        }

        public void CompleteStep(string _reason = "")
        {
            Debug.Log("IServiceLoadingStep: CompleteStep " + _reason + " " + gameObject.name, gameObject);
            if (!completed)
            {
                completed = true;
                OnCompleted?.Invoke();
                OnCompleted = null;
            }
        }

        public bool IsCompleted()
        {
            return completed;
        }
    }
}