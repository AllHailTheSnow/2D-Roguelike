using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemies Effect", menuName = "Data/ItemEffect/FreezeEnemiesEffect")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        if(playerStats.currentHP > playerStats.GetMaxHealth() * 0.1f)
        {
            return;
        }

        if (!Inventory.Instance.CanUseArmour()) { return; }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
