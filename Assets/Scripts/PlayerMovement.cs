using System;
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
    [SerializeField] GameObject InvinciblePowerup;
    [SerializeField] GameObject MagneticPowerup;
    private Shape currentShape = Shape.Cylinder;
    private TrackPosition currentTrackPosition = TrackPosition.Middle;
    private SwipeInput swipeInput;
    private Vector3 touchStartPosition;

    private float attractRadius = 3.5f;
    private float attractForce = 0.5f;
    private float attractDuration = 15f;
    private float noCollideDurationDuration = 15f;
 
    private Vector3 lastPosition;
    private PowerUp selectedPowerup = PowerUp.None;
    public bool collision;
    private bool isMovingHorizontal;
    private DistanceTraveled distanceTravelledObj;
    private float currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distanceTravelledObj = GetComponent<DistanceTraveled>();
        lastPosition = transform.position;
        activateShape();
        collision = false;
        isMovingHorizontal = false;
        currentSpeed = verticalMovementSpeed;
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

        if (!isMovingHorizontal) {
            if (horizontalInput > 0) {
                if (currentTrackPosition == TrackPosition.Left) {
                    currentTrackPosition = TrackPosition.Middle;
                    // StartCoroutine(MoveToPosition(new Vector3(0f, this.transform.localPosition.y,
                    //     this.transform.localPosition.z)));
                    DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(0f, this.transform.localPosition.y, this.transform.localPosition.z + currentSpeed * 0.2f), 0.2f);
                } else if (currentTrackPosition == TrackPosition.Middle) {
                    currentTrackPosition = TrackPosition.Right;
                    // StartCoroutine(MoveToPosition(new Vector3(0.46f, this.transform.localPosition.y,
                    //     this.transform.localPosition.z)));
                    DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(0.46f, this.transform.localPosition.y, this.transform.localPosition.z + currentSpeed * 0.2f), 0.2f);
                } else if (currentTrackPosition == TrackPosition.Right) {
                    currentTrackPosition = TrackPosition.Right;
                    // StartCoroutine(MoveToPosition(new Vector3(1f, this.transform.localPosition.y,
                    //     this.transform.localPosition.z)));
                    DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(1f, this.transform.localPosition.y, this.transform.localPosition.z + currentSpeed * 0.2f), 0.2f);
                }
            } else if (horizontalInput < 0) {
                if (currentTrackPosition == TrackPosition.Left) {
                    currentTrackPosition = TrackPosition.Left;
                    // StartCoroutine(MoveToPosition(new Vector3(-1f, this.transform.localPosition.y,
                    //     this.transform.localPosition.z)));
                    DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(-1f, this.transform.localPosition.y, this.transform.localPosition.z + currentSpeed * 0.5f), 0.5f);
                } else if (currentTrackPosition == TrackPosition.Middle) {
                    currentTrackPosition = TrackPosition.Left;
                    // StartCoroutine(MoveToPosition(new Vector3(-0.46f, this.transform.localPosition.y,
                    //     this.transform.localPosition.z)));
                    DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(-0.46f, this.transform.localPosition.y, this.transform.localPosition.z + currentSpeed * 0.5f), 0.5f);
                } else if (currentTrackPosition == TrackPosition.Right) {
                    currentTrackPosition = TrackPosition.Middle;
                    // StartCoroutine(MoveToPosition(new Vector3(0, this.transform.localPosition.y,
                    //     this.transform.localPosition.z)));
                    DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(0, this.transform.localPosition.y, this.transform.localPosition.z + currentSpeed * 0.5f), 0.5f);
                }
            }
        }

        MoveObjectWithDeltaTime();
        if (selectedPowerup == PowerUp.CoinAttract) StartCoinAttract();
        if (selectedPowerup == PowerUp.BreakObstacle) StartBreakObstacle();
        lastPosition = transform.position;
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // Jump();
        }
    }

    public void SetPowerup(PowerUp powerUp) {
       selectedPowerup = powerUp;
    }
    
    void MoveObjectWithDeltaTime() {
        // rb.velocity = new Vector3(0, - 1 * verticalMovementSpeed, verticalMovementSpeed);
        float speedIncreaseFactor =  (distanceTravelledObj.getDistance() / 1000) * 0.01f;
        float forwardSpeed = verticalMovementSpeed + speedIncreaseFactor;
        float distanceToMove = forwardSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * distanceToMove);
        currentSpeed = forwardSpeed;
        // verticalMovementSpeed = distanceToMove;
        // transform.Translate(Vector3.down * distanceToMove);
        rb.velocity = new Vector3(0, - 1 * verticalMovementSpeed, 0);
        float distanceTraveledThisFrame = Vector3.Distance(transform.position, lastPosition);
        GetComponent<DistanceTraveled>().UpdateDistance(distanceTraveledThisFrame, currentSpeed);
    }
    
    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMovingHorizontal = true;
        float initialXPosition = transform.position.x;

        while (Math.Abs(transform.position.x - targetPosition.x) > 0.01f)
        {
            // Calculate the horizontal movement based on the distance from the initial X position
            float horizontalMovement = Mathf.Lerp(0f, targetPosition.x - initialXPosition, (horizontalMovementSpeed / Vector3.Distance(transform.position, targetPosition)) * Time.deltaTime);

            // Move the player horizontally along the X-axis
            transform.Translate(Vector3.right * horizontalMovement);
            
            // rb.MovePosition(new Vector3(targetPosition.x, transform.position.y, transform.position.z));
            // rb.MovePosition(transform.position + Vector3.right * horizontalMovement);

            if (collision || !isMovingHorizontal) break;
            yield return null;
        }
        isMovingHorizontal = false;
    }
    
    private System.Collections.IEnumerator CoinAttractCoroutine()
    {
        float startTime = Time.time;
        InvinciblePowerup.SetActive(false);
        MagneticPowerup.SetActive(true);

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

                if (selectedPowerup == PowerUp.BreakObstacle || selectedPowerup == PowerUp.None) break;
            }

            yield return null; // Wait for the next frame
        }
        selectedPowerup = PowerUp.None;
        MagneticPowerup.SetActive(false);

        // isAttracting = false;
    }

    private void StartCoinAttract()
    {
        StartCoroutine(CoinAttractCoroutine());
    }
    
    private System.Collections.IEnumerator BreakObstacleCoroutine()
    {
        MagneticPowerup.SetActive(false);
        InvinciblePowerup.SetActive(true);
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
            if (selectedPowerup == PowerUp.CoinAttract || selectedPowerup == PowerUp.None) break;
            yield return null; // Wait for the next frame
        }
        selectedPowerup = PowerUp.None;
        InvinciblePowerup.SetActive(false);
        
    }

    private void StartBreakObstacle()
    {
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
