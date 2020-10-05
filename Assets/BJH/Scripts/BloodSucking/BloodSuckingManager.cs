﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BloodSuckingManager : MonoBehaviour
{
    [SerializeField] GameObject bloodSuckUI;
    [SerializeField] BloodSlider bsSlider;
    [SerializeField] PlayerMovement playerMove;

    [Header("")]
    [SerializeField] float minRange;
    [SerializeField] float maxRange;

    [Header("")]
    public float[] suckRageRate;

    public static BloodSuckingManager instance;


    [HideInInspector] public bool isSucking;


    #region UnityCallbacks
    private void Awake()
    {
        if (!instance) instance = this;
    }

    private void Start()
    {
    }

    private void Update()
    {
        GetInput();
    }
    #endregion

    public void SuckBtn()
    {
        bsSlider.SuckNow();
    }
    public void StartSuckingBtn()
    {
        int ran = Random.Range((int)minRange, (int)maxRange);

        bsSlider.StartSucking(ran);
    }


    bool CheckLandingOnSkin()
    {
        if (playerMove.state != PlayerMovement.PLAYERSTATE.LANDED)
            return false;

        //착지한 오브젝트가 피부일경우
        if (playerMove.landing.landedTransform.CompareTag("Skin"))
        {
            return true;
        }

        return false;
    }

    void GetInput()
    {
        if (playerMove.state == PlayerMovement.PLAYERSTATE.FLYING)
        {
            QuitSucking();
        }

        if(Input.GetMouseButton(0) && !UIClickChecker.GetIsInteractingWithUI())
        {
            if(CheckLandingOnSkin())
            {
                StartSucking();
            }
        }
    }

    public void StartSucking()
    {
        if (isSucking) return;

        bloodSuckUI.SetActive(true);
        isSucking = true;
        playerMove.SetAnimState(PlayerMovement.PLAYERSTATE.SUCK);

        int ran = Random.Range((int)minRange, (int)maxRange);

        bsSlider.StartSucking(ran);
    }
    public void QuitSucking()
    {
        if (!isSucking) return;
        isSucking = false;
        playerMove.SetAnimState(PlayerMovement.PLAYERSTATE.LANDED);
        bloodSuckUI.SetActive(false);
    }

    public float GetSuckRangeRate(BloodSlider.STATE state)
    {
        //None이 0.
        //1 깎아야함
        int a = (int)state - 1;
        return suckRageRate[a];
    }
}
