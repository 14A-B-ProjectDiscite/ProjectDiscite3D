using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Vector3Variable cameraPos;
    [SerializeField]
    private float Health = 100;
    [SerializeField]
    private ProgressBar HealthBar;
    [SerializeField] 
    private float MaxHealth;

    private void Awake()
    {
        MaxHealth = Health;
    }

    public void OnTakeDamage(float Damage)
    {
        Health -= Damage;
        HealthBar.SetProgress(Health / MaxHealth, 3);

        if (Health < 0)
        {
            OnDied();
        }
    }

    private void OnDied()
    {
        Destroy(gameObject, 1f);
        Destroy(HealthBar.gameObject, 1f);
    }

    public void SetupHealthBar(Canvas Canvas)
    {
        HealthBar.transform.SetParent(Canvas.transform);
        
    }
    private void Update()
    {
        HealthBar.transform.LookAt(cameraPos.Value, Vector3.up);
    }
}

public interface IDamageable
{
    public void OnTakeDamage(float Damage);
}