using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �� ��ũ��Ʈ�� �� ������Ʈ�� �ٿ��� �ٸ� �÷��̾ ���͵� ������ �� �ֵ��� �Ѵ�.
// ī�޶�� FreeLook ī�޶� ���     // Ray�� ���� �ٴ����� �ƴ��� üũ      // ���� ĳ���� �̵���� ����
public class PlayerMove : MonoBehaviour
{
    private CharacterController playerController;
    private Animator playerAnimator;
    private GroundChecker groundChecker;
    private Transform playerTr;
    private PlayerAttack playerAttack;
    public static bool isJumping;

    private float inputX;
    private float inputZ;
    float yVelocity = 0;

    private float moveSpeed;
    private float jumpPower;
    private float gravity;

    private static PlayerMove instance;
    private void Awake()
    {
        #region �̱���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
        playerController = tempPlayer.GetComponent<CharacterController>();
        groundChecker = tempPlayer.GetComponent<GroundChecker>();
        playerAnimator = tempPlayer.GetComponentInChildren<Animator>();
        playerAttack = tempPlayer.GetComponentInChildren<PlayerAttack>();
        playerTr = tempPlayer.transform;

        moveSpeed = 5f;
        jumpPower = 3f;
        gravity = 9.81f;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        Vector3 destination = new Vector3(inputX, 0f, inputZ);              // ������

        destination = Camera.main.transform.TransformDirection(destination);    // ĳ���� ��Ʈ�ѷ��� ���� ��ǥ �������� ������, ī�޶� ���� ������ǥ �������� ��� �������� �� �� �ֵ��� ��ȯ

        if (destination != Vector3.zero)
        {
            playerTr.forward = new Vector3(destination.x, 0, destination.z);       // �÷��̾ �̵��ϴ� ������ �Ĵٺ����� �Ѵ�
        }

        playerAnimator.SetFloat("inputX", inputX);
        playerAnimator.SetFloat("inputZ", inputZ);

        yVelocity -= gravity * Time.deltaTime; // �÷��̾�� �׻� �߷��� �޴´�
        destination.y = yVelocity; // destination ���� ���� �ǵ帮�� �������ϱ�, yVelocity�� ���� ��ȸ������ �ǵ帮��

        if (groundChecker.isGround) // ���̶��
        {
            isJumping = false;
            yVelocity = 0;
            destination.y = yVelocity;
            playerAnimator.SetBool("JumpInAir", false);

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                SoundManager.Instance.MaleJumpAudioPlay();
                isJumping = true;
                yVelocity = jumpPower;
                destination.y = yVelocity;
                playerAnimator.SetTrigger("JumpStart");
            }
        }
        else // �����̶��
        {
            if (groundChecker.inAirTime > 1f) // �÷��̾ ���߿� �� �ִ� �ð��� 1�� �̻��̶��
            {
                playerAnimator.SetBool("JumpInAir", true);
            }
        }

        if (!playerAttack.isAttacking)
        {
            // Move
            playerController.Move(destination * moveSpeed * Time.deltaTime);
        }
    }
}
