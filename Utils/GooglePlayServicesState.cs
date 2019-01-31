using System;

namespace SCore.Utils
{
    public class GooglePlayServicesState
    {
        static public event Action ServicesAvailableEvent;

        static public event Action ServicesErrorEvent;

        static private bool available;
        static private bool checkRunning;

        private enum PlayServicesState
        {
            Available,
            Service_missing,
            UpdateRequired,
            ServiceDisabled,
            Updating,
            Invalid,
            Null
        }

        static private PlayServicesState GetPlayServicesState()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            const string GoogleApiAvailability_Classname =
                "com.google.android.gms.common.GoogleApiAvailability";
            AndroidJavaClass clazz =
                new AndroidJavaClass(GoogleApiAvailability_Classname);
            AndroidJavaObject obj =
                clazz.CallStatic<AndroidJavaObject>("getInstance");

            var androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = androidJC.GetStatic<AndroidJavaObject>("currentActivity");

            int value = obj.Call<int>("isGooglePlayServicesAvailable", activity);

            // result codes from https://developers.google.com/android/reference/com/google/android/gms/common/ConnectionResult

            // 0 == success
            // 1 == service_missing
            // 2 == update service required
            // 3 == service disabled
            // 18 == service updating
            // 9 == service invalid

            switch (value)
            {
                case 0:
                    return PlayServicesState.Available;

                case 1:
                    return PlayServicesState.Service_missing;

                case 2:
                    return PlayServicesState.UpdateRequired;

                case 3:
                    return PlayServicesState.ServiceDisabled;

                case 9:
                    return PlayServicesState.Invalid;

                case 18:
                    return PlayServicesState.Updating;

                default:
                    return PlayServicesState.Null;
            }
#else
            return PlayServicesState.Null;
#endif
        }

        // Use this for initialization
        static public bool IsAvaliable()
        {
            if (GetPlayServicesState() == PlayServicesState.Available)
                return true;
            else
                return false;
        }
    }
}