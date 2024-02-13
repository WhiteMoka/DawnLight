using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// �� ��ũ��Ʈ�� InvenCanvas�� ���δ�        // �κ��丮 Panel�� ���� �ݱ� ���� ��ũ��Ʈ�̴�.
public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject invenPanel; // �г��� ���� �״� �� �� �ֵ��� SetActive������ GameObject�� ����
    bool isActive = false;

    [SerializeField] Transform slotHolder;       // Content
    [SerializeField] Transform tabSelectImage;
    InvenSlot[] invenSlots;

    [Header("��Ȯ��â")]
    public GameObject itemUseCheckBox;    // ���� Ŭ���� ����� Ȯ��â

    [Header("������ ����â")]
    public GameObject explainPanel;
    public Image itemImage;
    public Image starImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI explainText;

    // ��� ������ ������ �����ϴ� InteractableChecker ��ũ��Ʈ�� all Item List
    private InteractableChecker interactableChecker;
    string currType = "Equipment";

    public static InventoryUI instance;
    private void Awake()
    {
        #region �̱���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    void Start()
    {
        interactableChecker = FindAnyObjectByType<InteractableChecker>();
        invenPanel.SetActive(isActive);
        explainPanel.SetActive(false);
        invenSlots = slotHolder.GetComponentsInChildren<InvenSlot>();

        for (int i = 0; i < invenSlots.Length; i++)
        {
            invenSlots[i].slotNum = i; // ���Կ� ��ȣ �ٿ��ֱ�
        }
        Inventory.instance.onChangeItem += RedrawSlot;  // �������� �ٲ𶧸���
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))    // Ű���� BŰ�� ������ �κ��丮 â ȣ��
        {
            InvenOnOff();
        }
    }

    void RedrawSlot() // �������� �ٲ𶧸��� ������ �ٽ� �׷��ش�
    {
        for (int i = 0; i < invenSlots.Length; i++)
        {
            invenSlots[i].RemoveSlot(); // ���� �ʱ�ȭ
        }
        for (int i = 0; i < Inventory.instance.itemList.Count; i++)
        {
            invenSlots[i].itemInfo = Inventory.instance.itemList[i];
            invenSlots[i].UpdateSlotUI();
        }
    }

    public void InvenOnOff()    // �Ǵ� ���� ��ư Ŭ���� ���� �κ��丮 â ȣ��
    {
        isActive = !isActive;
        GameManager.Instance.isUiOn = isActive;
        invenPanel.SetActive(isActive);
        if (!invenPanel.activeSelf)
        {
            explainPanel.SetActive(false); // �κ��丮�� �����µ�, ����â�� �����ִٸ�
        }
        TabClick(currType);
        Time.timeScale = isActive ? 0 : 1;      // isActive�� ���̶�� timeScale�� 0, �����̶�� 1�� �ð��� �ٽ� �帥��
    }

    public void TabClick(string tabName)
    {
        currType = tabName;
        switch (currType)
        {
            case "Equipment": tabSelectImage.transform.localPosition = new Vector3((float)-162.6, 366, 0); break;
            case "Consumables": tabSelectImage.transform.localPosition = new Vector3((float)-54.8, 366, 0); break;
            case "Etc": tabSelectImage.transform.localPosition = new Vector3((float)56.4, 366, 0); break;
        }
        // ���, �Ҹ�ǰ, ��Ÿ Ŭ���ϸ� �ش��ϴ� Ÿ�Ը� �׸�����
        Inventory.instance.itemList = interactableChecker.allItemList.FindAll(x => x.itemType.ToString() == currType);
        Inventory.instance.onChangeItem?.Invoke();
    }
}
