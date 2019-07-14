using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    public GameObject cameraTarget;
    public GameObject player;
    public Camera mainCamera;

    Vector3 defaultCameraOffset;
    Vector3 defaultCameraTargetOffset;
    float defaultDistance;

    void Start() {
        mainCamera = GetComponent<Camera>();
        mainCamera.transparencySortMode = TransparencySortMode.Orthographic;
        defaultCameraOffset = player.transform.position - transform.position;
        defaultCameraTargetOffset = player.transform.position - cameraTarget.transform.position;
        defaultDistance = Vector3.Distance(player.transform.position, transform.position);
    }

    void LateUpdate() {
        setPosition();

        transform.LookAt(cameraTarget.transform);
    }

    void setPosition() {
        // Check if raycast hits from where the camera should be to the player position
        Vector3 playerPos = player.transform.position;
        Quaternion playerRotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
        Vector3 expectedCameraPos = player.transform.position - (playerRotation * defaultCameraOffset);
        RaycastHit hit = new RaycastHit();

        // Set camera position depending on obscurity
        if (Physics.Linecast(playerPos, expectedCameraPos, out hit)) {
            transform.position = hit.point;
        }
        else {
            transform.position = expectedCameraPos;
        }

        // Set camera target height
        float distanceRatio = Vector3.Distance(playerPos, transform.position) / defaultDistance;
        Vector3 heightChange = new Vector3(0, distanceRatio - 1, 0);
        Vector3 modifiedCameraTargetHeight = player.transform.position - defaultCameraTargetOffset + heightChange;
        cameraTarget.transform.position = modifiedCameraTargetHeight;
    }

}