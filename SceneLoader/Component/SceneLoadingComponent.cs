using UnityEngine;

namespace SCore.SceneLoading
{
    /// <summary>
    /// Component for loading scene
    /// </summary>
    public class SceneLoadingComponent : MonoBehaviour
    {
        public string sceneLoading;
        public bool fade = false;

        private void Start()
        {
        }

        public void LoadScene()
        {
            SceneLoadingHandler.Instance.LoadScene(sceneLoading, fade);
        }
    }
}