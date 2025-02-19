using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Mainmenuroute : MonoBehaviour
{

    public void PlayGame ()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void Continue()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void NewGame()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void Back()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
