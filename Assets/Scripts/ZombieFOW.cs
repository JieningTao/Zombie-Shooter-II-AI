using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFOW : MonoBehaviour
{
    [SerializeField]
    private float SightDistance = 10;
    [SerializeField]
    [Range(0, 360)]
    private float SightAngle = 10;

    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private LayerMask obstacleMask;


    void FindVisibleTargets()
    {
        Collider2D[] targetsInVisibleRadius = Physics2D.OverlapCircleAll(transform.position, SightDistance, targetMask);

        for (int i = 0; i < targetsInVisibleRadius.Length; i++)
        {
            Transform target = targetsInVisibleRadius[i].transform;
            Vector2 dirToTarget = target.position - transform.position;
            dirToTarget.Normalize();
            if (Vector2.Angle(transform.right, dirToTarget) < SightAngle / 2)
            {
                float distoTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distoTarget, obstacleMask))
                {
                    GetComponent<ZombieScript>().ChasePlayerAt(new Vector2(target.position.x,target.position.y));


                    Debug.Log("player seen by:" + name);
                }
            }


        }
            

    }
    private Vector2 DirectionFromAngle(float angleInDegrees)
    {
        angleInDegrees -= transform.eulerAngles.z;
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void FixedUpdate()
    {
        FindVisibleTargets();
        debugdraw();
    }

    private void debugdraw()
    {
        Debug.DrawRay(transform.position, ( DirectionFromAngle(90 - SightAngle/2)) * SightDistance, Color.red);
        Debug.DrawRay(transform.position, DirectionFromAngle(90 + SightAngle / 2) * SightDistance, Color.red);

        //Debug.DrawRay(transform.position, (new Vector3(transform.right.x + DirectionFromAngle(SightAngle / 2).normalized.x, transform.right.y + DirectionFromAngle(SightAngle / 2).normalized.y,0) ) * SightDistance, Color.red);
        //Debug.DrawRay(transform.position, (new Vector3(transform.right.x - DirectionFromAngle(SightAngle / 2).normalized.x, transform.right.y - DirectionFromAngle(SightAngle / 2).normalized.y, 0)) * SightDistance, Color.red);
    }


}
