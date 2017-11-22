using UnityEngine;

namespace SCore
{
    /// <summary>
    /// Follow rotation to look target by time/speed
    /// </summary>
    [DisallowMultipleComponent]
    public class FollowRotationToTarget : MonoBehaviour
    {
        public GameObject targetPosition;
        public GameObject targetRotation;
        public float angleSnap = 30.0F;
        public float speed = 5.0F;

        void Start()
        {

        }

        void Update()
        {
            if (targetPosition != null)
                gameObject.transform.position = targetPosition.transform.position;

            if (targetRotation != null)
            {
                float angle = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, targetRotation.transform.rotation.eulerAngles.y);
                if (Mathf.Abs(angle) > angleSnap)
                {
                    transform.Rotate(Vector3.up, speed * angle * Time.deltaTime);
                }
            }
        }
    }
}

