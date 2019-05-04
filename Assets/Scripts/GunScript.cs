using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class GunScript : MonoBehaviour
{
    [SerializeField]
    protected GameObject bullet;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected bool penetrate;
    [SerializeField]
    protected float despawnTime;
    [SerializeField]
    protected float TBS;


    protected ParticleSystem shellEject;
    
    
    // Start is called before the first frame update
    void Start()
    {
        shellEject = GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    virtual protected void Fire()
    {
        shellEject.Emit(1);
        GameObject bulletClone = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        BulletScript bulletCloneScript = bulletClone.GetComponent<BulletScript>();
        bulletCloneScript.damage = damage;
        bulletCloneScript.penetrate = penetrate;

    }
}
