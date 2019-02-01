using System;
using UnityEngine;

namespace SCore.Components
{
    /// <summary>
    /// Activate or deactivate by platform dependency
    /// </summary>
    public class ActiveByPlatform : MonoBehaviour
    {
        public bool activateOnThisPlatforms;
        public RuntimePlatform[] platforms;

        private bool deactivated = false;

        private void Start()
        {
            if (Array.IndexOf(platforms, Application.platform) >= 0)
            {
                if (!activateOnThisPlatforms)
                    deactivated = true;
            }
            else
            {
                if (activateOnThisPlatforms)
                    deactivated = true;
            }
        }

        private void Update()
        {
            if (deactivated && gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }
    }
}