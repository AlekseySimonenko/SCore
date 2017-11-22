using UnityEngine;
using UnityEngine.UI;

namespace SCore
{
    public class ButtonFullscreen : MonoBehaviour
    {


        void Start()
        {
            UpdateButton();
        }

        void UpdateButton()
        {
            Image image = gameObject.GetComponent<Image>();
            image.color = Screen.fullScreen ? new Color(1, 1, 1, 0.3F) : Color.white;
        }


        public void toggleFullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
            UpdateButton();
        }

    }
}