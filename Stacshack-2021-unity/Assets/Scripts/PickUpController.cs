using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour {
    public ItemController itemController; 
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, objectContainer, fpsCam;

    public float pickUpRange;

    public bool equipped;
    public static bool slotFull;


    void Start() {
        if (!equipped) {
            itemController.Disable();
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        else {
            itemController.Enable();
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    void Update() {
        // Check if player is in range and "E" pressed. Picking up item 
        Vector3 distnace_to_player = player.position - transform.position;

        if(!equipped && distnace_to_player.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) {
            PickUp();
        }


        // Drop if equipped and "Q" pressed 
        if(equipped && Input.GetKeyDown(KeyCode.Q)) {
            Drop();
        }
    }

    void PickUp() {
        // Set item to quipied 
        equipped = true;
        slotFull = true;

        // Make the item a child of the camera 
        transform.SetParent(objectContainer);
        transform.localPosition = Vector3.zero;

        // Stop item from moving 
        rb.isKinematic = true;
        coll.isTrigger = true;

        itemController.Enable();
    }


    void Drop() {
        // Drop item 
        equipped = false;
        slotFull = false;

        // Remove the item from the object container 
        transform.SetParent(null);

        // Stop item from moving 
        rb.isKinematic = false;
        coll.isTrigger = false;

        // Add momentum
        rb.velocity = player.GetComponent<CharacterController>().velocity;

        itemController.Disable();
    }

}
