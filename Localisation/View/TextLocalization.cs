using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
        //DEPENDENCIES

        [Inject]
        private ILocalisationManager _localisationManager;

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
                textMesh.text = _localisationManager.Get(idLocaleVar);
            if (textUI == null)
                textUI = GetComponent<Text>();
            if (textUI != null)
                textUI.text = _localisationManager.Get(idLocaleVar);
#if MESHPRO
            if (textMeshProUGUI != null)
                textMeshProUGUI.text = _localisationManager.Get(idLocaleVar);
#endif
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}