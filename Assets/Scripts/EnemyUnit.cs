using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public virtual void TakeTurn(){
    }

    public override void TakeDamage(int damage = 1)
    {
        base.TakeDamage(damage);

        if (currentHp <= 0)
        {
            animator.SetTrigger("Die");
            UIManager.Instance.UpdateControlButton();
        }
    }
}
