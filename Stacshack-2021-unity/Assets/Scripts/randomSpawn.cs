using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSpawn : MonoBehaviour
{
    public GameObject npc;
    public GameObject terrain;

    public float secondsBetweenSpawn;
    public float elapsedTime = 0.0f;

    public int currentNumberOfNpc;
    public int totalNumberOfNpc;

    // Start is called before the first frame update
    void Start()
    {
        // GameObject tmp = Instantiate(npc);
        // tmp.transform.position = new Vector3(31f, tmp.transform.position.y, -4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentNumberOfNpc != totalNumberOfNpc){
            elapsedTime += Time.deltaTime;
 
            if (elapsedTime > secondsBetweenSpawn)
            {
                elapsedTime = 0;
                Debug.Log(true);   
            
                Vector3 spawnPosition = new Vector3(31f, 3.3362f, -4f);
                GameObject newEnemy = (GameObject)Instantiate(npc, spawnPosition, Quaternion.Euler (0, 0, 0));
                currentNumberOfNpc++;
            }
        } else {
            elapsedTime = 0;
        }
        
    }
}