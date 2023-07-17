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
        EndDialogue();
    }

    public void StartDialogue(Talker playerTalker, Talker otherTalker)
    {
        ModeController.Instance.SwitchMode(Mode.Dialogue);
        dialogueArea.SetActive(true);
        dialogueText.SetText(otherTalker.Dialogue.text);
    }

    public void EndDialogue()
    {
        dialogueArea.SetActive(false);
    }

}
