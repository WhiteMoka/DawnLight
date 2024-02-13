using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 이 스크립트는 빈 오브젝트에 붙여서 다른 플레이어가 들어와도 움직일 수 있도록 한다.
// 카메라는 FreeLook 카메라 사용     // Ray를 쏴서 바닥인지 아닌지 체크      // 원신 캐릭터 이동방식 재현
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
        #region 싱글톤
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
        Vector3 destination = new Vector3(inputX, 0f, inputZ);              // 목적지

        destination = Camera.main.transform.TransformDirection(destination);    // 캐릭터 컨트롤러의 로컬 좌표 공간상의 방향을, 카메라 기준 월드좌표 공간에서 어느 방향인지 알 수 있도록 변환

        if (destination != Vector3.zero)
        {
            playerTr.forward = new Vector3(destination.x, 0, destination.z);       // 플레이어가 이동하는 방향을 쳐다보도록 한다
        }

        playerAnimator.SetFloat("inputX", inputX);
        playerAnimator.SetFloat("inputZ", inputZ);

        yVelocity -= gravity * Time.deltaTime; // 플레이어는 항상 중력을 받는다
        destination.y = yVelocity; // destination 값을 직접 건드리면 오류나니까, yVelocity를 통해 우회적으로 건드리자

        if (groundChecker.isGround) // 땅이라면
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
        else // 공중이라면
        {
            if (groundChecker.inAirTime > 1f) // 플레이어가 공중에 떠 있는 시간이 1초 이상이라면
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
