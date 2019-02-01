using UnityEngine;

namespace SCore.Components
{
    /// <summary>
    /// DontDestroyOnLoad to this object
    /// </summary>
    public class DontDestroy : MonoBehaviour
    {
        /// <summary>
        /// Only one time
        /// </summary>
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}