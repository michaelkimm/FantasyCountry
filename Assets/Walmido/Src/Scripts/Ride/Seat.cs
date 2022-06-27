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
            // ¿ÜÇü ²ô±â
            TurnOffExternalShape();
            
            // ¾ÉÀ½ ¼Ò¸® Àç»ý
            soundPlayer.PlaySound("SE_FL_savenpc");

            // °ÔÀÓ ¿ÀºêÁ§Æ® ²ô±â
            Invoke("DeactivateDelay", 1f);

            // Âø¼®µÊ ÀÌÆåÆ®
            Instantiate(seatEffectPrefab, transform.position, transform.rotation);
        }
    }

    void DeactivateDelay() => this.gameObject.SetActive(false);
}
