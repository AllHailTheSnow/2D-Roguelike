using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    private GameObject healthbarUI;
    private PlayerController player;

    [Header("Flash VFX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Element Colours")]
    [SerializeField] private Color[] fireColour;
    [SerializeField] private Color[] iceColour;
    [SerializeField] private Color[] LightningColour;

    [Header("Element Particles")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject criticalHitFX;
    [SerializeField] private ParticleSystem dustFX;

    [Header("Screen Shake VFX")]
    [SerializeField] private float screenShakeMultiplier;
    public Vector3 screenShakeSwordCatch;
    public Vector3 screenShakeHighDamage;
    private CinemachineImpulseSource screenShake;

    [Header("Popup Text VFX")]
    [SerializeField] private GameObject popupTextPrefab;

    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        healthbarUI = GetComponentInChildren<HealthbarUI>().gameObject;
        originalMat = spriteRenderer.material;
        player = PlayerManager.Instance.player;
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        
    }

    private void RedColourBlink()
    {
        if (spriteRenderer.color != Color.white)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void CancelColourChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void IgniteFX(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating(nameof(IgniteColourFX), 0, 0.3f);
        Invoke(nameof(CancelColourChange), _seconds);
    }

    public void ChillFX(float _seconds)
    {
        chillFX.Play();
        InvokeRepeating(nameof(ChillColourFX), 0, 0.3f);
        Invoke(nameof(CancelColourChange), _seconds);
    }

    public void LightningFX(float _seconds)
    {
        shockFX.Play();
        InvokeRepeating(nameof(LigntningColourFX), 0, 0.3f);
        Invoke(nameof(CancelColourChange), _seconds);
    }

    private void IgniteColourFX()
    {
        if (spriteRenderer.color != fireColour[0])
        {
            spriteRenderer.color = fireColour[0];
        }
        else
        {
            spriteRenderer.color = fireColour[1];
        }
    }

    private void ChillColourFX()
    {
        if (spriteRenderer.color != iceColour[0])
        {
            spriteRenderer.color = iceColour[0];
        }
        else
        {
            spriteRenderer.color = iceColour[1];
        }
    }

    private void LigntningColourFX()
    {
        if (spriteRenderer.color != LightningColour[0])
        {
            spriteRenderer.color = LightningColour[0];
        }
        else
        {
            spriteRenderer.color = LightningColour[1];
        }
    }

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
        {
            healthbarUI.SetActive(false);
            spriteRenderer.color = Color.clear;
        }
        else
        {
            healthbarUI.SetActive(true);
            spriteRenderer.color = Color.white;
        }
    }

    public void CreateHitFX(Transform _target, bool _critical)
    {
        // Randomly generate the position and rotation of the hit effect
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-0.5f, 0.5f);
        float yPosition = Random.Range(-0.5f, 0.5f);

        //sets the hit rotation
        Vector3 hitFXRotation = new Vector3(0, 0, zRotation);

        //sets the hit prefab
        GameObject hitPrefab = hitFX;

        //if the hit is critical, change the prefab and rotation
        if (_critical)
        {
            //sets the hit prefab to the critical hit prefab
            hitPrefab = criticalHitFX;

            //sets the hit rotation
            float yRoation = 0;
            zRotation = Random.Range(-45, 45);

            //sets the hit rotation based on the facing direction
            if (GetComponent<Entity>().facingDir == -1)
            {
                yRoation = 180;
            }

            //sets the hit position
            hitFXRotation = new Vector3(0, yRoation, zRotation);
        }

        //instantiates the hit effect prefab
        GameObject newHitFX = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
        //sets the hit effect prefab to the target
        newHitFX.transform.Rotate(hitFXRotation);
        //destroys the hit effect prefab after 0.5 seconds
        Destroy(newHitFX, .5f);
    }

    public void DustFX()
    {
        if(dustFX != null)
        {
            dustFX.Play();
        }
    }

    public void CreatePopupText(string _text)
    {
        float xPos = Random.Range(-1, 1);
        float yPos = Random.Range(1, 3);

        Vector3 posOffset = new Vector3(xPos, yPos, 0);

        GameObject newPopupText = Instantiate(popupTextPrefab, transform.position + posOffset, Quaternion.identity);

        newPopupText.GetComponent<TextMeshPro>().text = _text;
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * screenShakeMultiplier;
        screenShake.GenerateImpulse();
    }

    private IEnumerator FlashFXRoutine()
    {
        spriteRenderer.material = hitMat;
        Color currentColour = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = currentColour;
        spriteRenderer.material = originalMat;
    }
}
