using UnityEngine;
using UnityEngine.UI;

namespace SCore.Debuging
{
    public class DebugMemoryCounter : MonoBehaviour
    {
        public Text TextUI;
        public TextMesh TextMesh;

        //FPS метр
        private float updateTimer;

        private const float updateInterval = 1.0f;
        // Use this for initialization

        private void Start()
        {
            TextUI = GetComponent<Text>();
            TextMesh = GetComponent<TextMesh>();
        }

        // Update is called once per frame
        private void Update()
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