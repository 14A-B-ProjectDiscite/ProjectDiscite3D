using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    Animator anim;
    [SerializeField]
    private float Health = 100;
    [SerializeField]
    private ProgressBar HealthBar;
    public GameObject DeathEffect;
    public float experienceOrbCount;
    public GameObject ExperienceOrb; 

    private NavMeshAgent Agent;
    private float MaxHealth;

    bool dead;
    private void Awake()
    {
        MaxHealth = Health;
        Agent = GetComponent<NavMeshAgent>();
    }

    public void OnTakeDamage(float Damage)
    {
        Health -= Damage;
        HealthBar.SetProgress(Health / MaxHealth, 3);

        if (Health <= 0 && !dead)
        {
            OnDied();
            Agent.enabled = false;
        }
    }

    private void OnDied()
    {
        dead = true;
        for (int i = 0; i < experienceOrbCount; i++)
        {
            float speed = Random.Range(5, 15);
            GameObject go = Instantiate(ExperienceOrb, transform.position, Quaternion.identity);
            go.GetComponent<HomingMissile>()._speed = speed;
        }

        anim.SetTrigger("Die");
        Instantiate(DeathEffect, transform.position, Quaternion.identity);
        Destroy(DeathEffect, 3f);
        Destroy(HealthBar.gameObject, 3.1f);
        Destroy(gameObject, 3f);
        
    }

    public void SetupHealthBar(Canvas Canvas, Camera Camera)
    {
        HealthBar.transform.SetParent(Canvas.transform);
        if (HealthBar.TryGetComponent<FaceCamera>(out FaceCamera faceCamera))
        {
            faceCamera.Camera = Camera;
        }
    }
}

public interface IDamageable
{
    public void OnTakeDamage(float Damage);
}