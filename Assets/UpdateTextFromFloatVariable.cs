using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextFromFloatVariable : MonoBehaviour
{
    public Text text;
    public string message = "Essence: ";
    public FloatVariable variable;

    void Update()
    {
        text.text = String.Format(message + "{0:0,0}", variable.Value);
    }
}
