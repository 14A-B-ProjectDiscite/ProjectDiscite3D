using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [Header("General")]

    public float wallRunSmoothing = 1;
    public float wallRunZ = 20;

    public float headBopY = 1;
    public float gunBopY = .1f;
    public float headBopSmoothing = 1;
    public float headBoppingTime = .5f;

    private Transform head;
    [SerializeField]
    private Transform guntransform;

    private Vector3 headDefPos;
    private Vector3 gunDefPos;


    private bool wallrunning;
    private bool wallrunDir;
    private bool headBopping;
    private float headBoppingTimeLeft = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        wallrunning = false;
        gunDefPos = guntransform.localPosition;
        head = Movement.Instance.head;
        headDefPos = head.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = head.position;
        Vector3 gunPos = gunDefPos;
        if (headBopping)
        {

            headBoppingTimeLeft -= Time.deltaTime;
            targetPos.y = Mathf.Lerp(targetPos.y, targetPos.y - headBopY, Time.deltaTime * headBopSmoothing ) - (headBopY * (headBoppingTimeLeft / headBoppingTime));
            gunPos.y =  Mathf.Lerp(guntransform.localPosition.y, guntransform.localPosition.y - gunBopY, Time.deltaTime * headBopSmoothing * 3) - (gunBopY * (headBoppingTimeLeft / headBoppingTime));
            if (headBoppingTimeLeft < 0)
            {
                headBoppingTimeLeft = 0;    
                headBopping = false;
            }
        }
        else
        {
            gunPos.y = Mathf.Lerp(guntransform.localPosition.y, gunDefPos.y, Time.deltaTime * headBopSmoothing ) - (gunBopY * (headBoppingTimeLeft / headBoppingTime));
            
        }
        guntransform.localPosition = gunPos;

        transform.localPosition = targetPos;

        if (wallrunning && Input.GetAxisRaw("Vertical") == 1)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.LerpAngle(transform.rotation.eulerAngles.z,
                wallrunDir ? wallRunZ : -wallRunZ, Time.deltaTime * wallRunSmoothing));
        }
        else
        {
            if (Mathf.RoundToInt(transform.rotation.eulerAngles.z * 10) != 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.LerpAngle(transform.rotation.eulerAngles.z, 0, Time.deltaTime * wallRunSmoothing * 3f));
            }
        }
    }

    public void HeadBop()
    {
        headBoppingTimeLeft = headBoppingTime;
        headBopping = true;
    }

    public void StartWallrun(bool dir)
    {
        wallrunning = true;
        wallrunDir = dir;
    }

    public void StopWallrun()
    {
        wallrunning = false;
    }
}