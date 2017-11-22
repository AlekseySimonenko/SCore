
using System.Collections;
using UnityEngine;

namespace Core
{
    //// <summary>
    /// Static class for recursive finding childs of objects by tag
    /// </summary>
    public class TagChildsFinder
    {

        #region Public var
        #endregion

        #region Private var
        static private ArrayList list;
        #endregion

        static public ArrayList Find(GameObject _parent, string _tag)
        {
            list = new ArrayList();

            Search(_parent, _tag);

            return list;
        }

        static private void Search(GameObject _parent, string _tag)
        {
            foreach (Transform child in _parent.transform)
            {
                if (child.CompareTag(_tag))
                    list.Add(child.gameObject);

                if (child.childCount > 0)
                {
                    Search(child.gameObject, _tag);
                }
            }
        }

    }
}
