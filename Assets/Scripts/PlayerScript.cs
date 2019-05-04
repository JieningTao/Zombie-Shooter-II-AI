using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{


    [SerializeField]
    private Rigidbody2D MyRigidBody2D;



    [SerializeField]
    private float Speed=5;
    [SerializeField]
    private float SprintSpeed = 5;


    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

        LookAtmouse();
        Movement();
        //HandleNoiseMaking();
    }


    private void LookAtmouse()
    {

        var angle = Mathf.Atan2(Screen.height/2-Input.mousePosition.y, Screen.width/2-Input.mousePosition.x);
        
        //Debug.Log(angle+"   "+Screen.height+","+Screen.width);
        transform.rotation = Quaternion.Euler(0f, 0f, (angle-Mathf.PI) * Mathf.Rad2Deg-90);
    }


    private void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        if(Input.GetKey(KeyCode.LeftShift))
            MyRigidBody2D.velocity = movement * SprintSpeed;
        else
            MyRigidBody2D.velocity = movement * Speed;
    }

    private void HandleNoiseMaking()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            MakeSound(20);
            Debug.Log("player made sound");
        }
    }

    private void MakeSound(float radius)
    {

        Collider2D[] zombiesInRange = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D zombie in zombiesInRange)
        {
            if(zombie.CompareTag("Zombie"))
            zombie.GetComponent<ZombieScript>().InvestigateLocation(transform.position);
        }

    }


}
