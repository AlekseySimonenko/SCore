using UnityEngine;
using System.Collections;

namespace Core
{
    public class ButtonConsole : MonoBehaviour
    {



        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void toggleConsole()
        {
            Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
        }

    }
}