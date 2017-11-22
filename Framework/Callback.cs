using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SCore
{
    public class Callback : MonoBehaviour
    {

        public delegate void EventHandler();
        public delegate void EventHandlerObject(object _object);
        public delegate void EventHandlerTwoObject(object _object, object _object2);

    }
}