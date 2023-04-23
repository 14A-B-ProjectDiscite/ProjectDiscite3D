using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    Vector3Variable PlayerPos;
    public GameObject BuffEffect;
    public GameObject AureEffect;
    public GameObject LaunchEffect;
    public GameObject CollideEffect;
    public Rigidbody rb;
    public float delay;
    public float speed;
    public float upForce;
    bool isFlying;

    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartFlying());
    }

    IEnumerator StartFlying()
    {
        GameObject aureEff = Instantiate(AureEffect, transform.position, Quaternion.identity, transform);
        
        yield return new WaitForSeconds(delay);
        Destroy(aureEff );
        rb.AddForce(Vector3.up * upForce);
        GameObject launchEff = Instantiate(LaunchEffect, transform.position, Quaternion.identity);
        Destroy(launchEff, 3);
        isFlying = true;
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlying)
        {
            Vector3 dir = PlayerPos.Value - transform.position;
            rb.AddForce(dir * speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (isFlying)
        {
            GameObject collEff = Instantiate(CollideEffect, transform.position, Quaternion.identity);
            Destroy(collEff, 3);
            if (collision.collider.CompareTag("Player"))
            {
                Instantiate(BuffEffect, transform.position, Quaternion.Euler(90, 0, 0));
            }
            else
            {
                isFlying = false;
                rb.velocity = Vector3.zero;
                rb.useGravity = true;
                StartCoroutine(StartFlying());
            }
        }
    }
}
