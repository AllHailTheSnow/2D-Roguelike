using UnityEngine;
[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/ItemEffect/BuffEffect")]
public class BuffEffect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform enemyPos)
    {
        // Get the player's stats
        stats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        // Increase the stat by the buff amount for the buff duration
        stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetStat(buffType));
    }
}
