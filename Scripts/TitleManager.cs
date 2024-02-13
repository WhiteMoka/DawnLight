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

        // ��ũ��Ʈ�� ��ư�� OnClick�� �̺�Ʈ �߰�
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
    private void ChanageScene() // ī�޶� ��ȯ �ִϸ��̼��� 4.3��
    {
        SceneManager.LoadScene("Main1");
    }
    public void QuitBtn()
    {
        Application.Quit();
    }
}
