using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] AudioSource deathSound;
    [SerializeField] private Text ResultText;
    [SerializeField] private GameObject ResultPopup;
    [SerializeField] private Text PauseText;
    [SerializeField] private GameObject PausePopup;

    bool dead = false;

    private void Start() { ResultPopup.SetActive(false); PausePopup.SetActive(false);}

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
            || (collision.gameObject.CompareTag("Cyliner Line Tag") && currentShape != Shape.Cylinder))
        {
            // GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerMovement>().collision = true;
            ResultPopup.SetActive(true);
            ResultText.text = "Game Over \n\n\n Distance : " + GetComponent<DistanceTraveled>().getDistance().ToString() + "\nCoins :" + GetComponent<ItemCollector>().getCoinText().ToString();
            
            // Die();
            GetComponent<PlayerMovement>().PlayShapeBreakAnimation();

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

    public void onRetryClick() {
        Die();
    }

    public void onMainMenuClick() {
        // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }

    public void onResume() {
        
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<PlayerMovement>().enabled = true;
        PausePopup.SetActive(false);
    }
    public void onPausedClicked() {
        PausePopup.SetActive(true);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<PlayerMovement>().enabled = false;
        PauseText.text = " \n\n\n Distance : " + GetComponent<DistanceTraveled>().getDistance().ToString() + "\nCoins :" + GetComponent<ItemCollector>().getCoinText().ToString() + " \n";

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
