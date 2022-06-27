using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystickTranslation : VirtualJoystick
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        imageBackground.rectTransform.position = eventData.pressPosition;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imageBackground.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            // touchPosition 값 정규화 [-1 ~ 1]
            touchPosition.x = (touchPosition.x / imageBackground.rectTransform.sizeDelta.x);
            touchPosition.y = (touchPosition.y / imageBackground.rectTransform.sizeDelta.y);

            // touchPosition값 정규화 [-1 ~ 1]
            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;

            // 가상 조이스틱 컨트롤러 이미지 이동
            imageController.rectTransform.anchoredPosition = new Vector2(touchPosition.x * imageBackground.rectTransform.sizeDelta.x / 2,
                                                                            touchPosition.y * imageBackground.rectTransform.sizeDelta.y / 2);
            // 평행 이동
            inputManager.Move(touchPosition);
        }
    }
}
