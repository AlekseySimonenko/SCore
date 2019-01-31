using UnityEngine;

namespace SCore.Utils
{
    public class TargetFrameRate : MonoBehaviour
    {
        public int targetFrameRate = 60;
        public bool inEditor = false;

        // Use this for initialization
        private void Start()
        {
            if (!Application.isEditor || inEditor)
            {
                // Limit the framerate to 60 to keep device from burning through cpu
                Application.targetFrameRate = targetFrameRate;
            }
        }
    }
}