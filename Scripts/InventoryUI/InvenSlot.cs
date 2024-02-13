using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class InvenSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemInfo itemInfo;   // 아이템 정보
    public Image gradeColorImage;
    public Image itemImage;
    public Image gradeStarImage;
    public TextMeshProUGUI itemTotalSum;
    public int slotNum;         // 슬롯이 등록될 때 외부에서 자동으로 붙여줄 슬롯 번호

    // 아이템 등급에 따른 별 이미지
    public Image star5Grade;
    public Image star4Grade;
    public Image star3Grade;
    public Image star2Grade;
    public Image star1Grade;
    private InventoryUI inventoryUI;

    private void Start()
    {
        gradeColorImage = this.gameObject.transform.GetChild(0).GetComponent<Image>();
        itemImage = this.gameObject.transform.GetChild(1).GetComponent<Image>();
        gradeStarImage = this.gameObject.transform.GetChild(2).GetComponent<Image>();
        itemTotalSum = this.gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        RemoveSlot();
        inventoryUI = GameObject.FindAnyObjectByType<InventoryUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemInfo == null) return; // 슬롯 안에 아이템이 없으면 return

        GameObject itemUseCheckBox = inventoryUI.itemUseCheckBox;
        itemUseCheckBox.SetActive(true);
        itemUseCheckBox.GetComponent<ItemUseCheckBox>().slot = this;
    }

    internal void RemoveSlot() // 슬롯에 변화가 일어나면 그 칸 지우고
    {
        itemInfo = null;
        gradeColorImage.gameObject.SetActive(false);
        itemImage.gameObject.SetActive(false);
        gradeStarImage.gameObject.SetActive(false);
        itemTotalSum.gameObject.SetActive(false);       
    }

    internal void UpdateSlotUI() // 모든 칸 다시 그려준다
    {
        gradeColorImage.sprite = itemInfo.gradeImage;
        itemImage.sprite = itemInfo.itemImage;
        itemTotalSum.text = "x " + itemInfo.itemTotalSum.ToString();

        switch (itemInfo.gradeColor) // 아이템 등급에 따른 별 갯수 이미지
        {
            case ItemInfo.GradeColor.ColorLegend: gradeStarImage.sprite = star5Grade.sprite; break;
            case ItemInfo.GradeColor.ColorEpic: gradeStarImage.sprite = star4Grade.sprite; break;
            case ItemInfo.GradeColor.ColorRare: gradeStarImage.sprite = star2Grade.sprite; break;
            case ItemInfo.GradeColor.ColorNormal: gradeStarImage.sprite = star1Grade.sprite; break;
        }
        gradeColorImage.gameObject.SetActive(true);
        itemImage.gameObject.SetActive(true);
        gradeStarImage.gameObject.SetActive(true);
        itemTotalSum.gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInfo == null) return; // 슬롯 안에 아이템이 없으면 return
        SoundManager.Instance.BtnHoverAudioPlay();
        inventoryUI.explainPanel.gameObject.SetActive(true);
        inventoryUI.itemImage.sprite = itemInfo.itemImage;
        inventoryUI.starImage.sprite = gradeStarImage.sprite;
        inventoryUI.nameText.text = itemInfo.itemName.ToString();
        inventoryUI.explainText.text = itemInfo.explanation;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemInfo == null) return; // 슬롯 안에 아이템이 없으면 return
        inventoryUI.explainPanel.gameObject.SetActive(false);
    }
}
