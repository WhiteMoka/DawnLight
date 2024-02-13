using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// BoxCast를 이용하여 Character Controller 컴포넌트를 사용하는 플레이어가 바닥에 있는지 아닌지 체크한다
public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float maxDistance = 1.1f;
    [SerializeField] private Color rayColor = Color.red;
    [SerializeField] private LayerMask groundLayer;
    public bool isGround = true;
    public float inAirTime = 0;

    private void Update()
    {
        if (isGround) inAirTime = 0;
        else inAirTime += Time.deltaTime;

                        // ( 현재 위치,          Box의 1/3 사이즈,             Ray의 방향,    RaycastHit의 결과,   Box의 회전값, BoxCast를 진행할 거리, 반응할 레이어 )
        if (Physics.BoxCast(transform.position, transform.lossyScale / 3.0f, -transform.up, out RaycastHit hit, transform.rotation, maxDistance, groundLayer))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawRay(transform.position, -transform.up * maxDistance);
    }

    private void OnEnable()
    {
        
    }
}
