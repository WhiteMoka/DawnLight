using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isSwordEquip = false;
    public bool isUiOn = false;
    public string nextMapName;
    public List<EnemySkeleton> skeletonList = new List<EnemySkeleton>(); // ÇöÀç ¾À¿¡ ¼ÒÈ¯µÈ ½ºÄÌ·¹Åæ ¸®½ºÆ®
    
    private Camera mainCam;

    #region ½Ì±ÛÅæ
    private static GameManager instance = null;

    public static GameManager Instance
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
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    void Start()
    {
        mainCam = Camera.main;
    }
}
