using UnityEngine;

namespace Core
{
    //// <summary>
    /// Protected variables by key offset
    /// </summary>
    public class MemoryProtection
    {
        private static int key = 0;

        public static void GenerateKey()
        {
            if (key == 0)
                key = Random.Range(2, 8);
        }

        public static int ProtectInt(int _value)
        {
            GenerateKey();
            int value = (_value * key) + key;
            return value;
        }

        public static int UnProtectInt(int _value)
        {
            int value = (_value - key) / key;
            return value;
        }

        public static float ProtectFloat(float _value)
        {
            GenerateKey();
            float value = (_value * key) + key;
            return value;
        }

        public static float UnProtectFloat(float _value)
        {
            float value = (_value - key) / key;
            return value;
        }


    }
}
