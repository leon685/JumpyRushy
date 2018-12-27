using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introAnimFinish : MonoBehaviour {

    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() == null)
        {
            return;
        }
        SceneManager.LoadScene("Menu");
    }
}
