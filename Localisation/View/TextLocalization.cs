using UnityEngine;
using UnityEngine.UI;

#if MESHPRO

using TMPro;

#endif

namespace SCore.Localisation
{
    /// <summary>
    /// Use SCore.LanguageManager to apply localisation text to UI components
    /// </summary>
    public class TextLocalization : MonoBehaviour
    {
        public string idLocaleVar;
        public TextMesh textMesh;
#if MESHPRO
        public TextMeshProUGUI textMeshProUGUI;
#endif
        public Text textUI;

        // Use this for initialization
        private void Start()
        {
            if (textMesh == null)
                textMesh = GetComponent<TextMesh>();
            if (textMesh != null)
                textMesh.text = LanguageManager.Instance.Get(idLocaleVar);
            if (textUI == null)
                textUI = GetComponent<Text>();
            if (textUI != null)
                textUI.text = LanguageManager.Instance.Get(idLocaleVar);
#if MESHPRO
            if (textMeshProUGUI != null)
                textMeshProUGUI.text = LanguageManager.Instance.Get(idLocaleVar);
#endif
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}