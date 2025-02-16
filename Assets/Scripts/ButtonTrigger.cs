using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public Transform button;  // Reference to the button object
    public GameObject door;    // Reference to the door object (use GameObject instead of Transform)
    public float buttonPressDepth = 0.1f;  // How much the button lowers

    private Vector3 buttonOriginalPosition;
    private bool isPressed = false;

    void Start()
    {
        // Store original button position
        buttonOriginalPosition = button.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Memory")) && !isPressed)
        {
            isPressed = true;
            LowerButton();
            HideDoor();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Memory")) && isPressed)
        {
            isPressed = false;
            RaiseButton();
            ShowDoor();
        }
    }

    void LowerButton()
    {
        // Move the button down slightly
        button.position = new Vector3(button.position.x, buttonOriginalPosition.y - buttonPressDepth, button.position.z);
    }

    void RaiseButton()
    {
        // Move the button back to its original position
        button.position = buttonOriginalPosition;
    }

    void HideDoor()
    {
        if (door != null)
        {
            door.GetComponent<Renderer>().enabled = false; // Hide door
            door.GetComponent<Collider2D>().enabled = false; // Disable collisions
        }
    }

    void ShowDoor()
    {
        if (door != null)
        {
            door.GetComponent<Renderer>().enabled = true; // Show door
            door.GetComponent<Collider2D>().enabled = true; // Enable collisions
        }
    }
}
