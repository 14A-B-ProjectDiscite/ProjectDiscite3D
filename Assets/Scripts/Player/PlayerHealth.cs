using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] GameEvent onDie;
    [SerializeField] GameEvent onDamaged;
    [SerializeField] FloatVariable currHealth;
    [SerializeField] FloatVariable maxHealth;
    public Material fullscreenShader;
    public float maxColorIntensity;
    public float colorIntensity;
    public float colorIntensityLoseRate;
    public float maxEffectIntensity;
    public float effectIntensity;
    public float effectIntensityLoseRate;

    // Update is called once per frame
    void Update()
    {
        colorIntensity -= Time.deltaTime * colorIntensityLoseRate;
        colorIntensity = Mathf.Max(0, colorIntensity);
        fullscreenShader.SetFloat("_ColorIntensity", colorIntensity);
        effectIntensity -= Time.deltaTime * effectIntensityLoseRate;
        effectIntensity = Mathf.Max(0, effectIntensity);
        fullscreenShader.SetFloat("_FullscreenIntensity", effectIntensity);
    }

    private void Die()
    {
        onDie.Raise();
        Debug.Log("Dead");
        SceneManager.LoadScene("DeadScene");
    }

    public void OnTakeDamage(float Damage)
    {
        onDamaged.Raise();
        currHealth.Value -= Damage;
        colorIntensity = maxColorIntensity;
        effectIntensity = maxEffectIntensity;
        fullscreenShader.SetFloat("_ColorIntensity", colorIntensity);
        fullscreenShader.SetFloat("_FullscreenIntensity", effectIntensity);

        if (currHealth.Value <= 0)
        {
            Die();
        }
    }
}
