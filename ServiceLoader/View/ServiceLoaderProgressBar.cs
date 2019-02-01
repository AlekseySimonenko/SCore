using UnityEngine;
using UnityEngine.UI;

namespace SCore.Loading
{
    /// <summary>
    /// Display servic loading information by image bar
    /// </summary>
    [RequireComponent(typeof(ServiceLoader))]
    public class ServiceLoaderProgressBar : MonoBehaviour
    {
        public Image progressBar;

        // Use this for initialization
        private void Start()
        {
            GetComponent<ServiceLoader>().OnSyncStepLoadingEvent += OnSyncStepLoadingEvent;
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void OnSyncStepLoadingEvent(int step, int maxstep)
        {
            if (progressBar != null)
                progressBar.fillAmount = (float)step / (float)maxstep;
        }
    }
}