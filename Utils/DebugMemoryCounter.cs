using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SCore
{
    public class DebugMemoryCounter : MonoBehaviour
    {
        public Text TextUI;
        public TextMesh TextMesh;

        //FPS метр
        private float updateTimer;
        private const float updateInterval = 1.0f;
        // Use this for initialization

        void Start()
        {
            TextUI = GetComponent<Text>();
            TextMesh = GetComponent<TextMesh>();
        }

        // Update is called once per frame
        void Update()
        {
            updateTimer -= Time.unscaledDeltaTime;
            if (updateTimer < 0)
            {
                updateTimer = updateInterval;
                if (TextUI != null)
                    TextUI.text = "MB " + (UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / 1000000).ToString();
                if (TextMesh != null)
                    TextMesh.text = "MB " + (UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / 1000000).ToString();
            }
        }



    }
}
