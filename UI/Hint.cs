using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Core
{
    /// <summary>
    /// Hint text object based on unity UI system
    /// </summary>
    public class Hint : MonoBehaviour
    {

        #region Public variables
        //All hints must be in screen limits
        public int screenBound = 0;
        public event Callback.EventHandler HiddenEvent;
        public Text text;
        //Only 3D target in scene support now
        public GameObject target;
        #endregion

        #region Public constants
        #endregion

        #region Private constants
        #endregion

        #region Private variables
        private float offsetPosX = 0.0F;
        private float offsetPosY = 0.0F;
        private RectTransform hintTransform;
        private RectTransform textTransform;

        private float xmin;
        private float xmax;
        private float ymin;
        private float ymax;
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public void Init(string _text, int _offsetPosX, int _offsetPosY, GameObject _target = null)
        {
            hintTransform = GetComponent<RectTransform>();
            hintTransform.anchoredPosition = new Vector3(_offsetPosX, _offsetPosY, hintTransform.position.z);

            offsetPosX = _offsetPosX;
            offsetPosY = _offsetPosY;
            target = _target;

            //If hint with text and autosize
            if (text != null)
            {
                textTransform = text.GetComponent<RectTransform>();
                text.text = _text;
                UpdateForm();
            }

            xmin = screenBound + (hintTransform.pivot.x * hintTransform.rect.width);
            xmax = Screen.width - screenBound - ((1.0f - hintTransform.pivot.x) * hintTransform.rect.width);
            ymin = screenBound + (hintTransform.pivot.y * hintTransform.rect.height);
            ymax = Screen.height - screenBound - ((1.0f - hintTransform.pivot.y) * hintTransform.rect.height);

        }

        void UpdateForm()
        {
            float fixOffset = 2.0F;
            textTransform.sizeDelta = new Vector2(textTransform.rect.width, text.preferredHeight);
            float newHeight = fixOffset + text.preferredHeight + (Mathf.Abs(textTransform.anchoredPosition.y) * 2);
            hintTransform.sizeDelta = new Vector2(hintTransform.rect.width, newHeight);
        }

        public void UpdatePosition(float _x, float _y)
        {

            _x = Mathf.Min(xmax, Mathf.Max(xmin, _x));
            _y = Mathf.Min(ymax, Mathf.Max(ymin, _y));
            hintTransform.position = new Vector3(_x, _y, hintTransform.position.z);
        }

        void Update()
        {
            if (target != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
                UpdatePosition(screenPos.x + offsetPosX, screenPos.y + offsetPosY);
            }
        }

        public void Hide()
        {
            gameObject.GetComponent<Animator>().Play("Hide");
        }

        void Hidden()
        {
            if (HiddenEvent != null)
                HiddenEvent();
        }

    }
}