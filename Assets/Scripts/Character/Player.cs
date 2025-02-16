using UnityEngine;
using WorldMemory;

namespace Character
{
    public abstract class Player : MonoBehaviour
    {
        public GameManager GameManager;

        public float horizontalForce = 50f; // Movement force
        public float horizontalMaxSpeed = 8f; // Movement max speed
        public float verticalForce = 25f; // Jumping force

        protected float horizontalMovement = 0;
        protected bool isGrounded;
        protected bool isAboutToJump = false;
        protected bool isJumping = false;
        protected bool isVisible = false;

        protected Rigidbody2D rb;
        private BoxCollider2D boxCollider;

        public float groundCheckBuffer = 0.05f;
        private LayerMask platformMask;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
            platformMask = LayerMask.GetMask("Platform");
        }

        protected virtual void Update()
        {
            JumpStatusCheck();
        }

        protected virtual void FixedUpdate()
        {
            // perform horizontal moving
            rb.AddForce(new Vector2(horizontalMovement * horizontalForce, 0), ForceMode2D.Force);
            if (rb.velocity.x > horizontalMaxSpeed)
            {
                rb.velocity = new Vector2(horizontalMaxSpeed, rb.velocity.y);
            }
            else if (rb.velocity.x < -horizontalMaxSpeed)
            {
                rb.velocity = new Vector2(-horizontalMaxSpeed, rb.velocity.y);
            }

            // perform jumping
            if (isAboutToJump)
            {
                rb.AddForce(new Vector2(0f, verticalForce), ForceMode2D.Impulse);
                isGrounded = false;
                isAboutToJump = false;
                isJumping = true;
            }
        }

        private void JumpStatusCheck()
        {
            Vector2 groundCheckPoint1 = new Vector2(transform.position.x + boxCollider.bounds.size.x / 2,
                transform.position.y - boxCollider.bounds.size.y / 2);
            Vector2 groundCheckPoint2 = new Vector2(transform.position.x,
                transform.position.y - boxCollider.bounds.size.y / 2);
            Vector2 groundCheckPoint3 = new Vector2(transform.position.x - boxCollider.bounds.size.x / 2,
                transform.position.y - boxCollider.bounds.size.y / 2);
            RaycastHit2D hit1 = Physics2D.Raycast(groundCheckPoint1, Vector3.down,
                groundCheckBuffer, platformMask);
            RaycastHit2D hit2 = Physics2D.Raycast(groundCheckPoint2, Vector3.down,
                groundCheckBuffer, platformMask);
            RaycastHit2D hit3 = Physics2D.Raycast(groundCheckPoint3, Vector3.down,
                groundCheckBuffer, platformMask);
            isGrounded = hit1.collider is not null || hit2.collider is not null || hit3.collider is not null;

            isJumping = isJumping && rb.velocity.y > 0 && isGrounded;
        }

        public void initStatesFromLog(PlayerMemoryLog log)
        {
            transform.position = new Vector3(log.Position[0], log.Position[1], 0);
            rb.velocity = log.Velocity;
            horizontalMovement = log.MovementDirection;
        }


        public void HorizontalMove(float direction)
        {
            horizontalMovement = direction;
            if (horizontalMovement != 0)
            {
                transform.localScale = new Vector3(horizontalMovement, transform.localScale.y, transform.localScale.z);
            }
        }

        public void Jump()
        {
            isAboutToJump = true;
        }

        public void Appear()
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            GameObject visionTrigger = gameObject.transform.Find("Vision").gameObject;
            visionTrigger.SetActive(true);
        }

        public void Disappear()
        {
            isVisible = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            GameObject visionTrigger = gameObject.transform.Find("Vision").gameObject;
            visionTrigger.SetActive(false);
        }

        public abstract void Shroud();

        public abstract void UnShroud();
    }
}