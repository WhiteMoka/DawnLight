using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC���� �ٰ����� ��ȣ�ۿ� ĵ���� �߸鼭 NPC �̸� �ؽ�Ʈ �߰�, FŰ ������ ��ȭ �����ϵ��� â�� ���� �ϰ�
// Item�� �����ص� ��ȣ�ۿ� ĵ���� �߸鼭 Item �̸� �ؽ�Ʈ �߰�, FŰ ������ ȹ���ϵ��� �ؾ��ϴϱ�
// �������ڿ� �����ص� ��ȣ�ۿ� ĵ���� �߸鼭 �������� �ؽ�Ʈ �߰�, FŰ ������ �������� �ؾ��ϴϱ�
// â�� ���� �Ѵٴ°� �������̴�
public interface IInteractable
{
    public void CanvasOn(GameObject slotHolder, GameObject emptySlot);    // �ؽ�Ʈ ���� '����'���� �θ� �Ǵ� ���ӿ�����Ʈ�� �Ű������� �Ѱ��ش�
                                                                          // �ڽ����� ������ �� ���� ������Ʈ�� �Ű������� �Ѱ��ش�
}
