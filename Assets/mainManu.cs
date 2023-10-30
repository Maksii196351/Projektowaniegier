using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  
public class mainManu : MonoBehaviour
{
public void PlayGame()
{
    SceneManager.LoadSceneAsync(1);
}
public void EndGame()
{
    Application.Quit();
}
}