using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SCore
{
    public class SceneTracker : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            AnalyticsManager.DesignEvent("Scene_" + SceneManager.GetActiveScene().name, 0);
        }
    }
}
