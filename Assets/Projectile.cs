using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject impactParticle;
    public float damage;
    private bool hasCollided = false;

    void OnCollisionEnter(Collision hit)
    {
        if (!hasCollided)
        {
            hasCollided = true;
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.identity);

            IHittable hittable = hit.collider.transform.root.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.Hit(hit.GetContact(0).point, Vector3.zero);
            }

            IDamageable damageable = hit.collider.transform.root.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.OnTakeDamage(damage);
            Destroy(impactParticle, 5f);
            Destroy(gameObject);


        }
    }
}