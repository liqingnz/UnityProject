using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inside main camera
public class CameraCOntrol : MonoBehaviour {
    public float cameraScrollingSpeed = 10;
    public float cameraZoommingSpeed = 10;
    public float minCameraHeight = 4;
    Vector3 input;
    Vector3 cameraVelocity;

    // Use this for initialization
    void Start() {


    }

    // Update is called once per frame
    void Update() {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), -40 * Input.GetAxisRaw("Mouse ScrollWheel"), Input.GetAxisRaw("Vertical"));
        cameraVelocity = transform.position + input * cameraScrollingSpeed * Time.deltaTime;
        transform.position = cameraVelocity;
        if (transform.position.y < 4) {
            transform.position = new Vector3(transform.position.x, 4, transform.position.z);
        } else if (transform.position.y > 10) {
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        }
    }
}
