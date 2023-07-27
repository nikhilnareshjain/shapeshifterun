using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Platforms : MonoBehaviour {
    
    [Header("Platform Layer")]
    [SerializeField] GameObject[] Platform;

    public int zPos = 7;
    public bool creatingSection = false;
    public int platformNumber;

    private void Update() {
        if (creatingSection == false) {
            creatingSection = true;
            StartCoroutine(GeneratePlatform());
        }
    }

    IEnumerator GeneratePlatform() {
        // platformNumber 
        Instantiate(Platform[0], new Vector3(0,0,zPos), Quaternion.identity);
        zPos += 7;
        yield return new WaitForSeconds(3);
        creatingSection = false;
    }
    
}