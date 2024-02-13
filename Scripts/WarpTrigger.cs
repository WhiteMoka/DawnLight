using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// 이 스크립트는 포탈 오브젝트에 붙인다.
// next Map Name 을 public 으로 가지고 있으므로 각 오브젝트마다 입력가능
public class WarpTrigger : MonoBehaviour
{
    public string nextMapName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMove playerMoveManager = FindAnyObjectByType<PlayerMove>();
            playerMoveManager.enabled = false;
            SoundManager.Instance.HolyStartAudioPlay();
            GameManager.Instance.nextMapName = nextMapName;
            Invoke(nameof(SceneChange), 0.5f);
        }
    }
    void SceneChange()
    {
        SceneManager.LoadScene(GameManager.Instance.nextMapName);
    }
}
