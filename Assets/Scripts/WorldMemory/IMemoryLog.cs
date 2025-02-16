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
        // 1 or 0 or -1
        public float MovementDirection { get; set; } 
        public Vector3 Velocity { get; set; }

        public abstract void Replay(Player player);
    }

    public class StateLog : PlayerMemoryLog
    {
        public override void Replay(Player player)
        {
            // do nothing
        }
    }
    
    public class HorizontalMoveLog : PlayerMemoryLog
    {
        public override void Replay(Player player)
        {
            player.HorizontalMove(MovementDirection);
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