using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunProjectile : MonoBehaviour
{
    public GameObject bullet;

    public float shootForce, upwardForce;

    public float fireSpeed, Accuracy, reloadTime, fireRate;

    public int magSize, bulletsPerClick;

    public bool allowButtonHold;

    int remainingRounds, bulletsUsed;

    bool shooting, reloading, standby;

    public Camera fpsCam;

    public Transform attackPoint;

    public GameObject muzzleFlash;

    public TextMeshProUGUI ammoDisplay; 

    public bool allowInvoke = true;

    private void Awake()
    {
        remainingRounds = magSize;
        standby = true;
    }

    private void Update()
    {
        MyInput();

        if (ammoDisplay != null)
            ammoDisplay.SetText(remainingRounds / bulletsPerClick + " / " + magSize / bulletsPerClick);
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if(Input.GetKey(KeyCode.R) && remainingRounds < magSize && !reloading) Reload();

        if (standby && shooting && !reloading && remainingRounds <= 0) Reload();

        if (standby && shooting && !reloading && remainingRounds > 0)
        {
            bulletsUsed = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        standby = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(70);

        Vector3 directionWithoutAccuracy = targetPoint - attackPoint.position;

        float x = Random.Range(-Accuracy, Accuracy);
        float y = Random.Range(-Accuracy, Accuracy);

        Vector3 directionWithAccuracy = directionWithoutAccuracy + new Vector3(x,y,0);

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        currentBullet.transform.forward = directionWithAccuracy.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithAccuracy.normalized*shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up*upwardForce, ForceMode.Impulse);

        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        remainingRounds--;
        bulletsUsed++;

        if(allowInvoke)
        {
            Invoke("ResetShot", fireSpeed);
            allowInvoke = false;
        }
    }

    private void ResetShot()
    {
        standby = true;
        allowInvoke = true;

    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        remainingRounds = magSize;
        reloading = false;
    }
}
