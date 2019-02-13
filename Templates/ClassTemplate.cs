using SCore.Ads;
using System;
using UnityEngine;
using Zenject;

namespace SCore.Templates
{
    /// <summary>
    /// Controlling all ads platforms mediation
    /// </summary>
    public class ClassTemplate : MonoBehaviour
    {
        //DEPENDENCIES

        [Inject]
        private IAdsManager _adsManager;

        //EVENTS

        public event Action Completed;

        //STATIC

        private int _staticCounter;

        //EDITOR VARIABLES

        [SerializeField]
        private bool _editorVariable;

        //PRIVATE VARIABLES

        private bool _privateVariable;

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}