using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeekPlayer : MonoBehaviour
{
    private NavMeshAgent myAgent;
    private float setRandomMoveInterval = 0;
    private float setReactionSpeed = 0;

    [Header("Basic Setting")]
    public Transform player;
    public ParticleSystem particle;
    
    [Header("Move Setting")]
    public float randomMoveInterval = 0;

    [Header("Seek Setting")]
    [Range(1, 360)]
    public float seekAngle;
    public float seekDistance;

    [Header("Attack Setting")]
    [Range(1, 360)]
    public float attackAngle;
    public float attackDistance;
    public float reactionSpeed;
    public float rotationSpeed;

    [Header("Rage Setting")]
    public float fireInterval;
    public float restInterval;

    public float debugValue;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        debugValue = (CalculateAngle(player.position));
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

    public void SetAttack(bool isAttack)
    {
        if (isAttack)
        {
            particle.Play();
        }
        else
        {
            particle.Stop();
            setReactionSpeed = 0;
        }
    }

    public void RandomMove()
    {
        myAgent.isStopped = false;
        setRandomMoveInterval += Time.deltaTime;

        if (randomMoveInterval <= setRandomMoveInterval)
        {
            myAgent.SetDestination(new Vector3(Random.Range(-10, 10), transform.position.y, Random.Range(-10, 10)));
            setRandomMoveInterval = 0;
        }
    }

    public void Seek()
    {
        Debug.Log("Seek Player");
        SetAttack(false);
        myAgent.isStopped = false;
        myAgent.SetDestination(player.position);
    }

    public void Attack()
    {
        Debug.Log("Attack");
        myAgent.isStopped = true;
        setReactionSpeed += Time.deltaTime;

        Vector3 targetPos = player.position - transform.position;
        transform.rotation = Quaternion.Lerp(
            transform.rotation, 
            Quaternion.LookRotation(new Vector3(targetPos.x, 0, targetPos.z)), Time.deltaTime * rotationSpeed);
        if (setReactionSpeed <= reactionSpeed)
        {
            SetAttack(true);
        }
    }

    public void Rage()
    {
        StartCoroutine(FireOrder());
        Debug.Log("Rage");
    }
    public void RageEnd()
    {
        StopCoroutine(FireOrder());
        StopCoroutine(RestOrder());
    }

    private IEnumerator FireOrder()
    {
        SetAttack(true);
        yield return new WaitForSeconds(fireInterval);
        StartCoroutine(RestOrder());
    }
    private IEnumerator RestOrder()
    {
        SetAttack(false);
        yield return new WaitForSeconds(restInterval);
        StartCoroutine(FireOrder());
    }

    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    public float GetPlayerAngle()
    {
        return CalculateAngle(player.position);
    }
}
