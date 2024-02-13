using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

// 플레이어의 HP를 슬라이더에 연결하고, 데미지를 입거나 회복하는 상황을 구현  
public class PlayerHealth : LivingEntity
{
    private float maxHP = 999f;
    private Animator playerAnimator;
    public ParticleSystem healEffect;

    public GameObject moveManager;
    public Slider healthSlider;
    public TextMeshProUGUI healthValue;

    private static PlayerHealth instance;
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
    protected override void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        base.OnEnable();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (moveManager == null) moveManager = FindAnyObjectByType<PlayerMove>().gameObject;
        if (healthSlider == null) healthSlider = GameObject.Find("HP_Slider").GetComponent<Slider>();
        if (healthValue == null) healthValue = GameObject.Find("HP_Value_TMP").GetComponent<TextMeshProUGUI>();
        moveManager.SetActive(true);
        playerAnimator = GetComponentInChildren<Animator>();

        float savedHealth = PlayerPrefs.GetFloat("NowHP", maxHP); // 초기값이 없다면 maxHP를 부여한다
        health = savedHealth;
        healthSlider.maxValue = maxHP;      // 고정값인 최대 체력은 maxHP 변수
        healthSlider.value = health;        // 가변값인 현재 체력은 health 변수
        healthValue.text = health + " / " + maxHP;
        healEffect.Stop();

        if (SceneManager.GetActiveScene().name == "Intro")
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerPrefs.DeleteAll();
    }

    public override void OnDamage(float damage)
    {
        if (!dead)
        {
            playerAnimator.SetTrigger("Damage");
        }
        base.OnDamage(damage);
        if (health < 0) health = 0;         // 데미지 입고 죽었을 때 현재체력이 -값이 되는 것을 방지
        PlayerPrefs.SetFloat("NowHP", health);
        healthSlider.value = health;
        healthValue.text = health + " / " + maxHP;
    }
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        if (health > maxHP) health = maxHP;
        PlayerPrefs.SetFloat("NowHP", health);
        healthSlider.value = health;
        healthValue.text = health + " / " + maxHP;
        healEffect.Play();
    }
    public override void Die()
    {
        base.Die();
        playerAnimator.SetTrigger("Death");
        moveManager.SetActive(false);   // 플레이어가 죽으면 움직이지 못하도록
    }
    
    public void PlayerHealthOverride(float hpLoadData)
    {
        health = hpLoadData;
        PlayerPrefs.SetFloat("NowHP", health);
        healthSlider.value = health;
        healthValue.text = health + " / " + maxHP;
    }
}
