using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFootstep : MonoBehaviour
{
    [SerializeField] Vector3Variable PlayerPosition;
    [SerializeField] GameObject Particles;
    [SerializeField] AudioSource source;
    public float distance;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ground"))
        {
            return;
        }
        Instantiate(Particles, transform.position, Quaternion.identity);
        float distanceToPlayer = (PlayerPosition.Value - transform.position).magnitude;
        //CameraShake
        if (distanceToPlayer < distance)
        {
            CameraShaker.Instance.ShakeOnce(60f * 1/distanceToPlayer, 4f, .1f, .1f);
        }
        //Sound Effect
        source.Play();
    }
}
