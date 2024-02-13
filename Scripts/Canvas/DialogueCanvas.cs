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

    // �̱���
    public static DialogueCanvas instance;
    private void Awake() => instance = this;

    void Start()
    {
        npc = FindAnyObjectByType<QuestNPC>();
        tmp = dialogueText.GetComponentInChildren<TextMeshProUGUI>();
        UiOff();
    }
    public void TextChange(string talk) // ��� ���� ����
    {
        tmp.text = talk;
    }
    public void UiOff() // ��ȭâ�� ǥ���ϸ� �ٸ� ���ʿ��� UI�� ��� ��Ȱ��ȭ
    {
        backGround.SetActive(false);
        nameText.SetActive(false);
        dialogueText.SetActive(false);
        hP_Slider.gameObject.SetActive(true);
        hP_Title_TMP.gameObject.SetActive(true);
        hP_Value_TMP.gameObject.SetActive(true);
    }
    public void UiOn() // ��ȭ ����� ���� UI Ȱ��ȭ
    {
        backGround.SetActive(true);
        nameText.SetActive(true);
        dialogueText.SetActive(true);
        hP_Slider.gameObject.SetActive(false);
        hP_Title_TMP.gameObject.SetActive(false);
        hP_Value_TMP.gameObject.SetActive(false);
    }
}
