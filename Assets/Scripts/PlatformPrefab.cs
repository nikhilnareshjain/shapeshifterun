using System;
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
    private FloorType[] floorType = (FloorType[]) Enum.GetValues(typeof(FloorType));
    private static int indexFloor = 0;

    // Start is called before the first frame update
    void Awake() {
        if (indexFloor == 4) indexFloor = 0;
        initFloor(floorType[indexFloor]);
        indexFloor++;
    }
    
    void initFloor(FloorType floorType) {
        switch (floorType) {
            case FloorType.BASIC :
                break;
            case FloorType.HMOVING : 
                Floor.GetComponent<WaypointFollower>().enabled = true;
                break;
            case FloorType.LSHIFTED :
                Vector3 newPos = new Vector3(Floor.transform.position.x - 4f, Floor.transform.position.y,
                    Floor.transform.position.z);
                Floor.transform.localPosition = newPos;
                break;
            case FloorType.RSHIFTED :
                Vector3 newPosVal = new Vector3(Floor.transform.position.x + 4f, Floor.transform.position.y,
                    Floor.transform.position.z);
                Floor.transform.localPosition = newPosVal;
                break;
        }
    }

    // Update is called once per frame
    void Update() {
    }
    

}
