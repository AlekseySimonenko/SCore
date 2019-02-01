using UnityEngine;

namespace SCore.Components
{
    /// <summary>
    /// Deactivate gameobject
    /// </summary>
    public class Deactivator : MonoBehaviour
    {
        public bool deactiveOnStart = false;
        public bool deactiveOnUpdate = false;

        // Use this for initialization
        private void Start()
        {
            gameObject.SetActive(!deactiveOnStart);
        }

        // Update is called once per frame
        private void Update()
        {
            if (deactiveOnUpdate)
                gameObject.SetActive(!deactiveOnUpdate);
        }
    }
}