using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public enum SkeletonState  // �ڷ��� ����
{
    Idle, Patrol, Move, Attack, Missed, Damage, Die
}

// Enemy���� Idle, Patrol, Soldier ���̷����� �����ϸ�
// SkeletonSpawner ��ũ��Ʈ���� �޾ƿ� Ÿ�Կ� ���� �����ϴ� �ൿ�� �ٸ���
public class EnemySkeleton : LivingEntity
{
    #region ���� ����
    [SerializeField] SkeletonState state;   // �ν����� â���� ������ ���� Ȯ���ϱ� ���Ͽ� ����
    private SkeletonState originState;
    private Animator animator;
    private float maxHP;                    // HP �ִ밪
    public int knockBackDist = 10;          // �˹� �Ÿ�

    public LayerMask layerTarget;           // ������ ���̾� (Player)
    private LivingEntity targetEntity;      // �÷��̾ �����ϸ� Ÿ������ ��´�
    private NavMeshAgent navMeshAgent;
    private Transform myTr;

    private bool isPlayerNearby = false;    // �÷��̾ ��ó�� �ֳ���? No
    WaitForSeconds ws = new WaitForSeconds(0.25f);  // 0.25���� ���ð�
    WaitForSeconds ws1 = new WaitForSeconds(1f);    // 1���� ���ð�

    // UI ����
    public Slider healthSlider;
    public TextMeshProUGUI enemyName;
    public GameObject damageUI;

    // LookPlayer ����
    Vector3 dirA;
    Vector3 dirB;
    float damping = 3;

    // Patrol ����
    Vector3 patrolPoint;
    [SerializeField] List<Vector3> patrolList = new List<Vector3>();    // OverlapSphere���� ������ PatrolPoint�� ��� ����Ʈ
    int patrolPointNum = 0;

    // Attack ����
    [SerializeField] GameObject attackCollisionCheckBox;

    // SkeletonSpawner ��ũ��Ʈ���� �޾ƿ��� ���� ����
    float speed;                        // �ӵ�
    float damage;                       // ������
    float attackTime;                   // ���� �ִϸ��̼� ����ϰ� �ٽ� �����ϱ���� �ɸ��� �ð�
    public float detectionRange;        // �������� Lazy 6f / Normal 15 / Soldier 40
    private Vector3 startSpawnPoint;    // ó�� ������ ��ġ
    
    // �ڷ�ƾ ���Ҵ�(ó������ ����)�� ���� ��ȯ ������ Ÿ�� ����
    IEnumerator idleCoroutine;
    IEnumerator patrolCoroutine;
    IEnumerator moveCoroutine;
    IEnumerator missedCoroutine;        // �ߴܽ�ų���� string ������� �ʰ�, StopCoroutine(missedCoroutine); ���� ���� �����ϴ�.
    #endregion

    private void InitCoroutine() // �ڷ�ƾ�� �ʱ�ȭ �� ���Ҵ��ϴ� �Լ�
    {
        idleCoroutine = Idle();
        patrolCoroutine = Patrol();
        moveCoroutine = Move();
        missedCoroutine = Missed();
    }
    private void Awake()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        myTr = GetComponent<Transform>();
    }
    private void Start()
    {
        StartAction();
    }
    private void StartAction() // �ൿ ����
    {
        InitCoroutine();
        switch (state) // ���� ���´� Idle, Patrol, Move 3���� ���̴�
        {
            case SkeletonState.Idle: StartCoroutine(idleCoroutine); break;
            case SkeletonState.Patrol: StartCoroutine(patrolCoroutine); break;
            case SkeletonState.Move: StartCoroutine(moveCoroutine); break;
        }
    }

    // �ʱ� ���� ����
    public void SetUp(EnemyData data, SkeletonType type) // Spawner ��ũ��Ʈ���� ������ ���ÿ� ȣ��
    {
        // Scriptable ������Ʈ���� ������ ��������
        health = data.health;
        speed = data.speed;
        navMeshAgent.speed = speed;
        navMeshAgent.angularSpeed = 360;
        damage = data.damage;
        attackTime = data.attackTime;
        detectionRange = data.detectionRange;

        // UI �� �����̴� ��
        maxHP = health;                         // �ִ� ü��
        healthSlider.maxValue = maxHP;          // HP���� max��
        healthSlider.value = health;            // HP���� ���簪�� Ǯ�Ƿ� ����
        enemyName.text = type.ToString();       // ��ü��

        switch (type)
        {
            case SkeletonType.LazySkeleton: state = SkeletonState.Idle; break;      // ������ ���� ������ �ǿ�
            case SkeletonType.PatrolSkeleton:   // Ư�� ����Ʈ ����
                state = SkeletonState.Patrol;
                SearchPatrolPoint(); // ��ó ���� ����Ʈ�� Ȯ���ϰ� ����Ʈ�� ��´�
                break;
            case SkeletonType.SoldierSkeleton: state = SkeletonState.Move; break;   // �÷��̾�� �̵�
            default:
                break;
        }
        originState = state;
    }
    public void StartSpawnPoint(Transform transform) // Spawner ��ũ��Ʈ���� ������ ���ÿ� ȣ��
    {
        startSpawnPoint = transform.position;
    }

    void Update()
    {
        if (!isPlayerNearby && !dead) CheckPlayerNearby();   // �÷��̾ ��ó�� ���ٸ�? ��ó�� ������ üũ
    }

    // �÷��̾� ���� ����
    private void CheckPlayerNearby() // �������� �̳��� �÷��̾ �����ϴ��� üũ�ϴ� �޼ҵ�
    {
        Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, detectionRange, layerTarget);

        for (int i = 0; i < colliderArray.Length; i++)
        {
            LivingEntity livingEntity = colliderArray[i].GetComponent<LivingEntity>();

            if (livingEntity != null && !dead)
            {
                targetEntity = livingEntity;
                StartCoroutine(LookTarget(targetEntity.transform.position));
                isPlayerNearby = true;              // �÷��̾ ��ó�� �ֽ��ϴ�
                break;
            }
        }
    }
    IEnumerator LookTarget(Vector3 targetPos) // ���Ͱ� targetPos�� �Ĵٺ����� �ϴ� �޼���
    {
        yield return null;
        dirA = (targetPos - transform.position).normalized;      // ���ع��� A ( �÷��̾� ��ġ - ���� ��ġ )
        dirB = (transform.forward).normalized;                   // ������ �����ϱ� ���� �������� B�� ���� �ڽ��� �չ���
        float dot = Vector3.Dot(dirA, dirB);                     // dirA�� dirB�� ������ ���ٸ� 1, �ݴ�����̶�� -1�� ����� ��

        while (-1 < dot && dot < 0.75) // ������ 0.75~1�� �Ǿ�� �÷��̾ �ٶ󺸴� ������ ���´�
        {
            Quaternion target = Quaternion.LookRotation(dirA);      // Quaternion.LookRotation(����) -> �ش� ���� ������ �ٶ󺸴� ȸ������ ��ȯ�Ѵ�
            transform.rotation = Quaternion.Slerp(transform.rotation, target, damping * Time.deltaTime);
            yield return null;
        }
    }

    // State ����
    private IEnumerator Idle()
    {
        if (dead) yield break;
        isPlayerNearby = false;
        yield return ws;
        animator.SetTrigger("Idle");    // ������ �ִ� Idle �ִϸ��̼� ���

        while (!dead)
        {
            if (isPlayerNearby) // �÷��̾ ��ó�� �ִٸ�
            {
                yield return ws1;
                animator.SetTrigger("PlayerHere");  // 1.05��¥�� ��ų �ִϸ��̼� ���
                SoundManager.Instance.SkeletonRoarAudioPlay();
                yield return new WaitForSeconds(1.1f);
                state = SkeletonState.Move;
                StartCoroutine(moveCoroutine);  // Move ���·� ��ȯ
                yield break;
            }
            else
            {
                yield return ws;
            }
        }
    }
    private void SearchPatrolPoint() // ���� ���� ���� ���� ����Ʈ�� ã�´�
    {
        int patrolPointLayer = 1 << LayerMask.NameToLayer("PatrolPoint");
        Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, detectionRange, patrolPointLayer);

        for (int i = 0; i < colliderArray.Length; i++)
        {
            Vector3 pointPos = colliderArray[i].transform.position;
            patrolList.Add(pointPos);
        }
    }
    private IEnumerator Patrol()
    {
        if (dead) yield break;
        isPlayerNearby = false;
        yield return ws1;
        patrolPoint = patrolList[patrolPointNum];
        animator.SetTrigger("Walk");
        navMeshAgent.SetDestination(patrolPoint);

        while (!dead)
        {
            if (isPlayerNearby) // �÷��̾ ��ó�� �ִٸ�
            {
                navMeshAgent.ResetPath();
                animator.SetTrigger("PlayerHere");                  // 1.05��¥�� ��ų �ִϸ��̼� ���
                SoundManager.Instance.SkeletonRoarAudioPlay();
                yield return new WaitForSeconds(1.1f);
                state = SkeletonState.Move;
                StartCoroutine(moveCoroutine);
                yield break;
            }
            else
            {
                yield return ws;
                if (navMeshAgent.remainingDistance < 1f)
                {
                    // ������ ��������Ʈ ��ȣ�� >= ����Ʈ�� ī��Ʈ �̻��̶�� ? ���̶�� 0���� : �����̶�� ������ ��ȣ �״��
                    patrolPointNum = (++patrolPointNum >= patrolList.Count) ? 0 : patrolPointNum;
                    navMeshAgent.SetDestination(patrolList[patrolPointNum]);
                    StartCoroutine(LookTarget(patrolList[patrolPointNum]));
                }
            }
        }
    }
    private IEnumerator Move() // �÷��̾ ���� Move
    {
        if (dead) yield break;
        animator.SetBool("isRun", true);
        yield return ws;    // Soldier Skeleton�� �ٷ� Move�ڷ�ƾ �������� ���, TargetEntity�� �ν��� �ð� �ش�

        while (!dead)
        {
            navMeshAgent.SetDestination(targetEntity.transform.position); // �������� ������ ���Ŀ� �ٷ� remainingDistance �����ϸ� 0 �����ϱ�
            StartCoroutine(LookTarget(targetEntity.transform.position));
            yield return ws;    // �ð� ���� �ش�

            if (navMeshAgent.remainingDistance > detectionRange)
            {
                state = SkeletonState.Missed;                
                StartCoroutine(missedCoroutine);
                yield break;
            }
            else if (navMeshAgent.remainingDistance > 4f)
            {
                animator.SetBool("isRun", true);
            }
            else if (navMeshAgent.remainingDistance > 1.5f)
            {
                animator.SetBool("isRun", false);
            }
            else if (navMeshAgent.remainingDistance <= 1.5f)
            {
                state = SkeletonState.Attack;
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(attackTime); // ���� �ִϸ��̼��� ����ϴ� �ð���ŭ ��ٷ��ְ�
            }
        }
    }
    public void OnEnemyAttack() // �ִϸ��̼� �̺�Ʈ���� ȣ��   // ���� ���� üũ�ڽ� Ȱ��ȭ
    {
        attackCollisionCheckBox.SetActive(true);
        SoundManager.Instance.SkeletonAttackAudioPlay();
        attackCollisionCheckBox.GetComponent<EnemyAttackCheck>().SetDamage(damage); // data���� ���� damage ���� ����
    }
    private IEnumerator Missed()
    {
        if (dead) yield break;
        yield return ws;
        animator.SetBool("isRun", false);
        navMeshAgent.SetDestination(startSpawnPoint); // ó�� ������ ��ġ�� ���ư���
        StartCoroutine(LookTarget(startSpawnPoint));
        yield return ws;

        while (!dead)
        {
            if (navMeshAgent.remainingDistance <= 1f) // ���� �������� ���� �����ߴٸ�
            {
                state = originState;
                yield return null;
                StartAction();          // �ٽ� �ʱ� ���·�
                yield break;
            }
            yield return ws;
        }
    }

    // Living Entity�κ��� ���
    public override void OnDamage(float damage)
    {
        DamagePopUp(damage);
        healthSlider.value = health;
        base.OnDamage(damage);
        if (!dead)
        {
            animator.SetTrigger("Damage");
            StartCoroutine(KnockBack());
        }
    }
    void DamagePopUp(float damage) // ���Ͱ� �ǰ� ���ϸ� ������ ���ڸ� ǥ��
    {
        Vector3 offSet = Random.insideUnitCircle;
        Vector3 createPos = transform.position + new Vector3(0, 1.5f, 0) + offSet;
        GameObject textUI = Instantiate(damageUI, createPos, Quaternion.identity);
        textUI.GetComponentInChildren<TextMeshPro>().text = damage.ToString();
    }
    IEnumerator KnockBack() // ���Ͱ� ���ݴ��ϸ� �˹�
    {
        float timer = 0;
        navMeshAgent.updatePosition = false;	// �������� ���� ��� �� ȸ������ ����� ����������, ��ġ�� �������� ����ϵ���
        Vector3 dir = (myTr.position-targetEntity.transform.position).normalized;	// ���Ͱ� �з��� ���� ( ������ǥ )
        dir = myTr.InverseTransformDirection(dir);		// ������ǥ�� ������ǥ�� ��ȯ
        dir.y = 0;

        while (timer <= 0.1f)
        {
            timer += Time.deltaTime;
            myTr.Translate(dir * knockBackDist * Time.deltaTime);
            yield return null;
        }
        navMeshAgent.updatePosition = true;
    }
    public override void Die()
    {
        healthSlider.value = 0;
        base.Die();
        animator.SetTrigger("Death");
        SoundManager.Instance.SkeletonDeathAudioPlay();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
