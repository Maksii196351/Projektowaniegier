using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lvl2 : MonoBehaviour
{
     [SerializeField]
   
    private int respawn;
    void Start()
    {
        
    }

 
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            SceneManager.LoadScene(respawn);
        }
    }
}