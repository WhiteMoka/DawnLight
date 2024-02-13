using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// BoxCast�� �̿��Ͽ� Character Controller ������Ʈ�� ����ϴ� �÷��̾ �ٴڿ� �ִ��� �ƴ��� üũ�Ѵ�
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

                        // ( ���� ��ġ,          Box�� 1/3 ������,             Ray�� ����,    RaycastHit�� ���,   Box�� ȸ����, BoxCast�� ������ �Ÿ�, ������ ���̾� )
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
