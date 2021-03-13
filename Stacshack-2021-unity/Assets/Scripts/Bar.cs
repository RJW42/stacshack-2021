using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {
    // Time between AI allowed to order drink 
    public float timeBetweenOrders = 2f;
    public float timeOfLastOrder = 0;

    // Keep track of is there is currently an order being processed 
    bool npc_ordering = false;
    bool npc_at_bar = false;

    // Keep track of the call back
    System.Action<AIStateType> callback;
    AIStateType callback_arg;

    GameObject drink;

    // Update if the AI can take drink 
    private void Update() {
        // Check if there is not an npc at the bar but there is an npc taking up the order space
        if(this.npc_ordering && !this.npc_at_bar) {
            if (this.timeOfLastOrder > this.timeBetweenOrders) {
                // Clear drink 
                this.npc_ordering = false;

                this.timeOfLastOrder = 0;
            }
            else {

                // Add time to time of last order 
                this.timeOfLastOrder += Time.deltaTime;
            }
        }
    }

    public void OrderDrink(System.Action<AIStateType> callback, AIStateType callback_arg) {
        // Npc is at the bar ordering a drink 
        this.npc_at_bar = true;
        this.callback = callback;
        this.callback_arg = callback_arg;
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
        // Check if there is space 
        if (!this.npc_ordering) {
            this.npc_ordering = true;

            return true;
        }

        return false;
    }

    public GameObject TakeDrink() {
        // Give the AI the drink 
        this.npc_at_bar = false;

        // Return the drink 
        return this.drink;
    }

    bool DrinkIsValid(GameObject drink) {
        // Check the drink is at least 75% full 
        if(drink.GetComponentInChildren<Liquid>().fill_amount >= 0.75) {
            return true;
        }

        // Invlid drink
        return false;
    }
}
