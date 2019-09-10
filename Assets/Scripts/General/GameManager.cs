using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameHasEnded = false;
    public GameObject gameOverPanel;

    private EventManager eventManager;

    void Awake()
    {
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
    }

    public void EndGame()
    {
        if(!gameHasEnded)
        {
            gameHasEnded = true;
            gameOverPanel.SetActive(true);
        }
        eventManager.TriggerEvent(EventType.GameOver);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
