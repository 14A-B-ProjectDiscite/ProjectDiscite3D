using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSky : MonoBehaviour
{
    public Vector3Variable PlayerPos;
    public string attackAnimName = "SpellCast";
    public float attackSpeed;
    float nextAttack;
    public Transform chargeTransform;
    public Animator anim;
    public GameObject AttackChargeEffect;
    public float chargeTime;
    public GameObject attack;
    private Vector3 attackPos;

    private void Start()
    {
        float r = Random.Range(attackSpeed / 2, attackSpeed * 1.5f);
        nextAttack = Time.time + r * 2;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextAttack)
        {
            GameObject go = Instantiate(AttackChargeEffect, chargeTransform.position, Quaternion.identity, chargeTransform);
            Destroy(go, chargeTime);
            float r = Random.Range(attackSpeed/2, attackSpeed*1.5f);
            nextAttack = Time.time + r;
            attackPos = PlayerPos.Value;
            anim.SetTrigger(attackAnimName);
            StartCoroutine(Launch());
        }
    }
    IEnumerator Launch() { 
        yield return new WaitForSeconds(chargeTime);
        Instantiate(attack, attackPos, Quaternion.identity);
    }

}
