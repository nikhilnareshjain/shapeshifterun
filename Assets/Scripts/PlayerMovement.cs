using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shape {
        Sphere = 0,
        Cube = 1,
        Cylinder = 2,
        Pyramid = 3
    }

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float movementSpeed = 6f;
    [SerializeField] float verticalMovementSpeed = 3f;
    [SerializeField] float horizontalMovementSpeed = 6f;
    [SerializeField] float jumpForce = 5f;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    [SerializeField] AudioSource jumpSound;

    private Shape currentShape = Shape.Cube;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horizontalInput * horizontalMovementSpeed, rb.velocity.y, verticalMovementSpeed);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
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
            Destroy(collision.transform.parent.gameObject);
            Jump();
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, .1f, ground);
    }

    void ChangeShape(int shape)
    {
        currentShape = (Shape)shape;
    }

    public Shape GetShape()
    {
        return currentShape;
    }
}
