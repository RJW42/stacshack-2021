using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleCupControler : ItemController {
    public float rotate_speed = 2f;

    bool script_enabled = false;
    Quaternion start_rotation;


    void Start() {
        this.start_rotation = transform.rotation;
    }
    
    void Update() {
        // Check if enabled 
        if (script_enabled) {
            HandleMovement();
        }
    }

    void HandleMovement() {
        // Check for inputs 
        if (Input.GetKey(KeyCode.R)) {
            // Rotate Left 
            transform.Rotate(0, 45 * rotate_speed * Time.deltaTime, 0);
        }
        
        if(Input.GetKey(KeyCode.F)) {
            // Rotate Right 
            transform.Rotate(0, -45 * rotate_speed * Time.deltaTime, 0);
        }

        if(Input.GetKeyDown(KeyCode.C)) {
            // Reset rotation 
            transform.rotation = this.start_rotation;
        }
    }


    override public void Enable() {
        this.script_enabled = true;
    }

    override public void Disable() {
        this.script_enabled = false;
    }
}
