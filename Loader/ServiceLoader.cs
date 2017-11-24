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
        [Header("Synchronous loading")]
        [Tooltip("Steps with complete event")]
        public IServiceLoadingStep[] syncLoadingSteps;

        [Header("Asynchronous loading")]
        [Tooltip("Steps without complete event")]
        public GameObject[] asyncLoadingSteps;

        [Header("Final")]
        public UnityEvent finalActions;

        private int syncLoadingStep;

        // Use this for initialization
        void Start()
        {
            syncLoadingStep = -1;
            NextSyncLoadingStep();
            Debug.Log("ServiceLoader: Start");
        }

        // Update is called once per frame
        void Update()
        {

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
                serviceStep.OnCompleted += NextSyncLoadingStep;
            }
            else
            {
                RunASyncLoadingSteps();
            }
        }

        /// <summary>
        /// Final load all not important services whose can be loaded async.
        /// Run final steps without waiting end of loading!
        /// </summary>
        void RunASyncLoadingSteps()
        {
            Debug.Log("ServiceLoader: RunASyncLoadingSteps");

            foreach (GameObject nextAsyncStep in asyncLoadingSteps)
            {
                Instantiate(nextAsyncStep, gameObject.transform);
            }

            Debug.Log("ServiceLoader: finalActions");
            finalActions.Invoke();
        }


    }

}