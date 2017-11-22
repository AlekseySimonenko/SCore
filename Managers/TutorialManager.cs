using UnityEngine;
using System.Collections;
using Core;
using System.Collections.Generic;

/// <summary>
/// Static class managing tutorial steps queue
/// </summary>
public class TutorialManager
{
    #region Public var
    #endregion

    #region Public const
    #endregion

    #region Private const
    #endregion

    #region Private var
    private static List<TutorialStep> queue = new List<TutorialStep>();
    private static Transform canvas;
    private static GameObject hintPrefab;
    private static GameObject hintSelecterPrefab;
    #endregion

    /// <summary>
    /// Create new step and add it in the end of queue
    /// </summary>
    public static void Init(Transform _canvas, GameObject _hintPrefab, GameObject _hintSelecterPrefab)
    {
        canvas = _canvas;
        hintPrefab = _hintPrefab;
        hintSelecterPrefab = _hintSelecterPrefab;
    }

    /// <summary>
    /// Create new step and add it in the end of queue
    /// </summary>
    public static TutorialStep AddNewStep(string _text, int _timer = 0, int _offsetPosY = 0, int _offsetPosX = 0, float _invokedActivateTime = 0.0F, GameObject _target = null)
    {
        GameObject stepObject = new GameObject();
        stepObject.AddComponent<TutorialStep>();
        TutorialStep newStep = stepObject.GetComponent<TutorialStep>() as TutorialStep;

        newStep.Init(canvas, hintPrefab, hintSelecterPrefab, _text, _timer, _offsetPosY, _offsetPosX, _invokedActivateTime, _target);
        newStep.ActivateEvent += UpdateQueue;
        newStep.DeleteEvent += DeleteStep;
        queue.Add(newStep);

        return newStep;
    }

    //// <summary>
    /// Update queue listener
    /// </summary>
    public static void UpdateQueue()
    {
        TutorialStep step;
        for (int i = 0; i < queue.Count; i++)
        {
            step = queue[i] as TutorialStep;
            if (step.activated == true)
            {
                if (step.showed == false)
                {
                    step.Show();
                    queue.Remove(step);
                    queue.Insert(0, step);
                }
                break;
            }
        }

    }

    /// <summary>
    /// Delete step and remove it from queue and listeners
    /// </summary>
    public static void DeleteStep(object _object)
    {
        TutorialStep step = _object as TutorialStep;
        if (queue.Contains(step))
        {
            queue.Remove(step);
            step.ActivateEvent -= UpdateQueue;
            step.DeleteEvent -= DeleteStep;
            step.Destroy();

            UpdateQueue();
        }
    }

}