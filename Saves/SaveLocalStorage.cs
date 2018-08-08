using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace SCore.Saves
{
    /// <summary>
    /// Static class controlling saving and loading of game progress on local storage.
    /// </summary>
    public class SaveLocalStorage
    {
        //Universal path for quick saved games
        private const string fileSavePath = "/save.qs";


        static public void Save(Dictionary<string, object> _saveVO)
        {
            string saveString = DictionaryConverter.ConvertStrObjToString(_saveVO);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + fileSavePath);
            bf.Serialize(file, saveString);
            file.Close();
        }

        static public Dictionary<string, object> Load()
        {
            Dictionary<string, object> _saveVO = null;

            if (File.Exists(Application.persistentDataPath + fileSavePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + fileSavePath, FileMode.Open);
                try
                {
                    string _deserealizedObject = bf.Deserialize(file) as string;
                    _saveVO = DictionaryConverter.ConvertStringToStrObj(_deserealizedObject);
                }
                catch
                {
                    Debug.Log("SaveManager: ERROR deserialize local data");
                }
                file.Close();
            }
            else
            {
                Debug.Log("SaveManager: no local data");
            }

            return _saveVO;
        }


  


}
}
