using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float doubleJumpMulti = 0.5f;
   // public Rigidbody theRB;
    public float jumpForce;
    public CharacterController controller;

    private Vector3 moveDirection;
    public float gravityScale;

    public Animator anim;
    public Transform pivot;
    public float rotateSpeed;
    public GameObject playerModel;

    public float knockBackForce;
    public float knockBackTime;
    public float knockBackCounter;

    private bool canDoubleJump = false;


    // Start is called before the first frame update
    void Start()
    {
        //theRB = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*theRB.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, theRB.velocity.y, Input.GetAxis("Vertical") * moveSpeed);
        if(Input.GetButtonDown("Jump"))
        {
            theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
        }*/
        //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);

        if (knockBackCounter <= 0)
        {
            float yStore = moveDirection.y;
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;
            if (controller.isGrounded)
            {
                moveDirection.y = -3f;
                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                    canDoubleJump = true;
                }
            }
            else
            {
                if(Input.GetButtonDown("Jump") && canDoubleJump)
                {
                    moveDirection.y = jumpForce * doubleJumpMulti;
                    canDoubleJump = false;
                }
            }
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);

        // Move the character in different directions based on camera look direction
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        anim.SetBool("IsGrounded", controller.isGrounded);
        anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
    }

    public void KnockBack(Vector3 direction)
    {
        knockBackCounter = knockBackTime;

        moveDirection = direction * knockBackForce;
        moveDirection.y = knockBackForce;
    }
}
