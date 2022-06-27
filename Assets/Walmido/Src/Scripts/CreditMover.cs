using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreditMover : MonoBehaviour
{
    public UnityEvent OnCreditEnd = new UnityEvent();

    [SerializeField]
    float speed = 1f;
    bool isInScreen = false;

    [SerializeField]
    Transform bottomElement;

    [SerializeField]
    float bottomDeltaY;

    void Awake()
    {
        if (bottomElement == null)
            throw new System.Exception("CreditMover doesnt have bottomElement");
    }

    void Update()
    {
        if (isInScreen)
            return;

        this.transform.Translate(0, speed * Time.deltaTime, 0);
        if (bottomElement.position.y + bottomDeltaY > Screen.height)
        {
            isInScreen = true;
            OnCreditEnd.Invoke();
        }
    }
}
