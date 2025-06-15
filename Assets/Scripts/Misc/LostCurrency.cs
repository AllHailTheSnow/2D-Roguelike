using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostCurrency : MonoBehaviour
{
    public int currency;
    private bool active;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null && active == false)
        {
            Debug.Log("Souls found!");
            StartCoroutine(FadeAway());
        }
    }

    private IEnumerator FadeAway()
    {
        active = true;
        PlayerManager.Instance.currency += currency;

        float duration = 1f;
        float startAlpha = sr.color.a;
        float endAlpha = 0;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            Color newColour = sr.color;
            newColour.a = newAlpha;
            sr.color = newColour;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Color finalColour = sr.color;
        finalColour.a = endAlpha;
        sr.color = finalColour;
        Destroy(this.gameObject);

    }
}
