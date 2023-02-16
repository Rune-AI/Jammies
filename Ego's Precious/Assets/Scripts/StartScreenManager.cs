using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject loadingScreen;
    
    public void OnStartGame(InputAction.CallbackContext context)
    {
        //Debug.Log("Start Game");
        startScreen.SetActive(false);
        loadingScreen.SetActive(true);
        SceneManager.LoadScene(1);
        
    }
}
