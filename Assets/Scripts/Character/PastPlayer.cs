using System;
using System.Linq;
using UnityEngine;
using WorldMemory;

namespace Character
{
    public class PastPlayer : Player
    {
        protected override void Start()
        {
            base.Start();
        }

        private bool ShouldPlayerBeShrouded(IMemoryLog[] allMemoryLogs)
        {
            IMemoryLog log = allMemoryLogs
                .ToList()
                .FindAll(log => log is ShroudLog || log is UnShroudLog)
                .OrderBy(log => log.TimeStamp)
                .FirstOrDefault();

            if (log is null || log is ShroudLog) return false;

            else return true;
        }

        protected override void Update()
        {
            IMemoryLog[] allMemoryLogs = GameManager.GetMemoryFor(gameObject.name);
            IMemoryLog[] memoryLogs = GameManager.GetMemoryByFrame(gameObject.name);
            if (memoryLogs is null || memoryLogs.Length == 0)
            {
                return;
            }

            foreach (IMemoryLog memoryLog in memoryLogs)
            {
                PlayerMemoryLog playerMemoryLog = memoryLog as PlayerMemoryLog;
                if (!isVisible)
                {
                    Appear();
                    InitStatesFromLog(playerMemoryLog);
                    isVisible = true;

                    if (ShouldPlayerBeShrouded(allMemoryLogs))
                    {
                        Shroud();
                    }
                }

                playerMemoryLog.Replay(this);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Shroud()
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            var vision = gameObject.transform.Find("Vision").gameObject;
            vision.GetComponent<SpriteRenderer>().enabled = false;
        }

        public override void UnShroud()
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            var vision = gameObject.transform.Find("Vision").gameObject;
            vision.GetComponent<SpriteRenderer>().enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var ob = GameObject.Find("Canvas").transform.Find("YouLoseText").gameObject;
                ob.SetActive(true);

                Time.timeScale = 0;
            }
        }
    }
}