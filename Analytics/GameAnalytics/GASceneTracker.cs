using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GASceneTracker : MonoBehaviour
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
#if CORE_GA
        GameAnalyticsSDK.GameAnalytics.NewDesignEvent("Game:Scene:" + SceneManager.GetActiveScene().name);
#endif
    }
}
