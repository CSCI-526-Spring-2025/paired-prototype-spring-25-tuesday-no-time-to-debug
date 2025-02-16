using System;
using UnityEngine;
using WorldMemory;

namespace Character
{
    public class CurrentPlayer : Player, IPortalEnterable
    {
        public GameObject PastPlayerPrefab;

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            bool haveAddedLog = false;

            float currentHorizontalMovement = Input.GetAxisRaw("Horizontal");
            if (currentHorizontalMovement != horizontalMovement)
            {
                HorizontalMove(currentHorizontalMovement);
                GameManager.WorldMemory.AddLog(new HorizontalMoveLog()
                {
                    TimeStamp = GameManager.CurrentTime,
                    OwnerName = gameObject.name,
                    Position = transform.position,
                    MovementDirection = horizontalMovement, 
                    Velocity = rb.velocity
                });
                haveAddedLog = true;
            }

            if (Input.GetAxisRaw("Vertical") > 0 && isGrounded && !isAboutToJump && !isJumping)
            {
                Jump();
                GameManager.WorldMemory.AddLog(new JumpLog()
                {
                    TimeStamp = GameManager.CurrentTime,
                    OwnerName = gameObject.name,
                    Position = transform.position,
                    MovementDirection = horizontalMovement,
                    Velocity = rb.velocity
                });
                haveAddedLog = true;
            }

            bool halfSecondPassed = Math.Floor(10 * GameManager.CurrentTime) !=
                                    Math.Floor(10 * (GameManager.CurrentTime - Time.deltaTime));
            if (!haveAddedLog && halfSecondPassed)
            {
                GameManager.WorldMemory.AddLog(new StateLog()
                {
                    TimeStamp = GameManager.CurrentTime,
                    OwnerName = gameObject.name,
                    Position = transform.position,
                    MovementDirection = horizontalMovement,
                    Velocity = rb.velocity
                });
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void OnEnterPortal(Vector3 newPosition, int rewindTime)
        {
            GameManager.WorldMemory.AddLog(new DisappearLog()
            {
                TimeStamp = GameManager.CurrentTime,
                OwnerName = gameObject.name,
                Position = transform.position,
            });

            //to be edited
            var newPastPlayer = Instantiate(PastPlayerPrefab);

            newPastPlayer.tag = "Memory";
            newPastPlayer.name = gameObject.name;
            newPastPlayer.GetComponent<Player>().GameManager = GameManager;
            newPastPlayer.GetComponent<SpriteRenderer>().enabled = false;

            transform.position = newPosition;
            gameObject.name = $"Player_{++GameManager.PlayerIteration}";

            GameManager.RewindTime(rewindTime);
        }
    }
}