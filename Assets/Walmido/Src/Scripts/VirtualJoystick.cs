using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    protected InputManager inputManager;

    protected Image imageBackground;
    protected Image imageController;

    protected Vector2 touchPosition;
    public Vector3 TouchPosition { get => touchPosition; }

    virtual protected void Awake()
    {
        if (inputManager == null)
            throw new System.Exception("VirtualJoystick doesnt have inputManager!");

        imageBackground = GetComponent<Image>();
        imageController = transform.GetChild(0).GetComponent<Image>();
    }
    virtual public void OnPointerDown(PointerEventData eventData) { }

    virtual public void OnDrag(PointerEventData eventData) { }
    //virtual public void OnPointerDown(PointerEventData eventData)
    //{
    //    imageBackground.rectTransform.position = eventData.pressPosition;
    //}

    //virtual public void OnDrag(PointerEventData eventData)
    //{
    //    touchPosition = Vector2.zero;
    //    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imageBackground.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
    //    {
    //        // touchPosition �� ����ȭ [-1 ~ 1]
    //        touchPosition.x = (touchPosition.x / imageBackground.rectTransform.sizeDelta.x);
    //        touchPosition.y = (touchPosition.y / imageBackground.rectTransform.sizeDelta.y);

    //        // touchPosition�� ����ȭ [-1 ~ 1]
    //        touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;

    //        // ���� ���̽�ƽ ��Ʈ�ѷ� �̹��� �̵�
    //        imageController.rectTransform.anchoredPosition = new Vector2(touchPosition.x * imageBackground.rectTransform.sizeDelta.x / 2,
    //                                                                        touchPosition.y * imageBackground.rectTransform.sizeDelta.y / 2);

    //        inputManager.Move(touchPosition);
    //    }
    //}

    virtual public void OnPointerUp(PointerEventData eventData)
    {
        // ��ġ ���� �� ���̽�ƽ ��ġ�� �߾����� ����
        imageController.rectTransform.anchoredPosition = Vector2.zero;
        touchPosition = Vector2.zero;
    }
}
