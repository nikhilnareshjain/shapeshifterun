using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class Platforms : MonoBehaviour {
    
    [Header("Platform Layer")]
    [SerializeField] GameObject Platform;

    public int zPos = 0;
    public bool creatingSection = false;
    public int platformNumber;
    private Queue<GameObject> platformQueue = new Queue<GameObject>();

    private void Update() {
        if (creatingSection == false) {
            creatingSection = true;
            StartCoroutine(GeneratePlatform());
        }
    }

    IEnumerator GeneratePlatform() {
        // platformNumber 
        Vector3 cameraPos = Camera.main.transform.position;
        if (platformQueue.Count < 15 ) {
            GameObject platform = Instantiate(Platform, new Vector3(0,0,zPos), Quaternion.identity);
            platform.GetComponent<PlatformPrefab>().Init(Random.Range(0, 4));
            platformQueue.Enqueue(platform);
            platform.transform.SetParent(this.transform);
            zPos += 9;
        }
        if (platformQueue.Count >= 5) {
            GameObject platform = platformQueue.Peek();
            while(cameraPos.z - platform.transform.position.z > 10) {
                Destroy(platformQueue.Dequeue());
                platform = platformQueue.Peek();
            }
        }
        
        yield return new WaitForSeconds(1);
        creatingSection = false;
    }
    
}