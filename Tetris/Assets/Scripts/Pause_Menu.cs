using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject PausedMenuCanvas;
    public GameObject DeathMenuCanvas;
    public Board board {get; private set;}

    public void Initialize(Board board)
    {
        this.board = board;
    }


    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (this.board.dead)
        {
            GameEnd();
        }
    }

    public void Stop()
    {
        PausedMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void GameEnd()
    {
        if (!DeathMenuCanvas.activeSelf) // Prevent repeated calls.
        {
            DeathMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            paused = true;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Play()
    {
        PausedMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        
    }

    public void MainMenuButton(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
