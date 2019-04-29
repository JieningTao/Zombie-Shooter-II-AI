using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttractant : MonoBehaviour
{
    [SerializeField]
    private float initalRange;
    [SerializeField]
    private float finalRange;
    [SerializeField]
    private float spreadTime;
    [SerializeField]
    private LayerMask zombies;


    private float currentRange;
    private float endTime;
    private float differenceInRange;

    private void Start()
    {
        currentRange = initalRange;
        endTime = Time.fixedTime+spreadTime;
        differenceInRange = finalRange - initalRange;
    }

    private void FixedUpdate()
    {
        AttractAllInRange();
        ExpandRange();
    }

    private void ExpandRange()
    {
        if (Time.fixedTime >= endTime)
            EndAttraction();
        else
        {
            currentRange += differenceInRange / Time.deltaTime;
        }
    }


    private void AttractAllInRange()
    {
        Collider2D[] zombiesInRange = Physics2D.OverlapCircleAll(this.transform.position, currentRange, zombies);
        foreach (Collider2D Z in zombiesInRange)
        {
            ZombieScript zombieScript = Z.GetComponent<ZombieScript>();
            if(zombieScript.CurrentState != ZombieScript.ZombieState.Attracted)
            zombieScript.AttractedToLocation(transform.position);
        }
    }

    private void EndAttraction()
    {
        Collider2D[] zombiesInRange = Physics2D.OverlapCircleAll(this.transform.position, currentRange, zombies);

        foreach (Collider2D Z in zombiesInRange)
        {
            Z.GetComponent<ZombieScript>().AttractantDisappeared();
        }

        Destroy(this);
    }


}
