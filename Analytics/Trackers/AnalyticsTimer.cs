using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore.Analytics
{
    /// <summary>
    /// Track summary lifetime from install
    /// </summary>
    public class AnalyticsTimer : MonoBehaviour
    {
        //PUBLIC STATIC
        //PUBLIC EVENTS
        //PUBLIC VARIABLES
        public int[] trackMinutePoints;

        //PRIVATE STATIC
        private const string PREFS_KEY_LAUNCHTIME = "secondsFromFirstLaunch";
        private const string PREFS_KEY_TRACKEDMINUTES = "trackedMinutes";
        private const float TRACK_TIME = 1.0F;

        //PRIVATE VARIABLES
        private float trackTimer;
        private float timeFromFirstLaunch;
        private int trackedMinutes;

        private bool timerEnabled = false;

        // Use this for initialization
        void Start()
        {
            trackedMinutes = PlayerPrefs.GetInt(PREFS_KEY_TRACKEDMINUTES);
            timerEnabled = false;

            for (int i = 0; i < trackMinutePoints.Length; i++)
            {
                if (trackMinutePoints[i] > trackedMinutes)
                {
                    timerEnabled = true;
                    break;
                }
            }

            if (timerEnabled)
                timeFromFirstLaunch = PlayerPrefs.GetFloat(PREFS_KEY_LAUNCHTIME);
        }

        // Update is called once per frame
        void Update()
        {
            if (timerEnabled)
            {
                trackTimer += Time.unscaledDeltaTime;
                timeFromFirstLaunch += Time.unscaledDeltaTime;
                if (trackTimer > TRACK_TIME)
                {
                    trackTimer = 0;
                    CheckPoint();
                }
            }
        }

        void CheckPoint()
        {
            for (int i = 0; i < trackMinutePoints.Length; i++)
            {
                if (trackMinutePoints[i] > trackedMinutes && Mathf.CeilToInt(timeFromFirstLaunch / 60) > trackMinutePoints[i])
                    TrackNewPoint(trackMinutePoints[i]);
            }
        }

        void TrackNewPoint(int _minutes)
        {
            AnalyticsManager.DesignEvent("lifetime" + _minutes, _minutes);
            trackedMinutes = _minutes;
            SaveValues();
        }

        void SaveValues()
        {
            PlayerPrefs.SetInt(PREFS_KEY_TRACKEDMINUTES, trackedMinutes);
            PlayerPrefs.SetFloat(PREFS_KEY_LAUNCHTIME, timeFromFirstLaunch);
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause && timerEnabled)
                SaveValues();
        }

    }
}

