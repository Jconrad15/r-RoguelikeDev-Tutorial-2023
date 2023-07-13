using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject textPopupPrefab;

    public void CreateTextPopup(Vector2 position, string text, Color color)
    {
        position.y += 1f;
        GameObject newTextPopup = Instantiate(textPopupPrefab);
        newTextPopup.GetComponent<TextPopup>().Init(position, text, color);
    }

    public static TextPopupManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

}
