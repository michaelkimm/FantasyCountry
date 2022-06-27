using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidePart : MonoBehaviour
{
    public enum RidePartType
    {
        Ratchet,
        Bolt,
        Nut,
        Size
    }
    [SerializeField]
    RidePartType type = RidePartType.Size;
    public RidePartType Type { get => type; set { type = value; } }

    float disableDelay = 3f;

    BoxCollider collider;

    bool isInteractable = true;
    public bool IsInteractable
    {
        get => isInteractable;
        set
        {
            isInteractable = value;
            if (isInteractable)
            {
                collider.enabled = true;
            }
            else
            {
                collider.enabled = false;
            }
        }
    }

    void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }

    public void Initialize(RidePartType type_, bool needDestroy = true, float destroyDelay = 3f)
    {
        // 타입 & 파괴 시점 설정
        Type = type_;
        if (needDestroy)
        {
            isDestroyReserved = true;
            timePassedforDestroyReserved = 0f;
            this.destroyDelay = destroyDelay;
        }
    }

    public void SetInteractableDelay(float delay)
    {
        isInteractableReserved = true;
        timePassedforInteractableReserved = 0f;
        interactableDelay = delay;
    }

    // 파괴
    bool isDestroyReserved = false;
    float timePassedforDestroyReserved = 0f;
    float destroyDelay = 0f;

    // 상호작용
    bool isInteractableReserved = false;
    float timePassedforInteractableReserved = 0f;
    float interactableDelay = 0f;

    void Update()
    {
        if (!GameManager.Instance.isGameProgramStop && !GameManager.Instance.isGameStopAndStoryOn)
        {
            if (isDestroyReserved)
            {
                timePassedforDestroyReserved += Time.deltaTime;
                if (timePassedforDestroyReserved > destroyDelay)
                {
                    isDestroyReserved = false;
                    this.gameObject.SetActive(false);
                }
            }
            if (isInteractableReserved)
            {
                timePassedforInteractableReserved += Time.deltaTime;
                if (timePassedforInteractableReserved > interactableDelay)
                {
                    isInteractableReserved = false;
                    IsInteractable = true;
                }
            }
        }
    }
}
