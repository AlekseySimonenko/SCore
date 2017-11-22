
namespace Core
{
    /// <summary>
    /// User VO
    /// </summary
    public class User
    {

        #region Public variables
        public string uid { get; private set; }
        public string first_name { get; private set; }
        public string last_name { get; private set; }
        public string sex { get; private set; } // "" && 0 - unknown, 1 - female, 2 - male
        public int byear { get; private set; } // 0 - unknown, 
        public string photo_small { get; private set; }
        public bool inApp { get; private set; }
        public bool online { get; private set; }
        #endregion

        #region Public constants
        #endregion

        #region Private constants
        #endregion

        #region Private variables
        #endregion

        /// <summary>
        /// Constructor
        /// </summary
        public User(string _uid, string _first_name, string _last_name, string _sex, int _byear, string _photo_small, bool _inApp, bool _online)
        {
            uid = _uid;
            first_name = _first_name;
            last_name = _last_name;
            sex = _sex;
            byear = _byear;
            photo_small = _photo_small;
            inApp = _inApp;
            online = _online;
        }


    }

}