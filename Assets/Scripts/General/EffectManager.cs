using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public Material fullscreenShader;
    // Start is called before the first frame update
    void Start()
    {
        fullscreenShader.SetFloat("_FullscreenIntensity", 0);
        fullscreenShader.SetFloat("_ColorIntensity", 0);
    }
}
