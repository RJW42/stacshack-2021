using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomMovement : MonoBehaviour
{
    public float moveTime;
    public float movementSpeed;

    //on creation
    void Start(){
        transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
    }

    //on frame update
    void Update(){
        if (moveTime > 0){
            transform.Translate(Vector3.forward * movementSpeed);
            moveTime -= Time.deltaTime;
        } else {
            moveTime = Random.Range(1.0f, 10.0f);
            Move();
        }
    }

    void Move(){
        transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
    }
}