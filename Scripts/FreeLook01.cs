using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using System;

// 3인칭 플레이어 시점을 구현하는 시네머신의 FreeLook 카메라
public class FreeLook01 : MonoBehaviour
{
    public float scrollSpeed = 2000.0f;
    private CinemachineFreeLook cinemachine;

    private static FreeLook01 instance;
    private void Awake()
    {
        #region 싱글톤
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
        // 마우스 스크롤을 올리면 +0.1    스크롤 내리면 -0.1
        float scroollWheel = Input.GetAxis("Mouse ScrollWheel");

        // 시네머신의 Lens - Vertical FOV 값 조정으로 줌인 줌아웃 구현
        cinemachine.m_Lens.FieldOfView -= scroollWheel * scrollSpeed * Time.deltaTime;
        cinemachine.m_Lens.FieldOfView = Mathf.Clamp(cinemachine.m_Lens.FieldOfView, 20, 70);
    }
}
