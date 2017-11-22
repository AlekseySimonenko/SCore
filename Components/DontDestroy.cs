using UnityEngine;

namespace SCore
{

    /// <summary>
    /// DontDestroyOnLoad to this object
    /// </summary>
    public class DontDestroy : MonoBehaviour
    {
        /// <summary>
        /// Only one time
        /// </summary>
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }


    }
}
