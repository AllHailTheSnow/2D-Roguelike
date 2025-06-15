using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("After Image VFX")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float fadeRate;
    [SerializeField] private float afterImageCooldown;
    [SerializeField] private float afterImageCooldownTimer;

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            //sets the after image cooldown timer
            afterImageCooldownTimer = afterImageCooldown;
            //sets the after image prefab 
            GameObject newAfterImage = Instantiate(afterImagePrefab, new Vector3(transform.position.x, transform.position.y + 0.7f), transform.rotation);
            //sets the after image prefab to the target
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(fadeRate, spriteRenderer.sprite);
        }
    }
}
