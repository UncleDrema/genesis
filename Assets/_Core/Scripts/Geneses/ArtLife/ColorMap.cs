using System;
using System.Collections.Generic;
using UnityEngine;

namespace Geneses.ArtLife
{
    [CreateAssetMenu(fileName = "ColorMap", menuName = "Geneses/ColorMap", order = 1)]
    public class ColorMap : ScriptableObject
    {
        [SerializeField]
        private List<float> _scalars = new List<float>();
        [SerializeField]
        private List<Color> _colors = new List<Color>();

        public void Initialize(List<float> scalars, List<Color> colors)
        {
            _scalars = scalars;
            _colors = colors;
        }

        public Color GetColor(float value)
        {
            if (_scalars.Count == 0 || _colors.Count == 0)
            {
                throw new InvalidOperationException("Color map is not initialized.");
            }

            value = Mathf.Clamp01(value);

            for (int i = 0; i < _scalars.Count - 1; i++)
            {
                if (value <= _scalars[i + 1])
                {
                    // Linear interpolation
                    var t = (value - _scalars[i]) / (_scalars[i + 1] - _scalars[i]);
                    return Color.Lerp(_colors[i], _colors[i + 1], t);
                }
            }

            // If value is exactly 1.0, return the last color
            return _colors[^1];
        }
    }
}