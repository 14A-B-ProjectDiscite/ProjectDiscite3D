using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponChest : MonoBehaviour, IInteractable
{
    [Header("Weapon Chest")]
    public string InteractableName;
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

    public string Name { get { return InteractableName; } set { InteractableName = value; } }

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
            rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
public interface IInteractable
{
    public string Name { get; set; }
    public void OnInteracted();
    public void OnSelected();

    public void OnDeselected();
}