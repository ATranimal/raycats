using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Inputs {

}
public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;

    private int movementSpeed;
    // private int turnSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        movementSpeed = 50;
        // turnSpeed = 1;
    }

    void Update()
    {
        mapInputs();
    }

    void mapInputs() {
        if (Input.GetAxis("Vertical") != 0) {
            addForceInDirection(movementSpeed, Vector3.back * Input.GetAxis("Vertical"));
        }
        if (Input.GetAxis("Horizontal") != 0) {
            addForceInDirection(movementSpeed, Vector3.right * Input.GetAxis("Horizontal"));
        }
        if (Input.GetAxis("CameraRotate") != 0) {
            rotatePlayer(Input.GetAxis("CameraRotate"));
        }

        noInput();
    }

    void addForceInDirection(int amount, Vector3 direction) {
        rb.AddRelativeForce(direction * amount);
    }

    void rotatePlayer(float amount) {
        this.transform.Rotate(Vector3.up, amount);
    }

    void noInput() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
