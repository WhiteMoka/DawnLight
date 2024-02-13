using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾��� ���� ������ ����
// ���� �ִϸ��̼� �߿� Ư�� �����ӿ��� �ִϸ��̼� �̺�Ʈ�� ȣ���Ͽ� ���� ������ üũ�Ѵ�.
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
        // ��Ŭ������ ����
        if(Input.GetMouseButtonDown(0) && !isAttacking && !PlayerMove.isJumping && GameManager.Instance.isSwordEquip && !GameManager.Instance.isUiOn)
        {
            ComboAttack();
        }
    }

    void ComboAttack() // �޺� ���� �޼���
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

    public void SlashEffectOnOff()  // ���˸� ������ �� ��ƼŬ ��� 
    {
        if (jointItemR.GetComponentInChildren<SwordStatus>().gameObject.name == "������ ��")
        {
            foreach (ParticleSystem slash in slashEffs)
            {
                slash.gameObject.SetActive(false);
            }
        }
        if (jointItemR.GetComponentInChildren<SwordStatus>().gameObject.name == "����")
        {
            foreach (ParticleSystem slash in slashEffs)
            {
                slash.gameObject.SetActive(true);
            }
        }
    }

    public void OnAttackCollision() // �ִϸ��̼� �̺�Ʈ���� ȣ��   // ���� ������ üũ�ϴ� �ڽ� Ȱ��ȭ
    {
        attakCollisionCheckBox.SetActive(true);
        if (jointItemR.GetComponentInChildren<SwordStatus>().gameObject.name == "����")
        {
            if (slashEffs[attackCount - 1] != null) slashEffs[attackCount - 1].Play();
        }
    }
    public void CheckClickAgain() // �ִϸ��̼� �̺�Ʈ���� ȣ��     // ���� �ִϸ��̼� ������ ���� ��Ŭ�� ��Ÿ�ϰ� ������ ���� �ִϸ��̼� ���
    {
        isAttacking = false;
    }
    public void SlashParticleStop() // �ִϸ��̼� �̺�Ʈ���� ȣ��
    {
        if (jointItemR.GetComponentInChildren<SwordStatus>().gameObject.name == "����")
        {
            if (slashEffs[attackCount - 1] != null) slashEffs[attackCount - 1].Stop();
        }
    }
    public void OnAttackEnd() // �ִϸ��̼� �̺�Ʈ���� ȣ��     // �߰����� ��Ŭ�� �Է� ���� ������ �������� Idle �ִϸ��̼����� ����
    {
        attackCount = 0;
        isAttacking = false;
    }
}
