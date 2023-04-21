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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
