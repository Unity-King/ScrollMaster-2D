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
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
