using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpiderProceduralAnimation : MonoBehaviour
{

    public LayerMask groundLayer;
    public Transform[] legTargets;
    public Transform[] legDesired;
    public float stepSize = 1f;
    public int smoothness = 1;
    public float stepHeight = 0.1f;
    public bool bodyOrientation = true;
    public bool evenGrounded = true;
    public bool oddGrounded = true;

    private Vector3[] defaultLegPositions;
    private Vector3[] targetLegPositions;
    private Quaternion[] targetLegRotations;
    private Vector3[] lastLegPositions;
    private Quaternion[] lastLegRotations;
    private bool[] legMoving;
    private int nbLegs;

    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastBodyPos;

    private float velocityMultiplier = 15f;


    //body
    public Transform bodyTransform;
    public float bodyHeightBase = 0.3f;

    private Vector3 bodyPos;
    private Vector3 bodyUp;
    private Vector3 bodyForward;
    private Vector3 bodyRight;
    private Quaternion bodyRotation;

    public float PosAdjustRatio = 0.1f;
    public float RotAdjustRatio = 0.2f;
    public bool isLegStaggeredMode = true;

    static Vector3[] MatchToSurfaceFromAbove(Vector3 point, Vector3 up)
    {
        Vector3[] res = new Vector3[2];
        RaycastHit hit;
        Ray ray = new Ray(point + up, -up);

        if (Physics.Raycast(ray, out hit, 2f))
        {
            res[0] = hit.point;
            res[1] = hit.normal;
        }
        else
        {
            res[0] = point;
        }
        return res;
    }
    Vector3[] GetSurfacePoint(Vector3 origin, Vector3 dir)
    {
        Vector3[] result = new Vector3[2];
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin, dir, 20);
        /*for (int i = hits.Length - 1; i >= 0; i--)
        {
            RaycastHit hit = hits[i];
            if ((groundLayer & 1 << hit.collider.gameObject.layer) == 1 << hit.collider.gameObject.layer)
            {
                result[0] = hit.point;
                result[1] = hit.normal;
                return result;
            }
        }*/
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if ((groundLayer & 1 << hit.collider.gameObject.layer) == 1 << hit.collider.gameObject.layer)
            {
                result[0] = hit.point;
                result[1] = hit.normal;
                return result;
            }
        }
        return result;
    }
    void Start()
    {
        SetDefaultValues();
        StartCoroutine(AdjustBodyTransform());
    }

    private void SetDefaultValues()
    {
        //lastBodyUp = transform.up;

        nbLegs = legTargets.Length;
        defaultLegPositions = new Vector3[nbLegs];
        targetLegPositions = new Vector3[nbLegs];
        targetLegRotations = new Quaternion[nbLegs];
        lastLegPositions = new Vector3[nbLegs];
        lastLegRotations = new Quaternion[nbLegs];
        legMoving = new bool[nbLegs];
        for (int i = 0; i < nbLegs; ++i)
        {
            defaultLegPositions[i] = legTargets[i].localPosition;
            targetLegPositions[i] = legTargets[i].localPosition;
            targetLegRotations[i] = legTargets[i].rotation;
            lastLegPositions[i] = legTargets[i].position;
            lastLegRotations[i] = legTargets[i].rotation;
            legMoving[i] = false;
        }
        lastBodyPos = transform.position;
    }

    IEnumerator PerformStep(int index, Vector3 targetPoint, Quaternion targetRot)
    {
        if (index % 2 == 0)
        {
            evenGrounded = false;
        }
        else
        {
            oddGrounded = false;
        }
        Quaternion startRot = lastLegRotations[index];
        Vector3 startPos = lastLegPositions[index];
        for (int i = 1; i <= smoothness; ++i)
        {
            legTargets[index].rotation = Quaternion.Lerp(startRot, targetRot, i / (float)(smoothness + 1f));
            legTargets[index].position = Vector3.Lerp(startPos, targetPoint, i / (float)(smoothness + 1f));
            legTargets[index].position += transform.up * Mathf.Sin(i / (float)(smoothness + 1f) * Mathf.PI) * stepHeight;
            yield return new WaitForFixedUpdate();
        }
        legTargets[index].position = targetPoint;
        lastLegPositions[index] = legTargets[index].position;
        legMoving[0] = false;
        if (index % 2 == 0)
        {
            evenGrounded = true;
        } else {
            oddGrounded = true;
        }
    }


    void FixedUpdate()
    {
        //Move Legs
        for (int i = 0; i < nbLegs; i++)
        {
            bool evenLeg = i % 2 == 0;
            legTargets[i].position = lastLegPositions[i];
            Vector3 r = transform.TransformPoint(legDesired[i].localPosition);
            Vector3[] hit = GetSurfacePoint(r + bodyTransform.up * 10, bodyTransform.up * -1);
            targetLegPositions[i] = hit[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit[1]);
            float distance = (targetLegPositions[i] - legTargets[i].position).magnitude;

            if (distance > stepSize)
            {
                if (!isLegStaggeredMode)
                {
                    StartCoroutine(PerformStep(i, targetLegPositions[i], rotation));
                }
                else
                {
                    if (evenLeg && oddGrounded || !evenLeg && evenGrounded)
                    {
                        StartCoroutine(PerformStep(i, targetLegPositions[i], rotation));
                    }
                }
            }
        }

        //MoveBodyPosition


        
        //get move direction
        /*velocity = transform.position - lastBodyPos;
        velocity = (velocity + smoothness * lastVelocity) / (smoothness + 1f);

        if (velocity.magnitude < 0.000025f)
            velocity = lastVelocity;
        else
            lastVelocity = velocity;*/

        //get desired positions
        /*Vector3[] desiredPositions = new Vector3[nbLegs];
        int indexToMove = -1;
        float maxDistance = stepSize;
        for (int i = 0; i < nbLegs; ++i)
        {
            desiredPositions[i] = transform.TransformPoint(defaultLegPositions[i]);

            float distance = Vector3.ProjectOnPlane(desiredPositions[i] + velocity * velocityMultiplier - lastLegPositions[i], transform.up).magnitude;
            if (distance > maxDistance)
            {
                maxDistance = distance;
                indexToMove = i;
            }
        }*/
        //setting all legs positin to the last leg pos if they are not the leg to move
        /*for (int i = 0; i < nbLegs; ++i)
            if (i != indexToMove)
                legTargets[i].position = lastLegPositions[i];*/
        /*//moving the legs
        if (indexToMove != -1 && !legMoving[0])
        {
            Vector3 targetPoint = desiredPositions[indexToMove] + Mathf.Clamp(velocity.magnitude * velocityMultiplier, 0.0f, 1.5f) * (desiredPositions[indexToMove] - legTargets[indexToMove].position) + velocity * velocityMultiplier;
            Vector3[] positionAndNormal = MatchToSurfaceFromAbove(targetPoint, transform.up);
            legMoving[0] = true;
            StartCoroutine(PerformStep(indexToMove, positionAndNormal[0]));
        }*/

    }


    private IEnumerator AdjustBodyTransform()
    {
        while (true)
        {
            Vector3 tipCenter = Vector3.zero;
            bodyUp = Vector3.zero;

            // Collect leg information to calculate body transform
            foreach (Transform leg in legTargets)
            {
                tipCenter += leg.position;
                bodyUp += leg.up;
            }

            RaycastHit hit;
            if (Physics.Raycast(bodyTransform.position, bodyTransform.up * -1, out hit, 10.0f))
            {
                bodyUp += hit.normal;
            }

            tipCenter /= targetLegPositions.Length;
            bodyUp.Normalize();

            // Interpolate postition from old to new
            bodyPos = tipCenter + bodyUp * bodyHeightBase;
            bodyTransform.position = Vector3.Lerp(bodyTransform.position, bodyPos, PosAdjustRatio);

            // Calculate new body axis
            bodyRight = Vector3.Cross(bodyUp, bodyTransform.forward);
            bodyForward = Vector3.Cross(bodyRight, bodyUp);
            
            // Interpolate rotation from old to new
            bodyRotation = Quaternion.LookRotation(bodyForward, bodyUp);
            bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, bodyRotation, RotAdjustRatio);

            yield return new WaitForFixedUpdate();
        }
    }



    private void OnDrawGizmos()
    {
        for (int i = 0; i < nbLegs; ++i)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(legTargets[i].position, 0.05f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.TransformPoint(legDesired[i].localPosition), stepSize);
        }
        Gizmos.color = Color.white;

    }

    
}
