using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Geneses.ArtLife.UI
{
    public class SliderView : MonoBehaviour
    {
        public Slider Slider;
        public TMP_Text MinText;
        public TMP_Text MaxText;
        public TMP_Text TitleText;
        
        public void SetValueMinMaxAndTitle(float min, float max, string title)
        {
            Slider.minValue = min;
            Slider.maxValue = max;
            MinText.text = min.ToString();
            MaxText.text = max.ToString();
            TitleText.text = title;
        }
    }
}