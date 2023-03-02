using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public Material fullscreenShader;

    public float fullscreen;
    public float color;
    // Start is called before the first frame update
    void Start()
    {
        fullscreenShader.SetFloat("_FullscreenIntensity", fullscreen);
        fullscreenShader.SetFloat("_ColorIntensity", color);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
