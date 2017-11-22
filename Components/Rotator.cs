using UnityEngine;

namespace SCore
{
    /// <summary>
    /// Rotate object by axes
    /// </summary>
    public class Rotator : MonoBehaviour
    {
        public float xspeed;
        public float yspeed;
        public float zspeed;
        public bool unscaledTime;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float timeAnimation = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            transform.Rotate(xspeed * timeAnimation, yspeed * timeAnimation, zspeed * timeAnimation);
        }
    }
}
