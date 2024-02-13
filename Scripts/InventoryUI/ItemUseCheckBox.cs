using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트는 인벤토리 슬롯 클릭시, 정말로 아이템을 사용할지 최종적으로 확인하는 UI 창에 붙인다.
// 아이템 사용과 관련된 메소드를 포함한다 ( ex. 음식 사용시 HP 회복, 무기 사용시 교체 )
public class ItemUseCheckBox : MonoBehaviour
{
    public InvenSlot slot;          // OnPointerClick에서 클릭한 슬롯 정보를 InvenSlot 스크립트에서 받아온다
    public GameObject jointItemR;
    private InteractableChecker interactableChecker;
    private InventoryUI inventoryUI;
    private PlayerHealth playerHealth;

    private ItemInfo prevWeapon;    // private로 접근 제한시 -> null이 초기값
    private ItemInfo nowWeapon;     // public으로 접근 제한시 -> 초기값이 ItemInfo 껍데기

    private void OnEnable()
    {
        interactableChecker = FindAnyObjectByType<InteractableChecker>();
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        jointItemR = GameObject.Find("jointItemR");
    }
    public void ItemUseYes() // '확인' 버튼에 연결할 메소드
    {
        switch (slot.itemInfo.itemName)
        {
            case "쿠키": HealHP(); break;
            case "토마토": HealHP(); break;
            case "물고기": HealHP(); break;
            case "고기": HealHP(); break;
            case "오래된 검": EquipSword(); break;
            case "마검": EquipSword(); break;
            default:
                break;
        }

        if (slot.itemInfo.itemTotalSum <= 1) // 1개 이하로 가지고 있다면
        {
            // 클릭한 아이템의 이름과 같은 정보를 가진 ItemInfo를         all Item List 안에서 찾는다
            ItemInfo removeItem = interactableChecker.allItemList.Find(x => x.itemName.ToString() ==  slot.itemInfo.itemName);
            Inventory.instance.RemoveItem(slot.slotNum); // 클릭해서 아이템 사용했으면 인벤토리 슬롯인 itemList에서 지워줘야지
            interactableChecker.allItemList.Remove(removeItem); // 해당하는 ItemInfo를 all Item List 에서 지워준다
        }
        else if (slot.itemInfo.itemTotalSum > 1) // 2개 이상 가지고 있다면
        {
            slot.itemInfo.itemTotalSum--;
            slot.itemTotalSum.text = "x " + slot.itemInfo.itemTotalSum.ToString();
        }
        this.gameObject.SetActive(false);
    }

    void EquipSword() // 무기 아이템 교체시 클릭한 아이템은 소모되고, 들고있던 무기는 다시 인벤토리로 넣어준다
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
                interactableChecker.allItemList.Add(prevWeapon); // 인벤토리에 추가
            }
        }
        GameManager.Instance.isSwordEquip = true;

        SwordStatus[] weapons = jointItemR.GetComponentsInChildren<SwordStatus>();
        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }
        jointItemR.transform.Find(slot.itemInfo.itemName).gameObject.SetActive(true);   // 슬롯에서 클릭한 이름과 같은 무기 활성화
        SoundManager.Instance.SwordChangeAudioPlay();
        inventoryUI.InvenOnOff();
    }

    void HealHP()
    {
        float plusHP = 0;

        switch (slot.itemInfo.itemName)
        {
            case "쿠키": plusHP = 10; break;
            case "토마토": plusHP = 5; break;
            case "물고기": plusHP = 15; break;
            case "고기": plusHP = 30; break;
        }
        SoundManager.Instance.EatSoundAudioPlay();
        inventoryUI.InvenOnOff();
        playerHealth.RestoreHealth(plusHP);
    }
}
