using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데미지를 입을 수 있는 클래스에 본 인터페이스를 포함시킨다
public interface IDamageable
{
    void OnDamage(float damage);
}
