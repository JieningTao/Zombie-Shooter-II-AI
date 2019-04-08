using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolScript : GunScript
{

    // Start is called before the first frame update
    void Start()
    {
        shellEject = GetComponent<ParticleSystem>();
    }




    protected override void  Fire()
    {
        base.Fire();
    }


}
