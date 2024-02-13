using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// 플레이어가 공격 애니메이션을 재생하는데, 공격 판정이 필요한 순간에 박스가 활성화 되고 0.1초 지나면 비활성화된다.
// PlayerAttack 스크립트에서 CollisionCheck 박스를 활성화
public class AttackCollisionCheck : MonoBehaviour
{
    public GameObject jointItemR;
    public int damage;
    private int random;     // 0~2 사이의 값을 랜덤하게 더해준다

    void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !other.GetComponent<LivingEntity>().dead)
        {
            SoundManager.Instance.SwordAttackAudioPlay();
            random = Random.Range(0, 3);
            damage = jointItemR.GetComponentInChildren<SwordStatus>().swordDMG;
            other.GetComponent<EnemySkeleton>().OnDamage(damage + random); // 플레이어 -> 적 공격하는 데미지
        }
    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
