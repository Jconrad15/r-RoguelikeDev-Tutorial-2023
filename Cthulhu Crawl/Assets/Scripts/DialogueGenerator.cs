using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGenerator : MonoBehaviour
{

    public Dialogue GetDialogue(Entity entity)
    {
        string text = "Greatings from rhy'lon";
        Dialogue dialogue = new Dialogue(text);

        return dialogue;
    }
}
