using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SCore.Analytics
{
    public class SceneTracker : MonoBehaviour
    {
        //DEPENDENCIES

        [Inject]
        private IAnalyticsManager _analyticsManager;

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
            _analyticsManager.DesignEvent("Scene_" + SceneManager.GetActiveScene().name, 0);
        }
    }
}