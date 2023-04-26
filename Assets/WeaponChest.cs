using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponChest : MonoBehaviour, IInteractable
{
    [Header("Weapon Chest")]
    [SerializeField]
    FloatVariable Essence;
    public float price;
    public Outline outline;
    public Transform spawnPoint;
    public GameObject Weapon;
    public GameObject Effect;
    public float upForce;
    bool isBought;
    [Header("Refs")]
    private Rigidbody playerRb;
    public Transform Player;
    public Camera fpsCam;
    public Transform recoilMod;
    public Transform gunContainer;

    private void Start()
    {
        playerRb = Player.GetComponent<Rigidbody>();
    }

    public void OnInteracted()
    {
        Buy();
    }

    public void OnSelected()
    {
        if (isBought)
        {
            return;
        }
        outline.enabled = true;
    }
    public void OnDeselected()
    {
        outline.enabled = false;
    }

    private Transform highlight;
    private Transform selection;

    void Buy()
    {
        if (Essence.Value >= price && !isBought)
        {
            isBought = true;
            Essence.Value -= price;
            GameObject wep = Instantiate(Weapon, spawnPoint.position, Quaternion.identity);
            Rigidbody rb = wep.GetComponent<Rigidbody>();
            PickUpController controller = wep.GetComponent<PickUpController>();
            controller.player = Player;
            controller.gunContainer = gunContainer;
            controller.fpsCam = fpsCam.transform;
            GunSystem system = wep.GetComponent<GunSystem>();
            system.fpsCam = fpsCam;
            system.recoilMod = recoilMod;
            system.playerRb = playerRb;
            rb.AddForce(Vector3.up * upForce);
        }
    }
}
public interface IInteractable
{
    public void OnInteracted();
    public void OnSelected();

    public void OnDeselected();
}