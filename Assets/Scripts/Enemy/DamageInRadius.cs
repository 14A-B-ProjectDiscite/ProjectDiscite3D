using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInRadius : MonoBehaviour
{
    public Vector3 offset;
    public float radius;
    public Vector3 height;
    public float delay;
    public float damage;
    public float lifeTime;
    [SerializeField] Vector3Variable PlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Damage());
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(delay);
        Collider[] colls = Physics.OverlapCapsule(transform.position + offset, transform.position + offset + height, radius);
        foreach (Collider coll in colls)
        {
            if (coll.gameObject.CompareTag("Player"))
            {
                coll.GetComponent<PlayerHealth>().Damage(damage);
            }
        }
        float distanceToPlayer = (PlayerPosition.Value - transform.position).magnitude;
        //CameraShake
        CameraShaker.Instance.ShakeOnce(60f * 1 / distanceToPlayer, 4f, .1f, .1f);
        yield return new WaitForSeconds(lifeTime-delay);
        Destroy(gameObject);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
