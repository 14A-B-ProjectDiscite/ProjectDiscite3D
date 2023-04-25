using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowScript : MonoBehaviour
{
    [SerializeField]
    Vector3Variable PlayerPos;
    Rigidbody rb;
    public float turnForce;

    float f_RotSpeed = 3.0f, f_MoveSpeed = 3.0f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 targetDelta = PlayerPos.Value - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, targetDelta);
        Vector3 turnAxis = Vector3.Cross(transform.forward, targetDelta);

        rb.AddTorque(turnAxis * angleToTarget * turnForce);

    }
}
