using UnityEngine;

namespace SCore.Components
{
    /// <summary>
    /// Activate or deactivate target (linked) gameobject by time
    /// </summary>
    public class ActiveTargetByTime : MonoBehaviour
    {
        public GameObject TargetObject;
        public float activateOnTime = 0;
        public bool deactivate = false;

        private void Start()
        {
            Invoke("ActivateObject", activateOnTime);
        }

        private void Update()
        {
        }

        private void ActivateObject()
        {
            if (TargetObject != null)
            {
                TargetObject.SetActive(!deactivate);
            }
        }
    }
}