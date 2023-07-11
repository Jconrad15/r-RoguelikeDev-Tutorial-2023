using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayMessage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI displayText;

    public void Init(string message)
    {
        displayText.SetText(message);
    }

}
