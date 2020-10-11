using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Inspector Field
    public PLAYERSTATE state = PLAYERSTATE.FLYING;

    [Range(0, 100)]
    public float movementSpeed = 5f;

    [Range(0, 20)]
    public float cameraDistance = 10f;


    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform camRootTransform;

    public LandingProcess landing;

    [Header("Smooth Time")]
    [Range(0,1)]
    [SerializeField] float camRotSmoothTime = 0.2f;
    [Range(0, 1)]
    [SerializeField] float targetRotSmoothTime = 0.2f;
    // Inspector Field

    Animator anim;
    KeyBindings keys;

    bool isRotating = true;

    public enum PLAYERSTATE
    {
        NONE,
        FLYING,
        LANDING,
        LANDED,
        KNOCKBACK,
        TAKEOFF,
        SUCK,
        CANNOTMOVE,
        DEAD
    }




    float verticalInput;

    Vector3 currentMoveInput;
    Vector3 targetPos;
    Vector3 currentPos;
    Vector3 currentPosVelocity;
    float moveSmoothTime = 0.01f;

    Vector3 currentCamPos;
    Vector3 targetCamPos;
    Vector3 currentCamPosVelocity;

    Vector3 currentCamRotInput;
    Vector3 targetCamRot;
    Vector3 currentCamRot;
    Vector3 currentCamRotVelocity;

    Vector3 targetTargetRot;
    Vector3 currentTargetRot;
    Vector3 currentTargetRotVelocity;


    #region UnityCallbacks
    private void Awake()
    {
        //Cursor.visible = false;

        InitValue();
    }
    private void Start()
    {
        keys = KeybindingManager.instance.keyBindings;
        anim = targetTransform.GetComponentInChildren<Animator>();

        SetAnimState(PLAYERSTATE.FLYING);

        //StartCoroutine(dieTest());
        //PopUpManager.instance.FlowText("Dead", 1f);
    }

    IEnumerator dieTest()
    {
        yield return null;
        Die();
    }

    void Update()
    {
        if(Input.GetKeyDown(keys.GetKeyCode(KeyBindings.KeyBindIndex.VeryNiceKey)))
        {
            KnockBackFromLand(5,1);
        }
        //CheckIsRotating();

        //Check if landed
        if (landing.isLanded && state == PLAYERSTATE.FLYING)
        {
            Debug.Log("Flying->Landing");
            SetAnimState(PLAYERSTATE.LANDING);
        }

        GetInput();

        switch (state)
        {
            case PLAYERSTATE.LANDING:
                LandingMovement();
                break;

            case PLAYERSTATE.LANDED:
                LandedMovement();
                break;
            case PLAYERSTATE.FLYING:
                FlyingMovement();
                break;
            case PLAYERSTATE.KNOCKBACK:
                KnockBackMovement();
                break;
            case PLAYERSTATE.TAKEOFF:
                TakeOffMovement();
                break;
        }
        if (state != PLAYERSTATE.DEAD)
            CamMovement();

    }

    private void FixedUpdate()
    {
        if(state == PLAYERSTATE.DEAD)
        {
            DeadMovement();
        }
    }


    #endregion

    void InitValue()
    {
        targetPos = targetTransform.position;
        currentPos = targetPos;

        targetCamRot = targetTransform.eulerAngles;
        currentCamRot = targetCamRot;

        targetTargetRot = targetTransform.eulerAngles;
        currentTargetRot = targetTargetRot;

    }

    public void SetMovementInput(Vector3 value)
    {
        currentMoveInput = value;
    }

    public void SetRotationInput(Vector2 value)
    {
        currentCamRotInput = value;
    }

    void GetInput()
    {
        if (KeybindingManager.instance.IsGettingInput()) return;
        if (Input.GetKey(keys.GetKeyCode(KeyBindings.KeyBindIndex.MoveUp)))
        {
            landing.isLanded = false;
            verticalInput = 1;
        }
        else if (Input.GetKey(keys.GetKeyCode(KeyBindings.KeyBindIndex.MoveDown))) verticalInput = -1;
        else verticalInput = 0;

        SetMovementInput(new Vector3(keys.GetAxisRaw("Horizontal"), keys.GetAxisRaw("Vertical"), verticalInput));
        SetRotationInput(new Vector2(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X")));
    }
    // Update is called once per frame

    public void Die()
    {
        SetAnimState(PLAYERSTATE.DEAD);
        //게임 결과 출력

        //중력받아 떨어지게
        Rigidbody rig = targetTransform.GetComponent<Rigidbody>();
        rig.useGravity = true;
        rig.constraints = RigidbodyConstraints.FreezeRotation;
        Time.timeScale = 0.3f;
    }


    #region Methods:KnockBack
    Vector3 knockBackVel;
    public void KnockBack(Vector3 vel, float time)
    {
        landing.isLanded = false;
        SetAnimState(PLAYERSTATE.KNOCKBACK);
        knockBackVel = vel;

        Debug.Log(vel);
        StartCoroutine(StopKnockBack(time));
    }
    public void KnockBackFromLand(float power, float time)
    {
        if (!landing.isLanded) return;
        KnockBack(targetTransform.up.normalized * power, time);
    }

    IEnumerator StopKnockBack(float time)
    {
        yield return new WaitForSeconds(time);

        if (knockBackVel != Vector3.zero)
        {
            SetAnimState(PLAYERSTATE.FLYING);
            landing.isLanded = false;
        }
    }
    #endregion

    #region Methods:State

    public void SetAnimState(PLAYERSTATE state)
    {
        this.state = state;

        anim.SetBool("Down", false);
        anim.SetBool("Suck", false);
        switch (state)
        {
            case PLAYERSTATE.DEAD:
                anim.SetTrigger("Dead");
                break;
            case PLAYERSTATE.FLYING:
                anim.SetTrigger("Idle");
                break;
            case PLAYERSTATE.SUCK:
                anim.SetBool("Suck", true);
                break;
            case PLAYERSTATE.LANDED:
                anim.SetBool("Down", true);
                break;
        }
    }
    void LandingMovement()
    {
        Vector3 rot = landing.landingNormal;


        targetTransform.rotation = Quaternion.LookRotation(rot, -targetTransform.forward);
        targetTransform.Rotate(new Vector3(90, 0, 0));


        targetTransform.position = landing.landingPos;


        CamRotation();

        targetTransform.SetParent(landing.landedTransform);
        SetAnimState(PLAYERSTATE.LANDED);


        targetPos = targetTransform.position;
        currentPos = targetPos;

    }
    void LandedMovement()
    {
        //Landed->Flying
        if(Input.GetKeyDown(keys.GetKeyCode(KeyBindings.KeyBindIndex.MoveUp)))
        {
            landing.isLanded = false;
            SetAnimState(PLAYERSTATE.TAKEOFF);

            targetPos = targetTransform.position;
            currentPos = targetPos;

            takeOffdir = landing.landingNormal;
            takeOffStartTime = Time.time;
        }

        CamRotation();
    }


    void FlyingMovement()
    {
        Movement();

        CamRotation();

        TargetRotation();

        if(targetTransform.parent)
        {
            targetTransform.SetParent(null);
            targetTransform.localScale = Vector3.one;
        }
    }

    void KnockBackMovement()
    {
        Ray ray = new Ray(targetTransform.position, knockBackVel.normalized);
        if(Physics.Raycast(ray, 1))
        {
            landing.isLanded = false;
            knockBackVel = Vector3.zero;
            SetAnimState(PLAYERSTATE.FLYING);
        }

        targetPos = targetTransform.position + knockBackVel * Time.deltaTime;

        currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref currentPosVelocity, moveSmoothTime);

        targetTransform.position = currentPos;

        CamRotation();
    }

    Vector3 takeOffdir;
    float takeOffStartTime;
    void TakeOffMovement()
    {
        if((Time.time - takeOffStartTime > 0.5f) || Input.GetKeyUp(keys.GetKeyCode(KeyBindings.KeyBindIndex.MoveUp)))
        {
            takeOffdir = Vector3.zero;
            SetAnimState(PLAYERSTATE.FLYING);
        }

        //Movement
        targetPos = targetTransform.position + (takeOffdir * movementSpeed * Time.deltaTime);
        currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref currentPosVelocity, moveSmoothTime);

        targetTransform.position = currentPos;

        CamRotation();
    }

    void DeadMovement()
    {
        cameraTransform.LookAt(targetTransform);

        if(landing.isLanded)
        {
            //중력받아 떨어지게
            Rigidbody rig = targetTransform.GetComponent<Rigidbody>();
            rig.useGravity = false;
            rig.constraints = RigidbodyConstraints.FreezeAll;

            targetTransform.position = landing.landingPos;

        }
    }
    #endregion

    #region Methods:Movement
    void Movement()
    {
        //Movement
        if (currentMoveInput.sqrMagnitude > 0f)
        {
            Vector3 moveValue = ((currentMoveInput.y * targetTransform.forward)
                + (currentMoveInput.x * targetTransform.right)
                + (verticalInput * targetTransform.up)).normalized;

            targetPos += moveValue * movementSpeed * Time.deltaTime;
        }

        currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref currentPosVelocity, moveSmoothTime);

        targetTransform.position = currentPos;
    }
    void CamMovement()
    {
        //Cam Movement
        targetCamPos = targetTransform.position;

        currentCamPos = Vector3.SmoothDamp(currentCamPos, targetCamPos, ref currentCamPosVelocity, moveSmoothTime);

        camRootTransform.position = currentCamPos;
    }

    void CamRotation()
    {
        //if (!isRotating) return;
        //Cam Rotation
        if (currentCamRotInput.sqrMagnitude > 0f)
        {
            targetCamRot += currentCamRotInput * Time.deltaTime * GameSettings.instance.sensitivity * 10;
        }

        targetCamRot.x = Mathf.Clamp(targetCamRot.x, -90, 90);
        currentCamRot = Vector3.SmoothDamp(currentCamRot, targetCamRot, ref currentCamRotVelocity, camRotSmoothTime);
        camRootTransform.rotation = Quaternion.Euler(currentCamRot);

        AvoidCamPenetration();
    }

    void AvoidCamPenetration()
    {
        //Avoid cam penetration
        RaycastHit[] hits = Physics.RaycastAll(camRootTransform.position, -camRootTransform.forward, cameraDistance);

        RaycastHit hit;
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                hit = hits[i];

                if (Vector3.Distance(hit.point, targetTransform.position) > 0.15f)
                {
                    cameraTransform.position = hit.point + camRootTransform.forward * 0.1f;
                    break;
                }
            }
        }
        else
        {
            cameraTransform.position = camRootTransform.position + (camRootTransform.forward * -cameraDistance);
        }
    }

    void TargetRotation()
    {
        //Target Rotation
        if (currentTargetRot != currentCamRot)
        {
            currentTargetRot = Vector3.SmoothDamp(currentTargetRot, currentCamRot, ref currentTargetRotVelocity, targetRotSmoothTime);
            targetTransform.rotation = Quaternion.Euler(currentTargetRot);
        }
    }
    #endregion
}
