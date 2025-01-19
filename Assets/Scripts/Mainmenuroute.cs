using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Mainmenuroute : MonoBehaviour
{

    public void PlayGame (){
        SceneManager.LoadSceneAsync(1);
    }
}
