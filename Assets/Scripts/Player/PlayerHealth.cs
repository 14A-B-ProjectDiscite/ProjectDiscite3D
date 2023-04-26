using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
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
        Debug.Log("Dead");
    }

    public void OnTakeDamage(float Damage)
    {
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
