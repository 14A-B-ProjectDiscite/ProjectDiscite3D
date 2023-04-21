using LlamAcademy.Guns;
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
    public GunType GunType;
    [Header("Gun stats")]
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

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
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        RecoverRecoil();
        MyInput();

        //SetText
        text.text = bulletsLeft + " / " + magazineSize;


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

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
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

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsHittable))
        {
            Debug.Log(rayHit.collider.name);
            IHittable hittable = rayHit.collider.transform.root.GetComponent<IHittable>();
            if (hittable != null)
                hittable.Hit(rayHit.point, rayHit.normal);
            IDamageable damageable = rayHit.collider.transform.root.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.OnTakeDamage(damage);
        }

        //ShakeCamera
        CameraShaker.Instance.ShakeOnce(camShakeMagnitude, camShakeRoughness, camShakeFadeIn, camShakeFadeOut );
        //Graphics
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, rayHit.normal);
        Instantiate(bulletHoleGraphic, rayHit.point, spawnRotation);
        Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation, attackPoint);

        bulletsLeft--;
        bulletsShot--;
        RecoilOnce();
        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
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
        bulletsLeft = magazineSize;
        reloading = false;
    }
}

public enum GunType
{
    Raycast,
    Projectile,
    Beam
}