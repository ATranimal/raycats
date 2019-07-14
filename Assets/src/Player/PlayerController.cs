using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Inputs {

}
public class PlayerController : MonoBehaviour
{
    /* 
     * Public Variables
     */
    public Rigidbody rb;
    public Animator anim;
    public SpriteRenderer rend;
    
    /* 
     * Private Variables
     */
    private int movementSpeed = 200;
    private int jumpAmount = 5000;
    private float maxMoveSpeed = 1.5f;

    /* 
     * Operation Variables
     */
    private float verticalValue;
    private float horizontalValue;
    private float cameraRotateValue;
    private bool jumpValue;
    private bool crouchValue;


    bool isFacingRight;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();

        isFacingRight = true;
    }

    void Update()
    {
        setInputValues();
        handleMovement();
        handleAnimation();

        dampMovement();
    }

    void onTriggerStay(Collider other) {
        print("triggered");
    }

    private void setInputValues() {
        verticalValue = Input.GetAxis("Vertical");
        horizontalValue = Input.GetAxis("Horizontal");
        cameraRotateValue = Input.GetAxis("CameraRotate");
        jumpValue = Input.GetButtonDown("Jump");
        crouchValue = Input.GetButton("Crouch");
    }

    private void handleMovement() {
        if (verticalValue != 0) addForceInDirection(movementSpeed, Vector3.back * verticalValue);
        if (horizontalValue != 0) addForceInDirection(movementSpeed, Vector3.right * horizontalValue);
        if (cameraRotateValue != 0) rotatePlayer(Input.GetAxis("CameraRotate"));
        if (jumpValue) jumpPlayer();
    }

    private void handleAnimation() {
        // Animator Parameters
        float animVelocity = Mathf.Abs(verticalValue) + Mathf.Abs(horizontalValue);
        anim.SetFloat("Velocity", animVelocity);
        
        bool currentFacingBackwards = anim.GetBool("FacingBackwards");
        if (currentFacingBackwards && verticalValue > 0.1) {
            anim.SetBool("FacingBackwards", false);
        }
        else if (!currentFacingBackwards && verticalValue < -0.1) {
            anim.SetBool("FacingBackwards", true);
        }

        // Flipping X of sprite renderer
        if (horizontalValue > 0.1) {
            rend.flipX = false;
        }
        else if (horizontalValue < -0.1) {
            rend.flipX = true;
        }
    }

    private void addForceInDirection(int amount, Vector3 direction) {
        rb.AddRelativeForce(direction * amount);
    }

    private void rotatePlayer(float amount) {
        this.transform.Rotate(Vector3.up, amount);
    }

    private void dampMovement() {
        if (rb.velocity.x < -maxMoveSpeed) rb.velocity = new Vector3(-maxMoveSpeed, rb.velocity.y, rb.velocity.z);
        if (rb.velocity.x > maxMoveSpeed) rb.velocity = new Vector3(maxMoveSpeed, rb.velocity.y, rb.velocity.z);
        if (rb.velocity.z < -maxMoveSpeed) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -maxMoveSpeed);
        if (rb.velocity.z > maxMoveSpeed) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxMoveSpeed);
    }

    private void jumpPlayer() {
        addForceInDirection(jumpAmount, Vector3.up);
    }
}
