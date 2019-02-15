using UnityEngine;
using Zenject;

namespace SCore.SceneLoading
{
    /// <summary>
    /// Component for loading scene
    /// </summary>
    public class SceneLoadingComponent : MonoBehaviour
    {
        //DEPENDENCIES

        [Inject] private ISceneLoadingHandler _sceneLoadingHandler;

        public string sceneLoading;
        public bool fade = false;

        private void Start()
        {
        }

        public void LoadScene()
        {
            _sceneLoadingHandler.LoadScene(sceneLoading, fade);
        }
    }
}