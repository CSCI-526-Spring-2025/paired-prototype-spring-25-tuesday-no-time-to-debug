using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using UnityEngine;
using WorldMemory;

public class GameManager : MonoBehaviour
{
    public WorldMemory.WorldMemory WorldMemory;
    public GameObject PastPlayerPrefab;

    public int PlayerIteration;

    public float CurrentTime;
    
    private List<PastPlayer> PastPlayers = new List<PastPlayer>();

    public void RewindTime(int seconds)
    {
        foreach (PastPlayer pastPlayer in PastPlayers)
        {
            pastPlayer.Disappear();
        }
        
        CurrentTime = (float)(Math.Floor(2 * (CurrentTime - seconds)) / 2);

        //to be edited
        var newPastPlayer = Instantiate(PastPlayerPrefab);

        newPastPlayer.tag = "Memory";
        newPastPlayer.name = $"Player_{PlayerIteration}";
        newPastPlayer.GetComponent<Player>().GameManager = this;

        newPastPlayer.GetComponent<SpriteRenderer>().enabled = false;

        PlayerIteration++;
        PastPlayers.Add(newPastPlayer.GetComponent<PastPlayer>());
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