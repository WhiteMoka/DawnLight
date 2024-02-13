using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// �� ��ũ��Ʈ�� ��ȭ ��ȣ�ۿ��� ������ NPC ������Ʈ�� ���δ�.
public class QuestNPC : MonoBehaviour, IInteractable
{
    public string npcName;
    public bool isCanvasOn = false;
    public bool isTalking = false;
    public GameObject playerMoveManager;
    public string[] talks;
    public int talkIndex = 0;
    WaitForSeconds ws = new WaitForSeconds(0.25f);  // 0.25���� ���ð� ( 1�ʿ� 4�� )
    WaitForSeconds ws1 = new WaitForSeconds(1f);    // 1���� ���ð�

    public void CanvasOn(GameObject slotHolder, GameObject interactSlot) // ��ȣ�ۿ� ��� ������ ĵ������ ǥ��
    {
        if (!isCanvasOn)
        {
            isCanvasOn = true;
            GameObject slot = Instantiate(interactSlot, this.transform.position, Quaternion.identity);
            TextMeshProUGUI nameText = slot.GetComponentInChildren<TextMeshProUGUI>();
            nameText.text = npcName;
            slot.transform.SetParent(slotHolder.transform);
        }
    }

    public void TalkStart()
    {
        if (talkIndex < talks.Length) // 0 < 2
        {
            playerMoveManager.SetActive(false);
            DialogueCanvas.instance.UiOn();
            DialogueCanvas.instance.TextChange(talks[talkIndex]);
            talkIndex++;
        }
        else
        {
            playerMoveManager.SetActive(true);
            DialogueCanvas.instance.UiOff();
        }
    }
}
