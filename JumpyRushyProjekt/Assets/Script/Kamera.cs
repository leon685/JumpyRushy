using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamera : MonoBehaviour {
    private static float speed;
    public Rigidbody2D p;
    
	// Use this for initialization
	void Start () {
       // p = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        transform.position = new Vector3(p.transform.position.x, 0,0);
        //speed = Controls.speed;
        //transform.Translate(speed* Time.deltaTime, 0, 0);

    }
}
