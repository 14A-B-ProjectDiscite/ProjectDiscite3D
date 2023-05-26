using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpChest : MonoBehaviour, IInteractable
{
    [Header("Weapon Chest")]
    [SerializeField]
    FloatVariable Essence;
    public float price;
    public Outline outline;
    public Transform spawnPoint;
    public GameObject PowerUp;
    public GameObject Effect;
    [SerializeField]
    bool isBought;
    public string InteractableName;
    public string Name { get { return InteractableName; } set { InteractableName = value; } }

    private void Start()
    {
    }

    public void OnInteracted()
    {
        Buy();
    }

    public void OnSelected()
    {
        if (isBought && outline)
        {
            InteractableName = "Empty";
        }
        outline.enabled = true;
    }
    public void OnDeselected()
    {
        if (!outline)
        {
            return;
        }
        outline.enabled = false;
    }

    void Buy()
    {
        if (Essence.Value >= price && !isBought)
        {
            isBought = true;
            Essence.Value -= price;
            GameObject wep = Instantiate(PowerUp, spawnPoint.position, Quaternion.identity);
            GameObject effect = Instantiate(Effect, spawnPoint.position, spawnPoint.rotation);
            Destroy(effect, 2f);
            Destroy(gameObject);
        }
    }
}
