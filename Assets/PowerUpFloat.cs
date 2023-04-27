using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFloat : MonoBehaviour
{
    [SerializeField] FloatVariable variable;
    public float amount;
    bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (used)
        {
            return;
        }
        used = true;
        variable.Value += amount;
        Destroy(gameObject);
    }
}
