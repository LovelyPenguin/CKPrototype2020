using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMaster : MonoBehaviour
{
    private NavMeshAgent myAgent;
    private float setRandomMoveInterval = 0;
    private float setReactionSpeed = 0;
    private float timer = 0;

    [Header("Basic Setting")]
    public Transform player;
    public ParticleSystem particle;
    public Animator animator;
    public Vector3 lightSwitchLocation;
    public GameObject flashBang;

    [Header("Move Setting")]
    public float randomMoveInterval = 0;
    public float randomMoveRange;

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

    [Header("")]
    public float debugAngle;
    public float debugDistance;
    public float angryGauge;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        animator.SetFloat("angryGauge", angryGauge);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        debugAngle = CalculateAngle(player.position);
        debugDistance = GetPlayerDistance();

        animator.SetFloat("angryGauge", angryGauge);
    }

    private float CalculateAngle(Vector3 target)
    {
        Vector3 v = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z);
        float value = Mathf.Acos(
            Vector3.Dot(transform.forward, Vector3.Normalize(v)
            )) * Mathf.Rad2Deg;
        float originAngle = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
        return value;
    }

    private bool HideFromPlayer()
    {
        return true;
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
            myAgent.SetDestination(new Vector3(
                Random.Range(-randomMoveRange, randomMoveRange), transform.position.y, 
                Random.Range(-randomMoveRange, randomMoveRange)));
            setRandomMoveInterval = 0;
        }
    }

    public void Seek()
    {
        Debug.Log("Seek Player");
        SetAttack(false);
        myAgent.isStopped = false;
        myAgent.SetDestination(player.position);
        //transform.rotation = LookRotationTarget(player.position, 99);
    }

    public void Attack()
    {
        Debug.Log("Attack");
        myAgent.isStopped = true;
        setReactionSpeed += Time.deltaTime;

        transform.rotation = LookRotationTarget(player.position, rotationSpeed);
        if (setReactionSpeed <= reactionSpeed)
        {
            SetAttack(true);
        }
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

    public bool LightOnOff()
    {
        myAgent.SetDestination(lightSwitchLocation);
        if (!myAgent.pathPending)
        {
            if (myAgent.remainingDistance <= myAgent.stoppingDistance)
            {
                if (!myAgent.hasPath || myAgent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Light Off");
                    animator.SetTrigger("ArriveSwitch");
                    transform.rotation = LookRotationTarget(player.position, 5f);
                    timer += Time.deltaTime;

                    if (timer >= 5f && !animator.GetBool("isCompleteLightPhase"))
                    {
                        Debug.Log("Light On!");
                        flashBang.SetActive(true);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public float GetPlayerDistance()
    {
        Vector3 convertTransformPos = new Vector3(transform.position.x, 1, transform.position.z);
        Vector3 convertPlayerPos = new Vector3(player.position.x, 1, player.position.z);

        return Vector3.Distance(convertTransformPos, convertPlayerPos);
    }

    public float GetPlayerAngle()
    {
        return CalculateAngle(player.position);
    }

    public void IncreaseAngryGauge()
    {
        angryGauge += Time.deltaTime * 2;
        animator.SetFloat("angryGauge",angryGauge);
    }

    public void IncreaseAngryGauge(float increasePercent)
    {
        angryGauge += Time.deltaTime * increasePercent;
        animator.SetFloat("angryGauge", angryGauge);
    }

    public void AddAngryGauge(float value)
    {
        angryGauge += value;
        animator.SetFloat("angryGauge", angryGauge);
    }

    public Quaternion LookRotationTarget(Vector3 player, float rotationSpeed)
    {
        Vector3 targetPos = player - transform.position;
        return Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(new Vector3(targetPos.x, 0, targetPos.z)), Time.deltaTime * rotationSpeed);
    }

    public void ZeroIdle()
    {
        if (angryGauge < 50)
        {
            particle.Play();
        }
        else
        {
            particle.Stop();
        }
    }

    public void AIDied()
    {
        myAgent.speed = 0;
        myAgent.angularSpeed = 0;
        particle.gameObject.SetActive(false);
    }
}
