using System;
using UnityEngine;

namespace Character
{
    public class Player : MonoBehaviour
    {
        public float horizontalForce = 50f; // Movement force
        public float horizontalMaxSpeed = 8f; // Movement max speed
        public float verticalForce = 25f; // Jumping force

        protected float movement = 0;
        protected bool isJumping = false;
        protected bool isGrounded;

        private Rigidbody2D rb;
        private BoxCollider2D boxCollider;

        public float groundCheckBuffer = 0.05f;
        private LayerMask groundMask;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
            groundMask = LayerMask.GetMask("Ground");
        }

        protected virtual void Update()
        {
            GroundedCheck();
        }

        protected virtual void FixedUpdate()
        {
            // Move the character
            rb.AddForce(new Vector2(movement * horizontalForce, 0), ForceMode2D.Force);
            if (rb.velocity.x > horizontalMaxSpeed)
            {
                rb.velocity = new Vector2(horizontalMaxSpeed, rb.velocity.y);
            }
            else if (rb.velocity.x < -horizontalMaxSpeed)
            {
                rb.velocity = new Vector2(-horizontalMaxSpeed, rb.velocity.y);
            }

            if (isJumping)
            {
                rb.AddForce(new Vector2(0f, verticalForce), ForceMode2D.Impulse);
                isGrounded = false;
                isJumping = false;
            }
        }

        private void GroundedCheck()
        {
            Vector2 groundCheckPoint1 = new Vector2(transform.position.x + boxCollider.bounds.size.x / 2,
                transform.position.y - boxCollider.bounds.size.y / 2);
            Vector2 groundCheckPoint2 = new Vector2(transform.position.x - boxCollider.bounds.size.x / 2,
                transform.position.y - boxCollider.bounds.size.y / 2);
            RaycastHit2D hit1 = Physics2D.Raycast(groundCheckPoint1, Vector3.down,
                groundCheckBuffer, groundMask);
            RaycastHit2D hit2 = Physics2D.Raycast(groundCheckPoint2, Vector3.down,
                groundCheckBuffer, groundMask);
            if (hit1.collider is not null || hit2.collider is not null)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
}