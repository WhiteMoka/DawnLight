using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �� ��ũ��Ʈ�� ������ �̸� �Ǵ� ü��UI ���� ���� ī�޶� �Ĵٺ����� �Ѵ� 
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
