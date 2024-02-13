using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameClearScene ��ũ��Ʈ�� �����ϰ� DontDestroyOnLoad�� �Ѿ�� �ٸ� ��� ���ʿ��� ������Ʈ�� �����Ѵ�
public class GameOverScene : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(SceneChange), 3f);
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true); // �� ���� ��� ���� ������Ʈ �˻� (��Ȱ��ȭ ����)
        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.IsChildOf(transform)) // �ڽ�obj�� transform�� (�θ�transform)�� �ڽ� ������Ʈ���
            {
                continue; // ���� �ݺ������� ������ �ڵ带 �������� �ʰ� ���� �ݺ����� �ѱ��
            }

            if (!obj.GetComponent<GameOverScene>()) // <GameOverScene> ��ũ��Ʈ�� ������ ���� ���� DontDestroyOnLoad ������Ʈ�� ��� �ı��Ѵ�
            {
                Destroy(obj); // �ı��Ѵ�
            }
        }
    }
    void SceneChange()
    {
        SceneManager.LoadScene("Intro");
    }
}
