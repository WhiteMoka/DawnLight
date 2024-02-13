using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 이 스크립트는 대화 상호작용이 가능한 NPC 오브젝트에 붙인다.
public class QuestNPC : MonoBehaviour, IInteractable
{
    public string npcName;
    public bool isCanvasOn = false;
    public bool isTalking = false;
    public GameObject playerMoveManager;
    public string[] talks;
    public int talkIndex = 0;
    WaitForSeconds ws = new WaitForSeconds(0.25f);  // 0.25초의 대기시간 ( 1초에 4번 )
    WaitForSeconds ws1 = new WaitForSeconds(1f);    // 1초의 대기시간

    public void CanvasOn(GameObject slotHolder, GameObject interactSlot) // 상호작용 대상 정보를 캔버스에 표시
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
