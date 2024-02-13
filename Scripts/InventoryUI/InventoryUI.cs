using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 이 스크립트는 InvenCanvas에 붙인다        // 인벤토리 Panel을 열고 닫기 위한 스크립트이다.
public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject invenPanel; // 패널을 껐다 켰다 할 수 있도록 SetActive용으로 GameObject로 선언
    bool isActive = false;

    [SerializeField] Transform slotHolder;       // Content
    [SerializeField] Transform tabSelectImage;
    InvenSlot[] invenSlots;

    [Header("재확인창")]
    public GameObject itemUseCheckBox;    // 슬롯 클릭시 출력할 확인창

    [Header("아이템 설명창")]
    public GameObject explainPanel;
    public Image itemImage;
    public Image starImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI explainText;

    // 모든 아이템 정보를 보관하는 InteractableChecker 스크립트의 all Item List
    private InteractableChecker interactableChecker;
    string currType = "Equipment";

    public static InventoryUI instance;
    private void Awake()
    {
        #region 싱글톤
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
            invenSlots[i].slotNum = i; // 슬롯에 번호 붙여주기
        }
        Inventory.instance.onChangeItem += RedrawSlot;  // 아이템이 바뀔때마다
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))    // 키보드 B키를 눌러서 인벤토리 창 호출
        {
            InvenOnOff();
        }
    }

    void RedrawSlot() // 아이템이 바뀔때마다 슬롯을 다시 그려준다
    {
        for (int i = 0; i < invenSlots.Length; i++)
        {
            invenSlots[i].RemoveSlot(); // 슬롯 초기화
        }
        for (int i = 0; i < Inventory.instance.itemList.Count; i++)
        {
            invenSlots[i].itemInfo = Inventory.instance.itemList[i];
            invenSlots[i].UpdateSlotUI();
        }
    }

    public void InvenOnOff()    // 또는 가방 버튼 클릭을 통해 인벤토리 창 호출
    {
        isActive = !isActive;
        GameManager.Instance.isUiOn = isActive;
        invenPanel.SetActive(isActive);
        if (!invenPanel.activeSelf)
        {
            explainPanel.SetActive(false); // 인벤토리는 꺼졌는데, 설명창이 남아있다면
        }
        TabClick(currType);
        Time.timeScale = isActive ? 0 : 1;      // isActive가 참이라면 timeScale이 0, 거짓이라면 1로 시간이 다시 흐른다
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
        // 장비, 소모품, 기타 클릭하면 해당하는 타입만 그리도록
        Inventory.instance.itemList = interactableChecker.allItemList.FindAll(x => x.itemType.ToString() == currType);
        Inventory.instance.onChangeItem?.Invoke();
    }
}
