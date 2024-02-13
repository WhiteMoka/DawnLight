using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �� ��ũ��Ʈ�� �÷��̾��� �ڽ� ������Ʈ��, ��ȣ�ۿ� ������ ������ �˻��ϴ� ������Ʈ�� ���δ�.
public class InteractableChecker : MonoBehaviour
{
    public GameObject interactCanvas;
    public GameObject content;                      // ���Ե��� �θ��� content
    [SerializeField] GameObject interactSlot;       // ��ȣ�ۿ� ����

    public List<ItemInfo> infoSlotList = new List<ItemInfo>();          // ������ ���� ����Ʈ -> �÷��̾� �κ��丮 ��ũ��Ʈ�� ����
    public List<GameObject> itemObjList = new List<GameObject>();       // ������ ������Ʈ ����Ʈ -> ȹ��� �ʵ� �� ������ ������Ʈ �ı�
    public List<ItemInfo> allItemList = new List<ItemInfo>();           // ������ Ÿ�Կ� ���� �з��ϱ� ����, ��� �������� �����ϴ� ����Ʈ

    // ��ȣ�ۿ� �ð� ���� ����
    private float inputTimer = 0f;
    private float interval = 0.2f; // ��� ���� ����

    private void Start()
    {
        interactCanvas.gameObject.SetActive(false);
        inputTimer = 0.5f; // ó������ �ٷ� FŰ ���� �� �ֵ���
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            interactCanvas.gameObject.SetActive(true);          // ĵ���� ����
            other.GetComponent<IInteractable>().CanvasOn(content, interactSlot); // ���� �����ϰ�, ������ ������ �޾ƿ´�
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null) // ��ȣ�ۿ� ������ ������Ʈ�� �ְ�
        {
            inputTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.F) && inputTimer >= interval) // FŰ�� �����µ� ���� �ð� ����(0.2�� �̻�)�� �����ٸ�
            {
                inputTimer = 0f;

                // ������
                if (infoSlotList.Count > 0 && itemObjList.Count > 0) // ����Ʈ�� ����ִ� ���԰� ������ ������Ʈ�� �ϳ� �̻� �ִٸ�
                {
                    Destroy(content.transform.GetChild(0).gameObject);  // Content�� ù��° �ڽ��� ���� ������Ʈ�� �ı�
                    PickUpItem();
                    SoundManager.Instance.ItemPickUpAudioPlay();
                }
                // ��������
                if (other.GetComponent<TreasureBox>() && !other.GetComponent<TreasureBox>().isItemDrop) // �������� ������� ���� �������ڰ� �ִٸ�
                {
                    Destroy(content.transform.GetChild(0).gameObject); // �������� �ؽ�Ʈ�� ���� ù��° ���� �ı�
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
        ItemInfo existingItem =  allItemList.Find(x => x.itemName == infoSlotList[0].itemName); // �ߺ� ������ �ִ��� �˻�
        if (existingItem != null) // ���� �ߺ� �������� �����Ѵٸ�
        {
            existingItem.itemTotalSum++; // ������ + 1
        }
        else // ó�� ȹ���ϴ� �������̶��
        {
            allItemList.Add(infoSlotList[0]);
        }
        infoSlotList.RemoveAt(0);
        Destroy(itemObjList[0]);    // itemObjList�� ù��° ���Կ� �ش��ϴ� �ʵ� �� ���� ������Ʈ�� �ı�
        itemObjList.RemoveAt(0);    // itemObjList�� ù��° ������ Missing �̹Ƿ� �����ش�

        if (infoSlotList.Count == 0 && itemObjList.Count == 0)
        {
            interactCanvas.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other) // Ʈ���ſ��� ���������� ���� Canvas���� ǥ�� ����
    {       
        if (other.GetComponent<IInteractable>() != null)
        {
            if (other.GetComponent<Item>()) // ������ �ȸԾ�����
            {
                other.GetComponent<Item>().isCanvasOn = false;
                infoSlotList.Clear();
                itemObjList.Clear();
            }
            if (other.GetComponent<TreasureBox>() && !other.GetComponent<TreasureBox>().isItemDrop) // �������� ���� ��� ���ߴٸ� 
            {
                other.GetComponent<TreasureBox>().isCanvasOn = false;
            }
            if (other.GetComponent<QuestNPC>()) // NPC�� ��ȭ �ʱ�ȭ
            {
                other.GetComponent<QuestNPC>().isCanvasOn = false;
                other.GetComponent<QuestNPC>().talkIndex = 0;
            }

            // ����
            interactCanvas.SetActive(false);
            foreach (Transform child in content.transform)  // ĵ������ �ڽ� ������Ʈ�� �����ִ� ��� �̸� ������ ���� �ı�
            {
                Destroy(child.gameObject, 0.1f);
            }
        }
    }
}
