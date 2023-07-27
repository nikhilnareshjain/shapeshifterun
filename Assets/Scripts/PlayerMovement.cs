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
    float verticalMovementSpeed = 2f;
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        activateShape();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horizontalInput * horizontalMovementSpeed, rb.velocity.y, verticalMovementSpeed);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // Jump();
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
