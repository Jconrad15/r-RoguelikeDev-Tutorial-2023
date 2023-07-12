using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : Item
{
    private int healAmount = 4;

    public override bool TryActivate(Entity targetEntity)
    {
        if (inInventory == false) { return false; }

        if (targetEntity.TryGetComponent<Fighter>(out var fighter))
        {
            int healedBy = fighter.Heal(healAmount);
            if (healedBy > 0)
            {
                DisplayMessageSystem.Instance.DisplayMessage(
                    "Heal by " + healedBy.ToString(),
                    ColorPalette.b2);
                return true;
            }
        }

        return false;
    }



}
