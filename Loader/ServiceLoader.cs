using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SCore
{
    //// <summary>
    /// Control loading any services on game start
    /// </summary>
    public class ServiceLoader : MonoBehaviour
    {

        /// PUBLIC VARIABLES
        [Header("Synchronous loading")]
        [Tooltip("Steps with complete event")]
        public IServiceLoadingStep[] syncLoadingSteps;

        [Header("Asynchronous loading")]
        [Tooltip("Steps without complete event")]
        public GameObject[] asyncLoadingSteps;

        [Header("Final")]
        public UnityEvent finalActions;

        /// PUBLIC CONSTANTS

        /// PRIVATE CONSTANTS

        /// PRIVATE VARIABLES
        private int syncLoadingStep;
        private bool syncLoadingStepReady;

        private int asyncLoadingStep;
        private bool asyncLoadingStepReady;

        // Use this for initialization
        void Start()
        {
            syncLoadingStep = -1;
            NextSyncLoadingStep();
            Debug.Log("ServiceLoader: Start");
        }

        // Update is called once per frame
        //We used that to prevent from frame locked on low devices with only one work thread
        void Update()
        {
            //Sync steps
            if (syncLoadingStepReady)
            {
                syncLoadingStepReady = false;
                NextSyncLoadingStep();
            }
            //ASync steps
            if (asyncLoadingStepReady)
            {
                asyncLoadingStepReady = false;
                NextASyncLoadingStep();
            }
        }

        /// <summary>
        /// First load all important services step by step
        /// </summary>
        void NextSyncLoadingStep()
        {
            syncLoadingStep++;
            Debug.Log("ServiceLoader: NextSyncLoadingStep " + syncLoadingStep);

            if (syncLoadingStep < syncLoadingSteps.Length)
            {
                //Run next step
                IServiceLoadingStep serviceStep = Instantiate(syncLoadingSteps[syncLoadingStep].gameObject, gameObject.transform).GetComponent<IServiceLoadingStep>();
                serviceStep.OnCompleted += OnSyncLoadingStepCompleted;
            }
            else
            {
                asyncLoadingStep = -1;
                NextASyncLoadingStep();
            }
        }

        public void OnSyncLoadingStepCompleted()
        {
            syncLoadingStepReady = true;
        }

        /// <summary>
        /// Final load all not important services whose can be loaded async.
        /// Run final steps without waiting end of loading!
        /// </summary>
        void NextASyncLoadingStep()
        {
            asyncLoadingStep++;
            Debug.Log("ServiceLoader: NextASyncLoadingStep " + asyncLoadingStep);

            if (asyncLoadingStep < asyncLoadingSteps.Length)
            {
                //Run next step
                Instantiate(asyncLoadingSteps[asyncLoadingStep], gameObject.transform);
                asyncLoadingStepReady = true;
            }
            else
            {
                Debug.Log("ServiceLoader: finalActions");
                finalActions.Invoke();
            }
        }

    }

}