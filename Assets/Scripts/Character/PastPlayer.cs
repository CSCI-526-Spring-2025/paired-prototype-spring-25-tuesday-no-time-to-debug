using WorldMemory;

namespace Character
{
    public class PastPlayer : Player
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
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
                    initStatesFromLog(playerMemoryLog);
                    Appear();
                    isVisible = true;
                }
                playerMemoryLog.Replay(this);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}