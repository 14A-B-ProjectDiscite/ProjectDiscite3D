using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowVector3 : MonoBehaviour
{
    [SerializeField] 
    Vector3Variable pos;
    public Vector3 offset;
    void Update()
    {
        transform.position = pos.Value + offset;
    }
}
