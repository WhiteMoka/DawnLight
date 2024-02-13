using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractCanvas : MonoBehaviour
{
    private static InteractCanvas instance;
    private void Awake()
    {
        #region ΩÃ±€≈Ê
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
}
