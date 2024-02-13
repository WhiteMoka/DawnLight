using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//이 스크립트는 플레이어의 HP슬라이더, 인벤토리 아이콘 등 기본적인 UI를 가지는 캔버스에 붙인다.
public class MyCanvas : MonoBehaviour
{
    private static MyCanvas instance;

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
}
