using System;
using System.Collections;
using UnityEngine;
using WorldMemory;

namespace Character
{
    public class CurrentPlayer : Player, IPortalEnterable
    {
        public GameObject PastPlayerPrefab;

        public bool isShrouded = false;

        protected override void Start()
        {
            base.Start();
        }

        private IEnumerator PerformShroud()
        {
            Shroud();
            GameManager.WorldMemory.AddLog(new ShroudLog()
            {
                TimeStamp = GameManager.CurrentTime,
                OwnerName = gameObject.name,
                Position = transform.position,
            });

            yield return new WaitForSeconds(3f);

            UnShroud();
            GameManager.WorldMemory.AddLog(new UnShroudLog()
            {
                TimeStamp = GameManager.CurrentTime,
                OwnerName = gameObject.name,
                Position = transform.position,
            });
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
                    Direction = currentHorizontalMovement,
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
                });
                haveAddedLog = true;
            }

            if (Input.GetKey(KeyCode.LeftShift) && !isShrouded)
            {
                StartCoroutine(PerformShroud());
                haveAddedLog = true;
            }

            bool halfSecondPassed = Math.Floor(10 * GameManager.CurrentTime) != Math.Floor(10 * (GameManager.CurrentTime - Time.deltaTime));
            if (!haveAddedLog && halfSecondPassed)
            {
                GameManager.WorldMemory.AddLog(new PositionLog()
                {
                    TimeStamp = GameManager.CurrentTime,
                    OwnerName = gameObject.name,
                    Position = transform.position,
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

        public override void Shroud()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
            isShrouded = true;
        }

        public override void UnShroud()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
            isShrouded = false;
        }
    }
}