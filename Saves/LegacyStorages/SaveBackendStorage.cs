using SCore.Framework;
using SCore.Utils;
using SCore.Web;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCore.Saves
{
    //s/ <summary>
    /// Static class controlling work with server saves
    /// </summary>
    public class SaveBackendStorage : MonoBehaviourSingleton<SaveBackendStorage>
    {
        /// PUBLIC VARIABLES

        /// PUBLIC CONSTANTS

        /// PRIVATE CONSTANTS

        /// PRIVATE VARIABLES
        private string app_id;

        private string user_id;
        private string server_key;
        private string auth_key;
        private string server_request_url;

        private bool isInited = false;

        private Action<object> gameLoadSuccessCallbackFunction;
        private Action<object> gameLoadFailCallbackFunction;

        /// <summary>
        /// Game load profile from server storage
        /// </summary>
        public void Init(string _app_id, string _user_id, string _server_key, string _auth_key, string _server_request_url)
        {
            app_id = _app_id;
            user_id = _user_id;
            server_key = _server_key;
            auth_key = _auth_key;
            server_request_url = _server_request_url;

            isInited = true;
        }

        /// <summary>
        /// Game load profile from server storage
        /// </summary>
        public void Load(Action<object> successCallbackFunction = null, Action<object> failCallbackFunction = null, float timeLimitSeconds = 10.0F)
        {
            Debug.Log("SaveBackendStorage.Load");
            if (!CheckExceptions())
                return;

            if (server_request_url != "")
            {
                //Create request
                string requestURL = server_request_url + "save_load.php";
                requestURL += "?";
                requestURL += "auth=" + auth_key;
                requestURL += "&uid=" + user_id;
                WebRequestManager.Request(requestURL, Loaded, LoadError, timeLimitSeconds);
                gameLoadSuccessCallbackFunction = successCallbackFunction;
                gameLoadFailCallbackFunction = failCallbackFunction;
            }
            else
            {
                failCallbackFunction?.Invoke(null);
            }
        }

        /// <summary>
        /// Game load profile completed. Additionaly we take daily bonus and currency bonus.
        /// </summary>
        public void Loaded(object _object)
        {
            Debug.Log("SaveBackendStorage.Loaded");

            Dictionary<string, object> saveObject = null;
            if (_object == null)
            {
                LoadError("SaveBackendStorage ERROR null request response");
                return;
            }

            WWW www = _object as WWW;
            string data = www.text;
            if (data == "")
            {
                LoadError("SaveBackendStorage ERROR zero request response");
                return;
            }

            Dictionary<string, object> dataRequest = MiniJSON.Json.Deserialize(data) as Dictionary<string, object>;
            if (dataRequest != null)
            {
                if (dataRequest.ContainsKey("savedata"))
                {
                    if (dataRequest.ContainsKey("protect"))
                    {
                        string protectValidation = HashString.Hash(app_id + Convert.ToString(dataRequest["savedata"]) + user_id + server_key);
                        if (protectValidation == Convert.ToString(dataRequest["protect"]))
                        {
                            if (Convert.ToString(dataRequest["savedata"]) != "")
                            {
                                saveObject = DictionaryConverter.ConvertStringToStrObj(Convert.ToString(dataRequest["savedata"]));
                                gameLoadSuccessCallbackFunction?.Invoke(saveObject);
                            }
                            else
                            {
                                LoadNewSave("SaveBackendStorage Server does't have save file");
                            }
                        }
                        else
                            LoadError("SaveBackendStorage ERROR validate protection fail");
                    }
                    else
                        LoadError("SaveBackendStorage ERROR response not contain protect var");
                }
                else
                    LoadError("SaveBackendStorage ERROR response not contain savedata var");
            }
            else
                LoadError("SaveBackendStorage ERROR can't deserialize request response");
        }

        /// <summary>
        /// Game load error when loading
        /// </summary>
        public void LoadError(object _object)
        {
            string _message = _object as string;
            Debug.LogError(_message);
            gameLoadFailCallbackFunction?.Invoke(_message);
        }

        /// <summary>
        /// Game load error when loading
        /// </summary>
        public void LoadNewSave(object _object)
        {
            string _message = _object as string;
            Debug.Log(_message);
            gameLoadSuccessCallbackFunction?.Invoke(null);
        }

        private static Action gameSaveSuccessCallbackFunction;
        private static Action<object> gameSaveFailCallbackFunction;

        /// <summary>
        /// Game save profile push to server storage
        /// </summary>
        public void Save(Dictionary<string, object> _saveVO, Dictionary<string, object> _meta = null, Action successCallbackFunction = null, Action<object> failCallbackFunction = null, float timeLimitSeconds = 10.0F)
        {
            if (!CheckExceptions())
                return;

            string data = DictionaryConverter.ConvertStrObjToString(_saveVO);
            string metastring = MiniJSON.Json.Serialize(_meta);

            //Create request
            string requestURL = server_request_url + "save_save.php";
            requestURL += "?";
            requestURL += "auth=" + auth_key;
            requestURL += "&uid=" + user_id;
            requestURL += "&save=" + WWW.EscapeURL(data);
            requestURL += "&meta=" + metastring;

            //Protection
            string protect = HashString.Hash(app_id + data + user_id + metastring + server_key);
            requestURL += "&protect=" + protect;

            gameSaveSuccessCallbackFunction = successCallbackFunction;
            gameSaveFailCallbackFunction = failCallbackFunction;

            //Send request
            WebRequestManager.Request(requestURL, SaveSuccess, SaveError, timeLimitSeconds);
        }

        /// <summary>
        /// Game save error when loading
        /// </summary>
        public void SaveSuccess(object _object)
        {
            gameSaveSuccessCallbackFunction?.Invoke();
        }

        /// <summary>
        /// Game save error when loading
        /// </summary>
        public void SaveError(object _object)
        {
            string _message = _object as string;
            Debug.LogError(_message);
            gameSaveFailCallbackFunction?.Invoke(_message);
        }

        private bool CheckExceptions()
        {
            if (!isInited)
            {
                ThrowNotInited();
                return false;
            }
            if (server_request_url == "")
            {
                Debug.LogError("SaveBackendStorage : no server url");
                return false;
            }
            return true;
        }

        private void ThrowNotInited()
        {
            Debug.LogError("SaveBackendStorage Not inited!");
        }
    }
}