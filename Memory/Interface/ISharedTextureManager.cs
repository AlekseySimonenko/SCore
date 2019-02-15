using UnityEngine;

namespace SCore.Memory
{
    public interface ISharedTextureManager
    {
        void AddTextureRef(Texture _texture, object _reference);
        void ForceDestroyTexture(Texture _texture);
        void MarkTextureNeedDestroyed(Texture _texture);
        void RemoveTextureRef(Texture _texture, object _reference);
    }
}