using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] AudioSource deathSound;

    bool dead = false;

    // private void Awake() { GetComponent<ResultPopupScript>().gameObject.SetActive(false); }

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
            || (collision.gameObject.CompareTag("Sphere Tag") && currentShape != Shape.Sphere))
        {
            // GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<PlayerMovement>().enabled = false;
            string uName = "Nikhil";
            GetComponent<FetchUserData>().FetchUSerRank(uName, loadPlayerInfo);
            Die();
        } else if ((collision.gameObject.CompareTag("Wood Tag") && currentShape == Shape.Cube)
                || (collision.gameObject.CompareTag("Sphere Tag") && currentShape == Shape.Sphere)) {
            Destroy(collision.gameObject);
        }
    }

    void loadPlayerInfo(UserRankData userInfo) {
        string uName = "Nikhil";
        int coins = GetComponent<ItemCollector>().getCoins();
        int distance = GetComponent<DistanceTraveled>().getDistance();
        // GetComponent<ResultPopupScript>().gameObject.SetActive(true);
        // GetComponent<ResultPopupScript>().resultText.text = "Your Total Coin is : " + userInfo.coins + coins;
        Debug.Log("NJ: Your current score is : " + coins + " and your total score is : " + (userInfo.coins + coins));
        GetComponent<FetchUserData>().SaveUserInfo(uName, (userInfo.coins + coins), distance, 0, 0);
        // GetComponent<FetchUserData>().FetchLeaderboardData();
        // GetComponent<FetchUserData>().FetchUSerRank("nikhil");
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
