using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IdleLocation : MonoBehaviour
{

    // State for a location
    public IdleLocationState state;

    // Start is called before the first frame update
    void Start()
    {
        state = IdleLocationState.emtpy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

public enum IdleLocationState {
    taken, 
    wating,
    emtpy
}