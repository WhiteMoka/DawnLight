using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

// �÷��̾��� HP�� �����̴��� �����ϰ�, �������� �԰ų� ȸ���ϴ� ��Ȳ�� ����  
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

        float savedHealth = PlayerPrefs.GetFloat("NowHP", maxHP); // �ʱⰪ�� ���ٸ� maxHP�� �ο��Ѵ�
        health = savedHealth;
        healthSlider.maxValue = maxHP;      // �������� �ִ� ü���� maxHP ����
        healthSlider.value = health;        // �������� ���� ü���� health ����
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
        if (health < 0) health = 0;         // ������ �԰� �׾��� �� ����ü���� -���� �Ǵ� ���� ����
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
        moveManager.SetActive(false);   // �÷��̾ ������ �������� ���ϵ���
    }
    
    public void PlayerHealthOverride(float hpLoadData)
    {
        health = hpLoadData;
        PlayerPrefs.SetFloat("NowHP", health);
        healthSlider.value = health;
        healthValue.text = health + " / " + maxHP;
    }
}
