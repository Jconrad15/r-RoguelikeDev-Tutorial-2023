using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode { Exploration, Dialogue };
public class ModeController : MonoBehaviour
{
    private DialogueDisplayManager dialogueDisplayManager;

    public Mode CurrentMode { get; private set; }

    private void Start()
    {
        CurrentMode = Mode.Exploration;
        dialogueDisplayManager =
            FindAnyObjectByType<DialogueDisplayManager>();
    }

    // Singleton
    public static ModeController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public void SwitchMode(Mode targetMode)
    {
        CurrentMode = targetMode;

        if (targetMode == Mode.Exploration)
        {
            dialogueDisplayManager.EndDialogue();
        }
    }

}
