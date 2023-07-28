using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformPrefab : MonoBehaviour
{
    [Header("Floor Layer")]
    [SerializeField] GameObject FloorParent;
    // [SerializeField] Transform FloorTransform;
    
    [Header("Enemy Layer")]
    [SerializeField] GameObject Enemy;
    // [SerializeField] Transform EnemyTransform;
    
    [Header("Collectible Layer")]
    [SerializeField] GameObject Collectibles;
    // [SerializeField] GameObject CollectibleTransform;

    [SerializeField] GameObject CubeObstacle;
    [SerializeField] GameObject SphereObstacle;
    [SerializeField] GameObject CylinderObstacle;
    [SerializeField] GameObject PyramidObstacle;
    
    [SerializeField] GameObject CoinCollectible;
    [SerializeField] GameObject PowerupCollectible;
    [SerializeField] GameObject LeftPos;
    [SerializeField] GameObject RightPos;
    [SerializeField] GameObject CenterPos;
    private FloorType[] floorType = (FloorType[]) Enum.GetValues(typeof(FloorType));

    // Start is called before the first frame update
    void Awake() {
        activateObstacle();
    }

    public void Init(int indexFloor) {
        initFloor(floorType[indexFloor]);
        initEnemy();
    }

    void initEnemy() {
        int enemyPos = Random.Range(0, 3);
        switch (enemyPos) {
            case 0 :
                Enemy.transform.position = LeftPos.transform.position;
                break;
            case 1 :
                Enemy.transform.position = CenterPos.transform.position;
                break;
            case 2 :
                Enemy.transform.position = RightPos.transform.position;
                break;
        }
    }
    
    void initFloor(FloorType floorType) {
        return;
        switch (floorType) {
            case FloorType.BASIC :
            FloorParent.GetComponent<WaypointFollower>().enabled = false;
                break;
            case FloorType.HMOVING : 
                // FloorParent.GetComponent<WaypointFollower>().enabled = true;
                break;
            case FloorType.LSHIFTED :
            FloorParent.GetComponent<WaypointFollower>().enabled = false;
                Vector3 newPos = new Vector3(FloorParent.transform.localPosition.x - 2f, FloorParent.transform.localPosition.y,
                    FloorParent.transform.localPosition.z);
                FloorParent.transform.localPosition = newPos;
                break;
            case FloorType.RSHIFTED :
            FloorParent.GetComponent<WaypointFollower>().enabled = false;
                Vector3 newPosVal = new Vector3(FloorParent.transform.localPosition.x + 2f, FloorParent.transform.localPosition.y,
                    FloorParent.transform.localPosition.z);
                FloorParent.transform.localPosition = newPosVal;
                break;
        }
    }

    void activateObstacle() {
        int obstacleType = Random.Range(0, 3);
        CubeObstacle.SetActive(obstacleType == 0);
        CylinderObstacle.SetActive(obstacleType == 1);
        SphereObstacle.SetActive(obstacleType == 2);
    }

    // Update is called once per frame
    void Update() {
    }
    

}
