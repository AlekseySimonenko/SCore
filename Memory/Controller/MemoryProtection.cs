using UnityEngine;

namespace SCore.Memory
{
    /// <summary>
    /// Protected variables by key offset
    /// </summary>
    public class MemoryProtection : IMemoryProtection
    {
        private int key = 0;

        private void GenerateKey()
        {
            if (key == 0)
                key = Random.Range(2, 8);
        }

        public int ProtectInt(int _value)
        {
            GenerateKey();
            int value = (_value * key) + key;
            return value;
        }

        public int UnProtectInt(int _value)
        {
            int value = (_value - key) / key;
            return value;
        }

        public float ProtectFloat(float _value)
        {
            GenerateKey();
            float value = (_value * key) + key;
            return value;
        }

        public float UnProtectFloat(float _value)
        {
            float value = (_value - key) / key;
            return value;
        }
    }
}