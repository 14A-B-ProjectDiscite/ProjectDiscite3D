using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSliderFromHealth : MonoBehaviour
{
    [SerializeField]
    FloatVariable health;

    [SerializeField]
    FloatVariable maxHealth;

    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxHealth.Value;
    }

    // Update is called once per frame
    void Update()
    {
        slider.maxValue = maxHealth.Value;
        slider.value = health.Value;
    }
}
