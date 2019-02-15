using System.IO;

namespace SCore.Localisation
{
    public interface ILocalisationManager
    {
        string GetLanguage();

        bool ContainsKey(string _id);

        //TODO rename method to GetValue
        string Get(string _id, params string[] args);

        void Init();

    }
}