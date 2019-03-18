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
    [SerializeField]
    private float SightDistance = 10;
    [SerializeField]
    private ContactFilter2D IgnoreMyself;

    private int WanderTimer;
    private int WanderDecisionMakingTime;
    private int RandomNum;
    private Vector2 CurrentDirection;
    private float Rotation;
    private float CurrentSpeed;



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
        Rotation = 0;
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
        PlayerDetected();
    }





    private void Movement()
    {

        if (CurrentState == ZombieState.Wandering)
            CurrentSpeed = WanderSpeed;
        else
            CurrentSpeed = ChaseSpeed;

        ZombieRigidBody2D.velocity = CurrentDirection * CurrentSpeed;

        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(CurrentDirection.y,CurrentDirection.x)*Mathf.Rad2Deg);
    }





    private void PlayerDetected()
    {
        int HitAmount;
        RaycastHit2D[] Hitinfo = new RaycastHit2D[10];
        HitAmount = Physics2D.Raycast(transform.position, transform.right,IgnoreMyself,Hitinfo , SightDistance);

        //Debug.Log(this.name+" sees: "+Hitinfo[0].collider.gameObject.name);
        if (Hitinfo[0] != null)
        {
            if (Hitinfo[0].collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player Seen");
            }
        }
        

        Debug.DrawRay(transform.position, transform.right * SightDistance, Color.red);

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
                CurrentSpeed = 0;
            }
        }

        WanderTimer++;

        if (WanderTimer == WanderingDecisionInterval)
            WanderTimer = 0;
    }

    private Vector2 RandomDirection()
    {
        float x, y;
        x = Random.Range(-1.0f, 1.0f);
        y = Random.Range(-1.0f, 1.0f);
        Rotation = Mathf.Atan2(y, x);
        transform.rotation = Quaternion.Euler(0f, 0f, Rotation);
        return new Vector2(x, y);
        
    }




}
