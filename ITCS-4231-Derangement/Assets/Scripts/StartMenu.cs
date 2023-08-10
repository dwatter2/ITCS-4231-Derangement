using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Tutorial_Outside", LoadSceneMode.Single);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
