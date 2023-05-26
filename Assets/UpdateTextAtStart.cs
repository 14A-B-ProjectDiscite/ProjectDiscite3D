using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextAtStart : MonoBehaviour
{
    public bool isString;
    [SerializeField] StringVariable variable;
    [SerializeField] FloatVariable variable2;
    public string prefix;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        if (isString)
        {
            text.text = prefix + variable.Value;
        }
        else
        {
            text.text = prefix + variable2.Value;
        }
        
    }
}
