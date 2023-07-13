using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    public CreatedCharacter createdCharacter;

    private void Awake()
    {
        createdCharacter = new CreatedCharacter();
    }

    public void UpdateSprite(Sprite sprite)
    {
        createdCharacter.sprite = sprite;
    }

    public void UpdateName(string name)
    {
        createdCharacter.characterName = name;
    }

}
