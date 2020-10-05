﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BloodSlider : MonoBehaviour
{
    //Inspector
    [SerializeField] float speed;
    public SuckResult suckResult;
    public UnityEvent suckEvent;

    [Tooltip("차례대로 EXCELLENT, GOOD, BAD의 범위 지정\n" +
        "타이밍 지점 기준 양 쪽으로 같은 값이 적용됩니다.")]
    [SerializeField] float[] stateRate;

    [Header("")]
    [SerializeField] Transform handle;
    [SerializeField] Transform timingTarget;
    //Inspector

    float timingRate;
    Slider slider;

    Vector2 minPos;
    Vector2 maxPos;

    public enum STATE
    {
        NONE,
        EXCELLENT,
        GOOD,
        BAD,
        FAILED

    }


    public void StartSucking(float timingRate, float speed = 50)
    {
        Debug.Log($"목표 : {timingRate}%");

        InitSlider();
        this.timingRate = timingRate;
        Vector2 pos = new Vector2(minPos.x + (maxPos.x - minPos.x)*timingRate/100, minPos.y);
        timingTarget.position = pos;

        suckResult = new SuckResult();
        suckResult.state = STATE.NONE;

        StartCoroutine(SuckingCo());
    }

    void InitSlider()
    {
        slider = GetComponent<Slider>();

        slider.value = 100;
        maxPos = handle.position;
        slider.value = 0;
        minPos = handle.position;
    }

    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            SuckNow();
        }
    }

    IEnumerator SuckingCo()
    {
        while(suckResult.state == STATE.NONE)
        {
            slider.value += speed * Time.deltaTime;

            if(slider.value == slider.maxValue)
            {
                suckResult.state = STATE.FAILED;
                yield break;
            }
            yield return null;
        }
    }

    public void SuckNow()
    {
        float result = Mathf.Abs(slider.value - timingRate);

        if (result < stateRate[0])
        {
            suckResult.state = STATE.EXCELLENT;
        }
        else if (result < stateRate[1])
        {
            suckResult.state = STATE.GOOD;
        }
        else if (result < stateRate[2])
        {
            suckResult.state = STATE.BAD;
        }
        else
        {
            suckResult.state = STATE.FAILED;
        }

        BloodSuckingManager.instance.QuitSucking();

        //흡혈 성공시
        if(suckResult.state != STATE.FAILED)
        {
            AIMaster ai = FindObjectOfType<AIMaster>();
            if(ai)
                ai.IncreaseAngryGauge(
                BloodSuckingManager.instance.GetSuckRangeRate(suckResult.state));
            SubMissionManager.instance.OnSuck(suckResult);
        }
        else
        {
            SubMissionManager.instance.OnFailedSuck();
        }
        suckEvent.Invoke();
        Debug.Log(suckResult.state);
    }
}

public class SuckResult
{
    public BloodSlider.STATE state;
    public int bodyPartCode;
}
