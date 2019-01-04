using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
    public Rigidbody2D charbody;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask ground;
    private bool onGround;

    float old_z;
    float z;
    public float hitrost;
    static public float zacetna_hitrost;
    static public float speed;

    private Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        zacetna_hitrost = hitrost;
        speed = hitrost;
        charbody = GetComponent<Rigidbody2D>();
        old_z = Input.acceleration.z;
    }

    // Update is called once per frame
    void FixedUpdate() {

        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);

    }
    void Update () {
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
        Quaternion rot = new Quaternion(0, 0, 1, 0);
        z = Input.acceleration.z;
        //Debug.Log("x " +Input.acceleration.x);
        //Debug.Log("y " + Input.acceleration.y);
        //Debug.Log("z " + Input.acceleration.z);
        //Debug.Log("gyro: " + Input.gyro.attitude*rot);


        charbody.velocity = new Vector2(speed, charbody.velocity.y);
        anim.SetBool("Grounded", onGround);
        //Debug.Log(Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))));
        if ((Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) >= 0.15 && Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) < 0.20 && onGround && MainMenu.accelerometer==true) ||(Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 13);

        }
        else if ((Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) >= 0.20 && Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) <= 0.25 && onGround && MainMenu.accelerometer == true) || (Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 18);

        }
        else if ((Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) >= 0.8 && Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) < 0.15 && onGround && MainMenu.accelerometer == true) || (Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 8);

        }
        else
        {
            old_z = Input.acceleration.z;
            z = Input.acceleration.z;
        }
        /*if (Input.GetMouseButtonDown(0) && onGround)
        {
            charbody.velocity = new Vector2(charbody.velocity.x,5);
        }*/
    }
}
