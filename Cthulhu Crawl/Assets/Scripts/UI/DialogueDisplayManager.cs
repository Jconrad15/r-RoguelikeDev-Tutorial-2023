using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueDisplayManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private GameObject dialogueArea;

    private void Start()
    {
        HideDialogue();
    }

    public void StartDialogue(string text)
    {
        dialogueArea.SetActive(true);
        dialogueText.SetText(text);
    }

    public void HideDialogue()
    {
        dialogueArea.SetActive(false);
    }

}
