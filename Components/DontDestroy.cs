using UnityEngine;

namespace Core
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
