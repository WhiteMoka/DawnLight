using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���Ͱ� ���� �ִϸ��̼��� ����ϴµ�, ���� ������ �ʿ��� ������ �ڽ��� Ȱ��ȭ �ǰ� 0.1�� ������ ��Ȱ��ȭ�ȴ�.
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
            other.GetComponent<PlayerHealth>().OnDamage(damage); // �� -> �÷��̾� �����ϴ� ������
        }
    }
    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
    public void SetDamage(float dataDamage) // Enemy�� Player�� �����ϴ� ������ ����
    {
        damage = dataDamage;
    }
}
