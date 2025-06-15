using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeTime;

    public void SetupAfterImage(float _fadeTime, Sprite _sprite)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = _sprite;
        fadeTime = _fadeTime;
    }

    private void Update()
    {
        float alpha = spriteRenderer.color.a - (fadeTime * Time.deltaTime);

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

        if(spriteRenderer.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
