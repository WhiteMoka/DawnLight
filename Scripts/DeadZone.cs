using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Ʈ���� ������ �״� �Ҹ� ����ϰ�, ���ӿ��� ������ ��ȯ
public class DeadZone : MonoBehaviour
{
    public string nextMapName;  // ��ȯ�� �� �̸�

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
