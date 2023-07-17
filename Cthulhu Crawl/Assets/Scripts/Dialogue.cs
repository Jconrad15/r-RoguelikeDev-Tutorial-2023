using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public string text;
    public List<Topic> topics;
    public Dialogue(string text)
    {
        this.text = text;
        topics = new List<Topic>();
    }
}
