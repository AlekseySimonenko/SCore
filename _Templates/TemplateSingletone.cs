using SCore.Framework;
using UnityEngine;
using UnityEngine.Events;

namespace SCore.Templates
{
    /// <summary>
    /// Static class description
    /// </summary>
    public class TemplateSingletone : MonoBehaviourSingleton<TemplateSingletone>
    {
        //STATIC VARIABLES
        public static int PublicStaticVariable;

        private static int _privateStaticVariable;

        //EVENTS
        public UnityEvent Event;

        //PUBLIC VARIABLES
        public static int PublicVariable;

        //PRIVATE VARIABLES
        private int _privateVariable;

        /// <summary>
        /// Function description
        /// </summary>
        private void Start()
        {
            Debug.Log("TemplateSingletone:" + "Start", Instance.gameObject);
        }

        /// <summary>
        /// Function description
        /// </summary>
        private void Update()
        {
        }
    }
}