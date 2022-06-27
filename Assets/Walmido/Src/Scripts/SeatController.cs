using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SeatController : MonoBehaviour
{
    [SerializeField]
    GameObject SeatOwnObject;

    List<Seat> seatList;

    public UnityEvent OnToldOneSit = new UnityEvent();

    void Awake()
    {
        Seat[] seats = SeatOwnObject.GetComponentsInChildren<Seat>();
        seatList = new List<Seat>(seats);
        foreach (Seat seat in seatList)
        {
            seat.TellSeatFull.AddListener(OnToldSit);
        }

        TurnAllSeatOff();
    }

    void OnToldSit()
    {
        OnToldOneSit.Invoke();
    }

    public void Initialize(int nPCCount)
    {
        TurnAllSeatOff();

        // 스테이지 클리어 조건 불러오기
        // StageConditions stageConditions = StageManager.Instance.GetStageCondition(StageManager.Instance.CurrentStage);

        // 랜덤하게 의자 배치
        EnableRandomSeat(nPCCount);
    }

    public void EnableSeat(int idx)
    {
        seatList[idx].gameObject.SetActive(true);
    }

    void EnableRandomSeat(int targetCnt)
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < seatList.Count; i++)
        {
            indexList.Add(i);
        }

        while (targetCnt-- > 0)
        {
            int index = Random.Range(0, indexList.Count);
            seatList[index].gameObject.SetActive(true);
            indexList.RemoveAt(index);
        }
    }

    void TurnAllSeatOff()
    {
        // 전부 꺼놓기
        foreach (Seat seat in seatList)
        {
            seat.gameObject.SetActive(false);
        }
    }
}
