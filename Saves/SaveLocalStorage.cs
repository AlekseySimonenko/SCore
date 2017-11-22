using UnityEngine;

using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
 

namespace Core
{
    //// <summary>
    /// Static class controlling saving and loading of game progress on local storage.
    /// </summary>
    public class SaveLocalStorage
    {

        #region Public var
        #endregion

        #region Private const
        //Universal path for quick saved games
        private const string fileSavePath = "/save.qs";
        #endregion


        static public void Save(Dictionary<string, object> _saveVO)
        {
            string saveString = SaveData.SaveDataToString(_saveVO);

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
                    _saveVO = SaveData.LoadDataFromString(_deserealizedObject);
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
