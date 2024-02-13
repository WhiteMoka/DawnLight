using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트에는 다양한 Enemy의 데이터를 저장한다
[CreateAssetMenu(menuName = "ScriptableObject/EnemyData", fileName = "EnemyData")]

public class EnemyData : ScriptableObject
{
    // 1.체력
    public float health;
    // 2.이동속도
    public float speed = 2f;
    // 3.공격력
    public float damage = 20;
    // 4.공격 쿨타임
    public float attackTime = 3f;
    // 5.감지범위 Lazy 6f / Normal 15 / Soldier 40
    [Range(0.1f, 50f)]
    public float detectionRange = 6f;
}
