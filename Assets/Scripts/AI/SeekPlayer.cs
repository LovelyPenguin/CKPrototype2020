﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeekPlayer : MonoBehaviour
{
    private NavMeshAgent myAgent;
    public Transform player;
    [Range(1,180)]
    public float detectAngle;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private float CalculateAngle(Vector3 target)
    {
        Vector3 v = target - transform.position;
        float value = Mathf.Acos(
            Vector3.Dot(transform.forward, Vector3.Normalize(target - transform.position)
            )) * Mathf.Rad2Deg;
        float originAngle = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
        return value;
    }

    public float GetPlayerAngle()
    {
        return CalculateAngle(player.position);
    }

    public void RandomMove()
    {
        Debug.Log("Random Move");
        myAgent.SetDestination(Vector3.zero);
    }

    public void Seek()
    {
        Debug.Log("Seek Player");
        myAgent.SetDestination(player.position);
    }

    public void Attack()
    {
        Debug.Log("Attack");
    }
}
