using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodSlider : MonoBehaviour
{
    //Inspector
    [SerializeField] float speed;
    public STATE state = STATE.NONE;

    [Tooltip("차례대로 EXCELLENT, GOOD, BAD의 범위 지정\n타이밍 지점 기준 양 쪽으로 같은 값이 적용됩니다.")]
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


    public void StartSucking(float timingRate)
    {
        Debug.Log($"목표 : {timingRate}%");

        InitSlider();
        this.timingRate = timingRate;
        Vector2 pos = new Vector2(minPos.x + (maxPos.x - minPos.x)*timingRate/100, minPos.y);
        timingTarget.position = pos;
        state = STATE.NONE;

        StartCoroutine(SuckingCo());
    }

    void InitSlider()
    {
        slider = GetComponent<Slider>();

        slider.value = 100;
        maxPos = handle.position;
        slider.value = 0;
        minPos = handle.position;

        Debug.Log($"{minPos}, {maxPos}");
    }

    IEnumerator SuckingCo()
    {
        while(state == STATE.NONE)
        {
            slider.value += speed * Time.deltaTime;

            if(slider.value == slider.maxValue)
            {
                state = STATE.FAILED;
                yield break;
            }
            yield return null;
        }
    }

    public void PressBtn()
    {
        float result = Mathf.Abs(slider.value - timingRate);

        if (result < stateRate[0])
        {
            state = STATE.EXCELLENT;
        }
        else if (result < stateRate[1])
        {
            state = STATE.GOOD;
        }
        else if (result < stateRate[2])
        {
            state = STATE.BAD;
        }
        else
        {
            state = STATE.FAILED;
        }

        Debug.Log(state);
    }
}
