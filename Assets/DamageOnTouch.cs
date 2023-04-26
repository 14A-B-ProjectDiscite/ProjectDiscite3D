using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public float damage;
    public float explosionForce;
    public float explosionRange;
    bool damaged;
    
    private void OnTriggerEnter(Collider other)
    {
        if (damaged) { return; }
        damaged = true;
        IDamageable healthScript = other.transform.root.GetComponent<IDamageable>();
        if (healthScript != null)
        {
            healthScript.OnTakeDamage(damage);
        }
        Rigidbody rb = other.transform.root.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(explosionForce, transform.position, explosionRange);
        }
        Destroy(gameObject);
    }
}
