using UnityEngine;
using UnityEngine.UI;

public class GoalFlag : MonoBehaviour
{
    public GameObject levelCompleteUI;  // Reference to the Level Complete UI

    void Start()
    {
        // Ensure the UI is hidden at the start
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Check if the Player collides
        {
            ShowLevelCompleteUI();
            Time.timeScale = 0;
        }
    }

    void ShowLevelCompleteUI()
    {
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);  // Show "Level Complete" UI
        }
    }
}
