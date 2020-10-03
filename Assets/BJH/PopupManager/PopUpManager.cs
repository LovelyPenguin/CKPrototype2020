using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager instance;

    GameSettings _gs;

    [SerializeField] PopUp noticePrefab;

    [SerializeField] FlowText flowTextPrefab;




    string str;

    public delegate void BtnFunc();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _gs = GameSettings.instance;
    }

    public void FlowText(string str, float t)
    {
        FlowText temp = Instantiate<FlowText>(flowTextPrefab);

        temp.text.text = str;


        temp.StartCoroutine(temp.StartTimer(t));
    }

    public static void FlowText(string str)
    {
        instance.FlowText(str, 1f);
    }

    public void PopUpNotice(string str, BtnFunc btnFunc = null)
    {
        PopUp temp = Instantiate<PopUp>(noticePrefab);

        temp.SetNoticeString(str, btnFunc);
    }
}
