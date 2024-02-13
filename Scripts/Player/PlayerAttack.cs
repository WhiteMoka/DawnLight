using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 연속 공격을 구현
// 공격 애니메이션 중에 특정 프레임에서 애니메이션 이벤트를 호출하여 공격 판정을 체크한다.
public class PlayerAttack : MonoBehaviour
{
    private int attackCount;
    private Animator playerAnimator;
    public bool isAttacking = false;
    [SerializeField] private GameObject attakCollisionCheckBox;
    public GameObject jointItemR;
    public ParticleSystem[] slashEffs;
    

    void Start()
    {
        attackCount = 0;
        isAttacking = false;
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // 좌클릭으로 공격
        if(Input.GetMouseButtonDown(0) && !isAttacking && !PlayerMove.isJumping && GameManager.Instance.isSwordEquip && !GameManager.Instance.isUiOn)
        {
            ComboAttack();
        }
    }

    void ComboAttack() // 콤보 공격 메서드
    {
        if (attackCount < 3)
        {
            isAttacking = true;
            attackCount++;
            playerAnimator.SetTrigger("Attack" + attackCount);
        }
        else
        {
            OnAttackEnd();
        }
    }

    public void SlashEffectOnOff()  // 마검만 공격할 때 파티클 재생 
    {
        if (jointItemR.GetComponentInChildren<SwordStatus>().gameObject.name == "오래된 검")
        {
            foreach (ParticleSystem slash in slashEffs)
            {
                slash.gameObject.SetActive(false);
            }
        }
        if (jointItemR.GetComponentInChildren<SwordStatus>().gameObject.name == "마검")
        {
            foreach (ParticleSystem slash in slashEffs)
            {
                slash.gameObject.SetActive(true);
            }
        }
    }

    public void OnAttackCollision() // 애니메이션 이벤트에서 호출   // 공격 판정을 체크하는 박스 활성화
    {
        attakCollisionCheckBox.SetActive(true);
        if (jointItemR.GetComponentInChildren<SwordStatus>().gameObject.name == "마검")
        {
            if (slashEffs[attackCount - 1] != null) slashEffs[attackCount - 1].Play();
        }
    }
    public void CheckClickAgain() // 애니메이션 이벤트에서 호출     // 공격 애니메이션 끝나기 전에 좌클릭 연타하고 있으면 다음 애니메이션 재생
    {
        isAttacking = false;
    }
    public void SlashParticleStop() // 애니메이션 이벤트에서 호출
    {
        if (jointItemR.GetComponentInChildren<SwordStatus>().gameObject.name == "마검")
        {
            if (slashEffs[attackCount - 1] != null) slashEffs[attackCount - 1].Stop();
        }
    }
    public void OnAttackEnd() // 애니메이션 이벤트에서 호출     // 추가적인 좌클릭 입력 없이 공격이 끝났으면 Idle 애니메이션으로 복귀
    {
        attackCount = 0;
        isAttacking = false;
    }
}
