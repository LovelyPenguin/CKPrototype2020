﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;


    [Range(0,100)]
    public float sensitivity;

    private void Awake()
    {
        Time.timeScale = 1f;
        if (!instance)
        {
            instance = this;

            //DontDestroyOnLoad(gameObject);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
