using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : ObjectGenerator
{
    [SerializeField]
    GameObject ridePartItem;

    [SerializeField]
    float itemDisableDelay = 3f;

    WaitForSeconds routine = new WaitForSeconds(3f);
    RidePart.RidePartType generateType;

    int[] generatableItem = new int[(int)RidePart.RidePartType.Size];

    bool isGeneratingWithRoutine = false;

    //GameObject GetItemType(RidePart.RidePartType type)
    //{
    //    ridePartItem.GetComponent<RidePart>().Type = type;
    //    return ridePartItem;
    //}

    public void InitializeGeneratableItem(int[] generatableAry)
    {
        for (int i = 0; i < generatableAry.Length; i++)
        {
            generatableItem[i] = generatableAry[i];
        }
    }

    public void GenerateItemWithRoutine()
    {
        isGeneratingWithRoutine = true;
        StartCoroutine("GenerateItemWithCoroutine");
    }

    public void StopGeneratingItemWithRoutine()
    {
        isGeneratingWithRoutine = false;
    }

    protected override void GenerateObject()
    {
        base.GenerateObject();
        GameObject obj = Instantiate(ridePartItem, generatePose, this.transform.rotation, transform.parent);
        RidePart ridePart = obj.GetComponent<RidePart>();

        // �ʱ�ȸ: Ÿ��, �ı����� ����
        ridePart.Initialize(generateType, true, itemDisableDelay);
    }

    public void GenerateObject(float radius, float angle, bool needDestroyDelay = true, float destroyDelay = 3f, RidePart.RidePartType generateType_ = RidePart.RidePartType.Size, float interactableDelay = 0f)
    {
        Vector3 pos = GetRandomPose(this.transform.position, radius, radius, angle, angle);
        GameObject obj;

        obj = Instantiate(ridePartItem, pos, this.transform.rotation, transform.parent);
        RidePart ridePart = obj.GetComponent<RidePart>();
        if (ridePart != null)
        {
            // ��ȣ�ۿ� ����
            ridePart.IsInteractable = false;
            ridePart.SetInteractableDelay(interactableDelay);

            // �ʱ�ȸ: Ÿ��, �ı����� ����
            ridePart.Initialize(generateType_, needDestroyDelay, destroyDelay);
        }
    }

    IEnumerator GenerateItemWithCoroutine()
    {
        while (isGeneratingWithRoutine)
        {
            yield return routine;
            generateType = GetGeneratableItem();
            if (generateType == RidePart.RidePartType.Size)
            {
                isGeneratingWithRoutine = false;
            }
            GenerateObject();
        }
    }

    RidePart.RidePartType GetGeneratableItem()
    {
        RidePart.RidePartType result = RidePart.RidePartType.Size;
        // ��Ʈ�� Ȯ���� ���
        if (generatableItem[(int)RidePart.RidePartType.Nut] > 0)
        {
            // ��ġ 40, ��Ʈ 40, ��Ʈ 20
            float probability = Random.Range(0, 1f);
            if (probability > 0.8f && probability <= 1f)
                result = RidePart.RidePartType.Nut;
            else if (probability <= 0.8f && probability > 0.4f)
                result = RidePart.RidePartType.Bolt;
            else
                result = RidePart.RidePartType.Ratchet;

        }
        else if (generatableItem[(int)RidePart.RidePartType.Bolt] > 0)
        {
            // ��ġ 50, ��Ʈ 50
            int probability = Random.Range(0, 2);
            if (probability == 0)
                result = RidePart.RidePartType.Ratchet;
            else
                result = RidePart.RidePartType.Bolt;
        }
        else if (generatableItem[(int)RidePart.RidePartType.Ratchet] > 0)
        {
            // ��ġ 100
            result = RidePart.RidePartType.Ratchet;
        }

        return result;
    }
}
