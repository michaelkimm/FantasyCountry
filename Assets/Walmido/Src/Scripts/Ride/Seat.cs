using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Seat : MonoBehaviour
{
    public UnityEvent TellSeatFull = new UnityEvent();

    [SerializeField]
    GameObject seatEffectPrefab;

    [SerializeField]
    GameObject seatShape;

    SoundPlayer soundPlayer;

    bool isSat = false;
    public bool IsSat
    {
        get => isSat;
        set
        {
            isSat = value;

            // Tell StageMC Seat full
            TellSeatFull.Invoke();
        }
    }
    void Awake()
    {
        if (seatEffectPrefab == null)
            throw new System.Exception("Seat doesnt have seatEffect");

        if (seatShape == null)
            throw new System.Exception("Seat doesnt have seatShape");

        soundPlayer = GetComponent<SoundPlayer>();
    }

    void TurnOffExternalShape()
    {
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        seatShape.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.GetComponent<NPC>()?.Sit();
            IsSat = true;
            // ���� ����
            TurnOffExternalShape();
            
            // ���� �Ҹ� ���
            soundPlayer.PlaySound("SE_FL_savenpc");

            // ���� ������Ʈ ����
            Invoke("DeactivateDelay", 1f);

            // ������ ����Ʈ
            Instantiate(seatEffectPrefab, transform.position, transform.rotation);
        }
    }

    void DeactivateDelay() => this.gameObject.SetActive(false);
}
