using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] FloatVariable currHealth;
    [SerializeField] FloatVariable maxHealth;
    [SerializeField] GameEvent BossDamaged;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void OnDamaged()
    {

    }
}
