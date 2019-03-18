using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D ZombieRigidBody2D;

    [SerializeField]
    private float ChaseSpeed = 5.0f;
    [SerializeField]
    private float WanderSpeed = 5.0f;
    [SerializeField]
    private int WanderingDecisionInterval = 100;

    private int WanderTimer;
    private int WanderDecisionMakingTime;
    private int RandomNum;
    private Vector2 CurrentDirection;

    public enum ZombieState
    {
        Wandering,
        Investigate,
        Chase,
        Dead,
    }

    private ZombieState CurrentState;


    // Start is called before the first frame update
    void Start()
    {
        WanderTimer = 0;
        CurrentState = ZombieState.Wandering;
        WanderDecisionMakingTime = Random.Range(0, WanderingDecisionInterval);
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case ZombieState.Wandering: UpdateWanderState(); break;


        }
        Movement();
    }





    private void Movement()
    {

        if (CurrentState == ZombieState.Wandering)
            ZombieRigidBody2D.velocity = CurrentDirection * WanderSpeed;
        else
            ZombieRigidBody2D.velocity = CurrentDirection * ChaseSpeed;
    }

    private void UpdateWanderState()
    {



        //Everytime the Wander time is 0, a decision is made, usually random direction;
        if (WanderTimer == WanderDecisionMakingTime)
        {
            RandomNum = Random.Range(0, 10);
            if (RandomNum < 4) //40% chance of random direction movement;
            {
                CurrentDirection = RandomDirection();
            }
            else // else stop and do nothing;
            {
                CurrentDirection = Vector2.zero;
            }
        }

        WanderTimer++;

        if (WanderTimer == WanderingDecisionInterval)
            WanderTimer = 0;
    }

    private Vector2 RandomDirection()
    {
        return new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    }




}
