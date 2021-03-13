using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour{
    // Keep track of if we are moving 
    bool moving = false;

    // Keep track of call back function to say we have reached goal 
    System.Action<AIStateType> callback;
    AIStateType callback_arg;

    public void MoveToLocation(Transform goal, System.Action<AIStateType> callback, AIStateType callback_arg) {
        // Getting the agent
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        // Set the path 
        agent.destination = goal.position;

        // Set moving to true 
        this.moving = true;

        // Store call back 
        this.callback = callback;
        this.callback_arg = callback_arg;
    }


    private void Update() {
        if (moving) {
            // Check if we have reached the goal location 
            NavMeshAgent agent = GetComponent<NavMeshAgent>();

            if (!agent.pathPending) {
                if (agent.remainingDistance <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        // Reached our goal location 
                        this.moving = false;

                        // Call back to A.I to let know the goal has been achinved 
                        this.callback(this.callback_arg);
                    }
                }
            }
        }
    }
}


