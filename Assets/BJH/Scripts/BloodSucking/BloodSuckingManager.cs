using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodSuckingManager : MonoBehaviour
{
    [SerializeField] GameObject bloodSuckUI;
    [SerializeField] BloodSlider bsSlider;
    [SerializeField] PlayerMovement playerMove;

    public static BloodSuckingManager instance;

    [HideInInspector] public bool isSucking;

    private void Awake()
    {
        if (!instance) instance = this;
    }

    public void SuckBtn()
    {
        bsSlider.PressBtn();
    }
    public void StartSuckingBtn()
    {
        int ran = Random.Range(30, 90);

        bsSlider.StartSucking(ran);
    }

    private void Update()
    {
        GetInput();
    }

    bool CheckLandingOnSkin()
    {
        if (playerMove.state != PlayerMovement.PLAYERSTATE.LANDED)
            return false;

        //착지한 오브젝트가 피부일경우
        if (playerMove.landing.landedTransform.CompareTag("Skin"))
        {
            return true;
        }

        return false;
    }

    void GetInput()
    {
        if (playerMove.state == PlayerMovement.PLAYERSTATE.FLYING)
        {
            QuitSucking();
        }

        if(Input.GetMouseButton(0))
        {
            if(CheckLandingOnSkin())
            {
                StartSucking();
            }
        }
    }

    public void StartSucking()
    {
        if (isSucking) return;

        bloodSuckUI.SetActive(true);
        isSucking = true;
        playerMove.SetAnimState(PlayerMovement.PLAYERSTATE.SUCK);

        int ran = Random.Range(30, 90);

        bsSlider.StartSucking(ran);
    }
    public void QuitSucking()
    {
        if (!isSucking) return;
        isSucking = false;
        playerMove.SetAnimState(PlayerMovement.PLAYERSTATE.LANDED);
        bloodSuckUI.SetActive(false);
    }
}
