using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    
    

    [SerializeField]
    private float ChaseSpeed = 5.0f;
    [SerializeField]
    private float WanderSpeed = 1.0f;
    [SerializeField]
    private float InvestigateSpeed = 2.0f;
    [SerializeField]
    private float huddleCheckRange = 30f;
    [SerializeField]
    private float huddleGroupRadiusPerZombie = 5f;
    [SerializeField]
    private LayerMask otherZombiesToSwarmTo;
    [SerializeField]
    private int WanderingDecisionInterval = 100;
    [SerializeField]
    private float SightDistance = 10;
    [SerializeField]
    private ContactFilter2D IgnoreMyself;
    [SerializeField]
    public ZombieState CurrentState {get; private set; }

    [SerializeField]
    private int health;


    private int WanderTimer;
    private int WanderDecisionMakingTime;
    private int RandomNum;
    [SerializeField]
    private Vector2 currentDirection;
    private Vector2 promptLocation;
    private float Rotation;
    private float CurrentSpeed;
    private int HitAmount;
    private RaycastHit2D[] Hitinfo = new RaycastHit2D[10];
    Rigidbody2D ZombieRigidBody2D;
   // private ZombieState CurrentState;
    List<Vector2> nodesToFollow = new List<Vector2>();
    Collider2D[] nearbyZombieLocations;



    public enum ZombieState
    {
        Wandering,
        Investigate,
        Chase,
        Dead,
        Attracted,
    }

    

    

    

    // Start is called before the first frame update
    void Start()
    {
        ZombieRigidBody2D = GetComponent<Rigidbody2D>();
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
            case ZombieState.Chase: UpdateChaseState(); break;
            case ZombieState.Investigate: UpdateInvestigateState(); break;

        }

        Movement();
        //PlayerDetected();
        Debug.DrawRay(transform.position, transform.up * SightDistance, Color.red);
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

        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(currentDirection.y,currentDirection.x)*Mathf.Rad2Deg-90);
    }

    private Vector2 CheckSwarmCenter()
    {
        nearbyZombieLocations = Physics2D.OverlapCircleAll(transform.position, huddleCheckRange, otherZombiesToSwarmTo);

        float averageX = 0;
        float averageY = 0;

        foreach (Collider2D Z in nearbyZombieLocations)
        {
            averageX += Z.transform.position.x;
            averageY += Z.transform.position.y;
        }
        averageX = averageX / nearbyZombieLocations.Length;
        averageY = averageY / nearbyZombieLocations.Length;



        return new Vector2(averageX,averageY);
    }

    private bool PlayerDetected()
    {
        //returns true or false depending on if the raycast sees player;
        
        
        HitAmount = Physics2D.Raycast(transform.position, transform.up,IgnoreMyself,Hitinfo , SightDistance);
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
            Vector2 swarmCenter = CheckSwarmCenter();
            if (Vector2.Distance(transform.position, swarmCenter) > nearbyZombieLocations.Length * huddleGroupRadiusPerZombie)
            {
                TurnToVector(swarmCenter);
                CurrentSpeed = WanderSpeed * 1.5f;
            }
            else
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

    private void UpdateAttracted()
    {
        CurrentSpeed = ChaseSpeed;
        TurnToVector(promptLocation);
    }

    private void UpdateInvestigateState()
    {
        if (nodesToFollow.Count > 0)
        {
            if (Mathf.Abs(nodesToFollow[0].x - transform.position.x) + Mathf.Abs(nodesToFollow[0].y - transform.position.y) <= 0.5)
            {
                nodesToFollow.Remove(nodesToFollow[0]);
            }
            TurnToVector(nodesToFollow[0]);
        }
        else
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

    public void InvestigateLocation(Vector2 locationOfInterest)
    {
        CurrentSpeed = InvestigateSpeed;
        PathFinding aStarPathfinding = GetComponentInChildren<PathFinding>();

        nodesToFollow = aStarPathfinding.FindPath(this.transform.position,locationOfInterest);

        CurrentState = ZombieState.Investigate;

        Debug.Log(this.name + " Heard Player");
    }

    public void AttractedToLocation(Vector2 bileLocation)
    {
        promptLocation = bileLocation;
        this.CurrentState = ZombieState.Attracted;
    }

    public void AttractantDisappeared()
    {
        SetStateTo(ZombieState.Wandering);
    }


    public void TurnToVector(Vector2 nextWaypoint)
    {
        currentDirection = new Vector2(nextWaypoint.x - transform.position.x, nextWaypoint.y - transform.position.y);
        currentDirection.Normalize();
    }

    public void HitByBullet(BulletScript bullet)
    {
        health -= bullet.damage;
        if (health <= 0)
            Destroy(this.gameObject);
    }
}
