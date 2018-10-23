using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeByTime : MonoBehaviour
{

    public float TimeToInvoke;
    public UnityEvent EventsToInvoke;

    // Use this for initialization
    void Start()
    {
        if (TimeToInvoke == 0f)
            InvokeEvents();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeToInvoke > 0f)
        {
            TimeToInvoke -= Time.unscaledDeltaTime;
            if (TimeToInvoke <= 0f)
                InvokeEvents();
        }
    }

    void InvokeEvents()
    {
        EventsToInvoke?.Invoke();
    }
}
