using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;

    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    Vector3 veclocity;
    bool isGrounded;

    void Update(){
        // Move the player 
        this.MovePlayer();
    }


    void MovePlayer() {
        // Check if grounded 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && veclocity.y < 0) {
            veclocity.y = -2f;
        }


        // Get the input from user 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        // Move the player 
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

    
        if(Input.GetButtonDown("Jump") && isGrounded) {
            veclocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        veclocity.y += gravity * Time.deltaTime;

        controller.Move(veclocity * Time.deltaTime);
    }
}
