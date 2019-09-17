using UnityEngine;
using UnityEngine.UI;

public class CursorUIElement : MonoBehaviour
{
    public Image cursorImage;

    public void Initialize(Sprite sprite)
    {
        cursorImage.sprite = sprite;
    }
}