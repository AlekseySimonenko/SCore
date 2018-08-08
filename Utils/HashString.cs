using System;
using System.Security.Cryptography;
using System.Text;

namespace SCore
{
    /// <summary>
    /// Static class for hashing string to MD5 string
    /// </summary>
    public class HashString
    {

        static public string Hash(string _string)
        {
            MD5CryptoServiceProvider cryptography = new MD5CryptoServiceProvider();
            byte[] asciiBytes = ASCIIEncoding.ASCII.GetBytes(_string);
            byte[] hashedBytes = cryptography.ComputeHash(asciiBytes);
            string hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hash;
        }

    }
}
