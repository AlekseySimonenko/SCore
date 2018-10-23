using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMaterial : MonoBehaviour
{

    public string MaterialProperty = "";
    public Material targetMaterial;
    public float AnimationSpeed = 1f;
    public bool UnscaledTime;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrEmpty(MaterialProperty) && targetMaterial != null)
        {
            targetMaterial.SetFloat(MaterialProperty, UnscaledTime ? Time.unscaledTime * AnimationSpeed : Time.time * AnimationSpeed);
        }
    }
}
