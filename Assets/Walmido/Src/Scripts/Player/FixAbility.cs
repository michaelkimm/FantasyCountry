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
        // ����Ǵ� �� ���� �� ������ ���� ����
        StopShowAcquiredItemUI();

        // ������ ������ �����ֱ�
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
        // ��ǰ ����
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

            // ���� ���� ������ ���̵� ��Ʈ
            ridePartCntAry[(int)ridePart.Type] += 1;


            // ������ ������ UI �����ֱ�
            ShowAcquiredItemUI(ridePart.Type);

            // �߰��� ���Ϳ� �˷��ֱ�
            OnItemGet.Invoke(ridePartCntAry);

            // ��ǰ �ı�
            Destroy(ridePart.gameObject);
            return;
        }

        // ��ġ��
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
