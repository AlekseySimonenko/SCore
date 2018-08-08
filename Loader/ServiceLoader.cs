using System;
using UnityEngine;
using UnityEngine.Events;

namespace SCore
{
    /// <summary>
    /// Control loading any services on game start
    /// </summary>
    public class ServiceLoader : MonoBehaviourSingleton<ServiceLoader>
    {
        /// PUBLIC VARIABLES
        [Header("Synchronous loading")]
        [Tooltip("Steps with complete event")]
        public IServiceLoadingStep[] syncLoadingSteps;
        public event Action<int, int> OnSyncStepLoadingEvent;
        public event Action<int, int> OnAsyncStepLoadingEvent;
        public event Action OnLoadingCompletedEvent;

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

        private System.Diagnostics.Stopwatch stopwatch;

        // Use this for initialization
        void Start()
        {
            Debug.Log("ServiceLoader: Start");
            stopwatch = System.Diagnostics.Stopwatch.StartNew();
            syncLoadingStep = -1;
            NextSyncLoadingStep();
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
            Debug.Log("ServiceLoader: NextSyncLoadingStep " + syncLoadingStep + " on " + (stopwatch.ElapsedMilliseconds / 1000f));

            if (OnSyncStepLoadingEvent != null)
                OnSyncStepLoadingEvent(syncLoadingStep, syncLoadingSteps.Length);

            if (syncLoadingStep < syncLoadingSteps.Length)
            {
                Debug.Log("ServiceLoader: Loading: " + syncLoadingSteps[syncLoadingStep].gameObject.name);
                //Run next step
                IServiceLoadingStep serviceStep = Instantiate(syncLoadingSteps[syncLoadingStep].gameObject, gameObject.transform).GetComponent<IServiceLoadingStep>();
                serviceStep.OnCompleted += OnSyncLoadingStepCompleted;
            }
            else
            {
                Debug.Log("ServiceLoader: SyncLoadingSteps completed");
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
            Debug.Log("ServiceLoader: NextASyncLoadingStep " + asyncLoadingStep + " on " + (stopwatch.ElapsedMilliseconds / 1000f));

            if (OnAsyncStepLoadingEvent != null)
                OnAsyncStepLoadingEvent(asyncLoadingStep, asyncLoadingSteps.Length);

            if (asyncLoadingStep < asyncLoadingSteps.Length)
            {
                Debug.Log("ServiceLoader: Loading: " + asyncLoadingSteps[asyncLoadingStep].gameObject.name);
                //Run next step
                Instantiate(asyncLoadingSteps[asyncLoadingStep], gameObject.transform);
                asyncLoadingStepReady = true;
            }
            else
            {
                Debug.Log("ServiceLoader: finalActions");

                if (OnLoadingCompletedEvent != null)
                    OnLoadingCompletedEvent();

                stopwatch.Stop();
                finalActions.Invoke();
            }
        }

    }

}