using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePosition : MonoBehaviour
{
    [SerializeField]
    Vector3Variable Pos;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Pos.Value, Vector3.up);
    }
}
