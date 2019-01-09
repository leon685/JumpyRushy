using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
    public Rigidbody2D charbody;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask ground;
    private bool onGround;
    public AudioSource a;

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
        /*if (a.time >= 1)
        {
            a.Stop();
        }*/
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
        Quaternion rot = new Quaternion(0, 0, 1, 0);
        z = Input.acceleration.z;
        //Debug.Log("x " +Input.acceleration.x);
        //Debug.Log("y " + Input.acceleration.y);
        //Debug.Log("z " + Input.acceleration.z);
        Debug.Log("gyro: " + Input.gyro.attitude.x + " " + Input.gyro.attitude.y + " " + Input.gyro.attitude.z);

        int st = 0;

        List<double> vrX = new List<double>();
        List<double> vrY = new List<double>();
        List<double> vrZ = new List<double>();

        double[] gyroVR = new double[3];
        vrX.Add(Input.gyro.attitude.x);
        vrY.Add(Input.gyro.attitude.y);
        vrZ.Add(Input.gyro.attitude.z);
        if (vrX.Count == 3)
        {
            double Ps = (vrX[0] + vrX[1] + vrX[2]) / vrX.Count;
            double Pn = Ps * 0.66 + Input.gyro.attitude.x * 0.33;
            gyroVR[0] = Pn;
            vrX.Clear();
        }
        if (vrY.Count == 3)
        {
            double Ps = (vrY[0] + vrY[1] + vrY[2]) / vrY.Count;
            double Pn = Ps * 0.66 + Input.gyro.attitude.y * 0.33;
            gyroVR[1] = Pn;
            vrY.Clear();
        }
        if (vrZ.Count == 3)
        {
            double Ps = (vrZ[0] + vrZ[1] + vrZ[2]) / vrZ.Count;
            double Pn = Ps * 0.66 + Input.gyro.attitude.z * 0.33;
            gyroVR[2] = Pn;
            vrZ.Clear();
        }


        charbody.velocity = new Vector2(speed, charbody.velocity.y);
        anim.SetBool("Grounded", onGround);
        //Debug.Log(Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))));
        if ((Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) >= 0.15 && Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) < 0.20 && onGround && MainMenu.accelerometer==true) ||(Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 13);
            a.Stop();
            if (!Classification.stop)
            {
                Classification.skoki++;
            }
            if (DCMPEffects.soundEffect1 != null) { 
        
                //AudioSource.PlayClipAtPoint(Zvok.backgroundMusic, new Vector3(0, 0, 0), 1.0f);
                a.clip = DCMPEffects.soundEffect1;
                a.loop = false;
                a.Play();
            }
            else
            {
                a.Play();
            }
        }
        else if ((Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) >= 0.20 && Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) <= 0.25 && onGround && MainMenu.accelerometer == true) || (Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 18);
            a.Stop();
            if (!Classification.stop)
            {
                Classification.skoki++;
            }
            if (DCMPEffects.soundEffect1 != null)
            {

                //AudioSource.PlayClipAtPoint(Zvok.backgroundMusic, new Vector3(0, 0, 0), 1.0f);
                a.clip = DCMPEffects.soundEffect1;
                a.loop = false;
                a.Play();
            }
            else
            {
                a.Play();
            }

        }
        else if ((Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) >= 0.8 && Mathf.Abs((Mathf.Abs(old_z) - Mathf.Abs(z))) < 0.15 && onGround && MainMenu.accelerometer == true) || (Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 8);
            a.Stop();
            if (!Classification.stop)
            {
                Classification.skoki++;
            }
            if (DCMPEffects.soundEffect1 != null)
            {

                //AudioSource.PlayClipAtPoint(Zvok.backgroundMusic, new Vector3(0, 0, 0), 1.0f);
                a.clip = DCMPEffects.soundEffect1;
                a.loop = false;
                a.Play();
            }
            else
            {
                a.Play();
            }

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
