using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    private GameObject mainCam;
    private Animator camAnimator;
    public GameObject newBtn;
    public GameObject exitBtn;
    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        camAnimator = mainCam.GetComponent<Animator>();

        // 스크립트로 버튼의 OnClick에 이벤트 추가
        newBtn.GetComponent<Button>().onClick.AddListener(OnClickStart);
        exitBtn.GetComponent<Button>().onClick.AddListener(QuitBtn);
    }

    public void OnClickStart()
    {
        camAnimator.SetTrigger("Start");
        SoundManager.Instance.WindHoulingAudioPlay();
        Invoke(nameof(StartAudioPlay), 2.2f);
        Invoke(nameof(ChanageScene), 3.5f);
    }
    private void StartAudioPlay()
    {
        SoundManager.Instance.HolyStartAudioPlay();
    }
    private void ChanageScene() // 카메라 전환 애니메이션은 4.3초
    {
        SceneManager.LoadScene("Main1");
    }
    public void QuitBtn()
    {
        Application.Quit();
    }
}
