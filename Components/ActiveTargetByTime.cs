using System;
using UnityEngine;

namespace SCore
{
    /// <summary>
    /// Activate or deactivate target (linked) gameobject by time
    /// </summary>
    public class ActiveTargetByTime : MonoBehaviour
    {
        public GameObject TargetObject;
        public float activateOnTime = 0;
        public bool deactivate = false;

        void Start()
        {
            Invoke("ActivateObject", activateOnTime);
        }

        void Update()
        {

        }

        void ActivateObject()
        {
            if (TargetObject != null)
            {
                TargetObject.SetActive(!deactivate);
            }
        }
    }
}
