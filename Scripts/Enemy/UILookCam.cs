using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 이 스크립트는 몬스터의 이름 또는 체력UI 등이 메인 카메라를 쳐다보도록 한다 
public class UILookCam : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    void Update()
    {
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        }
    }
}
