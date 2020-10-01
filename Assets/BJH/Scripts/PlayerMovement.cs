using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Inspector Field
    public PLAYERSTATE state = PLAYERSTATE.FLYING;

    [Range(0, 100)]
    public float movementSpeed = 5f;

    [Range(1, 20)]
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

    KeyBindings keys;

    bool isRotating = true;

    public enum PLAYERSTATE
    {
        NONE,
        FLYING,
        LANDING,
        LANDED,
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



    private void Awake()
    {
        //Cursor.visible = false;

        InitValue();
    }

    void InitValue()
    {
        targetPos = targetTransform.position;
        currentPos = targetPos;

        targetCamRot = targetTransform.eulerAngles;
        currentCamRot = targetCamRot;

        targetTargetRot = targetTransform.eulerAngles;
        currentTargetRot = targetTargetRot;

    }

    private void Start()
    {
        keys = KeybindingManager.instance.keyBindings;
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
            state = PLAYERSTATE.FLYING;
            verticalInput = 1;
        }
        else if (Input.GetKey(keys.GetKeyCode(KeyBindings.KeyBindIndex.MoveDown))) verticalInput = -1;
        else verticalInput = 0;

        SetMovementInput(new Vector3(keys.GetAxisRaw("Horizontal"), keys.GetAxisRaw("Vertical"), verticalInput));
        SetRotationInput(new Vector2(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X")));
    }
    // Update is called once per frame
    void Update()
    {
        CheckIsRotating();

        //Check if landed
        if (landing.isLanded && state == PLAYERSTATE.FLYING) state = PLAYERSTATE.LANDING;

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
        camRootTransform.position = targetTransform.position;

    }

    void CheckIsRotating()
    {
        isRotating = true;
        if (BloodSuckingManager.instance.isSucking) isRotating = false;
    }


    void LandingMovement()
    {
        Vector3 rot = landing.landingNormal;


        targetTransform.rotation = Quaternion.LookRotation(rot, -targetTransform.forward);
        targetTransform.position = landing.landingPos;
        targetTransform.Rotate(new Vector3(90,0,0));

        targetPos = targetTransform.position;
        currentPos = targetPos;


        state = PLAYERSTATE.LANDED;
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
            state = PLAYERSTATE.FLYING;
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
        if (!isRotating) return;
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
            cameraTransform.position = hits[0].point + camRootTransform.forward * 0.1f;
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
}
