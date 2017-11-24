using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace SCore
{
    /// <summary>
    /// Singletone language and texts manager
    /// </summary>
    [RequireComponent(typeof(IServiceLoadingStep))]
    public class LanguageManager : MonoBehaviourSingleton<LanguageManager>
    {
        #region Public variables
        [Header("Manual Language")]
        public string languageManual;
        [Header("After inited")]
        public UnityEvent OnInitActions;

        public static string language;
        #endregion

        #region Public constants
        #endregion

        #region Private constants
        #endregion

        #region Private variables
        private static TextAsset xmlAsset;
        private static SmallXmlParser xmlParser = new SmallXmlParser();
        private static Handler xmlDoc = new Handler();

        // Only one init calling protect variables
        private static bool isInitComplete = false;
        #endregion

        private void Start()
        {
            Init();
        }

        public static void Init()
        {
            if (!isInitComplete)
            {
                Debug.Log("LanguageManager:init");

                language = Instance.languageManual;

                //Auto system language choise
                if (language == "")
                {
                    language = "en";
                    if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian || Application.systemLanguage == SystemLanguage.Bulgarian)
                        language = "ru";
                    if (Application.systemLanguage == SystemLanguage.German)
                        language = "de";
                    if (Application.systemLanguage == SystemLanguage.French)
                        language = "fr";
                    if (Application.systemLanguage == SystemLanguage.Spanish)
                        language = "es";
                    if (Application.systemLanguage == SystemLanguage.Portuguese)
                        language = "pt";
                    if (Application.systemLanguage == SystemLanguage.Turkish)
                        language = "tk";
                    if (Application.systemLanguage == SystemLanguage.Japanese)
                        language = "jp";
                    if (Application.systemLanguage == SystemLanguage.Korean)
                        language = "ko";
                    if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
                        language = "sc";
                    if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
                        language = "tc";
                }
                Debug.Log("LanguageManager:language " + language);

                if (!LoadFile(language))
                {
                    Debug.LogWarning("Language xml not found " + language);
                    language = "en";
                    if (!LoadFile(language))
                    {
                        Debug.LogError("Default Language xml not found " + language);
                    }
                }

                //Init completed
                isInitComplete = true;
                if (Instance.OnInitActions != null)
                    Instance.OnInitActions.Invoke();
            }
            else
            {
                Debug.LogError("LanguageManager:Repeating static class Init!");
            }
        }


        private static bool LoadFile(string _name)
        {
            xmlAsset = Resources.Load(language) as TextAsset;
            if (xmlAsset)
            {
                TextReader textReader = new StreamReader(GenerateStreamFromString(xmlAsset.text));
                xmlParser.Parse(textReader, xmlDoc);
                return true;
            }
            else
                return false;
        }


        public static string Get(string _id, string[] args = null)
        {
            if (xmlDoc.ContainsKey(_id) == true)
            {
                string _text = xmlDoc.SelectKey(_id);
                if (args != null)
                    _text = Replace(_text, args);
                return _text;
            }
            else
            {
                Debug.LogError("LanguageManager:return " + "!none on id: " + _id);
                return "!none";
            }
        }

        public static string Replace(string _text, string[] args)
        {
            if(args != null)
            for (int i = 0; i < args.Length; i++)
            {
                    _text = _text.Replace("{" + i.ToString() + "}", args[i]);
                }
            return _text;
        }

        public static bool ContainsKey(string _id)
        {
            return xmlDoc.ContainsKey(_id);
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


    }


    class Handler : SmallXmlParser.IContentHandler
    {

        private Dictionary<string, string> content = new Dictionary<string, string>();
        private string lastKey = "";

        public bool ContainsKey(string _id)
        {
            return content.ContainsKey(_id);
        }

        public string SelectKey(string _id)
        {
            return content[_id];
        }

        public void OnStartParsing(SmallXmlParser parser)
        {
        }

        public void OnEndParsing(SmallXmlParser parser)
        {
        }

        public void OnStartElement(string name, SmallXmlParser.IAttrList attrs)
        {
            lastKey = name;
        }

        public void OnEndElement(string name)
        {
        }

        public void OnChars(string s)
        {
            content[lastKey] = s;
        }

        public void OnIgnorableWhitespace(string s)
        {
        }

        public void OnProcessingInstruction(string name, string text)
        {
        }
    }




}
