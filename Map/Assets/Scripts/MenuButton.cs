using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void GoToMainScene()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void GoToHardMode()
    {
        SceneManager.LoadScene("HardMode");
    }

    public void GoToNewScene()
    {
        SceneManager.LoadScene("New Scene");
    }
}
