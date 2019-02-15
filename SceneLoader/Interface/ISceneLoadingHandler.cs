using System;
using UnityEngine.SceneManagement;

namespace SCore.SceneLoading
{
    public interface ISceneLoadingHandler
    {
        event Action LoadBeginEvent;
        event Action LoadCompletedEvent;

        void LoadScene(string sceneName, bool fadeOverlay = true);
        void OnSceneChanged(Scene previosScene, Scene newScene);
    }
}