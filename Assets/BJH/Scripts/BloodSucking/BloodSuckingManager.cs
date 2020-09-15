using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodSuckingManager : MonoBehaviour
{
    [SerializeField] BloodSlider bsSlider;

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
}
