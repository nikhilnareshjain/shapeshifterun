using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [SerializeField] Text NameText;
    [SerializeField] InputField NameInputField;
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    void Start() {
        string name = PlayerPrefs.GetString("name");
        NameText.text = "Hi " + name == "" ? "Alien" : name;
    }

    public void SetName(string name) {
        NameText.text = "Hi " + name;
        PlayerPrefs.SetString("name", name);
    }
}
