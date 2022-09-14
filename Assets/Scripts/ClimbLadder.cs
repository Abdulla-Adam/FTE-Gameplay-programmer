using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLadder : MonoBehaviour
{
    Animator anim;
    CharacterController characterController;
    public int climbVelocityHash;
    public float climbVelocity;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        climbVelocityHash = Animator.StringToHash("ClimbVelocity");
        anim.SetFloat(climbVelocityHash, 0);
        GetComponent<ThirdPersonMovement>().enabled = false;        
    }

    // Update is called once per frame
    void Update()
    {
        Climb();
    }

   
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ladder")
        {
            anim.SetFloat(climbVelocityHash, 0f);
            GetComponent<ThirdPersonMovement>().enabled = true;
            this.enabled = false;
        }
    }

    public void Climb()
    {
        if (anim.GetBool("IsClimbing"))
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                characterController.transform.position += Vector3.up / 10f;

                if (climbVelocity <= 0)
                {
                    climbVelocity = 1f;
                }
            }

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {

                characterController.transform.position += Vector3.down / 10f;

                if (climbVelocity <= 0)
                {
                    climbVelocity = 1;
                }
            }

            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                climbVelocity = 0;
            }



            anim.SetFloat(climbVelocityHash, climbVelocity);
        }
        
    }
}
