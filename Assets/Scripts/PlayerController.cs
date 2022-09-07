using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum State{
        Idle,
        Walking,
        Sprinting,
        Jumping
    }

    private State state;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpHeight;
    
    public Animator animator;

    
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
    
    // void MoveForward()
    //{
    //    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed);
    //}

     void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
             if (Input.GetKey(KeyCode.LeftShift))
                {
                    state = State.Sprinting;    
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * 1f);   
                    animator.Play("Sprint");

                    if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
                    {
                        animator.Play("Running Jump");
                    }
                }

                else
                {
                    state = State.Walking;
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed);
                    animator.Play("Walking");        
                }
            
        }

        if (Input.GetKey(KeyCode.A))
        {
            state = State.Walking;
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
            animator.Play("Walking");
        }

        if (Input.GetKey(KeyCode.S))
        {
            state = State.Walking;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed);
            animator.Play("Walking");
        }

        if (Input.GetKey(KeyCode.D))
        {
            state = State.Walking;
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
            animator.Play("Walking");
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            animator.Play("Idle");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play("Jump");
        }    
     

       
    }

    public void HandleAnimation()
    {
        switch (state)
        {
            case State.Idle:

                break;
            
            case State.Walking:

                
                break;
            
            case State.Sprinting:

                break;
   
            case State.Jumping:

                break;



        }
            
    }
}
