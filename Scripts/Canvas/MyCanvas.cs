using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//�� ��ũ��Ʈ�� �÷��̾��� HP�����̴�, �κ��丮 ������ �� �⺻���� UI�� ������ ĵ������ ���δ�.
public class MyCanvas : MonoBehaviour
{
    private static MyCanvas instance;

    private void Awake()
    {
        #region �̱���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }
}
