using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// 이 스크립트는 워프를 통과한 이후 플레이어의 시작 위치를 나타내는 오브젝트에 붙인다.
public class StartPoint : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance.nextMapName != "") // 맨 처음 시작할때는 워프 안되도록
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
