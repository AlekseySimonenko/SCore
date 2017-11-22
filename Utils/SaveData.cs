using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace SCore
{
    //// <summary>
    /// Convert save from and in to string
    /// </summary>
    public class SaveData
    {

        static public string SaveDataToString(Dictionary<string, object> _saveVO)
        {
            string _string = "";

            if (_saveVO != null)
            {
                _string = WWW.EscapeURL( MiniJSON.Json.Serialize(_saveVO) );
            }

            return _string;
        }


        static public Dictionary<string, object> LoadDataFromString(string _data)
        {
            Dictionary<string, object> _saveVO = null;

            if (_data != "")
            {
                _saveVO = MiniJSON.Json.Deserialize( WWW.UnEscapeURL(_data) ) as Dictionary<string, object>;
            }

            return _saveVO;
        }


}
}
