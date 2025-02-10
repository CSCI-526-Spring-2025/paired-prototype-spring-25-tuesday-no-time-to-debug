using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IMemoryLog
{
    public float TimeStamp { get; set; }
    public string OwnerName { get; set; }
}

public class PlayerMemoryLog : IMemoryLog
{
    public float TimeStamp { get; set; }
    public string OwnerName { get; set; }
    public Vector2 Position { get; set; }
    public KeyCode[] KeysPressed { get; set; }
    public bool IsAlive { get; set; }
}

public class WorldMemory : MonoBehaviour
{
    private List<IMemoryLog> memory = new List<IMemoryLog>();

    public void AddLog(IMemoryLog log)
    {
        memory.Add(log);
    }

    public IMemoryLog[] GetAlllogs()
    {
        return memory.ToArray();
    }

    public IMemoryLog[] GetLogsFor(string ownerName)
    {
        return memory.Where(log => log.OwnerName == ownerName).ToArray();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
