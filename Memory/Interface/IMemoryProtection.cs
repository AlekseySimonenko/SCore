namespace SCore.Memory
{
    public interface IMemoryProtection
    {
        float ProtectFloat(float _value);
        int ProtectInt(int _value);
        float UnProtectFloat(float _value);
        int UnProtectInt(int _value);
    }
}