using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SCore
{
    public class DebugFPSCounter : MonoBehaviour
    {
        public Text TextUI;
        public TextMesh TextMesh;

        //FPS метр
        float updateInterval = 0.5F;
        private float accum = 0.0F; // FPS accumulated over the interval
        private float frames = 0F; // Frames drawn over the interval
        private float timeleft; // Left time for current interval

        // Use this for initialization
        void Start()
        {
            TextUI = GetComponent<Text>();
            TextMesh = GetComponent<TextMesh>();
            timeleft = updateInterval;
        }

        // Update is called once per frame
        void Update()
        {
            //FPS
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;
            if (timeleft <= 0.0F)
            {
                if(TextUI != null)
                    TextUI.text = "FPS " + (accum / frames).ToString("f2");
                if (TextMesh != null)
                    TextMesh.text = "FPS " + (accum / frames).ToString("f2");

                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }



    }
}
