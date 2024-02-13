using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 이 스크립트는 보물상자에 붙인다.
// 스크립트에서 지정한 아이템 프리팹을 드롭하고, 보물상자는 열린 상태로 전환된다
public class TreasureBox : MonoBehaviour, IInteractable
{
    public bool isCanvasOn = false;                 // 상호작용 캔버스에 활성화 되었나요? No
    public bool isItemDrop = false;                 // 아이템을 드롭했나요? No
    [SerializeField] GameObject lootEffect;
    public string boxName = "보물상자";
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
            GetComponent<Collider>().enabled = false; // InteractableChecker의 Trigger에 두번 반응하지 않도록
            isItemDrop = true;
        }
    }
    IEnumerator AddEffect(GameObject drop) // 아이템이 드롭된 이후 잘 보이도록
    {
        yield return new WaitForSeconds(2f);
        if(drop != null)
        {
            GameObject effect = Instantiate(lootEffect, drop.transform.position, Quaternion.identity);
            effect.transform.SetParent(drop.transform);
        }
    }
}
