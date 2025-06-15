using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBossMusic : MonoBehaviour
{
    [SerializeField] private int areaMusicIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            AudioManager.Instance.PlayBGM(areaMusicIndex);
        }
    }
}
