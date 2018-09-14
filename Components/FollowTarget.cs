using UnityEngine;

namespace SCore
{

    /// <summary>
    /// Object follow another object by some rules
    /// </summary>
    [DisallowMultipleComponent]
    public class FollowTarget : MonoBehaviour
    {

        public GameObject target;
        public float lerpTime = 0;
        public bool unscaledTime = false;

        void Update()
        {
            if (target != null)
            {
                if (lerpTime > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, target.transform.position, (unscaledTime ? Time.deltaTime : Time.unscaledDeltaTime) / lerpTime);
                }
                else
                {
                    transform.position = target.transform.position;
                }
            }
        }

    }
}
