using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBus : MonoBehaviour
{
    public bool loadGame = false;
    public CreatedCharacter character;

    public void SetLoadGame()
    {
        loadGame = true;
    }

    public void GetSceneData()
    {
        CheckForCharacterCreation();
    }

    private void CheckForCharacterCreation()
    {
        CharacterCreator cc = FindAnyObjectByType<CharacterCreator>();
        if (cc != null)
        {
            character = cc.createdCharacter;
        }
    }

    // Singleton
    public static SceneBus Instance { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
}
