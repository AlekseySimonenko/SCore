using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deactivate gameobject
/// </summary>
public class Deactivator : MonoBehaviour
{
    public bool deactiveOnStart = false;
    public bool deactiveOnUpdate = false;

    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(!deactiveOnStart);
    }

    // Update is called once per frame
    void Update()
    {
        if (deactiveOnUpdate)
            gameObject.SetActive(!deactiveOnUpdate);
    }
}
