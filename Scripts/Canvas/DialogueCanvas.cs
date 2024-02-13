using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueCanvas : MonoBehaviour
{
    public Slider hP_Slider;
    public TextMeshProUGUI hP_Title_TMP;
    public TextMeshProUGUI hP_Value_TMP;

    public GameObject backGround;
    public GameObject nameText;
    public GameObject dialogueText;
    private TextMeshProUGUI tmp;

    private QuestNPC npc;

    // 싱글턴
    public static DialogueCanvas instance;
    private void Awake() => instance = this;

    void Start()
    {
        npc = FindAnyObjectByType<QuestNPC>();
        tmp = dialogueText.GetComponentInChildren<TextMeshProUGUI>();
        UiOff();
    }
    public void TextChange(string talk) // 대사 내용 변경
    {
        tmp.text = talk;
    }
    public void UiOff() // 대화창을 표시하면 다른 불필요한 UI를 잠시 비활성화
    {
        backGround.SetActive(false);
        nameText.SetActive(false);
        dialogueText.SetActive(false);
        hP_Slider.gameObject.SetActive(true);
        hP_Title_TMP.gameObject.SetActive(true);
        hP_Value_TMP.gameObject.SetActive(true);
    }
    public void UiOn() // 대화 종료시 기존 UI 활성화
    {
        backGround.SetActive(true);
        nameText.SetActive(true);
        dialogueText.SetActive(true);
        hP_Slider.gameObject.SetActive(false);
        hP_Title_TMP.gameObject.SetActive(false);
        hP_Value_TMP.gameObject.SetActive(false);
    }
}
