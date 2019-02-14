using SCore.Ads;
using System;
using UnityEngine;
using Zenject;

namespace SCore.Templates
{
    /// <summary>
    /// Controlling all ads platforms mediation
    /// </summary>
    public class NamingConvention : MonoBehaviour
    {
        //DEPENDENCIES

        [Inject] private IAdsManager _adsManager;

        //EVENTS

        public event Action Completed;

        //PUBLIC VARIABLES

        public int PublicVariable;

        //STATIC

        private static int _staticCounter;
        private const int CONSTANT_COUNT = 1;

        //EDITOR VARIABLES

        [SerializeField] private bool _editorVariable;

        //PRIVATE VARIABLES

        private bool _privateVariable;

        // Use this for initialization
        private void Method(int methodParameter)
        {
            int methodVariable = 2;
        }
    }
}