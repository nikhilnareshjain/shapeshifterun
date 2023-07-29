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
            Die();
        } else if ((collision.gameObject.CompareTag("Wood Tag") && currentShape == Shape.Cube)
                || (collision.gameObject.CompareTag("Sphere Tag") && currentShape == Shape.Sphere)) {
            // Destroy(collision.gameObject);
            StartCoroutine(DestroyGameObject(collision.gameObject));
        }
    }

    IEnumerator DestroyGameObject(GameObject go) {
        Destroy(go);
        yield return null;
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
