using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField] Text noticeTxt;
    [SerializeField] Button okBtn;
    public void SetNoticeString(string str, PopUpManager.BtnFunc btnFunc = null)
    {
        noticeTxt.text = str;

        transform.SetParent(GameObject.Find("Canvas").transform);
        transform.localScale = Vector3.one;
        transform.position = new Vector3(Screen.width / 2, Screen.height / 2);


        if (btnFunc == null) return;

        okBtn.onClick.AddListener(() => { btnFunc(); });
    }
    public void OKBtn()
    {
        Destroy(gameObject);
    }
}
