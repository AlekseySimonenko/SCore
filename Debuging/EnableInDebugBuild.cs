using UnityEngine;

namespace SCore.Debuging
{
    /// <summary>
    /// Marks gameobject to be enabled only in debug builds
    /// </summary>
    public class EnableInDebugBuild : MonoBehaviour
    {
        // Use this for initialization
        private void Awake()
        {
            gameObject.SetActive(Debug.isDebugBuild || Application.isEditor);
        }
    }
}