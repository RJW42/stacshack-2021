using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class AI : MonoBehaviour {

    // Thirst level of the AI
    [Range(0, 1)]
    public float thirst;
    public float thirst_threshold = 0.25f;
    public float thirst_reduce_rate = 0.01f;

    public Transform idle_location;
    public Transform order_location;

    public MoveTo moveTo;
    public GameObject barManger;


    // Current State of the AI
    AIStateType current_state;


    // Start is called before the first frame update
    void Start() {
        // Set state to moving 
        this.current_state = AIStateType.moving;

        // Move to idle location
        this.Move(idle_location, AIStateType.idle);
    }

    // Update is called once per frame
    void Update() {
        // Reduce The Ai's thirs 
        this.thirst -= this.thirst_reduce_rate * Time.deltaTime;

        // Handle the state 
        this.HandleState();
    }

    void UpdateState(AIStateType new_type) {
        // Decide how to handle this state transition 
        switch (new_type) {
            case AIStateType.idle : {
                // Set state to idle 
                this.current_state = new_type;
                break;
            }
            case AIStateType.order: {
                // Order a drink 
                this.current_state = new_type;
                break;
            }
            case AIStateType.waiting_for_order: {
                // Ordering a drink 
                this.current_state = new_type;
                break;
            }
            default: {
                print("Unkown State Type");
                break;
            }
        }
    }


    void HandleState() {
        // Decide how to handle this state transition 
        switch (this.current_state) {
            case AIStateType.idle: {
                // Idle 
                Idle();
                break;
            }
            case AIStateType.order: {
                // Order a drink 
                Order();
                break;
            }
            case AIStateType.waiting_for_order: {
                // Do Nothing
                break;
            }
            case AIStateType.moving: {
                 // Do nothing 
                 break;
            }
            default: {
                print("Unkown State Type");
                break;
            }
        }
    }

    void Idle() {
        // Check if too firsty 
        if(this.thirst <= this.thirst_threshold) {
            // Check if there space 
            if (this.barManger.GetComponent<Bar>().RequestSpace()) {
                // Move to the bar 
                this.Move(this.order_location, AIStateType.order);
            }
        }
    }


    void Order() {
        // Order a drink 
        this.barManger.GetComponent<Bar>().OrderDrink(new System.Action<AIStateType>(this.UpdateState), AIStateType.order_complete);

        // Update to waiting for order 
        this.UpdateState(AIStateType.waiting_for_order);
    }


    void Move(Transform goal, AIStateType callback_state) {
        // Set state to moving 
        this.current_state = AIStateType.moving;

        // Move to location 
        this.moveTo.MoveToLocation(goal, new System.Action<AIStateType>(this.UpdateState), callback_state);
    }
 }


public enum AIStateType {
    idle, 
    moving,
    order,
    waiting_for_order,
    order_complete
}