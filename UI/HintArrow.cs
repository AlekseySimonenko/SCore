using UnityEngine;


namespace Core
{
    [RequireComponent(typeof(RectTransform))]
    public class HintArrow : MonoBehaviour
    {

        public GameObject arrowImage;
        [Range(0.0f, 90.0f)]
        public float minRotation = 0.0f;
        [Range(90.0f, 180.0f)]
        public float maxRotation = 180.0f;
        [Range(-0.5f, 0.5f)]
        public float offsetScreenBounds = 0.0f;

        [SerializeField]
        private GameObject target;
        private RectTransform rectTransform;

        //How othen we check looking on sound
        private float timer = 0.0f;
        private const float timerDelay = 0.016f;

        private Vector3 centerOfScreen = new Vector3(0.5F, 0.5F, 0.0F);



        void Start()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            arrowImage.SetActive(false);
        }

        void Update()
        {
            //Looking timer
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                timer = timerDelay;
                CheckLooking();
            }

        }

        private void CheckLooking()
        {
            if (target != null && target.activeInHierarchy)
            {
                Vector3 screenPos = Camera.main.WorldToViewportPoint(target.transform.position);

                if (screenPos.x >  offsetScreenBounds && screenPos.x < 1.0F - offsetScreenBounds && screenPos.y > offsetScreenBounds && screenPos.y < 1.0F - offsetScreenBounds && screenPos.z > 0.0F)
                    arrowImage.SetActive(false);
                else
                {
                    arrowImage.SetActive(true);

                    //Rotate to target
                    if (screenPos.z < 0.0F)
                        screenPos *= -1.0F;

                    screenPos -= centerOfScreen;

                    float angle = -(Mathf.Atan2(screenPos.x, screenPos.y) * Mathf.Rad2Deg);

                    if(angle > 0)
                    {
                        if (angle < minRotation)
                            angle = minRotation;
                        if (angle > maxRotation)
                            angle = maxRotation;
                    }else
                    {
                        if (angle > -minRotation)
                            angle = -minRotation;
                        if (angle < -maxRotation)
                            angle = -maxRotation;
                    }

                    rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
                }
            }else
            {
                Destroy(gameObject);
            }
        }

        public void SetTarget(GameObject _newTarget)
        {
            target = _newTarget;
        }
    }
}

