using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ���� �Դ� ������ ���� �ؽ�Ʈ�� ���� ī�޶� �ٶ󺻴�
// �������� �������鼭 ��Ʈ ����� �۾�����
public class DamageText : MonoBehaviour
{
    private Camera cam;
    public TextMeshPro tmp;
    public float moveSpeed = 2;

    public float fontSizeStart = 10;        // ���� ��Ʈ ������
    public float fontSizeEnd = 5;           // ���� ��Ʈ ������
    float currentTime = 0f;
    public float destroyTime = 0.3f;        // ������ �ؽ�Ʈ�� �ı��Ǳ���� �ð�

    void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // Lerp�� �ɸ��� �ð��� destroyTime ������ ���� ����
        currentTime += Time.deltaTime;
        if (currentTime > destroyTime)
        {
            currentTime = destroyTime;
        }
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        tmp.fontSize = Mathf.Lerp(fontSizeStart, fontSizeEnd, currentTime/destroyTime);

        if (cam != null)
        {
            // ������ �ؽ�Ʈ�� ī�޶� �ٶ󺻴�
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        }
    }
}
