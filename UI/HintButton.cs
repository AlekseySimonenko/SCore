using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Core
{
    /// <summary>
    /// Hint text object based on unity UI system
    /// </summary>
    public class HintButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        #region Public variables
        public Text text;
        public string idLanguageString;
        #endregion

        #region Public constants
        #endregion

        #region Private constants
        #endregion

        #region Private variables
        #endregion

        void Start()
        {
            text.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (text != null)
            {
                text.text = LanguageManager.Get(idLanguageString);
                text.gameObject.SetActive(true);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (text != null)
            {
                text.gameObject.SetActive(false);
            }
        }

    }
}