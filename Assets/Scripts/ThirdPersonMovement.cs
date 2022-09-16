using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    //Set up Animator
    private Animator anim;
    public bool IsMoving;
    public bool IsPunch;
    public bool IsSprint;
    public bool IsJumping;
    public bool IsGrounded;
    public bool IsPushing;
    public bool attachOnce;
   
    //Set Up Speed
    private float moveSpeed = 2.5f;
    [SerializeField] private float WalkSpeed = 1f;
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

    public float gravity = -9.8f;
    Vector3 moveDownVelocity;

    public float pushingSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        IsInputEnabled = true;
        anim = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        characterController.detectCollisions = false;
        attachOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInputEnabled)
        {
            if (IsGrounded)
            {
                if (IsPushing)
                {
                    PushAndMove();
                }
                else
                {
                    Move();
                }
                
            }

            GroundedCheck();
            ApplyGravity();
            Jump();
            AttachPushObject();
        }
    }


    void Move()
    {
        //Getting Keyboard Input
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        //Storing Keyboard Input into a Direction Vector
        Vector3 Direction = new Vector3(Horizontal, 0f, Vertical).normalized;

        if (Input.GetButton("Fire1"))
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
        if (Direction.magnitude >= 0.1f)
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
            //SFXManager.instance.StopFootSteps();
        }

        MoveAnimation();

    }

    void MoveAnimation()
    {
        bool movementButtonPressed = IsPushing ? Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) : Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (!IsPushing)
        {
            velocityHash = Animator.StringToHash("Velocity");
            if (movementButtonPressed && velocity < 1f)
            {
                if (IsMoving && velocity < 0.5f)
                {
                    // velocity = Time.deltaTime * acceleration;    
                    velocity = Mathf.Lerp(velocity, 0.5f, acceleration);
                }

                if (IsSprint)
                {
                    // velocity += Time.deltaTime * acceleration;    
                    velocity = Mathf.Lerp(velocity, 1f, acceleration);
                }

                if (!IsSprint && velocity >= 0.5f)
                {
                    velocity -= Time.deltaTime * deceleration;
                }
            }

            if (!movementButtonPressed && velocity > 0f)
            {
                velocity -= Time.deltaTime * deceleration;
            }

            if (!movementButtonPressed && velocity < 0f)
            {
                velocity = 0f;
            }

        }

        //For Pushing animation
        if (IsPushing)
        {
            //PushVelocity is parameter for Push animation blend tree.
            velocityHash = Animator.StringToHash("PushVelocity");
            if (movementButtonPressed && velocity < 1f)
            {
                if (velocity < 1f)
                {
                    // velocity = Time.deltaTime * acceleration;    
                    velocity = Mathf.Lerp(velocity, 1f, acceleration);
                }

            }

            if (!movementButtonPressed && velocity > 0f)
            {
                velocity -= Time.deltaTime * deceleration;
            }

            if (!movementButtonPressed && velocity < 0f)
            {
                velocity = 0f;
            }
        }
            
        anim.SetFloat(velocityHash, velocity);
    }

    void PushAndMove()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * pushingSpeed);
        }
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * pushingSpeed);
        }

        MoveAnimation(); 
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        IsGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        if (IsGrounded)
        {
            IsJumping = false;
            anim.SetBool("IsJumping", false);
        }

        if (!IsGrounded)
        {
            IsMoving = false;
            IsSprint = false;
        }

    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded && UIManager.instance.PushButtonPanel.active == false)
        {
            if (IsSprint)
            {
                anim.SetFloat("JumpVelocity", 1);
            }

            else
            {
                anim.SetFloat("JumpVelocity", 0);
            }

            anim.SetTrigger("IsJumping");
            IsJumping = true;
            IsMoving = false;
        }

        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") )
        //{
        //    IsGrounded = false;
        //}
    }

    //Manually applying gravity on Player as character controller does not apply Physics.
    public void ApplyGravity()
    {
        if (!IsGrounded)
        {
            moveDownVelocity.y -= gravity * Time.deltaTime;
            characterController.Move(moveDownVelocity);
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "cube" && IsPushing)
        {
            Push(collision.gameObject);
            DetachPushObject(collision.gameObject);
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "cube" && attachOnce)
        {
            UIManager.instance.PushButtonPanel.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "cube")
        {
            
            if (!IsPushing && attachOnce) 
            {
                UIManager.instance.PushButtonPanel.SetActive(true);                
            }           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ladder")
        {
            //Setting position and rotation of player according to ladder.
            transform.position = new Vector3(other.gameObject.transform.position.x, transform.position.y, transform.position.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, other.gameObject.transform.eulerAngles.y, transform.eulerAngles.z);

            IsMoving = false;
            anim.SetBool("IsMoving", false);
            IsGrounded = false;
            anim.SetBool("IsClimbing", true);            
          
            GetComponent<ClimbLadder>().enabled = true;
            this.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ladder")
        {
            IsMoving = true;
            anim.SetBool("IsClimbing", false);
        }
    }

    void Push(GameObject collidedObject)
    {
        
        if (IsPushing && IsGrounded && attachOnce )
        {
            transform.LookAt(collidedObject.transform);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            Vector3.MoveTowards(transform.position , collidedObject.transform.position , 0.5f);
            collidedObject.transform.SetParent(transform);
            attachOnce = false;
           
        }
    }

    public void AttachPushObject()
    {
        if (UIManager.instance.PushButtonPanel.active == true )
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UIManager.instance.PushButtonPanel.SetActive(false);
                IsPushing = true;                
                anim.SetBool("IsPushing", true);
            }
        }
    }

    public void DetachPushObject(GameObject pushableObject)
    {
        if (IsPushing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                attachOnce = true;
                IsPushing = false;
                anim.SetBool("IsPushing", false);
                pushableObject.transform.parent = null;
            }
        }
    }
        
 }


