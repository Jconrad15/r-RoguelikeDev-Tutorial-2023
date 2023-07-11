using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentHealthText;
    [SerializeField]
    private TextMeshProUGUI maxHealthText;

    public void Init()
    {
        EntityManager em = FindAnyObjectByType<EntityManager>();
        Fighter fighter = em.Player.GetComponent<Fighter>();
        fighter.RegisterOnCurrentHealthChanged(
            OnPlayerCurrentHealthChanged);
        fighter.RegisterOnMaxHealthChanged(
            OnPlayerMaxHealthChanged);

        OnPlayerCurrentHealthChanged(fighter);
        OnPlayerMaxHealthChanged(fighter);
    }

    private void OnPlayerCurrentHealthChanged(Fighter f)
    {
        currentHealthText.SetText(f.CurrentHealth.ToString());
    }

    private void OnPlayerMaxHealthChanged(Fighter f)
    {
        maxHealthText.SetText(f.MaxHealth.ToString());
    }

}
