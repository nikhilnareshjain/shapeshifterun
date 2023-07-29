using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] AudioSource deathSound;

    bool dead = false;

    private void Update()
    {
        if (transform.position.y < -1f && !dead)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Shape currentShape = GetComponent<PlayerMovement>().GetShape();
        if (collision.gameObject.CompareTag("Enemy Body") 
            || (collision.gameObject.CompareTag("Wood Tag") && currentShape != Shape.Cube)
            || (collision.gameObject.CompareTag("Sphere Tag") && currentShape != Shape.Sphere)
            || (collision.gameObject.CompareTag("Cylinder Line Tag") && currentShape != Shape.Cylinder))
        {
            // GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerMovement>().collision = true;
            Die();
        } else if ((collision.gameObject.CompareTag("Wood Tag") && currentShape == Shape.Cube)
                || (collision.gameObject.CompareTag("Sphere Tag") && currentShape == Shape.Sphere)) {
            // Destroy(collision.gameObject);
            collision.gameObject.GetComponent<Obstacle>()?.DestroyWood();
            StartCoroutine(DestroyGameObject(collision.gameObject));
        }

        if (collision.gameObject.CompareTag("Magnetic")) {
            Destroy(collision.gameObject);
            GetComponent<PlayerMovement>().SetPowerup(PowerUp.CoinAttract);
        } else if (collision.gameObject.CompareTag("Invincible")) {
            Destroy(collision.gameObject);
            GetComponent<PlayerMovement>().SetPowerup(PowerUp.BreakObstacle);
        }
    }

    IEnumerator DestroyGameObject(GameObject go) {
        Destroy(go);
        yield break;
    }

    void Die()
    { 
        Invoke(nameof(ReloadLevel), 1.3f);
        dead = true;
        deathSound.Play();
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
