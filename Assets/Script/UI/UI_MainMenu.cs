using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UI_FadeScreen fadeScreen;

    private void Start()
    {
        if (!SaveManager.instance.HasSaveFile())
        {
            continueButton.SetActive(false);
        }
    }
    public void ContinueGame()
    {
        StartCoroutine(ScreenFadeOut(1.5f, () =>
        {
            SceneManager.LoadScene(sceneName);
        }));
    }



    public void NewGame()
    {
        SaveManager.instance.DeleteSaveFile();
        StartCoroutine(ScreenFadeOut(1.5f, () =>
        {
            SceneManager.LoadScene(sceneName);
        }));
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    private IEnumerator ScreenFadeOut(float _dalay, Action onComplete)
    {
        fadeScreen.TriggerFadeOut();
        yield return new WaitForSeconds(_dalay);
        onComplete?.Invoke();
    }
}
