using UnityEngine;
using UnityEngine.UI;

public class CursorUIElement : MonoBehaviour
{
    public Image cursorImage;
    private Vector3 targetPosition = Vector3.zero;

    public void Initialize(Sprite sprite,Transform iconPosition)
    {
        cursorImage.sprite = sprite;
        if (iconPosition != null)
            targetPosition = iconPosition.position;
    }

    private void LateUpdate()
    {
        if (targetPosition == Vector3.zero) return;
        transform.position = CameraManager.Instance.mainCamera.WorldToScreenPoint(targetPosition);
    }
}