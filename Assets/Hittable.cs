using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour, IHittable
{
    public GameObject prefab;
    public void Hit(Vector3 hitPoint, Vector3 normal)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, normal);
        Instantiate(prefab, hitPoint, spawnRotation);
    }

}
