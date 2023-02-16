using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditSceneManager : MonoBehaviour
{
    private bool isPlaying = false;

    private void OnEnable()
    {
        StartCoroutine(ChangeColor(5));
    }
    public IEnumerator ChangeColor(float t)
    {
        yield return new WaitForSeconds(t);
        isPlaying = true;
    }


    private void OnAnyButtonPressed()
    {
        
    }
}
