using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");




    }

    public void Levels()
    {
        SceneManager.LoadScene("Levels");

    }

    public void QuitGame()
    {
       Application.Quit();
    }

    public void MainScene()
    {
        SceneManager.LoadScene("Main Scene");

    }
    public void Level2()
    {
        SceneManager.LoadScene("Level 2");

    }
    public void Level3()
    {
        SceneManager.LoadScene("Level 3");

    }
    public void Level4()
    {
        SceneManager.LoadScene("Level 4");

    }

    public void Mainmenu() 
    {
        SceneManager.LoadScene("Main Menu");
    
    }

}
