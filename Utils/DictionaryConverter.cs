using System;
using System.Collections.Generic;

namespace SCore
{
    //// <summary>
    /// Static class for dictionary converter (from unknow to selected type)
    /// </summary>
    public class DictionaryConverter
    {

        #region Public var
        #endregion

        #region Private const
        #endregion

        static public Dictionary<string,string> ConvertStrObjToStrStr(Dictionary<string,object> _objectDictionary)
        {
            Dictionary<string, string> strDictionary = new Dictionary<string, string>();
            if (_objectDictionary != null)
            {
                foreach (KeyValuePair<string, object> kvp in _objectDictionary)
                {
                    strDictionary[kvp.Key] = Convert.ToString(kvp.Value);
                }
            }
            return strDictionary;
        }

        static public Dictionary<int, int> ConvertStrObjToIntInt(Dictionary<string, object> _objectDictionary)
        {
            Dictionary<int, int> intDictionary = new Dictionary<int, int>();

            if (_objectDictionary != null)
            {
                foreach (KeyValuePair<string, object> kvp in _objectDictionary)
                {
                    intDictionary[Convert.ToInt32(kvp.Key)] = Convert.ToInt32(kvp.Value);
                }
            }

            return intDictionary;
        }

        static public Dictionary<string, object> ConvertIntIntToStrObj(Dictionary<int, int> _objectDictionary)
        {
            Dictionary<string, object> intDictionary = new Dictionary<string, object>();

            if (_objectDictionary != null)
            {
                foreach (KeyValuePair<int, int> kvp in _objectDictionary)
                {
                    intDictionary[kvp.Key.ToString()] = kvp.Value;
                }
            }

            return intDictionary;
        }
    }
}
