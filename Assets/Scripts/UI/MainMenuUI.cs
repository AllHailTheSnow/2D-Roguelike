using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] FadeUI fadeUI;

    private void Start()
    {
        if(SaveManager.Instance.HasSavedData() == false)
        {
            continueButton.SetActive(false);
        }
    }

    public void NewGame()
    {
        SaveManager.Instance.DeleteSavedData();
        StartCoroutine(LoadSceneRoutine());
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneRoutine());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneRoutine()
    {
        fadeUI.FadeOut();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
