using SCore.Loading;
using SCore.Utils;
using SCore.Web;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace SCore.Localisation
{
    /// <summary>
    /// Locaslization manager
    /// 1. Remote config - xml file on server
    /// 2. Local config - xml file downloaded from server to device
    /// 3. Build config - xml file included in build
    /// </summary>
    public class LocalisationManager : MonoBehaviour, ILocalisationManager
    {
        //DEPENDENCIES

        [Inject] private IWebRequestManager _webRequestManager;

        //EDITOR VARIABLES
        [Header("Remote/Local version updater")]
        public int buildConfigVersion = 0;

        public int remoteConfigVersion = 0;
        public string remoteUrl;
        public float remoteUrlTimelimit = 5F;

        [Header("After inited")]
        public UnityEvent OnInitActions;

        [Header("Debug")]
        public string languageManual;

        //PRIVATE STATIC
        private const string LOCAL_CONFIG_VERSION = "sCore_language_local_v";

        //PRIVATE VARIABLES
        private string _language;

        private int _localConfigVersion = 0;

        private TextAsset _xmlAsset;
        private SmallXmlParser _xmlParser = new SmallXmlParser();
        private Handler _xmlDoc = new Handler();
        private XmlDocument _remoteXmlDocument;

        // Only one init calling protect variables
        private bool _isInitComplete = false;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            if (!_isInitComplete)
            {
                Debug.Log("LanguageManager:init", gameObject);

                if (Application.isEditor || Debug.isDebugBuild)
                {
                    string language_debug = PlayerPrefs.GetString("Language_debug");
                    if (!string.IsNullOrEmpty(language_debug))
                        _language = language_debug;
                    else
                        _language = languageManual;
                }

                // Spcial for Hindi
                if (GetDeviceLanguage() == "hi")
                    _language = "hi";

                //Auto system language choise
                if (string.IsNullOrEmpty(_language))
                {
                    _language = "en";
                    if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian || Application.systemLanguage == SystemLanguage.Bulgarian)
                        _language = "ru";
                    if (Application.systemLanguage == SystemLanguage.German)
                        _language = "de";
                    if (Application.systemLanguage == SystemLanguage.French)
                        _language = "fr";
                    if (Application.systemLanguage == SystemLanguage.Portuguese)
                        _language = "pt";
                    if (Application.systemLanguage == SystemLanguage.Turkish)
                        _language = "tr";
                    if (Application.systemLanguage == SystemLanguage.Japanese)
                        _language = "ja";
                    if (Application.systemLanguage == SystemLanguage.Korean)
                        _language = "ko";
                    if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
                        _language = "zh-CN";
                    if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
                        _language = "tc";
                    if (Application.systemLanguage == SystemLanguage.Arabic)
                        _language = "ar";
                    if (Application.systemLanguage == SystemLanguage.Dutch)
                        _language = "nl";
                    if (Application.systemLanguage == SystemLanguage.Thai)
                        _language = "th";
                    if (Application.systemLanguage == SystemLanguage.Indonesian)
                        _language = "id";
                    if (Application.systemLanguage == SystemLanguage.Italian)
                        _language = "it";
                    if (Application.systemLanguage == SystemLanguage.Spanish)
                        _language = "es-419";
                }
                Debug.Log("LanguageManager:language " + _language, gameObject);

                if (_language == "max")
                    if (LoadLocalMax()) return;

                //Localisation version updater
                if (PlayerPrefs.HasKey(LOCAL_CONFIG_VERSION))
                    _localConfigVersion = PlayerPrefs.GetInt(LOCAL_CONFIG_VERSION);

                //Try to load actual version
                if (buildConfigVersion == remoteConfigVersion)
                    LoadBuildVersion();
                else if (_localConfigVersion == remoteConfigVersion)
                    LoadLocalVersion();
                else
                    LoadRemoteVersion();
            }
            else
            {
                Debug.LogError("LanguageManager:Repeating class Init!", gameObject);
            }
        }

        public string GetLanguage()
        {
            return _language;
        }

        public bool ContainsKey(string id)
        {
            return _xmlDoc.ContainsKey(id);
        }

        public string Get(string id, params string[] args)
        {
            if (_xmlDoc.ContainsKey(id) == true)
            {
                string _text = _xmlDoc.SelectKey(id);
                if (args != null)
                    _text = Replace(_text, args);
                return _text;
            }
            else
            {
                Debug.LogError("LanguageManager: return " + "!none on id: " + id, gameObject);
                return "!none";
            }
        }

        private void InitCompleted()
        {
            if (!_isInitComplete)
            {
                Debug.Log("LanguageManager:Init completed", gameObject);
                _isInitComplete = true;
                OnInitActions?.Invoke();
            }
            else
            {
                Debug.LogError("LanguageManager:Repeating class Init Completed!", gameObject);
            }
        }

        private void LoadBuildVersion()
        {
            Debug.Log("LanguageManager: LoadBuildVersion " + _language, gameObject);
            if (!LoadResource(_language))
            {
                Debug.LogWarning("LanguageManager: Language xml not found " + _language, gameObject);
                _language = "en";
                if (!LoadResource(_language))
                {
                    Debug.LogError("LanguageManager: Default Language xml not found " + _language, gameObject);
                }
            }
        }

        private bool LoadResource(string name)
        {
            _xmlAsset = Resources.Load(_language) as TextAsset;
            if (_xmlAsset)
            {
                LoadFromString(_xmlAsset.text);
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
                _xmlAsset = Resources.Load(defaultLang) as TextAsset;
                TextReader textReader = new StreamReader(GenerateStreamFromString(_xmlAsset.text));
                _xmlParser.Parse(textReader, _xmlDoc);

                foreach (string lang in langs)
                {
                    TextAsset langAsset = Resources.Load(lang) as TextAsset;
                    using (TextReader reader = new StreamReader(GenerateStreamFromString(langAsset.text)))
                    {
                        Handler langDoc = new Handler();
                        _xmlParser.Parse(reader, langDoc);

                        List<string> keys = new List<string>();
                        foreach (KeyValuePair<string, string> kvp in _xmlDoc.content)
                            keys.Add(kvp.Key);

                        foreach (string key in keys)
                        {
                            if (langDoc.ContainsKey(key))
                            {
                                if (_xmlDoc.content[key].Length < langDoc.content[key].Length)
                                {
                                    _xmlDoc.content[key] = langDoc.content[key];
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("Unable to get lang max!: " + e.Message, gameObject);
                return false;
            }

            InitCompleted();
            return true;
        }

        private void LoadLocalVersion()
        {
            Debug.Log("LanguageManager: LoadLocalVersion " + GetLocalConfigName(_language), gameObject);
            if (File.Exists(GetLocalConfigName(_language)))
            {
                LoadFromFile(GetLocalConfigName(_language));
                //Complete init of language manager
                InitCompleted();
            }
            else
            {
                Debug.LogWarning("LanguageManager: Local language xml not found " + _language, gameObject);
                LoadBuildVersion();
            }
        }

        private void LoadRemoteVersion()
        {
            string configUrl = remoteUrl + _language + ".xml" + "?v=" + remoteConfigVersion;
            Debug.Log("LanguageManager: LoadRemoteVersion " + configUrl, gameObject);

            _webRequestManager.Request(configUrl, OnLoadRemoteCompleted, OnLoadRemoteError, remoteUrlTimelimit);
        }

        private void OnLoadRemoteCompleted(object remoteObject)
        {
            Debug.Log("LanguageManager: OnLoadRemoteCompleted", gameObject);

            if (remoteObject == null)
            {
                Debug.LogWarning("LanguageManager: ERROR null request response", gameObject);
                return;
            }

            WWW www = remoteObject as WWW;
            string data = www.text;
            if (data == "")
            {
                Debug.LogWarning("LanguageManager: ERROR zero request response", gameObject);
                return;
            }

            //Parse localisation config
            LoadFromString(data);
            //Backup loaded config
            SaveLocalConfig(data, _language);
            _localConfigVersion = remoteConfigVersion;
            PlayerPrefs.SetInt(LOCAL_CONFIG_VERSION, _localConfigVersion);
            //Complete init of language manager
            InitCompleted();
        }

        private void OnLoadRemoteError(object data)
        {
            Debug.LogWarning("LanguageManager: OnLoadRemoteError", gameObject);
            LoadLocalVersion();
        }

        private void LoadFromFile(string file)
        {
            TextReader textReader = File.OpenText(file);
            _xmlParser.Parse(textReader, _xmlDoc);
        }

        private void LoadFromString(string data)
        {
            TextReader textReader = new StreamReader(GenerateStreamFromString(data));
            _xmlParser.Parse(textReader, _xmlDoc);
        }

        private void SaveLocalConfig(string data, string language)
        {
            string path = GetLocalConfigName(language);
            File.WriteAllText(path, data);
        }

        private string GetLocalConfigName(string language)
        {
            return Application.persistentDataPath + "/" + language + ".xml";
        }

        private string Replace(string text, string[] args)
        {
            if (args != null)
                for (int i = 0; i < args.Length; i++)
                {
                    text = text.Replace("{" + i.ToString() + "}", args[i]);
                }
            return text;
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        // Returns system language from java enviroment (Android only)
        private string GetDeviceLanguage()
        {
#if UNITY_ANDROID
            if (!Application.isEditor)
            {
                try
                {
                    var locale = new AndroidJavaClass("java.util.Locale");
                    var localeInst = locale.CallStatic<AndroidJavaObject>("getDefault");
                    var name = localeInst.Call<string>("getLanguage");
                    return name;
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.Message);
                    return "Error";
                }
            }
            else
            {
                return "Not supported";
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