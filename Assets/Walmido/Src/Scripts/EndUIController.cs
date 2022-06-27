using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;
using UnityEngine.SceneManagement;

public class EndUIController : MonoBehaviour
{
    [SerializeField]
    UITransitionEffect backgroundImageEffect;

    [SerializeField]
    UITransitionEffect backgroundBlurImageEffect;

    [SerializeField]
    ParticleSystem developerCreditEffect;

    [SerializeField]
    GameObject developerCreditContent;

    [SerializeField]
    UITransitionEffect letterCover;
    bool isFading = false;

    [SerializeField]
    UITransitionEffect letter;

    [SerializeField]
    UITransitionEffect title;

    [SerializeField]
    GameObject sentencesSet;
    List<UITransitionEffect> sentenceList;
    bool isReadingLetter = false;
    WaitForSeconds readDelay = new WaitForSeconds(1.5f);

    [SerializeField]
    GameObject creditContent;

    [SerializeField]
    GameObject creditNextBtn;

    [SerializeField]
    CreditMover creditMover;

    //[SerializeField]
    //BGMSoundPlayer bGMSoundPlayer;
    [SerializeField]
    //string bgmName = "Warm-Memories-Emotional-Inspiring-Piano";

    void Awake()
    {
        if (backgroundImageEffect == null)
            throw new System.Exception("EndUIController doesnt have backgroundImageEffect");

        if (backgroundBlurImageEffect == null)
            throw new System.Exception("EndUIController doesnt have backgroundBlurImageEffect");

        if (developerCreditEffect == null)
            throw new System.Exception("EndUIController doesnt have developerCreditEffect");

        if (developerCreditContent == null)
            throw new System.Exception("EndUIController doesnt have developerCreditContent");

        if (letterCover == null)
            throw new System.Exception("EndUIController doesnt have letterCover");

        if (letter == null)
            throw new System.Exception("EndUIController doesnt have letter");

        if (title == null)
            throw new System.Exception("EndUIController doesnt have title");

        if (sentencesSet == null)
            throw new System.Exception("EndUIController doesnt have sentencesSet");

        if (creditContent == null)
            throw new System.Exception("EndUIController doesnt have creditContent");

        if (creditNextBtn == null)
            throw new System.Exception("EndUIController doesnt have creditNextBtn");

        //if (bGMSoundPlayer == null)
        //    throw new System.Exception("EndUIController doesnt have bGMSoundPlayer");

        letter.gameObject.SetActive(false);

        InitializeLetter();
        InitializeCredit();
    }

    void Start()
    {
        creditMover.OnCreditEnd.AddListener(OnFrontCreditEnd);
        PlayBGM();
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    public void ShowLetterCover()
    {
        letterCover.gameObject.SetActive(true);
    }

    void PlayBGM()
    {
        SoundManager.Instance.BgmSoundPlayer.MakeLoop(true);
        SoundManager.Instance.BgmSoundPlayer.PlaySound(SoundManager.Instance.GetSceneBGMName(SceneManager.GetActiveScene().name));
    }

    public void FadeLetterCover()
    {
        if (isFading)
            return;
        isFading = true;
        letterCover.Hide();

        Invoke("DeactivateLetterCoverDelay", letterCover.effectPlayer.duration);
        Invoke("OpenLetter", letterCover.effectPlayer.duration);
    }

    void DeactivateLetterCoverDelay() => letterCover.gameObject.SetActive(false);

    void InitializeLetter()
    {
        UITransitionEffect[] tmp = sentencesSet.GetComponentsInChildren<UITransitionEffect>();
        sentenceList = new List<UITransitionEffect>(tmp);

        creditNextBtn.SetActive(false);
    }

    public void OpenLetter()
    {
        // 편지 활성화
        letter.gameObject.SetActive(true);
        letter.Show();
        title.Show();

        // 문장 하나씩 보여주기
        isReadingLetter = true;
        Invoke("StartReadLetterDelay", letter.effectPlayer.duration + 0.5f);
    }

    void StartReadLetterDelay() => StartCoroutine("ReadLetter");

    IEnumerator ReadLetter()
    {
        int idx = 0;
        while (isReadingLetter)
        {
            if (idx < sentenceList.Count)
            {
                sentenceList[idx++].Show();
            }
            else
            {
                // 크레딧으로 갈 버튼 활성화
                creditNextBtn.SetActive(true);

                isReadingLetter = false;
                break;
            }
            yield return readDelay;
        }
    }

    public void StopReadLetter()
    {
        isReadingLetter = false;
        for (int i = 0; i < sentenceList.Count; i++)
        {
            sentenceList[i].Show();
        }

        // 편지와의 상호작용 멈춤
        letter.gameObject.GetComponent<Button>().interactable = false;

        // 크레딧으로 갈 버튼 활성화
        creditNextBtn.SetActive(true);
    }

    public void FadeLetterReadCredit()
    {
        float maxDuration = 0;

        letter.Hide();
        if (letter.effectPlayer.duration > maxDuration) maxDuration = letter.effectPlayer.duration;

        title.Hide();
        if (title.effectPlayer.duration > maxDuration) maxDuration = title.effectPlayer.duration;

        for (int i = 0; i < sentenceList.Count; i++)
        {
            sentenceList[i].Hide();
            if (sentenceList[i].effectPlayer.duration > maxDuration) maxDuration = sentenceList[i].effectPlayer.duration;
        }

        // 편지 비활성화
        Invoke("DeactivateLetterDelay", maxDuration);

        // 크레딧으로 갈 버튼 비활성화
        creditNextBtn.SetActive(false);

        Invoke("ReadCredit", maxDuration);
    }

    void DeactivateLetterDelay() => letter.gameObject.SetActive(false);

    void InitializeCredit()
    {
        creditContent.SetActive(false);
    }

    void ReadCredit()
    {
        creditContent.SetActive(true);
        creditContent.GetComponentInChildren<CreditContent>()?.UpdateCreditContent();
    }

    public void ReadyForDeveloperCredit()
    {
        backgroundImageEffect.Hide();
        backgroundBlurImageEffect.Hide();

        Invoke("ActivateDeveloperCredit", 2f);
    }

    void ActivateDeveloperCredit()
    {
        developerCreditEffect.gameObject.SetActive(true);
        developerCreditEffect.Play();
        developerCreditContent.SetActive(true);
    }

    void OnFrontCreditEnd()
    {
        ReadyForDeveloperCredit();
    }

    public void ChangeScene(string sceneName)
    {
        GameManager.Instance.ChangeScene(sceneName, true);
    }
}
