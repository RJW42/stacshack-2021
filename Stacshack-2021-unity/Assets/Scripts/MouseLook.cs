using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour{

    public float mouseSensativity = 100f;

    public Transform playerBody;


    float xRotation = 0f;


    void Start(){
        // Hide and lock cursor to cneter of screen 
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update(){
        // Get mouse x,y values on screen 
        float mouse_x = Input.GetAxis("Mouse X") * mouseSensativity * Time.deltaTime;
        float mouse_y = Input.GetAxis("Mouse Y") * mouseSensativity * Time.deltaTime;

        // Update the player body 
        playerBody.Rotate(Vector3.up * mouse_x);

        // Update the camera 
        xRotation -= mouse_y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
