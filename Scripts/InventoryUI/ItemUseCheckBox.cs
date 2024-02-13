using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ��ũ��Ʈ�� �κ��丮 ���� Ŭ����, ������ �������� ������� ���������� Ȯ���ϴ� UI â�� ���δ�.
// ������ ���� ���õ� �޼ҵ带 �����Ѵ� ( ex. ���� ���� HP ȸ��, ���� ���� ��ü )
public class ItemUseCheckBox : MonoBehaviour
{
    public InvenSlot slot;          // OnPointerClick���� Ŭ���� ���� ������ InvenSlot ��ũ��Ʈ���� �޾ƿ´�
    public GameObject jointItemR;
    private InteractableChecker interactableChecker;
    private InventoryUI inventoryUI;
    private PlayerHealth playerHealth;

    private ItemInfo prevWeapon;    // private�� ���� ���ѽ� -> null�� �ʱⰪ
    private ItemInfo nowWeapon;     // public���� ���� ���ѽ� -> �ʱⰪ�� ItemInfo ������

    private void OnEnable()
    {
        interactableChecker = FindAnyObjectByType<InteractableChecker>();
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        jointItemR = GameObject.Find("jointItemR");
    }
    public void ItemUseYes() // 'Ȯ��' ��ư�� ������ �޼ҵ�
    {
        switch (slot.itemInfo.itemName)
        {
            case "��Ű": HealHP(); break;
            case "�丶��": HealHP(); break;
            case "�����": HealHP(); break;
            case "���": HealHP(); break;
            case "������ ��": EquipSword(); break;
            case "����": EquipSword(); break;
            default:
                break;
        }

        if (slot.itemInfo.itemTotalSum <= 1) // 1�� ���Ϸ� ������ �ִٸ�
        {
            // Ŭ���� �������� �̸��� ���� ������ ���� ItemInfo��         all Item List �ȿ��� ã�´�
            ItemInfo removeItem = interactableChecker.allItemList.Find(x => x.itemName.ToString() ==  slot.itemInfo.itemName);
            Inventory.instance.RemoveItem(slot.slotNum); // Ŭ���ؼ� ������ ��������� �κ��丮 ������ itemList���� ���������
            interactableChecker.allItemList.Remove(removeItem); // �ش��ϴ� ItemInfo�� all Item List ���� �����ش�
        }
        else if (slot.itemInfo.itemTotalSum > 1) // 2�� �̻� ������ �ִٸ�
        {
            slot.itemInfo.itemTotalSum--;
            slot.itemTotalSum.text = "x " + slot.itemInfo.itemTotalSum.ToString();
        }
        this.gameObject.SetActive(false);
    }

    void EquipSword() // ���� ������ ��ü�� Ŭ���� �������� �Ҹ�ǰ�, ����ִ� ����� �ٽ� �κ��丮�� �־��ش�
    {
        if (nowWeapon == null)
        {
            nowWeapon = slot.itemInfo;
        }
        else
        {
            prevWeapon = nowWeapon.DeepCopy();
            nowWeapon = slot.itemInfo;

            if (prevWeapon != nowWeapon)
            {
                interactableChecker.allItemList.Add(prevWeapon); // �κ��丮�� �߰�
            }
        }
        GameManager.Instance.isSwordEquip = true;

        SwordStatus[] weapons = jointItemR.GetComponentsInChildren<SwordStatus>();
        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }
        jointItemR.transform.Find(slot.itemInfo.itemName).gameObject.SetActive(true);   // ���Կ��� Ŭ���� �̸��� ���� ���� Ȱ��ȭ
        SoundManager.Instance.SwordChangeAudioPlay();
        inventoryUI.InvenOnOff();
    }

    void HealHP()
    {
        float plusHP = 0;

        switch (slot.itemInfo.itemName)
        {
            case "��Ű": plusHP = 10; break;
            case "�丶��": plusHP = 5; break;
            case "�����": plusHP = 15; break;
            case "���": plusHP = 30; break;
        }
        SoundManager.Instance.EatSoundAudioPlay();
        inventoryUI.InvenOnOff();
        playerHealth.RestoreHealth(plusHP);
    }
}
