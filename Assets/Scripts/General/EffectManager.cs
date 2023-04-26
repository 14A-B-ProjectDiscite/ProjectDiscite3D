using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public Material fullscreenShader;

    public float fullscreen;
    public float color;
    public float fullscreenFadeSpeed;
    public float colorFadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        fullscreenShader.SetFloat("_FullscreenIntensity", 0);
        fullscreenShader.SetFloat("_ColorIntensity", 0);
    }

    // Update is called once per frame
    void Update()
    {
        fullscreen -= Time.deltaTime * fullscreenFadeSpeed;
        color -= Time.deltaTime * colorFadeSpeed;
        fullscreenShader.SetFloat("_FullscreenIntensity", fullscreen);
        fullscreenShader.SetFloat("_ColorIntensity", color);
    }
}
