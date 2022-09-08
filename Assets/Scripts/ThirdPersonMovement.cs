using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    //Set up Animator
    private Animator anim;
    private bool IsMoving;
    private bool IsPunch;
    private bool IsSprint;
    private bool IsJumping;
    public bool IsGrounded;
    
    //Set Up Speed
     private float moveSpeed = 2.5f;
    [SerializeField] private float WalkSpeed = 2.5f;
    [SerializeField] private float SprintSpeed = 8f;
    [SerializeField] private float velocity = 0f;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.1f;
    
    private int velocityHash;

    //Set Character Controller
    public CharacterController characterController;
    //Set Rotation Smoothness
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    //Set Cam
    public Transform cam;

    public float GroundedOffset = 0;
    public float GroundedRadius = 1.7f;    
    
    public LayerMask GroundLayers;

    public static bool IsInputEnabled;

 // Start is called before the first frame update
    void Start()
    {
        IsInputEnabled = true;        
        anim = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        characterController.detectCollisions = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInputEnabled)
        {
            if (IsGrounded)
            {
                Move();
            }

            GroundedCheck();
            Jump();
        }             
    }


    void Move()
    {        
        //Getting Keyboard Input
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        //Storing Keyboard Input into a Direction Vector
        Vector3 Direction = new Vector3(Horizontal, 0f, Vertical).normalized;
        
        if(Input.GetButton("Fire1"))
        {
            //Set Animation transition
            if (!IsPunch)
            {
                anim.SetBool("IsPunch", true);
                IsPunch = true;
            }
        }

        else
        {
            //Set Animation transition
            if (IsPunch)
            {
                anim.SetBool("IsPunch", false);
                IsPunch = false;
            }
        }

        //Start Movement if Direction Vector has magnitude
        if (Direction.magnitude >= 0.1f )
        {
            anim.SetBool("IsPunch", false);
            IsPunch = false;
            //Calculate Movement direction angle and set Smooth
            float TargetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetAngle, ref turnSmoothVelocity, turnSmoothTime);

            //Set Rotation according to Angle 
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0);

            //Set Movement in accordance with Camera angle
            Vector3 moveDir = Quaternion.Euler(0f, TargetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            //Set Animation transition
            if (!IsMoving)
            {
                anim.SetBool("IsMoving", true);
                IsMoving = true;                
            }

            //Set Animation transition
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!IsSprint)
                {
                    anim.SetBool("IsSprint", true);
                    IsSprint = true;
                    moveSpeed = SprintSpeed;          
                }
            }

            else
            {
                //Set Animation transition
                if (IsSprint)
                {
                    anim.SetBool("IsSprint", false);
                    IsSprint = false;
                    moveSpeed = WalkSpeed;
                    
                }
            }

        }

        else if (IsMoving)
        {
            anim.SetBool("IsMoving", false);
            IsMoving = false;
           
        }

        MoveAnimation();             

    }

    void MoveAnimation()
    {
        bool movementButtonPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        
        if (movementButtonPressed && velocity < 1f )
        {
            if (IsMoving && velocity < 0.5f)
            {
                // velocity = Time.deltaTime * acceleration;    
                velocity = Mathf.Lerp(velocity , 0.5f , acceleration);
            }
            
            if (IsSprint)
            {
                // velocity += Time.deltaTime * acceleration;    
                velocity = Mathf.Lerp(velocity , 1f , acceleration);
            }
            
            if (!IsSprint && velocity >= 0.5f)
            {
                velocity -= Time.deltaTime * deceleration;                
            }

        }

        if (!movementButtonPressed && velocity > 0f )
        {            
            velocity -= Time.deltaTime * deceleration;                           
        }
        
        if (!movementButtonPressed && velocity < 0f )
        {
            velocity = 0f;
        }

        anim.SetFloat(velocityHash , velocity);
    }

 private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            IsGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);


        //if (JumpTimeout <= 0)
        //{

        //anim.SetBool("IsJumping" , false);

            if (IsGrounded)
            {
                //transform.position = new Vector3(transform.position.x , transform.position.y + 0.1f , transform.position.z); 
                IsJumping = false;
                //anim.SetBool("IsJumping", false);
                IsMoving = true;
                IsSprint = true;
            }

            if (!IsGrounded)
            {
                IsMoving = false;
                IsSprint = false;
            }   
        //}   

        //if (IsJumping)
        //{
        //    if (JumpTimeout > 0)
        //    {
        //        JumpTimeout -= Time.deltaTime;    
        //    }              

        //    if (JumpTimeout <= 0)
        //    {
        //        JumpTimeout = 0;
        //        //transform.position = new Vector3(transform.position.x , transform.position.y - 0.1f , transform.position.z); 
        //    }              
        //}
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        { 
            anim.SetTrigger("IsJumping");
            IsJumping = true;
            IsMoving = false;      
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            IsGrounded = false;
        }
    }
}

