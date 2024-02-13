using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �� ��ũ��Ʈ�� �÷��̾ ���δ�
// InteractableChecker ��ũ��Ʈ���� ī�װ��� �з��� <ItemInfo> ����Ʈ�� ������ 
public class Inventory : MonoBehaviour
{
    // �̱���
    public static Inventory instance;
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

    // �κ��丮 ���Կ� ǥ�õ� �з��� ������ List
    public List<ItemInfo> itemList = new List<ItemInfo>();

    public Action onChangeItem; // �����ۿ� ��ȭ�� ���� ������ ȣ���� ��������Ʈ

    public void RemoveItem(int index) // ���� Ŭ���ؼ� ������ ����ϸ� -> ������ ���� ��������
    {
        itemList.RemoveAt(index);
        onChangeItem?.Invoke();
    }
}
