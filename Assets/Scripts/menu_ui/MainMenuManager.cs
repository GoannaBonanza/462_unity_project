using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject controlScreen;
    public GameObject settingsScreen;
    public GameObject levelsScreen;

    public void load_level1()
    {
        SceneManager.LoadScene("Garden");
    }
    public void load_boss1()
    {
        SceneManager.LoadScene("CainArena");
    }
    public void load_level2()
    {
        SceneManager.LoadScene("Heaven");
    }
    public void load_boss2()
    {
        SceneManager.LoadScene("GodArena");
    }
    public void load_tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void load_endless()
    {
        SceneManager.LoadScene("Endless");
    }
    public void controls()
    {
        controlScreen.SetActive(true);
    }
    public void settings()
    {
        settingsScreen.SetActive(true);
    }
    public void levels()
    {
        levelsScreen.SetActive(true);
    }
    public void quitGame()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
    public void control_back_button()
    {
        controlScreen.SetActive(false);
    }
    public void settings_back_button()
    {
        settingsScreen.SetActive(false);
    }
    public void levels_back_button()
    {
        levelsScreen.SetActive(false);
    }
}
