using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystickRotation : VirtualJoystick
{
    Vector2 pressedPosition = Vector2.zero;

    public override void OnPointerDown(PointerEventData eventData)
    {
        pressedPosition = eventData.pressPosition;
        imageController.rectTransform.position = eventData.pressPosition;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;

        Vector2 dxy = eventData.position - eventData.pressPosition;
        Vector2 normalizedDxy = new Vector2(dxy.x / Screen.width, dxy.y / Screen.height);

        // touchPosition�� ����ȭ [-1 ~ 1]
        normalizedDxy = (normalizedDxy.magnitude > 1) ? normalizedDxy.normalized : normalizedDxy;
        inputManager.Rotate(normalizedDxy);

        // print(normalizedDxy);
        //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imageBackground.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
        //{
        //    // touchPosition �� ����ȭ [-1 ~ 1]
        //    touchPosition.x = (touchPosition.x / imageBackground.rectTransform.sizeDelta.x);
        //    touchPosition.y = (touchPosition.y / imageBackground.rectTransform.sizeDelta.y);

        //    // touchPosition�� ����ȭ [-1 ~ 1]
        //    touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;

        //    // ���� ���̽�ƽ ��Ʈ�ѷ� �̹��� �̵�
        //    imageController.rectTransform.anchoredPosition = new Vector2(touchPosition.x * imageBackground.rectTransform.sizeDelta.x / 2,
        //                                                                    touchPosition.y * imageBackground.rectTransform.sizeDelta.y / 2);

        //    // ȸ��
        //    inputManager.Rotate(touchPosition);
        //}

    }
}
