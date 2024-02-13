using System;
using UnityEngine;
// �� ��ũ��Ʈ�� Player�� Enemy�� ���
// �������̽������� ���� ������ �Ұ��� �ϹǷ�, �������̽��� ��ӹ޴� �� ��ũ��Ʈ�� ������ش�
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float health // ü��
    {
        get;
        protected set;
    }
    public bool dead // �������
    {
        get;
        protected set;      
    }

    public event Action OnDeath;

    protected virtual void OnEnable()   
    {
        dead = false;
    }

    public virtual void Die() // �����
    {
        dead = true;
        OnDeath?.Invoke();
    }

    public virtual void OnDamage(float damage) // ������ ���� ���
    {
        health -= damage;

        if (health <= 0 && !dead)    // hp�� 0���� �۰ų� ��������, dead ���°� �ƴ϶��  ->  ���
        {
            Die();
        }
    }

    public virtual void RestoreHealth(float newHealth)   // ü�� ȸ��
    {
        if (dead) return; // �̹� �׾����� HP ȸ����Ű�� �ʵ���
        health += newHealth;
    }
}
