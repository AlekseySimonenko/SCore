using SCore.Framework;
using SCore.Loading;
using SCore.Utils;
using SCore.Web;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

namespace SCore.Localisation
{
    /// <summary>
    /// Singletone language and texts manager
    /// 1. Remote config - xml file on server
    /// 2. Local config - xml file downloaded from server to device
    /// 3. Build config - xml file included in build
    /// </summary>
    [RequireComponent(typeof(IServiceLoadingStep))]
    public class LanguageManager : MonoBehaviourSingleton<LanguageManager>
    {
        //STATIC VARIABLES

        //EVENTS

        //PUBLIC VARIABLES
        [HideInInspector]
        public string language;

        [Header("Remote/Local version updater")]
        public int buildConfigVersion = 0;

        public int remoteConfigVersion = 0;
        public string remoteUrl;
        public float remoteUrlTimelimit = 5F;

        [Header("After inited")]
        public UnityEvent OnInitActions;

        [Header("Debug")]
        public string languageManual;

        //PRIVATE VARIABLES

        //PRIVATE STATIC
        private const string LOCAL_CONFIG_VERSION = "sCore_language_local_v";

        private TextAsset xmlAsset;
        private SmallXmlParser xmlParser = new SmallXmlParser();
        private Handler xmlDoc = new Handler();

        private XmlDocument remoteXmlDocument;

        // Only one init calling protect variables
        private bool isInitComplete = false;

        //PRIVATE VARIABLES
        private int localConfigVersion = 0;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            if (!isInitComplete)
            {
                Debug.Log("LanguageManager:init", Instance.gameObject);

                if (Application.isEditor || Debug.isDebugBuild)
                {
                    string language_debug = PlayerPrefs.GetString("Language_debug");
                    if (!string.IsNullOrEmpty(language_debug))
                        language = language_debug;
                    else
                        language = Instance.languageManual;
                }

                // Spcial for Hindi
                if (GetLanguage() == "hi")
                    language = "hi";

                //Auto system language choise
                if (string.IsNullOrEmpty(language))
                {
                    language = "en";
                    if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian || Application.systemLanguage == SystemLanguage.Bulgarian)
                        language = "ru";
                    if (Application.systemLanguage == SystemLanguage.German)
                        language = "de";
                    if (Application.systemLanguage == SystemLanguage.French)
                        language = "fr";
                    if (Application.systemLanguage == SystemLanguage.Portuguese)
                        language = "pt";
                    if (Application.systemLanguage == SystemLanguage.Turkish)
                        language = "tr";
                    if (Application.systemLanguage == SystemLanguage.Japanese)
                        language = "ja";
                    if (Application.systemLanguage == SystemLanguage.Korean)
                        language = "ko";
                    if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
                        language = "zh-CN";
                    if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
                        language = "tc";
                    if (Application.systemLanguage == SystemLanguage.Arabic)
                        language = "ar";
                    if (Application.systemLanguage == SystemLanguage.Dutch)
                        language = "nl";
                    if (Application.systemLanguage == SystemLanguage.Thai)
                        language = "th";
                    if (Application.systemLanguage == SystemLanguage.Indonesian)
                        language = "id";
                    if (Application.systemLanguage == SystemLanguage.Italian)
                        language = "it";
                    if (Application.systemLanguage == SystemLanguage.Spanish)
                        language = "es-419";
                }
                Debug.Log("LanguageManager:language " + language, Instance.gameObject);

                if (language == "max")
                    if (LoadLocalMax()) return;

                //Localisation version updater
                if (PlayerPrefs.HasKey(LOCAL_CONFIG_VERSION))
                    Instance.localConfigVersion = PlayerPrefs.GetInt(LOCAL_CONFIG_VERSION);

                //Try to load actual version
                if (Instance.buildConfigVersion == Instance.remoteConfigVersion)
                    LoadBuildVersion();
                else if (Instance.localConfigVersion == Instance.remoteConfigVersion)
                    LoadLocalVersion();
                else
                    LoadRemoteVersion();
            }
            else
            {
                Debug.LogError("LanguageManager:Repeating static class Init!", Instance.gameObject);
            }
        }

        public void InitCompleted()
        {
            if (!isInitComplete)
            {
                Debug.Log("LanguageManager:Init completed", Instance.gameObject);
                isInitComplete = true;
                Instance.OnInitActions?.Invoke();
            }
            else
            {
                Debug.LogError("LanguageManager:Repeating static class Init Completed!", Instance.gameObject);
            }
        }

        private void LoadBuildVersion()
        {
            Debug.Log("LanguageManager: LoadBuildVersion " + language, Instance.gameObject);
            if (!LoadResource(language))
            {
                Debug.LogWarning("LanguageManager: Language xml not found " + language, Instance.gameObject);
                language = "en";
                if (!LoadResource(language))
                {
                    Debug.LogError("LanguageManager: Default Language xml not found " + language, Instance.gameObject);
                }
            }
        }

        private bool LoadResource(string _name)
        {
            xmlAsset = Resources.Load(language) as TextAsset;
            if (xmlAsset)
            {
                LoadFromString(xmlAsset.text);
                //Complete init of language manager
                InitCompleted();
                return true;
            }
            else
                return false;
        }

        private bool LoadLocalMax()
        {
            string defaultLang = "ru";
            string[] langs = { "ar", "de", "en", "es-419", "fr", "hi", "id", "it", "ja", "ko", "nl", "pt", "tc", "th", "tr", "zh-CN" };

            try
            {
                xmlAsset = Resources.Load(defaultLang) as TextAsset;
                TextReader textReader = new StreamReader(GenerateStreamFromString(xmlAsset.text));
                xmlParser.Parse(textReader, xmlDoc);

                foreach (string lang in langs)
                {
                    TextAsset langAsset = Resources.Load(lang) as TextAsset;
                    using (TextReader reader = new StreamReader(GenerateStreamFromString(langAsset.text)))
                    {
                        Handler langDoc = new Handler();
                        xmlParser.Parse(reader, langDoc);

                        List<string> keys = new List<string>();
                        foreach (KeyValuePair<string, string> kvp in xmlDoc.content)
                            keys.Add(kvp.Key);

                        foreach (string key in keys)
                        {
                            if (langDoc.ContainsKey(key))
                            {
                                if (xmlDoc.content[key].Length < langDoc.content[key].Length)
                                {
                                    xmlDoc.content[key] = langDoc.content[key];
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("Unable to get lang max!: " + e.Message, Instance.gameObject);
                return false;
            }

            InitCompleted();
            return true;
        }

        private void LoadLocalVersion()
        {
            Debug.Log("LanguageManager: LoadLocalVersion " + GetLocalConfigName(language), Instance.gameObject);
            if (File.Exists(GetLocalConfigName(language)))
            {
                LoadFromFile(GetLocalConfigName(language));
                //Complete init of language manager
                InitCompleted();
            }
            else
            {
                Debug.LogWarning("LanguageManager: Local language xml not found " + language, Instance.gameObject);
                LoadBuildVersion();
            }
        }

        private void LoadRemoteVersion()
        {
            string configUrl = Instance.remoteUrl + language + ".xml" + "?v=" + Instance.remoteConfigVersion;
            Debug.Log("LanguageManager: LoadRemoteVersion " + configUrl, Instance.gameObject);

            WebRequestManager.Request(configUrl, OnLoadRemoteCompleted, OnLoadRemoteError, Instance.remoteUrlTimelimit);

            /*
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest) System.Net.WebRequest.Create(configUrl);
            req.Timeout = 1000 * 60 * 5; // milliseconds
            System.Net.WebResponse res = req.GetResponse();
            Stream responseStream = res.GetResponseStream();
            remoteXmlDocument = new XmlDocument();
            remoteXmlDocument.Load(responseStream);
            responseStream.Close();
            */
        }

        private void OnLoadRemoteCompleted(object _object)
        {
            Debug.Log("LanguageManager: OnLoadRemoteCompleted", Instance.gameObject);

            if (_object == null)
            {
                Debug.LogWarning("LanguageManager: ERROR null request response", Instance.gameObject);
                return;
            }

            WWW www = _object as WWW;
            string data = www.text;
            if (data == "")
            {
                Debug.LogWarning("LanguageManager: ERROR zero request response", Instance.gameObject);
                return;
            }

            //Parse localisation config
            LoadFromString(data);
            //Backup loaded config
            SaveLocalConfig(data, language);
            Instance.localConfigVersion = Instance.remoteConfigVersion;
            PlayerPrefs.SetInt(LOCAL_CONFIG_VERSION, Instance.localConfigVersion);
            //Complete init of language manager
            InitCompleted();
        }

        private void OnLoadRemoteError(object data)
        {
            Debug.LogWarning("LanguageManager: OnLoadRemoteError", Instance.gameObject);
            LoadLocalVersion();
        }

        private void LoadFromFile(string _file)
        {
            TextReader textReader = File.OpenText(_file);
            xmlParser.Parse(textReader, xmlDoc);
        }

        private void LoadFromString(string _data)
        {
            TextReader textReader = new StreamReader(GenerateStreamFromString(_data));
            xmlParser.Parse(textReader, xmlDoc);
        }

        private void SaveLocalConfig(string _data, string _language)
        {
            string path = GetLocalConfigName(_language);
            File.WriteAllText(path, _data);
        }

        private string GetLocalConfigName(string _language)
        {
            return Application.persistentDataPath + "/" + _language + ".xml";
        }

        public string Get(string _id, params string[] args)
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
                Debug.LogError("LanguageManager: return " + "!none on id: " + _id, Instance.gameObject);
                return "!none";
            }
        }

        public string Replace(string _text, string[] args)
        {
            if (args != null)
                for (int i = 0; i < args.Length; i++)
                {
                    _text = _text.Replace("{" + i.ToString() + "}", args[i]);
                }
            return _text;
        }

        public bool ContainsKey(string _id)
        {
            return xmlDoc.ContainsKey(_id);
        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        // Returns system language from java enviroment (Android only)
        public string GetLanguage()
        {
#if UNITY_ANDROID
            try
            {
                var locale = new AndroidJavaClass("java.util.Locale");
                var localeInst = locale.CallStatic<AndroidJavaObject>("getDefault");
                var name = localeInst.Call<string>("getLanguage");
                return name;
            }
            catch (System.Exception e)
            {
                return "Error";
            }
#else
     return "Not supported";
#endif
        }
    }

    internal class Handler : SmallXmlParser.IContentHandler
    {
        public Dictionary<string, string> content = new Dictionary<string, string>();
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