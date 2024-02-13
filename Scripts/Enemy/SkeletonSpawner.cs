using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkeletonType
{
    LazySkeleton, PatrolSkeleton, SoldierSkeleton
}

// 이 스크립트는 스켈레톤 소환을 구현한다
// 어떤 타입인지, 어떤 능력치 데이터를 가지고 있는지 해당 몬스터가 가진 스크립트에 전달한다 
public class SkeletonSpawner : MonoBehaviour
{
    // 인스펙터 창에서 입력
    public SkeletonType skeletonType;       // 소환하고자 하는 타입
    public EnemySkeleton enemyPrefab;       // 소환할 프리팹
    public EnemyData enemyData;             // 소환할 스켈레톤의 ScriptableObject 데이터파일
    private bool isSoldierComing = false;

    private int soldierSpawnCount = 5;
    WaitForSeconds ws1 = new WaitForSeconds(1f);

    void Start()
    {
        switch (skeletonType) // 인스펙터 창에서 입력받은 skeletonType
        {
            case SkeletonType.LazySkeleton: CreateOnce(); break;
            case SkeletonType.PatrolSkeleton: CreateOnce(); break;
            case SkeletonType.SoldierSkeleton: StartCoroutine(CreateMany()); break;
        }
    }

    void CreateOnce()
    {
        EnemySkeleton createSkeleton =  Instantiate(enemyPrefab, this.transform.position, this.transform.rotation);
        createSkeleton.SetUp(enemyData, skeletonType); // 생성한 스켈레톤에게 능력치 데이터와 파일 전달
        createSkeleton.StartSpawnPoint(this.gameObject.transform);
        GameManager.Instance.skeletonList.Add(createSkeleton);

        // 죽으면 리스트에서 지워주기 위해 델리게이트 체인에 등록       // Animator는 델리게이트 체인에 무명메소드로 등록하면X
        createSkeleton.OnDeath += () => GameManager.Instance.skeletonList.Remove(createSkeleton);
        createSkeleton.OnDeath += () => Destroy(createSkeleton.gameObject, 5f);
    }

    IEnumerator CreateMany()
    {
        while (soldierSpawnCount > 1)
        {
            yield return ws1;
            if (GameManager.Instance.skeletonList.Count <= 2) // 필드의 스켈레톤이 2마리 이하가 되면 Soldier Skeleton 소환
            {
                if (!isSoldierComing) // 처음 한 번만 Soldier 스켈레톤 등장시 사운드 재생
                {
                    SoundManager.Instance.SoldierComingAudioPlay();
                    isSoldierComing = true;
                }
                EnemySkeleton createSkeleton = Instantiate(enemyPrefab, this.transform.position, this.transform.rotation);
                createSkeleton.SetUp(enemyData, skeletonType);
                createSkeleton.StartSpawnPoint(this.gameObject.transform);
                GameManager.Instance.skeletonList.Add(createSkeleton);
                soldierSpawnCount--;

                createSkeleton.OnDeath += () => GameManager.Instance.skeletonList.Remove(createSkeleton);
                createSkeleton.OnDeath += () => Destroy(createSkeleton.gameObject, 5f);
                yield return ws1;
            }
        }
    }
}
