using UnityEngine;
using Zenject;

namespace SCore.Analytics
{
    /// <summary>
    /// Track summary lifetime from install
    /// </summary>
    public class AnalyticsTimer : MonoBehaviour
    {
        //DEPENDENCIES

        [Inject] private IAnalyticsManager _analyticsManager;

        //EDITOR VARIABLES

        public int[] trackMinutePoints;

        //PRIVATE VARIABLES

        private const string PREFS_KEY_LAUNCHTIME = "secondsFromFirstLaunch";
        private const string PREFS_KEY_TRACKEDMINUTES = "trackedMinutes";
        private const float TRACK_TIME = 1.0F;

        private float _trackTimer;
        private float _timeFromFirstLaunch;
        private int _trackedMinutes;
        private bool _timerEnabled = false;

        // Use this for initialization
        private void Start()
        {
            _trackedMinutes = PlayerPrefs.GetInt(PREFS_KEY_TRACKEDMINUTES);
            _timerEnabled = false;

            for (int i = 0; i < trackMinutePoints.Length; i++)
            {
                if (trackMinutePoints[i] > _trackedMinutes)
                {
                    _timerEnabled = true;
                    break;
                }
            }

            if (_timerEnabled)
                _timeFromFirstLaunch = PlayerPrefs.GetFloat(PREFS_KEY_LAUNCHTIME);
        }

        // Update is called once per frame
        private void Update()
        {
            if (_timerEnabled)
            {
                _trackTimer += Time.unscaledDeltaTime;
                _timeFromFirstLaunch += Time.unscaledDeltaTime;
                if (_trackTimer > TRACK_TIME)
                {
                    _trackTimer = 0;
                    CheckPoint();
                }
            }
        }

        private void CheckPoint()
        {
            for (int i = 0; i < trackMinutePoints.Length; i++)
            {
                if (trackMinutePoints[i] > _trackedMinutes && Mathf.CeilToInt(_timeFromFirstLaunch / 60) > trackMinutePoints[i])
                    TrackNewPoint(trackMinutePoints[i]);
            }
        }

        private void TrackNewPoint(int _minutes)
        {
            _analyticsManager.DesignEvent("lifetime" + _minutes, _minutes);
            _trackedMinutes = _minutes;
            SaveValues();
        }

        private void SaveValues()
        {
            PlayerPrefs.SetInt(PREFS_KEY_TRACKEDMINUTES, _trackedMinutes);
            PlayerPrefs.SetFloat(PREFS_KEY_LAUNCHTIME, _timeFromFirstLaunch);
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause && _timerEnabled)
                SaveValues();
        }
    }
}