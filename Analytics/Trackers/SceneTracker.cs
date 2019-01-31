using UnityEngine;
using UnityEngine.SceneManagement;

namespace SCore.Analytics
{
    public class SceneTracker : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            AnalyticsManager.DesignEvent("Scene_" + SceneManager.GetActiveScene().name, 0);
        }
    }
}