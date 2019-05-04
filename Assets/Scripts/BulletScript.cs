using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage;
    public bool penetrate;
    public float bulletSpeed;
    public float despawnTime;

    private Rigidbody2D myRigidBody2D;
    private float despawnTimer;

    private void Start()
    {
        Destroy(this.gameObject, despawnTime);
        myRigidBody2D = GetComponent<Rigidbody2D>();
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
            Destroy(this.gameObject,0);
            myRigidBody2D.velocity = Vector2.zero;
        }
    }

}
