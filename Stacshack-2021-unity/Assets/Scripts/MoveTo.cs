using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public Transform goal;
    private bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        // Getting the agent
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        // Setting the path 
        agent.destination = goal.position;

        // Setting isMoving
        isMoving = true;
        
        
    }

    // void generateNewGoal() {

    // }

    // Update is called once per frame
    void Update() 
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        // Checking if the agent is still moving
        if(isMoving) {
            // Checking if the agent is still on a path
            isMoving = agent.hasPath;

            return;
        } else { // Not moving
            // Try and generate new path
        }

        print(agent.hasPath);
    }

}
