using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using System;

// 3��Ī �÷��̾� ������ �����ϴ� �ó׸ӽ��� FreeLook ī�޶�
public class FreeLook01 : MonoBehaviour
{
    public float scrollSpeed = 2000.0f;
    private CinemachineFreeLook cinemachine;

    private static FreeLook01 instance;
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
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        cinemachine = GetComponent<CinemachineFreeLook>();
        cinemachine.m_Follow = GameObject.FindGameObjectWithTag("Player").transform;
        cinemachine.m_LookAt = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        // ���콺 ��ũ���� �ø��� +0.1    ��ũ�� ������ -0.1
        float scroollWheel = Input.GetAxis("Mouse ScrollWheel");

        // �ó׸ӽ��� Lens - Vertical FOV �� �������� ���� �ܾƿ� ����
        cinemachine.m_Lens.FieldOfView -= scroollWheel * scrollSpeed * Time.deltaTime;
        cinemachine.m_Lens.FieldOfView = Mathf.Clamp(cinemachine.m_Lens.FieldOfView, 20, 70);
    }
}
