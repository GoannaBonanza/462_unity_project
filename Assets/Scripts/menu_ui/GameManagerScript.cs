using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject gameOverText;
    public GameObject winText;
    public GameObject pauseScreen;
    public GameObject hurtScreen;
    public int totalKilled = 0;
    public int winCount = 20;
    public bool bossKilled = false;
    public Enemy boss;
    private PlayerInteractions pi;
    void Awake()
    {
        pi = FindObjectOfType<PlayerInteractions>();
    }
    void Start()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (boss != null) bossKilled = boss.dead;
        if (WinConditionAchieved()) Win();
        if (gameOverUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public bool WinConditionAchieved()
    {
        return (winCount == 0 ? false : totalKilled >= winCount) || bossKilled;
    }
    public void Win()
    {
        StartCoroutine(ShowGameOver(3.0f));
        winText.SetActive(true);
        gameOverText.SetActive(false);
        pauseScreen.SetActive(false);
    }
    public void GameOver()
    {
        StartCoroutine(ShowGameOver(1.5f));
        winText.SetActive(false);
        gameOverText.SetActive(true);
        pauseScreen.SetActive(false);
    }
    public void HandleHurt()
    {
        StartCoroutine(DelayHurtRemove());
        hurtScreen.SetActive(true);
    }
    public void HandlePause()
    {
        //don't allow pause after winning
        if (WinConditionAchieved()) return;
        winText.SetActive(false);
        //immediately show screen
        StartCoroutine(ShowGameOver(0));
        pi.paused = true;
        pauseScreen.SetActive(true);
        gameOverText.SetActive(false);
    }
    private IEnumerator DelayHurtRemove()
    {
        yield return new WaitForSeconds(0.15f);
        hurtScreen.SetActive(false);
    }
    //button push functions
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void ResumeButton()
    {
        pauseScreen.SetActive(false);
        gameOverUI.SetActive(false);
        pi.paused = false;
        Time.timeScale = 1;
    }
    //delay game over, for both win and loss
    private IEnumerator ShowGameOver(float delay = 2.0f)
    {
        yield return new WaitForSeconds(delay);
        //used to lock camera after success
        if (WinConditionAchieved()) pi.success = true;
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
}
