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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        LookAtmouse();







        Movement();
    }


    void LookAtmouse()
    {

        var angle = Mathf.Atan2(Screen.height/2-Input.mousePosition.y, Screen.width/2-Input.mousePosition.x);
        
        //Debug.Log(angle+"   "+Screen.height+","+Screen.width);
        transform.rotation = Quaternion.Euler(0f, 0f, (angle-Mathf.PI) * Mathf.Rad2Deg);
    }


    void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        if(Input.GetKey(KeyCode.LeftShift))
            MyRigidBody2D.velocity = movement * SprintSpeed;
        else
            MyRigidBody2D.velocity = movement * Speed;
    }


}
