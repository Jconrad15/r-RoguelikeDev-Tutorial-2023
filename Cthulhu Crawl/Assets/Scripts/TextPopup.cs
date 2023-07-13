using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI displayText;
    [SerializeField]
    private Animator animator;

    public void Init(Vector2 postiion, string text, Color color)
    {
        transform.position = postiion;
        displayText.color = color;
        displayText.SetText(text);

        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}
