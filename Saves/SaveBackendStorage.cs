using UnityEngine;
using System.Collections.Generic;
using System;

namespace SCore
{
    //// <summary>
    /// Static class controlling work with server saves
    /// </summary>
    static public class SaveBackendStorage
    {
        /// PUBLIC VARIABLES

        /// PUBLIC CONSTANTS

        /// PRIVATE CONSTANTS

        /// PRIVATE VARIABLES
        static private string app_id;
        static private string user_id;
        static private string server_key;
        static private string auth_key;
        static private string server_request_url;

        static private bool isInited = false;

        private static Callback.EventHandlerObject gameLoadSuccessCallbackFunction;
        private static Callback.EventHandlerObject gameLoadFailCallbackFunction;


        /// <summary>
        /// Game load profile from server storage
        /// </summary>
        static public void Init(string _app_id, string _user_id, string _server_key, string _auth_key, string _server_request_url)
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
        static public void Load(Callback.EventHandlerObject successCallbackFunction = null, Callback.EventHandlerObject failCallbackFunction = null, float timeLimitSeconds = 10.0F)
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
                if (failCallbackFunction != null)
                    failCallbackFunction(null);
            }
        }

        /// <summary>
        /// Game load profile completed. Additionaly we take daily bonus and currency bonus.
        /// </summary>
        static public void Loaded(object _object)
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
                                if (gameLoadSuccessCallbackFunction != null)
                                    gameLoadSuccessCallbackFunction(saveObject);
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
        static public void LoadError(object _object)
        {
            string _message = _object as string;
            Debug.LogError(_message);
            if (gameLoadFailCallbackFunction != null)
                gameLoadFailCallbackFunction(_message);
        }

        /// <summary>
        /// Game load error when loading
        /// </summary>
        static public void LoadNewSave(object _object)
        {
            string _message = _object as string;
            Debug.Log(_message);
            if (gameLoadSuccessCallbackFunction != null)
                gameLoadSuccessCallbackFunction(null);
        }

        private static Callback.EventHandler gameSaveSuccessCallbackFunction;
        private static Callback.EventHandlerObject gameSaveFailCallbackFunction;

        /// <summary>
        /// Game save profile push to server storage
        /// </summary>
        static public void Save(Dictionary<string, object> _saveVO, Dictionary<string, object> _meta = null, Callback.EventHandler successCallbackFunction = null, Callback.EventHandlerObject failCallbackFunction = null, float timeLimitSeconds = 10.0F)
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
        static public void SaveSuccess(object _object)
        {
            if (gameSaveSuccessCallbackFunction != null)
                gameSaveSuccessCallbackFunction();
        }


        /// <summary>
        /// Game save error when loading
        /// </summary>
        static public void SaveError(object _object)
        {
            string _message = _object as string;
            Debug.LogError(_message);
            if (gameSaveFailCallbackFunction != null)
                gameSaveFailCallbackFunction(_message);
        }


        static private bool CheckExceptions()
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

        static private void ThrowNotInited()
        {
            Debug.LogError("SaveBackendStorage Not inited!");
        }


    }



}