using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFinder : MonoBehaviour
{
    public LayerMask groundLayer;
    public GameObject go;

    Vector3 GetSurfacePoint(Vector3 origin, Vector3 dir)
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin, dir, 20);
        for (int i = hits.Length-1; i >= 0; i--)
        {
            RaycastHit hit = hits[i];
            if ((groundLayer & 1 << hit.collider.gameObject.layer) == 1 << hit.collider.gameObject.layer)
            {
                return hit.point;
            }
        }
        return new Vector3(0, 10, 0);
    }
    // Update is called once per frame
    void Update()
    {
        go.transform.position = GetSurfacePoint(transform.position + transform.up *10, transform.up*-1);
    }
}
