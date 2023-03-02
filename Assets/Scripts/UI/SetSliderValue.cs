using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderValue : MonoBehaviour
{
    [SerializeField] FloatVariable value;
    [SerializeField] FloatVariable maxValue;
    public Slider slider;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxValue.Value;
        slider.value = value.Value;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = value.Value;
        text.text = slider.value.ToString() +  " / " +  slider.maxValue.ToString();
    }
}
