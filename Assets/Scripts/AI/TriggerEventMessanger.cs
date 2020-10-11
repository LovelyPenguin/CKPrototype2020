using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventMessanger : MonoBehaviour
{
    public string targetTag;
    public UnityEvent triggerEnter;
    public UnityEvent triggerStay;
    public UnityEvent triggerExit;

    public bool useLimitIncreaseAngry = true;
    public float limitAngry = 50f;

    public AIMaster ai;

    private void Start()
    {
        ai = GetComponentInParent<AIMaster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            triggerEnter.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (useLimitIncreaseAngry && ai.angryGauge < limitAngry)
            {
                triggerStay.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            triggerExit.Invoke();
        }
    }
}
