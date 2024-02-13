using System;
using UnityEngine;
// 이 스크립트는 Player와 Enemy에 상속
// 인터페이스에서는 변수 선언이 불가능 하므로, 인터페이스를 상속받는 본 스크립트를 상속해준다
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float health // 체력
    {
        get;
        protected set;
    }
    public bool dead // 사망여부
    {
        get;
        protected set;      
    }

    public event Action OnDeath;

    protected virtual void OnEnable()   
    {
        dead = false;
    }

    public virtual void Die() // 사망시
    {
        dead = true;
        OnDeath?.Invoke();
    }

    public virtual void OnDamage(float damage) // 데미지 입을 경우
    {
        health -= damage;

        if (health <= 0 && !dead)    // hp가 0보다 작거나 같아지고, dead 상태가 아니라면  ->  사망
        {
            Die();
        }
    }

    public virtual void RestoreHealth(float newHealth)   // 체력 회복
    {
        if (dead) return; // 이미 죽었으면 HP 회복시키지 않도록
        health += newHealth;
    }
}
