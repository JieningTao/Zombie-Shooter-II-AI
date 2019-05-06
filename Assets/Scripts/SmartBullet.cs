using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartBullet : BulletScript
{
    public GameObject Target;
    public float turnPower;

    private float despawnTimer;
    private Rigidbody2D myRigidBody2D;
    

    private void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, despawnTime);
        myRigidBody2D.velocity = transform.up * bulletSpeed;
        
    }

    private void FixedUpdate()
    {
        if (!Target)
        {

        }
        else
        {
            
            if (GetDegreeBetweenTargetAndHeading() < 1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(transform.position.y - Target.transform.position.y, transform.position.x - Target.transform.position.x) + 90);
                myRigidBody2D.velocity = transform.up * bulletSpeed;
            }
            else if (GetDegreeBetweenTargetAndHeading() < 135)
            {
                float currentDegree = transform.rotation.z;
                float targetDegree = Mathf.Rad2Deg * (Mathf.Atan2(transform.position.y - Target.transform.position.y, transform.position.x - Target.transform.position.x));

                if (currentDegree < targetDegree)
                {
                    RotateLeft();
                }
                else
                {
                    RotateRight();
                }
            }
            
            //transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(transform.position.y - Target.transform.position.y, transform.position.x - Target.transform.position.x) + 90);
        }
       
    }

    private float GetDegreeBetweenTargetAndHeading()
    {
       Debug.Log(Mathf.Abs(Vector2.Angle((transform.position - Target.transform.position).normalized, myRigidBody2D.velocity.normalized) -180));
       
        return Mathf.Abs(Vector2.Angle((transform.position - Target.transform.position).normalized, myRigidBody2D.velocity.normalized)-180);
        
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.up * 3, Color.red);
    }

    private void RotateLeft()
    {
        transform.Rotate(Vector3.forward, -turnPower * Time.deltaTime);
        myRigidBody2D.velocity = transform.up * bulletSpeed;
    }

    private void RotateRight()
    {
        transform.Rotate(Vector3.forward, turnPower * Time.deltaTime);
        myRigidBody2D.velocity = transform.up * bulletSpeed;
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.gameObject.GetComponent<ZombieScript>().HitByBullet(this);
            if (!penetrate)
            {
                Destroy(this.gameObject, 0);
                myRigidBody2D.velocity = Vector2.zero;
            }
        }
        else
        {
            Destroy(this.gameObject, 0);
            myRigidBody2D.velocity = Vector2.zero;
        }
    }


}
