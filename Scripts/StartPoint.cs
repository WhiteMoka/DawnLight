using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// �� ��ũ��Ʈ�� ������ ����� ���� �÷��̾��� ���� ��ġ�� ��Ÿ���� ������Ʈ�� ���δ�.
public class StartPoint : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance.nextMapName != "") // �� ó�� �����Ҷ��� ���� �ȵǵ���
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = this.transform.position;
                player.transform.rotation = this.transform.rotation;
                Invoke(nameof(MoveInputReStart), 1f);
            }
        }
    }
    void MoveInputReStart()
    {
        PlayerMove playerMoveManager = FindAnyObjectByType<PlayerMove>();
        playerMoveManager.enabled = true;
    }
}
