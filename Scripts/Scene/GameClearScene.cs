using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 이 스크립트가 붙어있는 빈 오브젝트(부모 오브젝트)의 자식 오브젝트에 파괴하면 안되는 오브젝트를 모두 모아준다.
// 클리어 씬에 진입하면 DontDestroyOnLoad로 넘어온 다른 모든 불필요한 오브젝트를 제거한다
public class GameClearScene : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(SceneChange), 3f);
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true); // 씬 내의 모든 게임 오브젝트 검색 (비활성화 포함)
        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.IsChildOf(transform)) // 자식obj의 transform이 (부모transform)의 자식 오브젝트라면
            {
                continue; // 현재 반복에서의 나머지 코드를 실행하지 않고 다음 반복으로 넘긴다
            }

            if (!obj.GetComponent<ClearScene>()) // <ClearScene> 스크립트를 가지고 있지 않은
                                                 // DontDestroyOnLoad 오브젝트를 모두 파괴한다
            {
                Destroy(obj); // 파괴한다
            }
        }
    }
    void SceneChange()
    {
        SceneManager.LoadScene("Intro");
    }
}
