using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    GameObjectRuntimeSet Monsters;
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
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    public GameObject damageText;
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
        GameObject damageT = Instantiate(damageText, bodyTransform.position, Quaternion.identity);
        damageT.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(String.Format("{0:0,0}", Damage));
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
            float speed = UnityEngine.Random.Range(5, 15);
            GameObject go = Instantiate(ExperienceOrb, transform.position, Quaternion.identity);
            go.GetComponent<HomingMissile>()._speed = speed;
        }
        Monsters.Items.Remove(gameObject);
        buyoScript.enabled = false;

        //anim.SetTrigger("Die");
        GameObject deathGO = Instantiate(DeathEffect, bodyTransform.position, Quaternion.identity, bodyTransform);
        meshRenderer.enabled = false;
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