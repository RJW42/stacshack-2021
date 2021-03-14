using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkSubmit : MonoBehaviour{
    public GameObject barManager;

    public void SubmitDrink(GameObject drink) {
        this.barManager.GetComponent<Bar>().SubmitDrink(drink);
    }
}
