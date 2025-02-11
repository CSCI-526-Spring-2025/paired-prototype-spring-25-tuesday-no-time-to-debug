using UnityEngine;

public interface IPortalEnterable
{
    public void OnEnterPortal(Vector3 newPosition, int rewindTime);
}

public class Portal : MonoBehaviour
{
    public Portal ExitPortal;

    public int RewindTime;

    public GameManager GameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!ExitPortal) return;

        if (other.gameObject.TryGetComponent<IPortalEnterable>(out var enteringObject))
        {
            enteringObject.OnEnterPortal(ExitPortal.transform.position + new Vector3(1f, 0, 0), RewindTime);
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
