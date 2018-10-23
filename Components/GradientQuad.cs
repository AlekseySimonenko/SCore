using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class GradientQuad : MonoBehaviour
{
    public Color topColor = Color.blue;
    public Color topColor2 = Color.blue;
    public Color bottomColor = Color.white;
    public Color bottomColor2 = Color.white;

    private MeshFilter meshQuadComponent;

    void Awake()
    {
        meshQuadComponent = GetComponent<MeshFilter>();
        ResetToBaseColor();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            ResetToBaseColor();
        }
    }

    public void ResetToBaseColor()
    {
        UpdateColor(topColor, topColor2, bottomColor, bottomColor2);
    }

    public void UpdateColor(Color _topLeft, Color _topRight, Color _downLeft, Color _downRight)
    {
        if (meshQuadComponent != null)
        {
            Mesh mesh = meshQuadComponent.sharedMesh;
            Vector3[] vertices = mesh.vertices;

            // create new colors array where the colors will be created.
            Color[] colors = new Color[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                if (i == 0)
                    colors[i] = _downLeft;
                if (i == 1)
                    colors[i] = _topRight;
                if (i == 2)
                    colors[i] = _downRight;
                if (i == 3)
                    colors[i] = _topLeft;
            }

            // assign the array of colors to the Mesh.
            meshQuadComponent.sharedMesh.colors = colors;
        }
    }


}

