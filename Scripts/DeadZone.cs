using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 트리거 반응시 죽는 소리 재생하고, 게임오버 씬으로 전환
public class DeadZone : MonoBehaviour
{
    public string nextMapName;  // 전환할 맵 이름

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.ManDeathAudioPlay();
            Invoke(nameof(ChangeScene), 1f);
        }
    }
    void ChangeScene()
    {
        SceneManager.LoadScene(nextMapName);
    }
}
