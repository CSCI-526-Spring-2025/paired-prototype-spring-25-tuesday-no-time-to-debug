using Character;
using UnityEngine;

namespace WorldMemory
{
    public interface IMemoryLog
    {
        public float TimeStamp { get; set; }
        public string OwnerName { get; set; }
    }
    
    public abstract class PlayerMemoryLog : IMemoryLog
    {
        public float TimeStamp { get; set; }
        public string OwnerName { get; set; }
        public Vector2 Position { get; set; }

        public abstract void Replay(Player player);
    }

    public class PositionLog : PlayerMemoryLog
    {
        public override void Replay(Player player)
        {
            // do nothing
        }
    }

    public class ShroudLog : PlayerMemoryLog
    {
        public override void Replay(Player player)
        {
            player.Shroud();
        }
    }

    public class UnShroudLog : PlayerMemoryLog
    {
        public override void Replay(Player player)
        {
            player.UnShroud();
        }
    }

    public class HorizontalMoveLog : PlayerMemoryLog
    {
        // 1 or 0 or -1
        public float Direction { get; set; } 
        
        public override void Replay(Player player)
        {
            player.HorizontalMove(Direction);
        }
    }
    
    public class JumpLog : PlayerMemoryLog
    {
        public override void Replay(Player player)
        {
            player.Jump();
        }
    }
    
    public class DisappearLog : PlayerMemoryLog
    {
        public override void Replay(Player player)
        {
            player.Disappear();
        }
    }
}