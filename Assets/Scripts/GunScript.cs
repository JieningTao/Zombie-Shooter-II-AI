using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class GunScript : MonoBehaviour
{
    [SerializeField]
    protected GameObject bullet;

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
    }
}
