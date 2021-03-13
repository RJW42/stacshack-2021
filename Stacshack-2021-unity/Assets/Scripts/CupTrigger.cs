using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupTrigger : MonoBehaviour{
    private void OnTriggerStay(Collider other) {
        other.gameObject.GetComponent<DrinkSubmit>().SubmitDrink(this.gameObject);
    }
}
