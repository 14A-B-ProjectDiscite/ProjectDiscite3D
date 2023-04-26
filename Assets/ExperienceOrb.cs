using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    public float score;
    public GameObject CollideEffect;
    public GameObject BuffEffect;
    [SerializeField]
    FloatVariable Score;
    [SerializeField]
    FloatVariable Essence;
    public float EssenceValue;

    private void OnTriggerEnter(Collider other)
    {
        GameObject collEff = Instantiate(CollideEffect, transform.position, Quaternion.identity);
        Destroy(collEff, 3);
        if (other.CompareTag("Player"))
        {
            Instantiate(BuffEffect, transform.position, Quaternion.Euler(90, 0, 0));
            Score.Value += score;
            Destroy(gameObject);
            Essence.Value += Random.Range(EssenceValue * .75f, EssenceValue * 1.5f);
        }
    } 
}
