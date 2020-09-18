using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodSuckingManager : MonoBehaviour
{
    [SerializeField] BloodSlider bsSlider;
    [SerializeField] PlayerMovement playerMove;

    private void Start()
    {

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
        CheckLandingOnSkin();   
    }

    void CheckLandingOnSkin()
    {
        if (playerMove.state != PlayerMovement.PLAYERSTATE.LANDED) return;

        //착지한 오브젝트가 피부일경우
        if (playerMove.landing.landingTransform.CompareTag("Skin"))
        {
            Debug.Log("Landed on Skin");
        }
    }
}
