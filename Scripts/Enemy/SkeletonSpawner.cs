using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkeletonType
{
    LazySkeleton, PatrolSkeleton, SoldierSkeleton
}

// �� ��ũ��Ʈ�� ���̷��� ��ȯ�� �����Ѵ�
// � Ÿ������, � �ɷ�ġ �����͸� ������ �ִ��� �ش� ���Ͱ� ���� ��ũ��Ʈ�� �����Ѵ� 
public class SkeletonSpawner : MonoBehaviour
{
    // �ν����� â���� �Է�
    public SkeletonType skeletonType;       // ��ȯ�ϰ��� �ϴ� Ÿ��
    public EnemySkeleton enemyPrefab;       // ��ȯ�� ������
    public EnemyData enemyData;             // ��ȯ�� ���̷����� ScriptableObject ����������
    private bool isSoldierComing = false;

    private int soldierSpawnCount = 5;
    WaitForSeconds ws1 = new WaitForSeconds(1f);

    void Start()
    {
        switch (skeletonType) // �ν����� â���� �Է¹��� skeletonType
        {
            case SkeletonType.LazySkeleton: CreateOnce(); break;
            case SkeletonType.PatrolSkeleton: CreateOnce(); break;
            case SkeletonType.SoldierSkeleton: StartCoroutine(CreateMany()); break;
        }
    }

    void CreateOnce()
    {
        EnemySkeleton createSkeleton =  Instantiate(enemyPrefab, this.transform.position, this.transform.rotation);
        createSkeleton.SetUp(enemyData, skeletonType); // ������ ���̷��濡�� �ɷ�ġ �����Ϳ� ���� ����
        createSkeleton.StartSpawnPoint(this.gameObject.transform);
        GameManager.Instance.skeletonList.Add(createSkeleton);

        // ������ ����Ʈ���� �����ֱ� ���� ��������Ʈ ü�ο� ���       // Animator�� ��������Ʈ ü�ο� ����޼ҵ�� ����ϸ�X
        createSkeleton.OnDeath += () => GameManager.Instance.skeletonList.Remove(createSkeleton);
        createSkeleton.OnDeath += () => Destroy(createSkeleton.gameObject, 5f);
    }

    IEnumerator CreateMany()
    {
        while (soldierSpawnCount > 1)
        {
            yield return ws1;
            if (GameManager.Instance.skeletonList.Count <= 2) // �ʵ��� ���̷����� 2���� ���ϰ� �Ǹ� Soldier Skeleton ��ȯ
            {
                if (!isSoldierComing) // ó�� �� ���� Soldier ���̷��� ����� ���� ���
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
