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
    [SerializeField] GameObject Collectible;
    // [SerializeField] GameObject CollectibleTransform;
    
    [SerializeField] GameObject CoinCollectible;

    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // float horizontalInput = Input.GetAxis("Horizontal");
        // float verticalInput = Input.GetAxis("Vertical");
        //
        // rb.velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, verticalInput * movementSpeed);
        //
        // if (Input.GetButtonDown("Jump") && IsGrounded())
        // {
        //     Jump();
        // }
    }

}
