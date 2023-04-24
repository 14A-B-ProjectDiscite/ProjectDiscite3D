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
    //Gun stats
    public GameObject Projectile;
    public float projectileForce;
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int bulletsPerTap;
    public bool allowButtonHold;
    int bulletsShot;
    public float BulletSpeed;
    [SerializeField]
    private TrailRenderer BulletTrail;

    [Header("Recoil")]
    //Recoil
    public Transform recoilMod;
    public GameObject weapon;
    public float maxRecoil_x = -20f;
    public float recoilSpeed = 10f;
    public float recoilRecoverySmoothing;
    private float recoil = 0f;
    public SpreadConfig spreadConfig;


    private Quaternion defRecoilModRot;
    private Quaternion defWeaponRot;
    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsHittable;


    //Graphics
    [Header("Graphics")]
    public GameObject muzzleFlash, bulletHoleGraphic;
    public float camShakeMagnitude, camShakeFadeIn, camShakeFadeOut, camShakeRoughness;
    public Text text;
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
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Shoot
        if (readyToShoot && shooting && !reloading)
        {
            bulletsShot = bulletsPerTap;
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
                Debug.Log(rayHit.collider.name);
                IHittable hittable = rayHit.collider.transform.root.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.Hit(rayHit.point, rayHit.normal);
                }

                IDamageable damageable = rayHit.collider.transform.root.GetComponent<IDamageable>();
                if (damageable != null)
                    damageable.OnTakeDamage(damage);

                TrailRenderer trail = Instantiate(BulletTrail, attackPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, rayHit.point, rayHit.normal, true));
            }
            else
            {
                TrailRenderer trail = Instantiate(BulletTrail, attackPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, rayHit.point, rayHit.normal, false));
            }
        }
        else if (gunType == GunType.Projectile)
        {
            GameObject projectileGO = Instantiate(Projectile, attackPoint.position, Quaternion.identity);
            projectileGO.GetComponent<Rigidbody>().AddForce(attackPoint.forward * -projectileForce);
        }


        //ShakeCamera
        CameraShaker.Instance.ShakeOnce(camShakeMagnitude, camShakeRoughness, camShakeFadeIn, camShakeFadeOut);
        //Graphics
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, rayHit.normal);
        Instantiate(bulletHoleGraphic, rayHit.point, spawnRotation);
        Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation, attackPoint);
        bulletsShot--;
        RecoilOnce();
        Invoke("ResetShot", timeBetweenShooting);

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

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        // This has been updated from the video implementation to fix a commonly raised issue about the bullet trails
        // moving slowly when hitting something close, and not
        Vector3 startPosition = Trail.transform.position;
        Vector3 desination = HitPoint;
        if (!MadeImpact)
        {
            desination = attackPoint.position + attackPoint.forward * -100;
        }
        float distance = Vector3.Distance(Trail.transform.position, desination);
        
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, desination, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        //Animator.SetBool("IsShooting", false);
        Trail.transform.position = desination;
        /*if (MadeImpact)
        {
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
        }*/

        Destroy(Trail.gameObject, Trail.time);
    }

}

public enum GunType
{
    Raycast,
    Projectile,
    Beam
}