using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int MaxHealth { get; private set; }
    public int Defense { get; private set; }
    public int Power { get; private set; }

    private int currentHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        private set
        {
            if (currentHealth > MaxHealth)
            {
                value = MaxHealth;
            }
            else if (currentHealth < 0)
            {
                value = 0;
            }
            currentHealth = value;
        }
    }

    public Fighter(
        int maxHealth, int currentHealth,
        int defense, int power)
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        Defense = defense;
        Power = power;
    }


}
