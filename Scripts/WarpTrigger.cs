using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// �� ��ũ��Ʈ�� ��Ż ������Ʈ�� ���δ�.
// next Map Name �� public ���� ������ �����Ƿ� �� ������Ʈ���� �Է°���
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
