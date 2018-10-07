using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour {

    public string powerup;
    public static bool shield=false;
    void OnTriggerEnter2D(Collider2D other)
    {  
        if (other.GetComponent<Rigidbody2D>() == null)
        {
            return;
        }
        if (powerup == "SpeedBoost")
        {
            StartCoroutine(Speed_Boost());
        }
        if (powerup == "Shield")
        {
            shield = true;
            Destroy(gameObject);
        }
        if (powerup == "Jump")
        {
            Rigidbody2D charbody;
            charbody=other.GetComponent<Rigidbody2D>();
            charbody.velocity = new Vector2(charbody.velocity.x, 13*1.5f);
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    IEnumerator Speed_Boost() {
        float org_speed = Controls.speed;
        Controls.speed = Controls.speed*2;
        yield return new WaitForSeconds(0.8f);
        Controls.speed = Controls.speed - 2;
        yield return new WaitForSeconds(0.8f);
        Controls.speed = Controls.speed - 2;
        yield return new WaitForSeconds(0.8f);
        Controls.speed = org_speed;
        Debug.Log("po powerup "+Controls.speed);
        Destroy(gameObject);


    }


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
