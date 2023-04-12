using UnityEngine;
using UnityEngine.UI;

public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //recoil
    public Transform recoilMod;
    public GameObject weapon;
    public float maxRecoil_x = -20f;
    public float recoilSpeed = 10f;
    private float recoil = 0f;
    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;


    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    //public CamShake camShake;
    public float camShakeMagnitude, camShakeFadeIn, camShakeFadeOut, camShakeRoughness;
    public Text text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        MyInput();

        //SetText
        text.text = bulletsLeft + " / " + magazineSize;
    }

    void Recoiling()
    {
        if (recoil > 0)
        {
            Quaternion maxRecoil = Quaternion.Euler(maxRecoil_x, 0f, 0f);
            // Dampen towards the target rotation
            recoilMod.rotation = Quaternion.Slerp(recoilMod.rotation, maxRecoil, Time.deltaTime * recoilSpeed);
            weapon.transform.localEulerAngles = new Vector3(recoilMod.localEulerAngles.x, weapon.transform.localEulerAngles.y, weapon.transform.localEulerAngles.z);
            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0;
            Quaternion minRecoil = Quaternion.Euler(0f, 0f, 0f);
            // Dampen towards the target rotation
            recoilMod.rotation = Quaternion.Slerp(recoilMod.rotation, minRecoil, Time.deltaTime * recoilSpeed / 2f);
            weapon.transform.localEulerAngles = new Vector3(recoilMod.localEulerAngles.x, weapon.transform.localEulerAngles.y, weapon.transform.localEulerAngles.z);
        }
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
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            IDamageable damageable = rayHit.collider.transform.root.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.OnTakeDamage(damage);
        }

        //ShakeCamera
        //camShake.Shake(camShakeDuration, camShakeMagnitude);
        CameraShaker.Instance.ShakeOnce(camShakeMagnitude, camShakeRoughness, camShakeFadeIn, camShakeFadeOut );
        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

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