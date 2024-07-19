using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScrollMaster2D.UI
{ 
    public class ExpBar : MonoBehaviour
    {
        public Slider slider;

        public void SetExp(float exp)
        {
            slider.value = exp;
        }
        public void SetMaxExp(float exp)
        {
            slider.value = exp;
            slider.maxValue = exp;
        }
        void Start()
        {
        
        }

        void Update()
        {
        
        }
    }
}
