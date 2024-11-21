using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerFPS : MonoBehaviour
{
    public float moveSpeed = 5f; // Hareket hýzý
    public float jumpForce = 5f; // Zýplama gücü
    public float gravityScale = 1f; // Yerçekimi ölçeði
    public bool isGrounded; // Karakterin yerde olup olmadýðýný kontrol etmek için


    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MovementHandler();
        JumpHandler();

    }

    void MovementHandler()
    {
        // Hareket
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ) * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Zýplama
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

        // Yerçekimi
        rb.velocity += Vector3.up * Physics.gravity.y * gravityScale * Time.deltaTime;
    }

    void JumpHandler()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }

            // Yerçekimi
            rb.velocity += Vector3.up * Physics.gravity.y * gravityScale * Time.deltaTime;
        }
    }
}
