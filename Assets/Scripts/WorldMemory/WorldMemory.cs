using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldMemory
{
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
}