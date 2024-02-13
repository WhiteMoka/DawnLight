using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ItemInfo                   // 클래스 자료형은 '참조'형식이다. 참조형식을 복제하는 DeepCopy 메소드를 만들어주자!
    public enum ItemType { Equipment, Consumables, Etc }
    public ItemType itemType;           // 아이템 유형
    public enum GradeColor { ColorLegend, ColorEpic, ColorRare, ColorNormal }
    public GradeColor gradeColor;       // 등급 색깔
    public string itemName;             // 아이템 이름
    public int itemTotalSum = 1;        // 아이템 획득한 총 갯수
    public Sprite itemImage;            // (코드로 호출) 아이템 이미지
    public Sprite gradeImage;           // (코드로 호출) 등급 이미지
    public string explanation;          // 아이템 설명

    public ItemInfo DeepCopy()          // 힙 영역에서 복사해서 그 값을 돌려주는 DeepCopy 메소드 
    {
        ItemInfo temp = new ItemInfo();
        temp.itemType = itemType;
        temp.gradeColor = gradeColor;
        temp.itemName = itemName;
        temp.itemTotalSum = itemTotalSum;
        temp.itemImage = itemImage;
        temp.gradeImage = gradeImage;
        temp.explanation = explanation;
        return temp;
    }
}

public class Item : MonoBehaviour, IInteractable
{
    public ItemInfo itemInfo;
    private InteractableChecker interactCheckBox;

    public bool isCanvasOn = false;                // 상호작용 캔버스에 활성화 되었나요? No

    private void Start()
    {
        itemInfo.itemImage = Resources.Load<Sprite>("ItemResources/" + itemInfo.itemName);
        itemInfo.gradeImage = Resources.Load<Sprite>(itemInfo.gradeColor.ToString());
        interactCheckBox = GameObject.FindAnyObjectByType<InteractableChecker>();
    }

    public void CanvasOn(GameObject slotHolder, GameObject interactSlot) // ( 부모 오브젝트, 생성할 슬롯 )
    {
        if (!isCanvasOn)
        {
            isCanvasOn = true;
            GameObject slot = Instantiate(interactSlot, this.transform.position, Quaternion.identity);
            TextMeshProUGUI nameText = slot.GetComponentInChildren<TextMeshProUGUI>();
            nameText.text = itemInfo.itemName;
            slot.transform.SetParent(slotHolder.transform);
            interactCheckBox.infoSlotList.Add(itemInfo);            // 아이템 정보를 리스트에 넘겨줌
            interactCheckBox.itemObjList.Add(this.gameObject);      // 아이템 오브젝트 정보를 넘겨줌
        }
    }
}
