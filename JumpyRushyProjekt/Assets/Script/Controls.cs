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

    float old_y;
    float y;
    public float hitrost;
    static public float zacetna_hitrost;
    static public float speed;
    List<double> vrX = new List<double>();
    List<double> vrY = new List<double>();
    List<double> vrZ = new List<double>();
    private Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        zacetna_hitrost = hitrost;
        speed = hitrost;
        charbody = GetComponent<Rigidbody2D>();
        old_y = Input.acceleration.y;
    }

    // Update is called once per frame
    void FixedUpdate() {

        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);

    }
    void Update () {
        bool nJump = false;
        bool hJump = false;
        bool sJump = false;
        /*-1 do 1 so vrednosti*/
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
        Quaternion rot = new Quaternion(0, 0, 1, 0);
        y = Input.acceleration.y;
        int st = 0;

        Debug.Log("y: "+Input.acceleration.y);

        //double[] gyroVR = new double[3];
        //vrX.Add(Input.acceleration.x);
        //vrY.Add(Input.acceleration.y);
        //vrZ.Add(Input.acceleration.z);
        //if (vrX.Count == 3)
        //{
        //    double Ps = (vrX[0] + vrX[1] + vrX[2]) / vrX.Count;
        //    double Pn = Ps * 0.66 + Input.acceleration.x * 0.33;
        //    gyroVR[0] = Pn;
        //    vrX.Clear();
        //}
        //if (vrY.Count == 3)
        //{
        //    double Ps = (vrY[0] + vrY[1] + vrY[2]) / vrY.Count;
        //    double Pn = Ps * 0.66 + Input.acceleration.y * 0.33;
        //    gyroVR[1] = Pn;
        //    old_y = (float)gyroVR[1];
        //    Debug.Log("y: " + gyroVR[0] + " org: " + Input.acceleration.y);
        //    vrY.Clear();
        //}
        //if (vrZ.Count == 3)
        //{
        //    double Ps = (vrZ[0] + vrZ[1] + vrZ[2]) / vrZ.Count;
        //    double Pn = Ps * 0.66 + Input.acceleration.z * 0.33;
        //    gyroVR[2] = Pn;
        //    vrZ.Clear();
        //}

        charbody.velocity = new Vector2(speed, charbody.velocity.y);
        anim.SetBool("Grounded", onGround);
        float diffY = Mathf.Abs(y) - Mathf.Abs(old_y);
        Debug.Log("diffY: " + diffY);
        if (diffY >= 0.3f)
        {
            Debug.Log("oldY: "+ old_y + " newY:"+ y);
            hJump = true;
        }
        else if (diffY >= 0.15f)
        {
            Debug.Log("oldY: " + old_y + " newY:" + y);
            nJump = true;
        }
        else if (diffY >= 0.5f)
        {
            Debug.Log("oldY: " + old_y + " newY:" + y);
            sJump = true;
        }
        if ((nJump && onGround && MainMenu.accelerometer==true) ||(Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 13);
            a.Stop();
            if (!Classification.stop)
            {
                Classification.skoki++;
            }
            if (DCMPEffects.soundEffect1 != null) { 
        
                if (ToggleSound.stopnja_zvok == 0)
                    a.clip = CompressEffect1.orgEF1clip;
                else
                    a.clip = DCMPEffects.soundEffect1;
                a.loop = false;
                a.Play();
            }
            else
            {
                a.Play();
            }
        }
        else if ((hJump && onGround && MainMenu.accelerometer == true) || (Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 18);
            a.Stop();
            if (!Classification.stop)
            {
                Classification.skoki++;
            }
            if (DCMPEffects.soundEffect1 != null)
            {

                if (ToggleSound.stopnja_zvok == 0)
                    a.clip = CompressEffect1.orgEF1clip;
                else
                    a.clip = DCMPEffects.soundEffect1; a.loop = false;
                a.Play();
            }
            else
            {
                a.Play();
            }

        }
        else if ((sJump && onGround && MainMenu.accelerometer == true) || (Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 8);
            a.Stop();
            if (!Classification.stop)
            {
                Classification.skoki++;
            }
            if (DCMPEffects.soundEffect1 != null)
            {
                if (ToggleSound.stopnja_zvok == 0)
                    a.clip = CompressEffect1.orgEF1clip;
                else
                    a.clip = DCMPEffects.soundEffect1; a.loop = false;
                a.Play();
            }
            else
            {
                a.Play();
            }

        }
        else
        {
            old_y = Input.acceleration.y;
            y = Input.acceleration.y;
        }
    }
}
