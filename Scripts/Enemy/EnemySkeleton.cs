using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public enum SkeletonState  // 자료형 선언
{
    Idle, Patrol, Move, Attack, Missed, Damage, Die
}

// Enemy에는 Idle, Patrol, Soldier 스켈레톤이 존재하며
// SkeletonSpawner 스크립트에서 받아온 타입에 따라 시작하는 행동이 다르다
public class EnemySkeleton : LivingEntity
{
    #region 변수 모음
    [SerializeField] SkeletonState state;   // 인스펙터 창에서 눈으로 상태 확인하기 위하여 선언
    private SkeletonState originState;
    private Animator animator;
    private float maxHP;                    // HP 최대값
    public int knockBackDist = 10;          // 넉백 거리

    public LayerMask layerTarget;           // 추적할 레이어 (Player)
    private LivingEntity targetEntity;      // 플레이어를 감지하면 타겟으로 삼는다
    private NavMeshAgent navMeshAgent;
    private Transform myTr;

    private bool isPlayerNearby = false;    // 플레이어가 근처에 있나요? No
    WaitForSeconds ws = new WaitForSeconds(0.25f);  // 0.25초의 대기시간
    WaitForSeconds ws1 = new WaitForSeconds(1f);    // 1초의 대기시간

    // UI 관련
    public Slider healthSlider;
    public TextMeshProUGUI enemyName;
    public GameObject damageUI;

    // LookPlayer 관련
    Vector3 dirA;
    Vector3 dirB;
    float damping = 3;

    // Patrol 관련
    Vector3 patrolPoint;
    [SerializeField] List<Vector3> patrolList = new List<Vector3>();    // OverlapSphere으로 감지한 PatrolPoint를 담는 리스트
    int patrolPointNum = 0;

    // Attack 관련
    [SerializeField] GameObject attackCollisionCheckBox;

    // SkeletonSpawner 스크립트에서 받아오는 변수 모음
    float speed;                        // 속도
    float damage;                       // 데미지
    float attackTime;                   // 공격 애니메이션 재생하고 다시 공격하기까지 걸리는 시간
    public float detectionRange;        // 감지범위 Lazy 6f / Normal 15 / Soldier 40
    private Vector3 startSpawnPoint;    // 처음 스폰된 위치
    
    // 코루틴 재할당(처음부터 재사용)을 위한 반환 데이터 타입 선언
    IEnumerator idleCoroutine;
    IEnumerator patrolCoroutine;
    IEnumerator moveCoroutine;
    IEnumerator missedCoroutine;        // 중단시킬때도 string 사용하지 않고, StopCoroutine(missedCoroutine); 으로 중지 가능하다.
    #endregion

    private void InitCoroutine() // 코루틴을 초기화 및 재할당하는 함수
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
    private void StartAction() // 행동 시작
    {
        InitCoroutine();
        switch (state) // 시작 상태는 Idle, Patrol, Move 3가지 뿐이다
        {
            case SkeletonState.Idle: StartCoroutine(idleCoroutine); break;
            case SkeletonState.Patrol: StartCoroutine(patrolCoroutine); break;
            case SkeletonState.Move: StartCoroutine(moveCoroutine); break;
        }
    }

    // 초기 세팅 관련
    public void SetUp(EnemyData data, SkeletonType type) // Spawner 스크립트에서 스폰과 동시에 호출
    {
        // Scriptable 오브젝트에서 데이터 가져오기
        health = data.health;
        speed = data.speed;
        navMeshAgent.speed = speed;
        navMeshAgent.angularSpeed = 360;
        damage = data.damage;
        attackTime = data.attackTime;
        detectionRange = data.detectionRange;

        // UI 및 슬라이더 바
        maxHP = health;                         // 최대 체력
        healthSlider.maxValue = maxHP;          // HP바의 max값
        healthSlider.value = health;            // HP바의 현재값은 풀피로 시작
        enemyName.text = type.ToString();       // 개체명

        switch (type)
        {
            case SkeletonType.LazySkeleton: state = SkeletonState.Idle; break;      // 가만히 서서 게으름 피움
            case SkeletonType.PatrolSkeleton:   // 특정 포인트 순찰
                state = SkeletonState.Patrol;
                SearchPatrolPoint(); // 근처 순찰 포인트를 확인하고 리스트에 담는다
                break;
            case SkeletonType.SoldierSkeleton: state = SkeletonState.Move; break;   // 플레이어에게 이동
            default:
                break;
        }
        originState = state;
    }
    public void StartSpawnPoint(Transform transform) // Spawner 스크립트에서 스폰과 동시에 호출
    {
        startSpawnPoint = transform.position;
    }

    void Update()
    {
        if (!isPlayerNearby && !dead) CheckPlayerNearby();   // 플레이어가 근처에 없다면? 근처에 오는지 체크
    }

    // 플레이어 감지 관련
    private void CheckPlayerNearby() // 감지범위 이내에 플레이어가 진입하는지 체크하는 메소드
    {
        Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, detectionRange, layerTarget);

        for (int i = 0; i < colliderArray.Length; i++)
        {
            LivingEntity livingEntity = colliderArray[i].GetComponent<LivingEntity>();

            if (livingEntity != null && !dead)
            {
                targetEntity = livingEntity;
                StartCoroutine(LookTarget(targetEntity.transform.position));
                isPlayerNearby = true;              // 플레이어가 근처에 있습니다
                break;
            }
        }
    }
    IEnumerator LookTarget(Vector3 targetPos) // 몬스터가 targetPos를 쳐다보도록 하는 메서드
    {
        yield return null;
        dirA = (targetPos - transform.position).normalized;      // 기준방향 A ( 플레이어 위치 - 몬스터 위치 )
        dirB = (transform.forward).normalized;                   // 내적을 측정하기 위한 가변방향 B는 몬스터 자신의 앞방향
        float dot = Vector3.Dot(dirA, dirB);                     // dirA와 dirB의 각도가 같다면 1, 반대방향이라면 -1에 가까운 값

        while (-1 < dot && dot < 0.75) // 내적이 0.75~1은 되어야 플레이어를 바라보는 각도가 나온다
        {
            Quaternion target = Quaternion.LookRotation(dirA);      // Quaternion.LookRotation(방향) -> 해당 벡터 방향을 바라보는 회전값을 반환한다
            transform.rotation = Quaternion.Slerp(transform.rotation, target, damping * Time.deltaTime);
            yield return null;
        }
    }

    // State 관련
    private IEnumerator Idle()
    {
        if (dead) yield break;
        isPlayerNearby = false;
        yield return ws;
        animator.SetTrigger("Idle");    // 가만히 있는 Idle 애니메이션 재생

        while (!dead)
        {
            if (isPlayerNearby) // 플레이어가 근처에 있다면
            {
                yield return ws1;
                animator.SetTrigger("PlayerHere");  // 1.05초짜리 스킬 애니메이션 재생
                SoundManager.Instance.SkeletonRoarAudioPlay();
                yield return new WaitForSeconds(1.1f);
                state = SkeletonState.Move;
                StartCoroutine(moveCoroutine);  // Move 상태로 전환
                yield break;
            }
            else
            {
                yield return ws;
            }
        }
    }
    private void SearchPatrolPoint() // 감지 범위 내의 순찰 포인트를 찾는다
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
            if (isPlayerNearby) // 플레이어가 근처에 있다면
            {
                navMeshAgent.ResetPath();
                animator.SetTrigger("PlayerHere");                  // 1.05초짜리 스킬 애니메이션 재생
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
                    // 더해준 순찰포인트 번호가 >= 리스트의 카운트 이상이라면 ? 참이라면 0으로 : 거짓이라면 더해준 번호 그대로
                    patrolPointNum = (++patrolPointNum >= patrolList.Count) ? 0 : patrolPointNum;
                    navMeshAgent.SetDestination(patrolList[patrolPointNum]);
                    StartCoroutine(LookTarget(patrolList[patrolPointNum]));
                }
            }
        }
    }
    private IEnumerator Move() // 플레이어를 향해 Move
    {
        if (dead) yield break;
        animator.SetBool("isRun", true);
        yield return ws;    // Soldier Skeleton이 바로 Move코루틴 진입했을 경우, TargetEntity를 인식할 시간 준다

        while (!dead)
        {
            navMeshAgent.SetDestination(targetEntity.transform.position); // 목적지를 정해준 이후에 바로 remainingDistance 측정하면 0 나오니까
            StartCoroutine(LookTarget(targetEntity.transform.position));
            yield return ws;    // 시간 텀을 준다

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
                yield return new WaitForSeconds(attackTime); // 공격 애니메이션을 재생하는 시간만큼 기다려주고
            }
        }
    }
    public void OnEnemyAttack() // 애니메이션 이벤트에서 호출   // 공격 판정 체크박스 활성화
    {
        attackCollisionCheckBox.SetActive(true);
        SoundManager.Instance.SkeletonAttackAudioPlay();
        attackCollisionCheckBox.GetComponent<EnemyAttackCheck>().SetDamage(damage); // data에서 받은 damage 값을 전달
    }
    private IEnumerator Missed()
    {
        if (dead) yield break;
        yield return ws;
        animator.SetBool("isRun", false);
        navMeshAgent.SetDestination(startSpawnPoint); // 처음 스폰된 위치로 돌아간다
        StartCoroutine(LookTarget(startSpawnPoint));
        yield return ws;

        while (!dead)
        {
            if (navMeshAgent.remainingDistance <= 1f) // 만약 목적지에 거의 도착했다면
            {
                state = originState;
                yield return null;
                StartAction();          // 다시 초기 상태로
                yield break;
            }
            yield return ws;
        }
    }

    // Living Entity로부터 상속
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
    void DamagePopUp(float damage) // 몬스터가 피격 당하면 데미지 숫자를 표시
    {
        Vector3 offSet = Random.insideUnitCircle;
        Vector3 createPos = transform.position + new Vector3(0, 1.5f, 0) + offSet;
        GameObject textUI = Instantiate(damageUI, createPos, Quaternion.identity);
        textUI.GetComponentInChildren<TextMeshPro>().text = damage.ToString();
    }
    IEnumerator KnockBack() // 몬스터가 공격당하면 넉백
    {
        float timer = 0;
        navMeshAgent.updatePosition = false;	// 목적지에 대한 계산 및 회전같은 기능은 유지되지만, 위치는 수동으로 사용하도록
        Vector3 dir = (myTr.position-targetEntity.transform.position).normalized;	// 몬스터가 밀려날 방향 ( 월드좌표 )
        dir = myTr.InverseTransformDirection(dir);		// 월드좌표를 로컬좌표로 변환
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
