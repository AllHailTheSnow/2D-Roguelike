using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/ItemEffect/IceAndFire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] float xVelocity;

    public override void ExecuteEffect(Transform respawnPos)
    {
        PlayerController player = PlayerManager.Instance.player;

        bool thridAttack = player.PrimaryAttack.ComboCounter == 2;

        if(thridAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, respawnPos.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);
            Destroy(newIceAndFire, 3f);

        }
    }
}
