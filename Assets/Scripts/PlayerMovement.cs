using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Shape {
    Sphere = 0,
    Cube = 1,
    Cylinder = 2,
    Pyramid = 3
}

public enum TrackPosition {
    Left = 0,
    Middle = 1,
    Right = 2
}

public enum SwipeDirection {
    Left,
    Right
}

public enum PowerUp {
    None,
    CoinAttract,
    BreakObstacle
}

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    float verticalMovementSpeed = 3f;
    float horizontalMovementSpeed = 3f;
    [SerializeField] float jumpForce = 5f;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    [SerializeField] AudioSource jumpSound;

    [SerializeField] GameObject CubeShape;
    [SerializeField] GameObject SphereShape;
    [SerializeField] GameObject CylinderShape;
    [SerializeField] GameObject PyramidShape;

    private Shape currentShape = Shape.Cylinder;
    private TrackPosition currentTrackPosition = TrackPosition.Middle;
    private SwipeInput swipeInput;
    private Vector3 touchStartPosition;

    private bool isPowerupOn = true;
    private float attractRadius = 3.5f;
    private float attractForce = 4f;
    private float attractDuration = 30f;
    private float noCollideDurationDuration = 10f;
 
    private Vector3 lastPosition;
    private PowerUp selectedPowerup = PowerUp.None;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        activateShape();
        swipeInput = GetComponent<SwipeInput>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = 0f;
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
            // Code to handle left arrow key released
        }

        // Check for right arrow key released
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
            // Code to handle right arrow key released
        }

        SwipeInput.SwipeDirection swipeDirection = swipeInput.GetSwipeDirection();

        // Use the swipe direction for your game logic
        switch (swipeDirection)
        {
            case SwipeInput.SwipeDirection.Right:
                // Handle right swipe
                horizontalInput = 1f;
                break;

            case SwipeInput.SwipeDirection.Left:
                // Handle left swipe
                horizontalInput = -1f;
                break;

            case SwipeInput.SwipeDirection.Up:
                // Handle up swipe
                break;

            case SwipeInput.SwipeDirection.Down:
                // Handle down swipe
                break;

            case SwipeInput.SwipeDirection.None:
                // No swipe detected
                break;
        }

        if (horizontalInput > 0) {
            if (currentTrackPosition == TrackPosition.Left) {
                currentTrackPosition = TrackPosition.Middle;
                DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(0, this.transform.localPosition.y, this.transform.localPosition.z + verticalMovementSpeed * 0.5f), 0.5f);
            } else if (currentTrackPosition == TrackPosition.Middle) {
                currentTrackPosition = TrackPosition.Right;
                DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(0.46f, this.transform.localPosition.y, this.transform.localPosition.z + verticalMovementSpeed * 0.5f), 0.5f);
            } else if (currentTrackPosition == TrackPosition.Right) {
                currentTrackPosition = TrackPosition.Right;
                DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(1f, this.transform.localPosition.y, this.transform.localPosition.z + verticalMovementSpeed * 0.5f), 0.5f);
            }
        } else if (horizontalInput < 0) {
            if (currentTrackPosition == TrackPosition.Left) {
                currentTrackPosition = TrackPosition.Left;
                DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(-1f, this.transform.localPosition.y, this.transform.localPosition.z + verticalMovementSpeed * 0.5f), 0.5f);
            } else if (currentTrackPosition == TrackPosition.Middle) {
                currentTrackPosition = TrackPosition.Left;
                DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(-0.46f, this.transform.localPosition.y, this.transform.localPosition.z + verticalMovementSpeed * 0.5f), 0.5f);
            } else if (currentTrackPosition == TrackPosition.Right) {
                currentTrackPosition = TrackPosition.Middle;
                DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(0, this.transform.localPosition.y, this.transform.localPosition.z + verticalMovementSpeed * 0.5f), 0.5f);
            }
        }

        MoveObjectWithDeltaTime();
        if (isPowerupOn)
        {
            if (selectedPowerup == PowerUp.CoinAttract) StartCoinAttract();
            if (selectedPowerup == PowerUp.BreakObstacle) StartBreakObstacle();
        }
        float distanceTraveledThisFrame = Vector3.Distance(transform.position, lastPosition);
        GetComponent<DistanceTraveled>().UpdateDistance(distanceTraveledThisFrame);
        lastPosition = transform.position;
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // Jump();
        }
    }
    
    void MoveObjectWithDeltaTime() {
        // rb.velocity = new Vector3(0, - 1 * verticalMovementSpeed, verticalMovementSpeed);
        float distanceToMove = verticalMovementSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * distanceToMove);
        // transform.Translate(Vector3.down * distanceToMove);
        rb.velocity = new Vector3(0, - 1 * verticalMovementSpeed, 0);
    }

    private System.Collections.IEnumerator CoinAttractCoroutine()
    {
        float startTime = Time.time;

        while (Time.time - startTime < attractDuration)
        {
            Collider[] nearbyCoins = Physics.OverlapSphere(transform.position, attractRadius);
            foreach (Collider coinCollider in nearbyCoins)
            {
                if (coinCollider.CompareTag("Coin"))
                {
                    Transform coinTransform = coinCollider.transform;
                    Vector3 directionToPlayer = transform.position - coinTransform.position;
                    float distanceToPlayer = directionToPlayer.magnitude;

                    if (distanceToPlayer > 0.1f) // To avoid jitter when the coin is too close to the player
                    {
                        float step = attractForce * Time.deltaTime;
                        coinTransform.position = Vector3.MoveTowards(coinTransform.position, transform.position, step);
                    }
                    // else
                    // {
                    //     CollectCoin(coinCollider.gameObject);
                    // }
                }
            }

            yield return null; // Wait for the next frame
        }

        // isAttracting = false;
    }

    private void StartCoinAttract()
    {
        isPowerupOn = false;
        StartCoroutine(CoinAttractCoroutine());
    }
    
    private System.Collections.IEnumerator BreakObstacleCoroutine()
    {
        float startTime = Time.time;
        while (Time.time - startTime < noCollideDurationDuration)
        {
            Collider[] nearbyEnemy = Physics.OverlapSphere(transform.position, attractRadius);
            foreach (Collider coinCollider in nearbyEnemy)
            {
                if (coinCollider.gameObject.CompareTag("Wood Tag") || coinCollider.gameObject.CompareTag("Enemy Body") ||
                    coinCollider.gameObject.CompareTag("Sphere Tag")) {
                    coinCollider.GetComponent<Collider>().enabled = false;
                }
            }
            yield return null; // Wait for the next frame
        }
        
    }

    private void StartBreakObstacle()
    {
        isPowerupOn = false;
        StartCoroutine(BreakObstacleCoroutine());
    }
    

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        jumpSound.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy Head"))
        {
            // Destroy(collision.transform.parent.gameObject);
            // Jump();
        }
    }

    private void moveInDirection(SwipeDirection direction) {
        if (direction == SwipeDirection.Left) {
            
        } else if (direction == SwipeDirection.Right) {

        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, .1f, ground);
    }

    public void ChangeShape(int shape)
    {
        currentShape = (Shape)shape;
        activateShape();
    }

    public Shape GetShape()
    {
        return currentShape;
    }

    private void activateShape() {
        CubeShape.SetActive(currentShape == Shape.Cube);
        SphereShape.SetActive(currentShape == Shape.Sphere);
        CylinderShape.SetActive(currentShape == Shape.Cylinder);
        // PyramidShape.SetActive(currentShape == Shape.Pyramid);
    }
}
