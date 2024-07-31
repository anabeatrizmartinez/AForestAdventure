using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target; // What the camera will follow.
    public Vector3 offset = new Vector3(-1.8f, 0.3f, -1f); // The distance of the camera from target.
    public float dampingTime = 0.3f; // Damping  time of the camera, to make the movement more fluid.
    public Vector3 velocity = Vector3.zero; // Camera velocity.


    void Awake() {
        Application.targetFrameRate = 60; // FPS
    }

    // Start is called before the first frame update
    void Start() {
    
    }

    // Update is called once per frame
    void Update() {
        MoveCamera(true);
    }

    public void ResetCameraPosition() {
        MoveCamera(false);
    }

    void MoveCamera(bool smooth) {
        Vector3 destination = new Vector3(
            target.position.x - offset.x,
            offset.y,
            offset.z
        );

        if (smooth) {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, dampingTime); // With ref the velocity is calculated by unity in real time.
        } else {
            this.transform.position = destination;
        }
    }
}
