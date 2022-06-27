using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FixAbility : MonoBehaviour
{
    [SerializeField]
    List<Image> acquiredItemImages = new List<Image>();

    [SerializeField]
    ParticleSystem acquiredEffect;

    [SerializeField]
    Slider fixedAmountUI;

    SoundPlayer soundPlayer;

    [SerializeField]
    int[] ridePartCntAry = new int[(int)RidePart.RidePartType.Size];

    [SerializeField]
    int[] missingParts = new int[(int)RidePart.RidePartType.Size];

    FixablePart fixablePart;
    bool inFixableArea = false;
    bool isFixing = false;

    public UnityEvent<int[]> OnItemGet = new UnityEvent<int[]>();

    void Awake()
    {
        if (acquiredItemImages.Count != (int)RidePart.RidePartType.Size)
            throw new System.Exception("FixAbility doesn't have ranchetAcquiredUI");

        if (acquiredEffect == null)
            throw new System.Exception("FixAbility doesn't have acquiredEffect");

        if (fixedAmountUI == null)
            throw new System.Exception("FixAbility doesn't have fixedAmountUI");

        soundPlayer = GetComponent<SoundPlayer>();
    }

    void Update()
    {
        if (isFixing)
            fixablePart?.Fixed(Time.deltaTime);
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    public void Fix()
    {
        isFixing = true;
    }

    public void StopFixing()
    {
        isFixing = false;
        fixablePart?.BeingFixedStopped();
    }

    bool CheckFixableCondition(FixablePart part)
    {
        return part.ReturnIfFixable(ridePartCntAry);
    }

    bool CheckCurrentNeededPart(RidePart.RidePartType type)
    {
        return missingParts[(int)type] > ridePartCntAry[(int)type];
    }

    void ShowAcquiredItemUI(RidePart.RidePartType type)
    {
        // 진행되는 것 있을 수 있으니 전부 끄기
        StopShowAcquiredItemUI();

        // 습득한 아이템 보여주기
        acquiredItemImages[(int)type].gameObject.SetActive(true);

        acquiredEffect.Play();

        Invoke("StopShowAcquiredItemUI", 0.75f);
    }

    void StopShowAcquiredItemUI()
    {
        for (int i = 0; i < acquiredItemImages.Count; i++)
        {
            acquiredItemImages[i].gameObject.SetActive(false);
        }
    }

    public void LetKnowMissingPart(int[] missingParts_)
    {
        for (int i = 0; i < missingParts.Length; i++)
        {
            missingParts[i] = missingParts_[i];
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 부품 습득
        RidePart ridePart = other.GetComponent<RidePart>();
        if (ridePart != null)
        {

            soundPlayer.SetForcePlay(true);
            if (CheckCurrentNeededPart(ridePart.Type))
            {
                soundPlayer.PlaySound("SE_FL_gooditem");
            }
            else
            {
                soundPlayer.PlaySound("SE_FL_baditem");
            }

            // 현재 내가 습득한 라이드 파트
            ridePartCntAry[(int)ridePart.Type] += 1;


            // 습득한 아이템 UI 보여주기
            ShowAcquiredItemUI(ridePart.Type);

            // 중개자 센터에 알려주기
            OnItemGet.Invoke(ridePartCntAry);

            // 부품 파괴
            Destroy(ridePart.gameObject);
            return;
        }

        // 고치기
        fixablePart = other.GetComponent<FixablePart>();
        if (fixablePart == null)
            return;

        if (fixablePart.State != FixablePart.FixState.Broken)
            return;

        inFixableArea = true;
        fixedAmountUI.gameObject.SetActive(true);
    }

    void OnTriggerStay(Collider other)
    {
        if (fixablePart == null)
            return;

        if (fixablePart.State != FixablePart.FixState.Broken)
            return;

        if (!CheckFixableCondition(fixablePart))
            return;

        fixedAmountUI.value = fixablePart.FixedAmountTime / fixablePart.FixedDelay;
    }

    void OnTriggerExit(Collider other)
    {
        if (fixablePart == null)
            return;

        fixedAmountUI.gameObject.SetActive(false);

        inFixableArea = false;
        if (fixablePart.State != FixablePart.FixState.Broken)
            return;
        StopFixing();
        fixablePart = null;
    }
}
