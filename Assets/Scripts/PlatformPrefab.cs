using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPrefab : MonoBehaviour
{
    [Header("Floor Layer")]
    [SerializeField] GameObject Floor;
    // [SerializeField] Transform FloorTransform;
    
    [Header("Enemy Layer")]
    [SerializeField] GameObject Enemy;
    // [SerializeField] Transform EnemyTransform;
    
    [Header("Collectible Layer")]
    [SerializeField] GameObject Collectibles;
    // [SerializeField] GameObject CollectibleTransform;
    
    [SerializeField] GameObject CoinCollectible;
    [SerializeField] GameObject PowerupCollectible;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }
    

}
