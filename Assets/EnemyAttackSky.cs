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
        nextAttack = Time.time + attackSpeed * 2;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextAttack)
        {
            GameObject go = Instantiate(AttackChargeEffect, chargeTransform.position, Quaternion.identity, chargeTransform);
            Destroy(go, chargeTime);
            nextAttack = Time.time + attackSpeed;
            attackPos = PlayerPos.Value;
            anim.SetTrigger(attackAnimName);
            StartCoroutine(Launch());
        }
    }
    IEnumerator Launch() { 
        yield return new WaitForSeconds(chargeTime);
        Instantiate(attack, PlayerPos.Value, Quaternion.identity);
    }

}
