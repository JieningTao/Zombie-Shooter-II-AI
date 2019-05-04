using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartPistolScript : PistolScript
{
    [SerializeField]
    private LayerMask ZombieMarks;

    private List<GameObject> MarkedZombies;

    private bool firing;
    private float fireTimer;



    void Start()
    {
        MarkedZombies = new List<GameObject>();
        shellEject = GetComponent<ParticleSystem>();
        fireTimer = 0;
    }

    private void FixedUpdate()
    {


        if (firing)
        {
            FireAllSmart();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("mouse down");
                StopAllCoroutines();
                MarkedZombies.Clear();
            }
            else if (Input.GetMouseButton(0))
            {
                MarkZombies();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (MarkedZombies.Count > 0)
                {
                    firing = true;
                }
                //MarkedZombies.Clear();
            }
        }


    }

    protected void FireAllSmart()
    {
        if (MarkedZombies.Count > 0)
        {
            if (fireTimer < 0)
            {
                Fire(MarkedZombies[0]);
                MarkedZombies.Remove(MarkedZombies[0]);
                fireTimer = TBS;
            }
            else
                fireTimer-=Time.deltaTime;
        }
        else
        {
            firing = false;
        }
       // return null;
    }

    protected void MarkZombies()
    {
        Collider2D[] currentMarks;
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentMarks = Physics2D.OverlapCircleAll(pz, 5,ZombieMarks);
        foreach (Collider2D C in currentMarks)
        {
            if (!MarkedZombies.Contains(C.gameObject))
            {
                MarkedZombies.Add(C.gameObject);
                Debug.Log(C.gameObject.name + "Marked");
            }
                
        }
    }
    


    protected void Fire(GameObject target)
    {
        shellEject.Emit(1);
        GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation);
        SmartBullet bulletCloneScript = bulletClone.GetComponent<SmartBullet>();

        bulletCloneScript.damage = damage;
        bulletCloneScript.penetrate = penetrate;
        bulletCloneScript.Target = target;
    }
}
