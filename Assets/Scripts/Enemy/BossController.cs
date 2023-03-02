using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] Vector3Variable target;
    Rigidbody rb;
    public float speed;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Vector3 TargetDir = target.Value - transform.position;
        if (TargetDir.magnitude >= 1)
        {
            Vector3 targetPos = transform.position + TargetDir.normalized * speed * Time.deltaTime;
            rb.MovePosition(targetPos);
        }

    }

}
