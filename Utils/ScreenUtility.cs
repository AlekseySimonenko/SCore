using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SCore
{
    public class ScreenUtility
    {
        public static float DeviceDiagonalSizeInInches()
        {
            float screenWidth = Screen.width / Screen.dpi;
            float screenHeight = Screen.height / Screen.dpi;
            float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
            return diagonalInches;
        }

        public static bool IsTablet()
        {
            if (Application.isEditor)
#if DEBUG_TABLET
                return true;
#else
                return false;
#endif
            else
                return DeviceDiagonalSizeInInches() > 6.5F;
        }
    }
}