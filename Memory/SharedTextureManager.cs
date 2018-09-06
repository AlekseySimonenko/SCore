using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SCore.Memory
{
    /// <summary>
    /// Static class provide methods for register texture reference,
    /// control for textures usage & destroy
    /// Recomennded usage only for shared textures with async parallel destroy
    /// </summary>
    public class SharedTextureManager : MonoBehaviourSingleton<SharedTextureManager>
    {
        //PUBLIC STATIC

        //PUBLIC EVENTS

        //PUBLIC VARIABLES

        //PRIVATE STATIC

        //PRIVATE VARIABLES
        private class SharedTextureVariables
        {
            public List<object> Refferences = new List<object>();
            public bool DestroyFlag = false;
        };
        private Dictionary<Texture, SharedTextureVariables> sharedTextures = new Dictionary<Texture, SharedTextureVariables>();

        /// <summary>
        /// Function description
        /// </summary>
        void Start()
        {
            Debug.Log("SharedTextureManager:" + "Start", Instance.gameObject);
        }

        /// <summary>
        /// Function description
        /// </summary>
        void Update()
        {

        }

        /// <summary>
        /// If texture is need for somebody
        /// </summary>
        public void AddTextureRef(Texture _texture, object _reference)
        {
            if (!sharedTextures.ContainsKey(_texture))
                AddTexture(_texture);

            if (!sharedTextures[_texture].Refferences.Contains(_reference))
                sharedTextures[_texture].Refferences.Add(_reference);
        }

        /// <summary>
        /// If texture not need more for somebody
        /// </summary>
        public void RemoveTextureRef(Texture _texture, object _reference)
        {
            if (sharedTextures.ContainsKey(_texture))
                sharedTextures[_texture].Refferences.Remove(_reference);
            else
                Debug.LogWarning("SharedTextureManager:DecreaseTextureRef " + "Shared texture not found", Instance.gameObject);
            TryDestroyTextures();
        }

        /// <summary>
        /// Mark Texture as need to be destroyed
        /// </summary>
        public void MarkTextureNeedDestroyed(Texture _texture)
        {
            if (sharedTextures.ContainsKey(_texture))
                sharedTextures[_texture].DestroyFlag = true;
            TryDestroyTextures();
        }

        /// <summary>
        /// Unsafe Destroy texture with ignore of reference usage count
        /// </summary>
        public void ForceDestroyTexture(Texture _texture)
        {
            sharedTextures.Remove(_texture);
            Destroy(_texture);
        }

        /// <summary>
        /// Register new shared texture
        /// </summary>
        private void AddTexture(Texture _texture)
        {
            sharedTextures.Add(_texture, new SharedTextureVariables());
        }

        /// <summary>
        /// Check conditions for textures destroyed
        /// </summary>
        private void TryDestroyTextures()
        {
            List<Texture> keys = new List<Texture>(sharedTextures.Keys);
            foreach (Texture key in keys)
            {
                if (sharedTextures[key].DestroyFlag == true && sharedTextures[key].Refferences.Count <= 0)
                {
                    ForceDestroyTexture(key);
                }
            }
        }




    }
}
