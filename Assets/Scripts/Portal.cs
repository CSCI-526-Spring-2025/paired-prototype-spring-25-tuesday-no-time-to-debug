using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal ExitPortal;

    public int RewindTime;

    public GameManager GameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!ExitPortal) return;

        if (other.CompareTag("Player"))
        {
            string enteredPlayerName = other.gameObject.name;

            GameManager.WorldMemory.AddLog(new PlayerMemoryLog
            {
                TimeStamp = GameManager.CurrentTime,
                OwnerName = enteredPlayerName,
                KeysPressed = { },
                Position = other.transform.position,
                IsAlive = false
            });

            var newGhost = Instantiate(other.gameObject);

            newGhost.tag = "Memory";
            newGhost.name = enteredPlayerName;
            newGhost.GetComponent<SpriteRenderer>().enabled = false;

            other.transform.position = ExitPortal.transform.position + new Vector3(1f, 0, 0);
            other.gameObject.name = $"Player_{++GameManager.PlayerIteration}";

            GameManager.rewindTime(RewindTime);

        }
        else if (other.CompareTag("Memory"))
        {

        }
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
