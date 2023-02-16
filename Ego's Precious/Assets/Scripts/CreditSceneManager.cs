using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject credits;

    private bool isPlaying = false;
    
    private void OnEnable()
    {
        StartCoroutine(StartCredits(5));
    }
    public IEnumerator StartCredits(float t)
    {
        yield return new WaitForSeconds(t);
        isPlaying = true;
    }


    private void OnStartGame()
    {
        if(isPlaying)
        {
            gameObject.SetActive(false);
            credits.SetActive(true);
        }
    }
}
