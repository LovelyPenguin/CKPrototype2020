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

    Vector3 currentCamRotInput;
    Vector3 targetCamRot;
    Vector3 currentCamRot;
    Vector3 currentCamRotVelocity;

    Vector3 currentTargetRotInput;
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
        PopUpManager.instance.FlowText("Dead", 1f);
    }

    IEnumerator dieTest()
    {
        yield return null;
        Die();
    }

    void Update()
    {
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
        }
        if (state != PLAYERSTATE.DEAD) 
            camRootTransform.position = targetTransform.position;

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
            SetAnimState(PLAYERSTATE.FLYING);
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

    void CheckIsRotating()
    {
        isRotating = true;
        if (BloodSuckingManager.instance.isSucking) isRotating = false;
    }


    #region Methods:State

    public void SetAnimState(PLAYERSTATE state)
    {
        this.state = state;

        anim.SetBool("Down", false);
        switch (state)
        {
            case PLAYERSTATE.DEAD:
                anim.SetTrigger("Dead");
                break;
            case PLAYERSTATE.FLYING:
                anim.SetTrigger("Idle");
                break;
            case PLAYERSTATE.SUCK:
                anim.SetTrigger("Suck");
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



        targetTransform.position = currentPos;


        CamRotation();

        if (Vector3.SqrMagnitude(targetTransform.position - targetPos) < 0.001)
        {
            SetAnimState(PLAYERSTATE.LANDED);
            Debug.Log("Landing -> landed");

            targetPos = targetTransform.position;
            currentPos = targetPos;

        }
    }
    void LandedMovement()
    {
        if(Input.GetKeyDown(keys.GetKeyCode(KeyBindings.KeyBindIndex.MoveForward)) ||
            Input.GetKeyDown(keys.GetKeyCode(KeyBindings.KeyBindIndex.MoveBackward)) ||
            Input.GetKeyDown(keys.GetKeyCode(KeyBindings.KeyBindIndex.MoveLeft)) ||
            Input.GetKeyDown(keys.GetKeyCode(KeyBindings.KeyBindIndex.MoveRight)) ||
            Input.GetKeyDown(keys.GetKeyCode(KeyBindings.KeyBindIndex.MoveUp)))
        {
            landing.isLanded = false;
            SetAnimState(PLAYERSTATE.FLYING);
            FlyingMovement();
        }

        CamRotation();
    }


    void FlyingMovement()
    {
        Movement();

        CamRotation();

        TargetRotation();
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

        //Avoid cam penetration
        RaycastHit[] hits = Physics.RaycastAll(camRootTransform.position, -camRootTransform.forward, cameraDistance);
        if (hits.Length > 0)
        {
            for(int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform != targetTransform)
                {
                    cameraTransform.position = hits[i].point + camRootTransform.forward * 0.1f;
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
