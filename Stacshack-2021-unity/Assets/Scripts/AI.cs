using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

 public class AI : MonoBehaviour {

    // Thirst level of the AI
    [Range(0, 1)]
    public float thirst;
    public float thirst_threshold = 0.25f;
    public float thirst_reduce_rate = 0.01f;
    public float gulp_rate = 0.05f;
    public float time_between_gulps = 2f;
    public float time_of_last_gulp = 0f;

    public Transform idle_location;
    public Transform order_location;
    public Transform deposite_location;
    public GameObject locationManager;

    public MoveTo moveTo;
    public GameObject barManger;

    // Store the NPCs drink 
    public GameObject drink = null;

    // Keep track of death prefab 
    public GameObject death_prefab;

    // Keep track of npc animations 
    public RuntimeAnimatorController no_controller;
    public RuntimeAnimatorController walk_controller;

    // Current State of the AI
    public AIStateType current_state;

    bool bad_drink_recived = false;
    float time_since_animation = 0f;
    float time_of_animation = 1f;

    // Start is called before the first frame update
    void Start() {
        // Set state to moving 
        this.current_state = AIStateType.moving;

        // Get free idle location

        // Move to idle location
        this.idle_location = this.locationManager.GetComponent<LocationController>().getIdleLocation();

        this.Move(this.idle_location, AIStateType.idle);
    }


    // Update is called once per frame
    void Update() {
        // Reduce The Ai's thirs 
        this.thirst -= this.thirst_reduce_rate * Time.deltaTime;

        // Turn off the no animation 
        if (this.bad_drink_recived) {
            if(this.time_since_animation > this.time_of_animation) {
                // Get the animator 
                Animator animator = gameObject.GetComponent<Animator>();

                animator.runtimeAnimatorController = this.walk_controller;
                animator.enabled = false;

                this.bad_drink_recived = false;
                this.time_since_animation = 0f;
            }
            else {
                this.time_since_animation += Time.deltaTime;
            }
        }

        // Handle the state 
        this.HandleState();
    }


    void UpdateState(AIStateType new_type) {
        // Check if switching from idle 
        if(this.current_state == AIStateType.idle && new_type != AIStateType.idle) {
            // Set the location to three 
            this.locationManager.GetComponent<LocationController>().leaveIdleLocation(this.idle_location);
        }

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
            case AIStateType.deposit_drink: {
                // Ordering a drink 
                this.current_state = new_type;
                break;
            }
            case AIStateType.deposited_drink: {
                // Ordering a drink 
                this.current_state = new_type;
                break;
            }
            case AIStateType.order_complete: {
                // Take a drink from the bar
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
            case AIStateType.deposit_drink: {
                // Drop off drink 
                DepositDrink();
                break;
            }
            case AIStateType.deposited_drink: {
                // Decide What to do next
                DepositedDrink();
                break;
            }
            case AIStateType.order_complete: {
                // Take a drink from the bar
                OrderComplete();
                break;
            }
            default: {
                print("Unkown State Type");
                break;
            }
        }
    }

    void Idle() {
        // Check if there is drink to drink 
        if(this.drink != null && this.drink.GetComponentInChildren<Liquid>().fill_amount > 0) {
            // Drink 
            Drink();
        } else {
            // Check if too firsty 
            if (this.thirst <= this.thirst_threshold) {
                // Check if the npc is holding a drink 
                if (this.drink != null) {
                    // Need to put away the drink 

                    // Request permission to drop of drink from bar 
                    if(this.barManger.GetComponent<Bar>().RequestDropoff()){
                        this.Move(this.deposite_location, AIStateType.deposit_drink);
                    }
                }
                else if (this.barManger.GetComponent<Bar>().RequestSpace()) {
                    // Npc not holding a drink. And there is a space at the bar 

                    // Move to the bar 
                    this.Move(this.order_location, AIStateType.order);
                }
            }
        }
    }


    void Order() {
        // Order a drink 
        this.barManger.GetComponent<Bar>().OrderDrink(new System.Action<AIStateType>(this.UpdateState), new System.Action(this.BadDrink), AIStateType.order_complete);

        // Update to waiting for order 
        this.UpdateState(AIStateType.waiting_for_order);

        // Look forward 
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    void OrderComplete() {
        // Remove the drink from the bar 
        GameObject drink = this.barManger.GetComponent<Bar>().TakeDrink();

        drink.transform.parent = this.transform;

        drink.GetComponent<Rigidbody>().isKinematic = true;

        // Save the drink
        this.drink = drink;

        // Move back to idle 
        this.idle_location = this.locationManager.GetComponent<LocationController>().getIdleLocation();

        this.Move(this.idle_location, AIStateType.idle);
    }

    void DepositDrink() {
        // Remove the drink from the npc
        this.drink.transform.parent = null;
        this.drink.GetComponent<Rigidbody>().isKinematic = false;

        // Tell bar we have dropped off drink
        this.barManger.GetComponent<Bar>().DropOffDrink();

        // Set drink to null
        this.drink = null;

        // Change state
        UpdateState(AIStateType.deposited_drink);
    }


    void DepositedDrink() {
        // Check if thisry 
        if(this.thirst < this.thirst_threshold) {
            // See if we can order a drink 
            if (this.barManger.GetComponent<Bar>().RequestSpace()) {
                // Move to the bar to get drink 
                this.Move(this.order_location, AIStateType.order);
            }
            else {
                // Can't so return to idle 
                this.idle_location = this.locationManager.GetComponent<LocationController>().getIdleLocation();

                this.Move(this.idle_location, AIStateType.idle);
            }
        }
        else {
            // Not thirsty so idle
            this.idle_location = this.locationManager.GetComponent<LocationController>().getIdleLocation();

            this.Move(this.idle_location, AIStateType.idle);
        }
    }


    void Move(Transform goal, AIStateType callback_state) {
        // Set state to moving 
        this.current_state = AIStateType.moving;

        // Move to location 
        this.moveTo.MoveToLocation(goal, new System.Action<AIStateType>(this.UpdateState), callback_state);
    }


    void Drink() {
        // Check if we can gulp
        if(this.time_of_last_gulp > this.time_between_gulps) {
            // Check if the liquid is poisen
            if (this.drink.GetComponentInChildren<Liquid>().isPoisen) {
                Die();
            }

            // Add drink
            this.thirst += gulp_rate * 2f;
            this.time_of_last_gulp = 0;

            // Remove drink from
            float new_fill = this.drink.GetComponentInChildren<Liquid>().fill_amount - this.gulp_rate;

            if(new_fill < 0) {
                new_fill = 0;
            }

            this.drink.GetComponentInChildren<Liquid>().fill_amount = new_fill;
        }
        else {
            // Increment last gulp time 
            this.time_of_last_gulp += Time.deltaTime;
        }
    }


    void BadDrink() {
        // Set animation to no 
        if (!this.bad_drink_recived) {
            // Set no animation
            this.bad_drink_recived = true;

            // Get the animator 
            Animator animator = gameObject.GetComponent<Animator>();

            animator.runtimeAnimatorController = this.no_controller;
            animator.enabled = true;
        }
    }

    void Die() {
        // Create a death prefab 
        GameObject death = Instantiate(this.death_prefab);

        death.transform.position = transform.position;
        death.GetComponent<Dead>().username = GetComponent<SkinUpdator>().username;
        death.GetComponent<Dead>().Refresh();

        // Detroy this object 
        Destroy(this.gameObject);
    }
 }


public enum AIStateType {
    idle, 
    moving,
    order,
    waiting_for_order,
    order_complete,
    deposit_drink,
    deposited_drink,
}