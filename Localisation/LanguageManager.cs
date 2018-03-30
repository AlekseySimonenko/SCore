using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

namespace SCore
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
        [Header("Remote/Local version updater")]
        public int buildConfigVersion = 0;
        private int localConfigVersion = 0;
        public int remoteConfigVersion = 0;
        public string remoteUrl;
        public float remoteUrlTimelimit = 5F;

        [Header("After inited")]
        public UnityEvent OnInitActions;

        [Header("Debug")]
        public string languageManual;

        public static string language;

        private const string LOCAL_CONFIG_VERSION = "sCore_language_local_v";

        private static TextAsset xmlAsset;
        private static SmallXmlParser xmlParser = new SmallXmlParser();
        private static Handler xmlDoc = new Handler();

        private static XmlDocument remoteXmlDocument;

        // Only one init calling protect variables
        private static bool isInitComplete = false;


        private void Start()
        {
            Init();
        }

        public static void Init()
        {
            if (!isInitComplete)
            {
                Debug.Log("LanguageManager:init");

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
                Debug.Log("LanguageManager:language " + language);


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
                Debug.LogError("LanguageManager:Repeating static class Init!");
            }
        }

        public static void InitCompleted()
        {
            if (!isInitComplete)
            {
                Debug.Log("LanguageManager:Init completed");
                isInitComplete = true;
                if (Instance.OnInitActions != null)
                    Instance.OnInitActions.Invoke();
            }
            else
            {
                Debug.LogError("LanguageManager:Repeating static class Init Completed!");
            }
        }


        private static void LoadBuildVersion()
        {
            Debug.Log("LanguageManager: LoadBuildVersion " + language);
            if (!LoadResource(language))
            {
                Debug.LogWarning("LanguageManager: Language xml not found " + language);
                language = "en";
                if (!LoadResource(language))
                {
                    Debug.LogError("LanguageManager: Default Language xml not found " + language);
                }
            }
        }

        private static bool LoadResource(string _name)
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


        private static void LoadLocalVersion()
        {
            Debug.Log("LanguageManager: LoadLocalVersion " + GetLocalConfigName(language));
            if (File.Exists(GetLocalConfigName(language)))
            {
                LoadFromFile(GetLocalConfigName(language));
                //Complete init of language manager
                InitCompleted();
            }
            else
            {
                Debug.LogWarning("LanguageManager: Local language xml not found " + language);
                LoadBuildVersion();
            }
        }

        private static void LoadRemoteVersion()
        {
            string configUrl = Instance.remoteUrl + language + ".xml" + "?v=" + Instance.remoteConfigVersion;
            Debug.Log("LanguageManager: LoadRemoteVersion " + configUrl);

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

        private static void OnLoadRemoteCompleted(object _object)
        {
            Debug.Log("LanguageManager: OnLoadRemoteCompleted");

            if (_object == null)
            {
                Debug.LogWarning("LanguageManager: ERROR null request response");
                return;
            }

            WWW www = _object as WWW;
            string data = www.text;
            if (data == "")
            {
                Debug.LogWarning("LanguageManager: ERROR zero request response");
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

        private static void OnLoadRemoteError(object data)
        {
            Debug.LogWarning("LanguageManager: OnLoadRemoteError");
            LoadLocalVersion();
        }

        private static void LoadFromFile(string _file)
        {
            TextReader textReader = File.OpenText(_file);
            xmlParser.Parse(textReader, xmlDoc);            
        }

        private static void LoadFromString(string _data)
        {
            TextReader textReader = new StreamReader(GenerateStreamFromString(_data));
            xmlParser.Parse(textReader, xmlDoc);
        }

        private static void SaveLocalConfig(string _data, string _language)
        {
            string path = GetLocalConfigName(_language);
            File.WriteAllText(path, _data);
        }

        private static string GetLocalConfigName(string _language)
        {
            return Application.persistentDataPath + "/" + _language + ".xml";
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
                Debug.LogError("LanguageManager: return " + "!none on id: " + _id);
                return "!none";
            }
        }

        public static string Replace(string _text, string[] args)
        {
            if (args != null)
                for (int i = 0; i < args.Length; i++)
                {
                    _text = _text.Replace("{" + i.ToString() + "}", args[ i ]);
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

        // Returns system language from java enviroment (Android only)
        public static string GetLanguage()
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
            return content[ _id ];
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
            content[ lastKey ] = s;
        }

        public void OnIgnorableWhitespace(string s)
        {
        }

        public void OnProcessingInstruction(string name, string text)
        {
        }
    }




}
