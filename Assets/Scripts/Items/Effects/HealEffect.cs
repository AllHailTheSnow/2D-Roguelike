using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/ItemEffect/HealEffect")]
public class HealEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;

    public override void ExecuteEffect(Transform enemyPos)
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealth() * healPercent);

        playerStats.IncreaseHealthBy(healAmount);
    }
}
