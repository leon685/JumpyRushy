using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    //public GameObject gameOverUI;
    public void Leaderboard()
    {
        SceneManager.LoadScene("Leaderboards");
    }
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Use this for initialization
    private Rigidbody2D charbody;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() == null)
        {
            return;
        }
        if (PowerUps.shield == true)
        {
            StartCoroutine(fade(other.GetComponent<SpriteRenderer>()));
            charbody = other.GetComponent<Rigidbody2D>();
            // charbody.velocity = new Vector2(charbody.velocity.x, 4);
            Controls.speed = Controls.speed / 2;
            PowerUps.shield = false;
        }
        else
        {
            //gameOverUI.SetActive(true);
            Classification.konecIgre = true;
            SceneManager.LoadScene("GameOver");
        }
    }
    IEnumerator fade(SpriteRenderer other)
    {
        other.color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(0.2f);
        other.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.2f);
        other.color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(0.2f);
        other.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.2f);
        other.color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(0.2f);
        other.color = new Color(1, 1, 1, 1);
    }


}
