using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [Header("Intro")]
    public GameObject seaWave;
    private AudioSource seaWaveAudio;
    public GameObject windHouling;
    private AudioSource windHoulingAudio;
    public GameObject holyStart;
    private AudioSource holyStartAudio;

    [Header("Main1")]
    public GameObject thunder;
    private AudioSource thunderAudio;
    public GameObject talkSound;
    private AudioSource talkSoundAudio;
    public GameObject boxOpen;
    private AudioSource boxOpenAudio;
    public GameObject itemPickUp;
    private AudioSource itemPickUpAudio;
    
    [Header("Public")]
    public GameObject maleJump;
    private AudioSource maleJumpAudio;
    public GameObject buttonHover;
    private AudioSource buttonHoverAudio;
    public GameObject swordChange;
    private AudioSource swordChangeAudio;
    public GameObject swordAttack;
    private AudioSource swordAttackAudio;
    public GameObject eatSound;
    private AudioSource eatSoundAudio;
    public GameObject skeletonRoar;
    private AudioSource skeletonRoarAudio;
    public GameObject skeletonAttack;
    private AudioSource skeletonAttackAudio;
    public GameObject skeletonDeath;
    private AudioSource skeletonDeathAudio;
    public GameObject manDeath;
    private AudioSource manDeathAudio;
    public GameObject soldierComing;
    private AudioSource soldierComingAudio;

    #region ΩÃ±€≈Ê
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // Intro
        seaWaveAudio = seaWave.GetComponent<AudioSource>();
        SeaWaveAudioPlay();
        windHoulingAudio = windHouling.GetComponent<AudioSource>();
        holyStartAudio = holyStart.GetComponent<AudioSource>();

        // Main1
        thunderAudio = thunder.GetComponent<AudioSource>();
        ThunderAudioPlay();
        talkSoundAudio = talkSound.GetComponent<AudioSource>();
        boxOpenAudio = boxOpen.GetComponent<AudioSource>();
        itemPickUpAudio = itemPickUp.GetComponent<AudioSource>();
        
        // Public
        maleJumpAudio = maleJump.GetComponent<AudioSource>();
        buttonHoverAudio = buttonHover.GetComponent<AudioSource>();
        swordChangeAudio = swordChange.GetComponent<AudioSource>();
        swordAttackAudio = swordAttack.GetComponent<AudioSource>();
        eatSoundAudio = eatSound.GetComponent<AudioSource>();
        skeletonRoarAudio = skeletonRoar.GetComponent<AudioSource>();
        skeletonAttackAudio = skeletonAttack.GetComponent<AudioSource>();
        skeletonDeathAudio = skeletonDeath.GetComponent<AudioSource>();
        manDeathAudio = manDeath.GetComponent<AudioSource>();
        soldierComingAudio = soldierComing.GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Intro
    public void SeaWaveAudioPlay() // Intro BGM
    {
        if (SceneManager.GetActiveScene().name == "Intro") // ¿Œ∆Æ∑Œ æ¿¿Ã∂Û∏È
        {
            seaWaveAudio.Play();
        }
        else // ¿Œ∆Æ∑Œ æ¿¿Ã æ∆¥œ∂Û∏È
        {
            seaWaveAudio.Stop();
        }
    }
    public void WindHoulingAudioPlay()
    {
        windHoulingAudio.PlayOneShot(windHoulingAudio.clip);
    }
    public void HolyStartAudioPlay()
    {
        holyStartAudio.PlayOneShot(holyStartAudio.clip);
    }

    // Main 1
    public void ThunderAudioPlay() // Main1 BGM
    {
        if (SceneManager.GetActiveScene().name == "Main1") // Main 1 æ¿¿Ã∂Û∏È
        {
            thunderAudio.Play();
        }
        else
        {
            thunderAudio.Stop();
        }
    }
    public void TalkSoundAudioPlay()
    {
        talkSoundAudio.PlayOneShot(talkSoundAudio.clip);
    }
    public void BoxOpenAudioPlay()
    {
        boxOpenAudio.PlayOneShot(boxOpenAudio.clip);
    }
    public void ItemPickUpAudioPlay()
    {
        itemPickUpAudio.PlayOneShot(itemPickUpAudio.clip);
    }
    public void SkeletonRoarAudioPlay()
    {
        skeletonRoarAudio.PlayOneShot(skeletonRoarAudio.clip);
    }
    public void SkeletonAttackAudioPlay()
    {
        skeletonAttackAudio.PlayOneShot(skeletonAttackAudio.clip);
    }

    // Public
    public void MaleJumpAudioPlay()
    {
        maleJumpAudio.PlayOneShot(maleJumpAudio.clip);
    }
    public void BtnHoverAudioPlay()
    {
        buttonHoverAudio.PlayOneShot(buttonHoverAudio.clip);
    }
    public void SwordChangeAudioPlay()
    {
        swordChangeAudio.PlayOneShot(swordChangeAudio.clip);
    }
    public void SwordAttackAudioPlay()
    {
        swordAttackAudio.PlayOneShot(swordAttackAudio.clip);
    }
    public void EatSoundAudioPlay()
    {
        eatSoundAudio.PlayOneShot(eatSoundAudio.clip);
    }
    public void SkeletonDeathAudioPlay()
    {
        skeletonDeathAudio.PlayOneShot(skeletonDeathAudio.clip);
    }
    public void ManDeathAudioPlay()
    {
        manDeathAudio.PlayOneShot(manDeathAudio.clip);
    }
    public void SoldierComingAudioPlay()
    {
        soldierComingAudio.PlayOneShot(soldierComingAudio.clip);
    }
}
