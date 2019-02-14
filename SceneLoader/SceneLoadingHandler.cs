using SCore.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SCore.SceneLoading
{
    /// <summary>
    /// Controlls scene loading process
    /// </summary>
    public class SceneLoadingHandler : MonoBehaviourSingleton<SceneLoadingHandler>
    {
        //PUBLIC STATIC

        //PUBLIC EVENTS
        public event Action LoadBeginEvent;

        public event Action LoadCompletedEvent;

        //PUBLIC VARIABLES
        public CanvasGroup loaderCanvasGroup;

        public float fadeTime = 0.25F;
        public Image processbar;
        public bool hideOnStart = true;

        //PRIVATE STATIC

        //PRIVATE VARIABLES
        private string loadingScene = "";

        private bool loadingFade = false;
        private bool loadingStarted = false;

        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        public void LoadScene(string sceneName, bool fadeOverlay = true)
        {
            Debug.Log("SceneLoadingHandler: LoadScene " + sceneName);
            Instance.loadingScene = sceneName;
            Instance.LoadBeginEvent?.Invoke();
            if (fadeOverlay)
            {
                Instance.loadingFade = true;
                if (Instance.loaderCanvasGroup != null)
                    Instance.loaderCanvasGroup.gameObject.SetActive(true);
            }
            else
            {
                Instance.StartLoading();
            }
        }

        private void Update()
        {
            if (loadingFade)
            {
                //Fade IN
                if (loaderCanvasGroup != null)
                {
                    if (loaderCanvasGroup.alpha < 1.0F)
                    {
                        loaderCanvasGroup.alpha = Mathf.Clamp(loaderCanvasGroup.alpha + (Time.unscaledDeltaTime / fadeTime), 0F, 1F);
                    }
                    else
                    {
                        StartLoading();
                    }
                }
                else
                {
                    StartLoading();
                }
            }
            else
            {
                //Fade OUT
                if (loaderCanvasGroup != null && !loadingStarted && hideOnStart)
                    if (loaderCanvasGroup.alpha > 0F)
                    {
                        loaderCanvasGroup.alpha = Mathf.Clamp(loaderCanvasGroup.alpha - (Time.unscaledDeltaTime / fadeTime), 0F, 1F);
                        if (loaderCanvasGroup.alpha <= 0F)
                            loaderCanvasGroup.gameObject.SetActive(false);
                    }
            }
        }

        private void StartLoading()
        {
            if (!loadingStarted)
            {
                Debug.Log("SceneLoadingHandler: StartLoading");
                loadingStarted = true;
                Instance.StartCoroutine(Instance.HandleLoading(loadingScene));
            }
        }

        // Handle loading
        private IEnumerator HandleLoading(string sceneName)
        {
            Debug.Log("SceneLoadingHandler: HandleLoading " + sceneName);

            if (processbar != null)
            {
                processbar.fillAmount = 0f;
            }

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
            while (async.progress < 0.89)
            {
                if (processbar != null)
                {
                    processbar.fillAmount = async.progress;
                    yield return null;
                }
            }

            if (processbar != null)
            {
                processbar.fillAmount = 1f;
            }
            yield return null;
        }

        public void OnSceneChanged(Scene previosScene, Scene newScene)
        {
            Debug.Log("SceneLoadingHandler: OnSceneChanged");
            if (Instance.loaderCanvasGroup != null)
            {
                loaderCanvasGroup.gameObject.SetActive(!hideOnStart);
                Canvas loaderCanvas = loaderCanvasGroup.GetComponent<Canvas>();
                if (loaderCanvas != null)
                    loaderCanvas.worldCamera = Camera.main;
            }
            hideOnStart = true;
            loadingFade = false;
            loadingStarted = false;
            Instance.LoadCompletedEvent?.Invoke();
        }
    }
}