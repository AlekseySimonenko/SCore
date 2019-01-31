using UnityEngine;
using UnityEngine.UI;

namespace SCore.Components
{
    public class AnimatedMaterial : MonoBehaviour
    {
        public string MaterialProperty = "";
        public Image targetImage;
        public float AnimationSpeed = 1f;
        public bool UnscaledTime;

        private Material targetMaterial;

        // Use this for initialization
        private void Start()
        {
            if (targetImage != null)
            {
                targetMaterial = new Material(targetImage.material);
                targetImage.material = targetMaterial;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (!string.IsNullOrEmpty(MaterialProperty) && targetMaterial != null)
            {
                targetMaterial.SetFloat(MaterialProperty, UnscaledTime ? Time.unscaledTime * AnimationSpeed : Time.time * AnimationSpeed);
            }
        }
    }
}