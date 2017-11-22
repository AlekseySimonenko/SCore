using UnityEngine;
using System.Collections;
using SCore;
using UnityEngine.UI;

namespace SCore
{
    public class TextLocalization : MonoBehaviour
    {

        public string idLocaleVar;
        public TextMesh textMesh;
        public Text textUI;

        // Use this for initialization
        void Start()
        {
            if (textMesh == null)
                textMesh = GetComponent<TextMesh>();
            if (textMesh != null)
                textMesh.text = LanguageManager.Get(idLocaleVar);
            if (textUI == null)
                textUI = GetComponent<Text>();
            if (textUI != null)
                textUI.text = LanguageManager.Get(idLocaleVar);
        }

        // Update is called once per frame
        void Update()
        {

        }


    }
}
