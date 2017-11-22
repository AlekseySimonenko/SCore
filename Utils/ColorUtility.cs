using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Core
{
    public class ColorUtility
    {
        static public Color ColorLerp(Color _startColor, Color _endColor, float _value)
        {
            Color returnedColor = new Color(_startColor.r + ((_endColor.r - _startColor.r)*_value), _startColor.g + ((_endColor.g - _startColor.g) * _value), _startColor.b + ((_endColor.b - _startColor.b) * _value), _startColor.a + ((_endColor.a - _startColor.a) * _value) );
            return returnedColor;
        }
    }
}