using UnityEngine;

namespace Character
{
    public class CurrentPlayer : Player
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            // Get input from arrow keys or WASD
            movement = Input.GetAxisRaw("Horizontal");
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (isGrounded)
                {
                    isJumping = true;
                }
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}