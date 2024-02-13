using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// �� ��ũ��Ʈ�� �������ڿ� ���δ�.
// ��ũ��Ʈ���� ������ ������ �������� ����ϰ�, �������ڴ� ���� ���·� ��ȯ�ȴ�
public class TreasureBox : MonoBehaviour, IInteractable
{
    public bool isCanvasOn = false;                 // ��ȣ�ۿ� ĵ������ Ȱ��ȭ �Ǿ�����? No
    public bool isItemDrop = false;                 // �������� ����߳���? No
    [SerializeField] GameObject lootEffect;
    public string boxName = "��������";
    private Animator boxAnimator;
    [SerializeField] GameObject[] dropItems;

    void Start()
    {
        boxAnimator = GetComponent<Animator>();
    }
    public void CanvasOn(GameObject slotHolder, GameObject interactSlot)
    {
        if (!isCanvasOn)
        {
            isCanvasOn = true;
            GameObject slot = Instantiate(interactSlot, this.transform.position, Quaternion.identity);
            TextMeshProUGUI nameText = slot.GetComponentInChildren<TextMeshProUGUI>();
            nameText.text = boxName;
            slot.transform.SetParent(slotHolder.transform);
        }
    }
    public void DropItem()
    {
        if (!isItemDrop)
        {
            boxAnimator.SetTrigger("Open");
            Vector3 throwAngle = (-transform.forward) + Vector3.up;
            for (int i = 0; i < dropItems.Length; i++)
            {
                GameObject drop = Instantiate(dropItems[i], this.transform.position+Vector3.up, Quaternion.identity);
                drop.GetComponent<Rigidbody>().AddForce(throwAngle*2, ForceMode.Impulse);
                StartCoroutine(AddEffect(drop));
            }
            GetComponent<Collider>().enabled = false; // InteractableChecker�� Trigger�� �ι� �������� �ʵ���
            isItemDrop = true;
        }
    }
    IEnumerator AddEffect(GameObject drop) // �������� ��ӵ� ���� �� ���̵���
    {
        yield return new WaitForSeconds(2f);
        if(drop != null)
        {
            GameObject effect = Instantiate(lootEffect, drop.transform.position, Quaternion.identity);
            effect.transform.SetParent(drop.transform);
        }
    }
}
