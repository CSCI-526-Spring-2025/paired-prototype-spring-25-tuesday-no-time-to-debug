using System;
using System.Linq;
using UnityEngine;
using WorldMemory;

public class GameManager : MonoBehaviour
{
    public WorldMemory.WorldMemory WorldMemory;

    public int PlayerIteration;

    public float CurrentTime;

    public void RewindTime(int seconds)
    {
        CurrentTime = (float)(Math.Floor(2 * (CurrentTime - seconds)) / 2);
    }

    public IMemoryLog[] GetMemoryFor(string ownerName)
    {
        return WorldMemory.GetLogsFor(ownerName)
            .Where(log => log.TimeStamp >= CurrentTime)
            .OrderBy(log => log.TimeStamp)
            .ToArray();
    }

    public IMemoryLog[] GetMemoryByFrame(string ownerName)
    {
        return WorldMemory.GetLogsFor(ownerName)
            .Where(log => log.TimeStamp >= CurrentTime && log.TimeStamp < CurrentTime + Time.deltaTime)
            .OrderBy(log => log.TimeStamp)
            .ToArray();
    }

    public IMemoryLog GetNextMemoryLogFor(string ownerName)
    {
        return GetMemoryFor(ownerName).FirstOrDefault();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LateUpdate()
    {
        CurrentTime += Time.deltaTime;
    }
}