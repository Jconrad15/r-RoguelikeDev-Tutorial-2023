using UnityEngine;
using UnityEngine.UI;

public class AvailableSprites : MonoBehaviour
{
    [SerializeField]
    private CharacterCreator creator;

    [SerializeField]
    private Image image; 

    [SerializeField]
    private Sprite[] sprites;
    private int current;

    private void Start()
    {
        current = 0;
        UpdateDisplay();
    }

    public void Next()
    {
        current++;
        if (current >= sprites.Length)
        {
            current = 0;
        }
        UpdateDisplay();
    }

    public void Previous()
    {
        current--;
        if (current < 0)
        {
            current = sprites.Length - 1;
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        image.sprite = sprites[current];
        creator.UpdateSprite(sprites[current]);
    }

}
