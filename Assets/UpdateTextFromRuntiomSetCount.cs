using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextFromRuntiomSetCount : MonoBehaviour
{
    public Text text;
    public string message = "Remaining: ";
    public GameObjectRuntimeSet set;

    void Update()
    {
        text.text = message +  set.Items.Count;
    }
}
