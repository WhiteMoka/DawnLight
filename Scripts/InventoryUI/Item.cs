using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ItemInfo                   // Ŭ���� �ڷ����� '����'�����̴�. ���������� �����ϴ� DeepCopy �޼ҵ带 ���������!
    public enum ItemType { Equipment, Consumables, Etc }
    public ItemType itemType;           // ������ ����
    public enum GradeColor { ColorLegend, ColorEpic, ColorRare, ColorNormal }
    public GradeColor gradeColor;       // ��� ����
    public string itemName;             // ������ �̸�
    public int itemTotalSum = 1;        // ������ ȹ���� �� ����
    public Sprite itemImage;            // (�ڵ�� ȣ��) ������ �̹���
    public Sprite gradeImage;           // (�ڵ�� ȣ��) ��� �̹���
    public string explanation;          // ������ ����

    public ItemInfo DeepCopy()          // �� �������� �����ؼ� �� ���� �����ִ� DeepCopy �޼ҵ� 
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

    public bool isCanvasOn = false;                // ��ȣ�ۿ� ĵ������ Ȱ��ȭ �Ǿ�����? No

    private void Start()
    {
        itemInfo.itemImage = Resources.Load<Sprite>("ItemResources/" + itemInfo.itemName);
        itemInfo.gradeImage = Resources.Load<Sprite>(itemInfo.gradeColor.ToString());
        interactCheckBox = GameObject.FindAnyObjectByType<InteractableChecker>();
    }

    public void CanvasOn(GameObject slotHolder, GameObject interactSlot) // ( �θ� ������Ʈ, ������ ���� )
    {
        if (!isCanvasOn)
        {
            isCanvasOn = true;
            GameObject slot = Instantiate(interactSlot, this.transform.position, Quaternion.identity);
            TextMeshProUGUI nameText = slot.GetComponentInChildren<TextMeshProUGUI>();
            nameText.text = itemInfo.itemName;
            slot.transform.SetParent(slotHolder.transform);
            interactCheckBox.infoSlotList.Add(itemInfo);            // ������ ������ ����Ʈ�� �Ѱ���
            interactCheckBox.itemObjList.Add(this.gameObject);      // ������ ������Ʈ ������ �Ѱ���
        }
    }
}
