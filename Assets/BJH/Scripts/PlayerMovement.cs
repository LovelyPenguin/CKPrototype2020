using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PLAYERSTATE
    {
        NONE,
        FLYING,
        LANDING,
        LANDED,
        CANNOTMOVE,
        DEAD
    }
    public PLAYERSTATE state = PLAYERSTATE.FLYING;


    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform camRootTransform;

    public LandingProcess landing;

    [Range(0, 10)]
    public float movementSpeed = 5f;

    [SerializeField] float verticalInput;

    Vector3 currentMoveInput;
    Vector3 targetPos;
    Vector3 currentPos;
    Vector3 currentPosVelocity;
    float moveSmoothTime;

    Vector3 currentCamRotInput;
    Vector3 targetCamRot;
    Vector3 currentCamRot;
    Vector3 currentCamRotVelocity;
    float camRotSmoothTime;

    Vector3 currentTargetRotInput;
    Vector3 targetTargetRot;
    Vector3 currentTargetRot;
    Vector3 currentTargetRotVelocity;
    float targetRotSmoothTime;



    private void Awake()
    {
        Cursor.visible = false;

        targetPos = targetTransform.position;
        currentPos = targetPos;

        targetCamRot = targetTransform.eulerAngles;
        currentCamRot = targetCamRot;

        targetTargetRot = targetTransform.eulerAngles;
        currentTargetRot = targetTargetRot;

        moveSmoothTime = 0.05f;
        camRotSmoothTime = 0.2f;
        targetRotSmoothTime = 0.2f;
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
        if (Input.GetKey(KeyCode.E))
        {
            landing.isLanded = false;
            state = PLAYERSTATE.FLYING;
            verticalInput = 1;
        }
        else if (Input.GetKey(KeyCode.Q)) verticalInput = -1;
        else verticalInput = 0;

        SetMovementInput(new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), verticalInput));
        SetRotationInput(new Vector2(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X")));
    }
    // Update is called once per frame
    void Update()
    {
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

    void LandingMovement()
    {
        Vector3 rot = landing.landingNormal;


        targetTransform.rotation = Quaternion.LookRotation(rot, -targetTransform.forward);
        targetTransform.position = landing.landingPos;


        state = PLAYERSTATE.LANDED;
    }
    void LandedMovement()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || Input.GetKeyDown(KeyCode.E))
        {
            landing.isLanded = false;
            state = PLAYERSTATE.FLYING;
            FlyingMovement();
        }

        //Cam Rotation
        if (currentCamRotInput.sqrMagnitude > 0f)
        {
            targetCamRot += currentCamRotInput * Time.deltaTime * GameSettings.instance.sensitivity;
        }

        currentCamRot = Vector3.SmoothDamp(currentCamRot, targetCamRot, ref currentCamRotVelocity, camRotSmoothTime);
        camRootTransform.rotation = Quaternion.Euler(currentCamRot);

    }


    void FlyingMovement()
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

        //Cam Rotation
        if (currentCamRotInput.sqrMagnitude > 0f)
        {
            targetCamRot += currentCamRotInput * Time.deltaTime * GameSettings.instance.sensitivity;
        }

        targetCamRot.x = Mathf.Clamp(targetCamRot.x, -90, 90);
        currentCamRot = Vector3.SmoothDamp(currentCamRot, targetCamRot, ref currentCamRotVelocity, camRotSmoothTime);
        camRootTransform.rotation = Quaternion.Euler(currentCamRot);

        RaycastHit[] hits = Physics.RaycastAll(camRootTransform.position, -camRootTransform.forward, 10f);
        if (hits.Length > 0)
        {
            //Debug.Log($"{hits[0].transform.gameObject.name}, pos : {hits[0].point}");
        }


        //Target Rotation

        if (currentTargetRot != currentCamRot)
        {
            currentTargetRot = Vector3.SmoothDamp(currentTargetRot, currentCamRot, ref currentTargetRotVelocity, targetRotSmoothTime);
            targetTransform.rotation = Quaternion.Euler(currentTargetRot);
        }

    }
}
