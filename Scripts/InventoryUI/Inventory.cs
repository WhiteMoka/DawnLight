using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 이 스크립트는 플레이어에 붙인다
// InteractableChecker 스크립트에서 카테고리가 분류된 <ItemInfo> 리스트를 가진다 
public class Inventory : MonoBehaviour
{
    // 싱글턴
    public static Inventory instance;
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

    // 인벤토리 슬롯에 표시될 분류된 아이템 List
    public List<ItemInfo> itemList = new List<ItemInfo>();

    public Action onChangeItem; // 아이템에 변화가 있을 때마다 호출할 델리게이트

    public void RemoveItem(int index) // 슬롯 클릭해서 아이템 사용하면 -> 아이템 정보 지워주자
    {
        itemList.RemoveAt(index);
        onChangeItem?.Invoke();
    }
}
