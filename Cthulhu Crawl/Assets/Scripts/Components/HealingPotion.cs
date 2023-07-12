using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : Item
{
    private int healAmount = 4;

    public void Init()
    {

    }

    public override void Activate(Entity targetEntity)
    {
        base.Activate(targetEntity);
        
        Fighter fighter = targetEntity.GetComponent<Fighter>();
        if (fighter != null)
        {
            fighter.Heal(healAmount);
            if (healAmount > 0)
            {
                DisplayMessageSystem.Instance.DisplayMessage(
                    "Heal by " + healAmount.ToString(),
                    ColorPalette.b2);
            }
        }
    }

}
