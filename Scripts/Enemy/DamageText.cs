using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 적이 입는 데미지 숫자 텍스트는 메인 카메라를 바라본다
// 위쪽으로 떠오르면서 폰트 사이즈가 작아진다
public class DamageText : MonoBehaviour
{
    private Camera cam;
    public TextMeshPro tmp;
    public float moveSpeed = 2;

    public float fontSizeStart = 10;        // 시작 폰트 사이즈
    public float fontSizeEnd = 5;           // 종료 폰트 사이즈
    float currentTime = 0f;
    public float destroyTime = 0.3f;        // 데미지 텍스트가 파괴되기까지 시간

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
        // Lerp에 걸리는 시간을 destroyTime 변수로 조절 가능
        currentTime += Time.deltaTime;
        if (currentTime > destroyTime)
        {
            currentTime = destroyTime;
        }
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        tmp.fontSize = Mathf.Lerp(fontSizeStart, fontSizeEnd, currentTime/destroyTime);

        if (cam != null)
        {
            // 데미지 텍스트는 카메라를 바라본다
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        }
    }
}
