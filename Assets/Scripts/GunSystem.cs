using LlamAcademy.Guns;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public interface IHittable
{
    public void Hit(Vector3 hitPoint, Vector3 normal);
}

public class GunSystem : MonoBehaviour
{
    [Header("Gun Type")]
    public GunType gunType;
    [Header("Gun stats")]
    [SerializeField]
    FloatVariable AttackSpeedModifier;
    [SerializeField]
    FloatVariable DamageModifier;
    [SerializeField]
    FloatVariable BulletsPerTapModifier;
    [SerializeField]
    BoolVariable AllowHoldingDown;
    [Header("RayCastSettings")]
    public int damage;
    [Header("Projectile Settings")]
    //Gun stats
    public GameObject Projectile;
    public float projectileForce;
    [Header("Universal Settings")]
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int bulletsPerTap;
    public bool allowButtonHold;
    int bulletsShot;
    [SerializeField]
    private LineRenderer BulletTrail;

    [Header("Recoil")]
    //Recoil
    public Transform recoilMod;
    public GameObject weapon;
    public SpreadConfig spreadConfig;


    private Quaternion defRecoilModRot;
    private Quaternion defWeaponRot;
    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    [Header("Reference")]
    public Rigidbody playerRb;
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsHittable;


    //Graphics
    [Header("Graphics")]
    public GameObject muzzleFlash, bulletHoleGraphic;
    [Header("Camera Shake")]
    public float camShakeMagnitude, camShakeFadeIn, camShakeFadeOut, camShakeRoughness;
    private void Start()
    {
        defRecoilModRot = recoilMod.rotation;
        defWeaponRot = weapon.transform.rotation;
    }
    private void Awake()
    {
        readyToShoot = true;
    }
    private void Update()
    {
        RecoverRecoil();
        MyInput();
    }
    void RecoverRecoil()
    {
        recoilMod.transform.localRotation = Quaternion.Lerp(
                recoilMod.transform.localRotation,
                Quaternion.Euler(Quaternion.identity.eulerAngles),
                Time.deltaTime * spreadConfig.RecoilRecoverySpeed
            );

    }
    void RecoilOnce()
    {
        Vector3 spreadAmount = spreadConfig.GetSpread(10);

        Vector3 shootDirection = Vector3.zero;
        recoilMod.transform.forward += recoilMod.transform.TransformDirection(spreadAmount);
    }

    private void MyInput()
    {
        if (allowButtonHold || AllowHoldingDown.Value) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Shoot
        if (readyToShoot && shooting && !reloading)
        {
            bulletsShot = bulletsPerTap + Mathf.RoundToInt(BulletsPerTapModifier.Value) ;
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = weapon.transform.forward + new Vector3(x, y, 0);
        if (gunType == GunType.Raycast)
        {
            //RayCast
            if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsHittable))
            {
                IHittable hittable = rayHit.collider.transform.root.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.Hit(rayHit.point, rayHit.normal);
                }
                if (!rayHit.collider.CompareTag("Player"))
                {
                    IDamageable damageable = rayHit.collider.transform.root.GetComponent<IDamageable>();
                    if (damageable != null)
                        damageable.OnTakeDamage(damage * DamageModifier.Value);
                }
                

                //LineRenderer trail = Instantiate(BulletTrail, attackPoint.position, attackPoint.rotation, attackPoint);
                //float dist = Vector3.Distance(attackPoint.position, rayHit.point);
                //trail.SetPosition(1, new Vector3(0,0, dist));
                //Destroy(trail, 0.05f);
            }
            else
            {
                //LineRenderer trail = Instantiate(BulletTrail, attackPoint.position, Quaternion.identity, attackPoint);
                //trail.SetPosition(0, attackPoint.position);
                //trail.SetPosition(1, attackPoint.position + attackPoint.forward * -100);
                //Destroy(trail, 1);
            }
        }
        else if (gunType == GunType.Projectile)
        {
            GameObject projectileGO = Instantiate(Projectile, attackPoint.position, attackPoint.rotation);
            Rigidbody rb = projectileGO.GetComponent<Rigidbody>();
            rb.velocity = playerRb.velocity; 
            rb.AddForce(attackPoint.forward * -projectileForce);


        }


        //ShakeCamera
        CameraShaker.Instance.ShakeOnce(camShakeMagnitude, camShakeRoughness, camShakeFadeIn, camShakeFadeOut);
        //Graphics
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, rayHit.normal);
        Instantiate(bulletHoleGraphic, rayHit.point, spawnRotation);
        Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation, attackPoint);
        bulletsShot--;
        RecoilOnce();
        Invoke("ResetShot", timeBetweenShooting * Mathf.Max(0.3f,  AttackSpeedModifier.Value));

        if (bulletsShot > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        reloading = false;
    }

}

public enum GunType
{
    Raycast,
    Projectile,
    Beam
}