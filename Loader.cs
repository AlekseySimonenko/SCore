using UnityEngine;
using Core;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

/// <summary>
/// Preloader class initiated in firts project's scene only one time. 
/// </summary>
public class Loader : MonoBehaviour
{
    //When core loader complete it game object will be activated!
    public GameObject gameLoaderToActivate;
    public string firstSceneID;

    public Platform customPlatform;

    [Header("Final action")]
    public UnityEvent finalActions;

    //This var provide to protect from more that only one loading in app
    private static bool isInitComplete = false;
    //Variable for NextLoadingStep() states
    private static int loadingStep = 0;



    /// <summary>
    /// The entry point for the application.
    /// </summary>
    void Start()
    {
        if (!isInitComplete)
        {
            Debug.Log("Loader:Start");

            //Only one init protect
            isInitComplete = true;

            //Start loading process
            NextLoadingStep();
        }
        else
        {
            Debug.LogError("Loader: Repeating singletone class init!");
        }
    }


    /// <summary>
    /// Itterator of loading steps
    /// </summary>
    private void NextLoadingStep()
    {
        switch (loadingStep)
        {
            case 0:
                if (customPlatform != null)
                    PlatformManager.InitAnyPlatform(customPlatform, onEndLoadingStep);
                else
                    PlatformManager.Init(onEndLoadingStep);
                break;
            case 1:
                LanguageManager.Init(onEndLoadingStep, PlatformManager.GetLanguage());
                break;
            case 2:
                AnalyticsManager.Init(onEndLoadingStep, PlatformManager.GetAnalyticsSystems());
                break;
            case 3:
                //Last step of loading
                Complete();
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// Every loading step ending event handler
    /// </summary>
    private void onEndLoadingStep()
    {
        loadingStep++;
        NextLoadingStep();
    }


    /// <summary>
    /// The end of app loading. We load main game scene after all
    /// </summary>
    private void Complete()
    {
        Debug.Log("Loader:Complete");
        if (gameLoaderToActivate != null)
            gameLoaderToActivate.SetActive(true);

        if (firstSceneID != "")
            SceneManager.LoadSceneAsync(firstSceneID);

        finalActions.Invoke();
    }


}
