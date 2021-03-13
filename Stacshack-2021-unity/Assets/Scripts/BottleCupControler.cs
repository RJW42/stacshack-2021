using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleCupControler : ItemController {
    public float rotate_speed = 2f;
    public float start_x = -90;
    public float start_y = 90;
    public float start_z = 90; 

    bool script_enabled = false;

    
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
            transform.Rotate(0, -45 * rotate_speed * Time.deltaTime, 0);
        }
        
        if(Input.GetKey(KeyCode.F)) {
            // Rotate Right 
            transform.Rotate(0, 45 * rotate_speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.C)) {
            this.transform.localRotation = Quaternion.Euler(start_x, start_y, start_z);
        }
    }


    override public void Enable() {
        this.script_enabled = true;

        this.transform.localRotation = Quaternion.Euler(start_x, start_y, start_z);
    }

    override public void Disable() {
        this.script_enabled = false;
    }
}
