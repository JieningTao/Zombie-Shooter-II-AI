using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    
    

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
    private Vector2 currentDirection;
    private Vector2 promptLocation;
    private float Rotation;
    private float CurrentSpeed;
    private int HitAmount;
    private RaycastHit2D[] Hitinfo = new RaycastHit2D[10];
    Rigidbody2D ZombieRigidBody2D;
    private ZombieState CurrentState;



    public enum ZombieState
    {
        Wandering,
        Investigate,
        Chase,
        Dead,
    }

    

    

    

    // Start is called before the first frame update
    void Start()
    {
        ZombieRigidBody2D = GetComponent<Rigidbody2D>();
        Rotation = 0;
        WanderTimer = 0;
        CurrentState = ZombieState.Chase;
        WanderDecisionMakingTime = Random.Range(0, WanderingDecisionInterval);
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case ZombieState.Wandering: UpdateWanderState(); break;
            case ZombieState.Chase: UpdateChaseState(); break;

        }

        Movement();
        //PlayerDetected();
        Debug.DrawRay(transform.position, transform.right * SightDistance, Color.red);
    }





    private void Movement()
    {
        /*
        if (CurrentState == ZombieState.Wandering)
            CurrentSpeed = WanderSpeed;
        else
            CurrentSpeed = ChaseSpeed;
            */




        ZombieRigidBody2D.velocity = currentDirection * CurrentSpeed;

        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(currentDirection.y,currentDirection.x)*Mathf.Rad2Deg);
    }





    private bool PlayerDetected()
    {
        //returns true or false depending on if the raycast sees player;
        
        
        HitAmount = Physics2D.Raycast(transform.position, transform.right,IgnoreMyself,Hitinfo , SightDistance);
        if (HitAmount>0)
        {
            if (Hitinfo[0].collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player Seen");
                return true;
            }
        }
        return false;
    }


    private void UpdateWanderState()
    {
        if (PlayerDetected())
        {
            this.CurrentState = ZombieState.Chase;
        }

        //Everytime the Wander time is 0, a decision is made, usually random direction;
        if (WanderTimer == WanderDecisionMakingTime)
        {
            RandomNum = Random.Range(0, 10);
            if (RandomNum < 4) //40% chance of random direction movement;
            {
                currentDirection = RandomDirection();
                
                CurrentSpeed = Random.Range(0, WanderSpeed);
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

    private void UpdateChaseState()
    {
        CurrentSpeed = ChaseSpeed;
        if (PlayerDetected())
        {
            promptLocation = Hitinfo[0].transform.position;
            currentDirection = new Vector2(promptLocation.x - transform.position.x, promptLocation.y - transform.position.y);
            currentDirection.Normalize();
        }
        else if(!PlayerDetected()&&Mathf.Abs(promptLocation.x - transform.position.x)+Mathf.Abs(promptLocation.y - transform.position.y)<=2)
        {
            CurrentState = ZombieState.Wandering;
        }
    }


    private Vector2 RandomDirection()
    {
        float x, y;
        x = Random.Range(-1.0f, 1.0f);
        y = Random.Range(-1.0f, 1.0f);
        Rotation = Mathf.Atan2(y, x);
        
        return new Vector2(x, y);
        
    }

    public void SetStateTo(ZombieState statetoSetTo)
    {
        CurrentState = statetoSetTo;
    }

    public void ChasePlayerAt(Vector2 playerLocation)
    {
        CurrentState = ZombieState.Chase;
        promptLocation = playerLocation;
        currentDirection = new Vector2(promptLocation.x - transform.position.x, promptLocation.y - transform.position.y).normalized;
    }

}
