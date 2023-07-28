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
    private float attractForce = 2f;
    private float attractDuration = 10f;
    private bool isAttracting = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

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
            Debug.Log("Left arrow key released!");
        }

        // Check for right arrow key released
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
            // Code to handle right arrow key released
            Debug.Log("Right arrow key released!");
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
            Debug.Log("Current local pos " + this.transform.localPosition.x + ", " + this.transform.localPosition.y + ", " + this.transform.localPosition.z);
            // this.transform.localPosition = new Vector3(this.transform.localPosition.x + 0.35f, this.transform.localPosition.y, this.transform.localPosition.z);
            DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(this.transform.localPosition.x + 0.35f, this.transform.localPosition.y, this.transform.localPosition.z + verticalMovementSpeed * 0.5f), 0.5f);
        } else if (horizontalInput < 0) {
            Debug.Log("Current local pos " + this.transform.localPosition.x + ", " + this.transform.localPosition.y + ", " + this.transform.localPosition.z);
            // this.transform.localPosition = new Vector3(this.transform.localPosition.x - 0.35f, this.transform.localPosition.y, this.transform.localPosition.z);
            DOTween.To(() => this.transform.localPosition, x => this.transform.localPosition = x, new Vector3(this.transform.localPosition.x - 0.35f, this.transform.localPosition.y, this.transform.localPosition.z + verticalMovementSpeed * 0.5f), 0.5f);
        }
      
    //   if (Input.touchCount > 0)
    //     {
    //         // Get the first touch (you can handle multi-touch by looping through Input.touches)
    //         Touch touch = Input.GetTouch(0);

    //         // Check the phase of the touch
    //         switch (touch.phase)
    //         {
    //             case TouchPhase.Began:
    //                 // Handle touch start
    //                 touchStartPosition = Camera.main.WorldToViewportPoint(touch.position);
    //                 break;

    //             case TouchPhase.Moved:
    //                 // Handle touch movement
    //                 Vector3 touchEndPosition = Camera.main.WorldToViewportPoint(touch.position);
    //                 float xMoved = touchEndPosition.x - touchStartPosition.x;
    //                 xMoved = xMoved * 1.2f * (touchStartPosition.x / touch.position.x);
    //                 Vector3 newPos = new Vector3(transform.localPosition.x + xMoved, transform.localPosition.y, transform.localPosition.z);
    //                 transform.localPosition = newPos;
    //                 return;

    //             case TouchPhase.Ended:
    //             case TouchPhase.Canceled:
    //                 // Handle touch end or cancellation
    //                 break;
    //         }
    //     }

        rb.velocity = new Vector3(0, - 1 * verticalMovementSpeed, verticalMovementSpeed);
        if (!isAttracting)
        {
            StartCoinAttract();
        }
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // Jump();
        }
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
        isAttracting = true;
        StartCoroutine(CoinAttractCoroutine());
    }

    
    public void UseAttractorPowerup() {
        if (isPowerupOn) {
            float startTime = Time.time;
            isPowerupOn = false;
            while (Time.time - startTime < attractDuration)
            {// do something

                Collider[] nearbyCoins = Physics.OverlapSphere(transform.localPosition, attractRadius);

                foreach (Collider coinCollider in nearbyCoins) {
                    if (coinCollider.CompareTag("Coin")) {
                        Transform coinTransform = coinCollider.transform;
                        Vector3 directionToPlayer = this.transform.position - coinTransform.position;
                        float distanceToPlayer = directionToPlayer.magnitude;

                        if (distanceToPlayer > 0.1f) // To avoid jitter when the coin is too close to the player
                        {
                            float step = attractForce * Time.deltaTime;
                            coinTransform.position = Vector3.MoveTowards(coinTransform.position, transform.position, step);
                        }
                    }
                }
            }
        }
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
