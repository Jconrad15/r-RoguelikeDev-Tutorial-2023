using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayMessageSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject displayTextObject;

    [SerializeField]
    private GameObject displayTextPrefab;
    private Queue<GameObject> messageQueue = new Queue<GameObject>();
    private readonly int maxMessage = 4;

    public static DisplayMessageSystem Instance { get; private set; }
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

    private void Start()
    {
        displayTextObject.SetActive(true);
    }

    public void DisplayMessage(string message, Color color)
    {
        displayTextObject.SetActive(true);

        GameObject displayGO = Instantiate(
            displayTextPrefab, displayTextObject.transform);

        displayGO.GetComponent<DisplayMessage>().Init(message, color);
        messageQueue.Enqueue(displayGO);

        if (messageQueue.Count > maxMessage)
        {
            GameObject oldMessage = messageQueue.Dequeue();
            Destroy(oldMessage);
        }
    }
}
