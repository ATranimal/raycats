using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Inputs {

}
public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public SpriteRenderer rend;
    

    private int movementSpeed;

    // Input Values
    private float verticalValue;
    private float horizontalValue;
    private float cameraRotateValue;

    bool isFacingRight;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();

        movementSpeed = 50;

        isFacingRight = true;
    }

    void Update()
    {
        setValues();
        handleMovement();
        handleAnimation();

        dragMovement();
    }

    void setValues() {
        verticalValue = Input.GetAxis("Vertical");
        horizontalValue = Input.GetAxis("Horizontal");
        cameraRotateValue = Input.GetAxis("CameraRotate");
    }

    private void handleMovement() {
        if (verticalValue != 0) {
            addForceInDirection(movementSpeed, Vector3.back * verticalValue);
        }
        if (horizontalValue != 0) {
            addForceInDirection(movementSpeed, Vector3.right * horizontalValue);
        }
        if (cameraRotateValue != 0) {
            rotatePlayer(Input.GetAxis("CameraRotate"));
        }
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

    private void dragMovement() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
