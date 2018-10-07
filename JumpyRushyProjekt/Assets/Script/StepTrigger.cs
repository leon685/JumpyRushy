using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTrigger : MonoBehaviour {

    public string imePasti;
    private Rigidbody2D charbody;
    public static string stepTrigger;
    void Start() {
        stepTrigger = "";
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() == null)
        {
            return;
        }
        else
        {
            stepTrigger = imePasti;
        }
    }

}
