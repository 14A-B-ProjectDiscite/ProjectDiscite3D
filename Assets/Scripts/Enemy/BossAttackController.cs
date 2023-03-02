using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    public GameObject SkyAttackEffect;
    public float SkyAttackRate;
    public float SkyAttackRateIncrease;
    public Vector3Variable PlayerPos;
    private float nextSkyAttack;

    
    // Start is called before the first frame update
    void Start()
    {
        nextSkyAttack = Time.time + SkyAttackRate;
    }

    // Update is called once per frame
    void Update()
    {
        
        SkyAttack();
    }

    public void SkyAttack()
    {
        SkyAttackRate += SkyAttackRateIncrease * Time.deltaTime;
        if (Time.time >= nextSkyAttack)
        {
            Instantiate( SkyAttackEffect, new Vector3(PlayerPos.Value.x, 0, PlayerPos.Value.z), Quaternion.identity);
            nextSkyAttack = Time.time + SkyAttackRate;
        }
    }
}
