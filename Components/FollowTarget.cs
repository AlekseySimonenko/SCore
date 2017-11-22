using UnityEngine;

namespace Core
{

    //// <summary>
    /// Object follow another object by some rules
    /// </summary>
    [DisallowMultipleComponent]
    public class FollowTarget : MonoBehaviour
    {

        public GameObject target;

        void Update()
        {
            if (target != null)
            {
                transform.position = target.transform.position;
            }
        }

    }
}
