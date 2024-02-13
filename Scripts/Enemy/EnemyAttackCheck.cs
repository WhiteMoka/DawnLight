using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터가 공격 애니메이션을 재생하는데, 공격 판정이 필요한 순간에 박스가 활성화 되고 0.1초 지나면 비활성화된다.
public class EnemyAttackCheck : MonoBehaviour
{
    float damage;

    void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().OnDamage(damage); // 적 -> 플레이어 공격하는 데미지
        }
    }
    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
    public void SetDamage(float dataDamage) // Enemy가 Player를 공격하는 데미지 설정
    {
        damage = dataDamage;
    }
}
