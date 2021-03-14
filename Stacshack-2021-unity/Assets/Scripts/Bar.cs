using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {
    // Time between AI allowed to order drink 
    public float timeBetweenOrders = 2f;
    public float timeOfLastOrder = 0;

    // Keep track of is there is currently an order being processed 
    public bool npc_ordering = false;
    public bool npc_at_bar = false;
    public bool npc_droping_off = false;

    // Keep track of the call back
    System.Action<AIStateType> callback;
    System.Action bad_drink;
    AIStateType callback_arg;

    GameObject drink;

    // Request quque to precent races 
    readonly object ordering_lock = new object();
    readonly object deposite_lock = new object();


    // Update if the AI can take drink 
    private void Update() {
        // Check if there is not an npc at the bar but there is an npc taking up the order space
        /*
        if(this.npc_ordering && !this.npc_at_bar) {
            if (this.timeOfLastOrder > this.timeBetweenOrders) {
                // Clear drink 
                lock (ordering_lock) {
                    this.npc_ordering = false;
                }

                this.timeOfLastOrder = 0;
            }
            else {

                // Add time to time of last order 
                this.timeOfLastOrder += Time.deltaTime;
            }
        }*/
    }

    public void OrderDrink(System.Action<AIStateType> callback, System.Action bad_drink, AIStateType callback_arg) {
        // Npc is at the bar ordering a drink 
        this.npc_at_bar = true;
        this.callback = callback;
        this.callback_arg = callback_arg;
        this.bad_drink = bad_drink;
    }

    public void SubmitDrink(GameObject drink) {
        // Check if a npc is ordering a drink 
        if (this.npc_at_bar) {
            // Npc at the bar waitng for a drink and the user has submitted a drink 
            // Check this drink is valid 
            if (this.DrinkIsValid(drink)) {
                // Save this drink 
                this.drink = drink;

                // Tell the npc they can pickup the drink 
                this.callback(this.callback_arg);
            }
        }
    }

    public bool RequestSpace() {
        bool output = false;

        // Lock the request check then perform the check 
        lock (ordering_lock) {
            // Check if there is space 
            if (!this.npc_ordering && !this.npc_at_bar) {
                this.npc_ordering = true;
                output = true;
            }
        }

        return output;
    }


    public bool RequestDropoff() {
        bool output = false;

        // Lock the deposite check then perform the check 
        lock (deposite_lock) {
            // Check if there is space to drop off drink
            if (!this.npc_droping_off) {
                this.npc_droping_off = true;
                output = true;
            }
        }

        return output;
    }


    public GameObject TakeDrink() {
        // Give the AI the drink 
        lock (ordering_lock) {
            this.npc_at_bar = false;
            this.npc_ordering = false;
        }

        this.bad_drink = null;

        // Return the drink 
        return this.drink;
    }


    public void DropOffDrink() {
        this.npc_droping_off = false;
    }


    bool DrinkIsValid(GameObject drink) {
        // Check the drink is at least 75% full 
        if(drink.GetComponentInChildren<Liquid>().fill_amount >= 0.75) {
            return true;
        }
        // Invlid drink
        this.bad_drink();

        return false;
    }
}
