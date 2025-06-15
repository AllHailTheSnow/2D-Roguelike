using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlightAnimationTriggers : EnemyAnimationTrigger
{
    private EnemyDeathBlight enemyDeathBlight => GetComponentInParent<EnemyDeathBlight>();

    private void Relocate()
    {
        enemyDeathBlight.FindPosition();
    }

    private void MakeInvisible()
    {
        enemyDeathBlight.entityFX.MakeTransparent(true);
    }

    private void MakeVisible()
    {
        enemyDeathBlight.entityFX.MakeTransparent(false);
    }
}
