using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC에게 다가가도 상호작용 캔버스 뜨면서 NPC 이름 텍스트 뜨고, F키 누르면 대화 가능하도록 창이 떠야 하고
// Item에 접근해도 상호작용 캔버스 뜨면서 Item 이름 텍스트 뜨고, F키 누르면 획득하도록 해야하니까
// 보물상자에 접근해도 상호작용 캔버스 뜨면서 보물상자 텍스트 뜨고, F키 누르면 열리도록 해야하니까
// 창이 떠야 한다는건 공통점이다
public interface IInteractable
{
    public void CanvasOn(GameObject slotHolder, GameObject emptySlot);    // 텍스트 띄우는 '슬롯'들의 부모가 되는 게임오브젝트를 매개변수로 넘겨준다
                                                                          // 자식으로 생성할 빈 슬롯 오브젝트도 매개변수로 넘겨준다
}
