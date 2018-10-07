using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animControler : MonoBehaviour {
    public Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (StepTrigger.stepTrigger=="spike1XL")
        {
            anim.Play("spikeenlarge");
        }
        else if (StepTrigger.stepTrigger == "saw")
        {
            anim.Play("Saw");
        }
    }
}
