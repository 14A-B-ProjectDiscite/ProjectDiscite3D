using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    Buoyancy buyoScript;
    [SerializeField]
    Animator anim;
    [SerializeField]
    private float Health = 100;
    [SerializeField]
    private ProgressBar HealthBar;
    public GameObject DeathEffect;
    public float experienceOrbCount;
    public GameObject ExperienceOrb;
    public Transform bodyTransform;
    [SerializeField] SkinnedMeshRenderer renderer;

    private float MaxHealth;

    bool dead;
    private void Awake()
    {
        MaxHealth = Health;
    }

    public void OnTakeDamage(float Damage)
    {
        anim.SetTrigger("Take Damage");
        /*anim.SetTrigger("Spell Cast");
        anim.SetTrigger("Spit Poison Attack");
        anim.SetTrigger("Take Damage");*/
        Health -= Damage;
        HealthBar.SetProgress(Health / MaxHealth, 3);

        if (Health <= 0 && !dead)
        {
            OnDied();
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
        buyoScript.enabled = false;

        //anim.SetTrigger("Die");
        GameObject deathGO = Instantiate(DeathEffect, bodyTransform.position, Quaternion.identity, bodyTransform);
        renderer.enabled = false;
        Destroy(deathGO, 2f);
        Destroy(gameObject, 2f);
        HealthBar.DisableImages();

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