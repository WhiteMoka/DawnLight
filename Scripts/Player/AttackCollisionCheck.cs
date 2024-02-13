using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// �÷��̾ ���� �ִϸ��̼��� ����ϴµ�, ���� ������ �ʿ��� ������ �ڽ��� Ȱ��ȭ �ǰ� 0.1�� ������ ��Ȱ��ȭ�ȴ�.
// PlayerAttack ��ũ��Ʈ���� CollisionCheck �ڽ��� Ȱ��ȭ
public class AttackCollisionCheck : MonoBehaviour
{
    public GameObject jointItemR;
    public int damage;
    private int random;     // 0~2 ������ ���� �����ϰ� �����ش�

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
            other.GetComponent<EnemySkeleton>().OnDamage(damage + random); // �÷��̾� -> �� �����ϴ� ������
        }
    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
