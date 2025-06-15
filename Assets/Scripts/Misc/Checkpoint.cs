using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject lighting;
    private Animator animator;
    public string id;
    public bool activeStatus;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    [ContextMenu("Generate Checkpoint ID")]
    private void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        if (activeStatus == false)
        {
            AudioManager.Instance.PlaySFX(4, null);
        }
        activeStatus = true;
        animator.SetBool("Active", true);
        lighting.SetActive(true);
    }
}
