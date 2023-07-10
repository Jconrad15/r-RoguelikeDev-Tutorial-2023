using UnityEngine;

[CreateAssetMenu(
    fileName = "EntitySO",
    menuName = "ScriptableObjects/EntitySO",
    order = 1)]
public class EntitySO : ScriptableObject
{
    public string characterName;
    public Sprite sprite;
    public Color color;
    public bool blocksMovement;
}