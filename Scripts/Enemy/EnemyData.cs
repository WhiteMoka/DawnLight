using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ��ũ��Ʈ���� �پ��� Enemy�� �����͸� �����Ѵ�
[CreateAssetMenu(menuName = "ScriptableObject/EnemyData", fileName = "EnemyData")]

public class EnemyData : ScriptableObject
{
    // 1.ü��
    public float health;
    // 2.�̵��ӵ�
    public float speed = 2f;
    // 3.���ݷ�
    public float damage = 20;
    // 4.���� ��Ÿ��
    public float attackTime = 3f;
    // 5.�������� Lazy 6f / Normal 15 / Soldier 40
    [Range(0.1f, 50f)]
    public float detectionRange = 6f;
}
