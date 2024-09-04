using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;          
    private Rigidbody2D rb;                             
    private float moveInput;                            

    private void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get horizontal input from the player (-1 for left, 1 for right)
        moveInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        // Apply movement based on input and speed
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }
}