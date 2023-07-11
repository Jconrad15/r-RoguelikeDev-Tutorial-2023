using UnityEngine;

[CreateAssetMenu(
    fileName = "FighterSO",
    menuName = "ScriptableObjects/FighterSO",
    order = 2)]
public class FighterSO : ScriptableObject
{
    public int maxHealth;
    public int defense;
    public int power;
}