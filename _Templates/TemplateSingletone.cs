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
        //STATIC
        public static int PublicStaticVariable;

        private static int _privateStaticVariable;

        private enum EnumConstant { FIRST, SECOND };

        private const int CONSTANT_NAME = 0;

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