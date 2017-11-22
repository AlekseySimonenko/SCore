using UnityEngine;

namespace Core
{
    //// <summary>
    /// Step of tutorial with start and complete events and another conditions.
    /// </summary>
    public class TutorialStep : MonoBehaviour
    {
        #region Public variables
        public bool activated { get; private set; }
        public bool showed { get; private set; }
        public bool completed { get; private set; }
        public bool deleted { get; private set; }
        public event Callback.EventHandler ActivateEvent;
        public event Callback.EventHandlerObject DeleteEvent;
        #endregion

        #region Public constants
        #endregion

        #region Private constants
        #endregion

        #region Private variables
        private bool isTimeLimited = false;
        private float timer = 0.0F;
        private bool isHasTarget = false;
        private GameObject target;

        private float invokedActivateTime;

        private Transform hintCanvas;
        private GameObject hintPrefab;
        private GameObject hintSelecterPrefab;
        private string hintText;
        private int hintOffsetPosY;
        private int hintOffsetPosX;

        private GameObject hint;
        private GameObject selecter;
        #endregion


        /// <summary>
        /// Create new step and add it in the end of queue
        /// </summary>
        public void Init(Transform _canvas, GameObject _hintPrefab, GameObject _hintSelecterPrefab, string _text, int _timer = 0, int _offsetPosY = 0, int _offsetPosX = 0, float _invokedActivateTime = 0.0F, GameObject _target = null)
        {
            hintCanvas = _canvas;
            hintPrefab = _hintPrefab;
            hintSelecterPrefab = _hintSelecterPrefab;
            hintText = _text;

            if (_timer > 0)
            {
                timer = _timer;
                isTimeLimited = true;
            }
            if (_target != null)
            {
                target = _target;
                isHasTarget = true;
            }
            hintOffsetPosY = _offsetPosY;
            hintOffsetPosX = _offsetPosX;

            invokedActivateTime = _invokedActivateTime;

            activated = false;
            showed = false;
            completed = false;
            deleted = false;
        }


        //// <summary>
        /// Game event listener.
        /// </summary>
        public void ActivateWithoutTarget()
        {
            if (showed == false)
            {
                Invoke("ActivateInvoke", invokedActivateTime);
            }
        }
        public void ActivateWithoutTargetOneParam(object _dummy)
        {
            if (showed == false)
            {
                Invoke("ActivateInvoke", invokedActivateTime);
            }
        }

        //// <summary>
        /// Game event listener
        /// </summary>
        public void ActivateWithTarget(object _object)
        {
            if (showed == false)
            {
                target = _object as GameObject;
                isHasTarget = true;
                Invoke("ActivateInvoke", invokedActivateTime);
            }
        }

        //// <summary>
        /// Game event listener
        /// </summary>
        void ActivateInvoke()
        {
            if (activated == false)
            {
                activated = true;
                if (ActivateEvent != null)
                    ActivateEvent();
            }
        }


        //// <summary>
        /// Show visual hint to this step
        /// </summary>
        public void Show()
        {
            if (showed == false)
            {
                showed = true;
                //Create and show hint
                hint = hintPrefab.Spawn(new Vector3(), new Quaternion());
                hint.SetActive(false);
                hint.transform.SetParent(hintCanvas, false);
                hint.GetComponent<Hint>().Init(hintText, hintOffsetPosX, hintOffsetPosY, target);
                hint.SetActive(true);
                hint.GetComponent<Hint>().HiddenEvent += Delete;
                //Create and show  selecter
                if (target != null)
                {
                    selecter = Instantiate(hintSelecterPrefab, new Vector3(), new Quaternion()) as GameObject;
                    selecter.transform.position = target.transform.position;
                    selecter.GetComponent<FollowTarget>().target = target;
                }
            }
        }


        //// <summary>
        /// Timer and check target
        /// </summary>
        void Update()
        {
            if (completed == false && showed == true)
            {
                if (isTimeLimited)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0.0f)
                        Complete();
                }
                if (isHasTarget && (target == null || target.activeInHierarchy == false))
                {
                    Complete();
                }
            }

        }


        //// <summary>
        /// Game event listener
        /// </summary>
        public void Complete()
        {
            if (completed == false)
            {
                completed = true;

                if (hint != null)
                {
                    hint.GetComponent<Hint>().Hide();
                    if (selecter != null)
                    {
                        Destroy(selecter.gameObject);
                    }
                }
                else
                {
                    Delete();
                }
            }
        }

        //// <summary>
        /// Hint event listener
        /// </summary>
        public void Delete()
        {
            deleted = true;
            if (DeleteEvent != null)
                DeleteEvent(this);
        }

        //// <summary>
        /// Destroy gameObject
        /// </summary>
        public void Destroy()
        {
            if (hint != null)
            {
                Destroy(hint.gameObject);
            }
            if (selecter != null)
            {
                Destroy(selecter.gameObject);
            }
            Destroy(gameObject);
        }


    }
}