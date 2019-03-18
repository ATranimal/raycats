using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    public GameObject cameraTarget;
    public GameObject player;
    public float damping = 1;
    Vector3 offset;

    void Start() {
        offset = cameraTarget.transform.position - transform.position;
    }

    void LateUpdate() {
        setPosition();

        transform.LookAt(cameraTarget.transform);
    }

    void setPosition() {
        Vector3 playerPos = player.transform.position;
        Vector3 cameraPos = transform.position;
        RaycastHit hit = new RaycastHit();
        if (Physics.Linecast(playerPos, cameraPos, out hit)) {

        }
        else {
            float currentAngle = transform.eulerAngles.y;
            float desiredAngle = cameraTarget.transform.eulerAngles.y;
            float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.position = cameraTarget.transform.position - (rotation * offset);
        }
    }
}