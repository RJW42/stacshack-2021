using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationController : MonoBehaviour
{

    public GameObject idleLocationObject;

    public IdleLocation[] idle_locations;
    public GameObject orderLocationObject;

    // Start is called before the first frame update
    void Start()
    {
        
        for(int i = 0; i < idleLocationObject.transform.childCount; i++) {
            
            idle_locations = idleLocationObject.transform.GetComponentsInChildren<IdleLocation>();
           
        }

        
    }

    // This will return an idle location for the obejct to travel to
    public Transform getIdleLocation() {
        for(int i =0; i < idle_locations.Length; i++) {
            if ( idle_locations[i].state == IdleLocationState.emtpy) {
                return idle_locations[i].gameObject.transform;
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
