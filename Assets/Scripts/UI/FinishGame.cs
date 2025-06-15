using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null)
        {
            StartCoroutine(EndScreenRoutine());
        }
    }

    private IEnumerator EndScreenRoutine()
    {
        GameObject.Find("Canvas").GetComponent<UIController>().FinishGame();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("EndScene");
    }
}
