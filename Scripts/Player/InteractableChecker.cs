using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 이 스크립트는 플레이어의 자식 오브젝트인, 상호작용 가능한 물건을 검사하는 오브젝트에 붙인다.
public class InteractableChecker : MonoBehaviour
{
    public GameObject interactCanvas;
    public GameObject content;                      // 슬롯들의 부모인 content
    [SerializeField] GameObject interactSlot;       // 상호작용 슬롯

    public List<ItemInfo> infoSlotList = new List<ItemInfo>();          // 아이템 정보 리스트 -> 플레이어 인벤토리 스크립트에 전달
    public List<GameObject> itemObjList = new List<GameObject>();       // 아이템 오브젝트 리스트 -> 획득시 필드 위 아이템 오브젝트 파괴
    public List<ItemInfo> allItemList = new List<ItemInfo>();           // 아이템 타입에 따라 분류하기 전에, 모든 아이템을 보관하는 리스트

    // 상호작용 시간 간격 설정
    private float inputTimer = 0f;
    private float interval = 0.2f; // 출력 간격 설정

    private void Start()
    {
        interactCanvas.gameObject.SetActive(false);
        inputTimer = 0.5f; // 처음에는 바로 F키 누를 수 있도록
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            interactCanvas.gameObject.SetActive(true);          // 캔버스 띄우고
            other.GetComponent<IInteractable>().CanvasOn(content, interactSlot); // 슬롯 생성하고, 아이템 정보를 받아온다
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null) // 상호작용 가능한 오브젝트가 있고
        {
            inputTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.F) && inputTimer >= interval) // F키를 눌렀는데 일정 시간 간격(0.2초 이상)이 지났다면
            {
                inputTimer = 0f;

                // 아이템
                if (infoSlotList.Count > 0 && itemObjList.Count > 0) // 리스트에 담겨있는 슬롯과 아이템 오브젝트가 하나 이상 있다면
                {
                    Destroy(content.transform.GetChild(0).gameObject);  // Content의 첫번째 자식인 슬롯 오브젝트를 파괴
                    PickUpItem();
                    SoundManager.Instance.ItemPickUpAudioPlay();
                }
                // 보물상자
                if (other.GetComponent<TreasureBox>() && !other.GetComponent<TreasureBox>().isItemDrop) // 아이템을 드롭하지 않은 보물상자가 있다면
                {
                    Destroy(content.transform.GetChild(0).gameObject); // 보물상자 텍스트를 가진 첫번째 슬롯 파괴
                    other.GetComponent<TreasureBox>().DropItem();
                    SoundManager.Instance.BoxOpenAudioPlay();
                }
                // NPC
                if (other.GetComponent<QuestNPC>())
                {
                    other.GetComponent<QuestNPC>().TalkStart();
                    SoundManager.Instance.TalkSoundAudioPlay();
                    interactCanvas.SetActive(false);
                }
            }
        }
    }
    void PickUpItem()
    {
        ItemInfo existingItem =  allItemList.Find(x => x.itemName == infoSlotList[0].itemName); // 중복 아이템 있는지 검사
        if (existingItem != null) // 만약 중복 아이템이 존재한다면
        {
            existingItem.itemTotalSum++; // 갯수를 + 1
        }
        else // 처음 획득하는 아이템이라면
        {
            allItemList.Add(infoSlotList[0]);
        }
        infoSlotList.RemoveAt(0);
        Destroy(itemObjList[0]);    // itemObjList의 첫번째 슬롯에 해당하는 필드 위 게임 오브젝트를 파괴
        itemObjList.RemoveAt(0);    // itemObjList의 첫번째 슬롯이 Missing 이므로 지워준다

        if (infoSlotList.Count == 0 && itemObjList.Count == 0)
        {
            interactCanvas.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other) // 트리거에서 빠져나가는 순간 Canvas에서 표시 없앰
    {       
        if (other.GetComponent<IInteractable>() != null)
        {
            if (other.GetComponent<Item>()) // 아이템 안먹었으면
            {
                other.GetComponent<Item>().isCanvasOn = false;
                infoSlotList.Clear();
                itemObjList.Clear();
            }
            if (other.GetComponent<TreasureBox>() && !other.GetComponent<TreasureBox>().isItemDrop) // 보물상자 아직 사용 안했다면 
            {
                other.GetComponent<TreasureBox>().isCanvasOn = false;
            }
            if (other.GetComponent<QuestNPC>()) // NPC와 대화 초기화
            {
                other.GetComponent<QuestNPC>().isCanvasOn = false;
                other.GetComponent<QuestNPC>().talkIndex = 0;
            }

            // 공통
            interactCanvas.SetActive(false);
            foreach (Transform child in content.transform)  // 캔버스의 자식 오브젝트에 남아있는 대상 이름 슬롯을 전부 파괴
            {
                Destroy(child.gameObject, 0.1f);
            }
        }
    }
}
