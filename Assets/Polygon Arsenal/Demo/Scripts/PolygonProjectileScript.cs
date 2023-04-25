using UnityEngine;
using System.Collections;
using UnityEngine.Animations.Rigging;

namespace PolygonArsenal
{
    public class PolygonProjectileScript : MonoBehaviour
    {
        public ProjectileType type;
        public float explosionRange;
        public float explosionForce;
        public GameObject impactParticle;
        public float damage;
        public LayerMask HitLayer;
        private bool hasCollided = false;

        void OnCollisionEnter(Collision hit)
        {
            if (hit.transform.CompareTag("Player") || hasCollided)
            {
                return;
            }

            hasCollided = true;
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.identity);

            if (type == ProjectileType.Normal)
            {
                IHittable hittable = hit.collider.transform.root.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.Hit(hit.GetContact(0).point, Vector3.zero);
                }

                IDamageable damageable = hit.collider.transform.root.GetComponent<IDamageable>();
                if (damageable != null)
                    damageable.OnTakeDamage(damage);
            }
            else if (type == ProjectileType.Explosive)
            {
                Collider[] Hits = Physics.OverlapSphere(transform.position, explosionRange, HitLayer);

                foreach (Collider col in Hits)
                {
                    Rigidbody rb = col.GetComponent<Rigidbody>();
                    if (rb)
                    {
                        float distance = Vector3.Distance(transform.position, col.transform.position);

                        rb.AddExplosionForce(explosionForce, transform.position, explosionRange);

                    }
                    IDamageable damageable = col.transform.root.GetComponent<IDamageable>();
                    if (damageable != null)
                        damageable.OnTakeDamage(damage);
                }
            }
            Destroy(impactParticle, 5f);
            Destroy(gameObject);


            
        }
    }
    public enum ProjectileType
    {
        Normal,
        Explosive
    }
}