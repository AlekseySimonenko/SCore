using UnityEngine;

namespace SCore.Framework
{
    /// <summary>
    /// Singleton.
    /// </summary>
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Instance of this object.
        /// </summary>
        private static T instance;

        private void Awake()
        {
            if (instance == null)
                instance = this.GetComponent<T>();
        }

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;

                var component = FindObjectOfType(typeof(T));
                if (component == null)
                {
                    instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                    Debug.LogWarning("No instance of " + typeof(T) + " add new on scene!");
                }
                else
                {
                    instance = component as T;
                }

                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) + " is not found or can't be created !!!");
                }

                return instance;
            }
        }

        public static void Delete()
        {
            Debug.Log("Deleting MonoBehaviourSingleton of type: " + typeof(T));
            instance = default(T);
        }

        public static bool IsNotNull()
        {
            return instance != null;
        }
    }
}