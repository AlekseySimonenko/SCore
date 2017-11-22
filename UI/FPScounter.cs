using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SCore
{
    public class FPScounter : MonoBehaviour
    {
        public Text fpsTextUI;
        public TextMesh fpsText3D;

        //FPS метр
        private Text fpsText;
        float updateInterval = 0.5F;
        private float accum = 0.0F; // FPS accumulated over the interval
        private float frames = 0F; // Frames drawn over the interval
        private float timeleft; // Left time for current interval

        // Use this for initialization
        void Start()
        {
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
                if(fpsTextUI != null)
                    fpsTextUI.text = "FPS " + (accum / frames).ToString("f2");
                if (fpsText3D != null)
                    fpsText3D.text = "FPS " + (accum / frames).ToString("f2");

                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }



    }
}
