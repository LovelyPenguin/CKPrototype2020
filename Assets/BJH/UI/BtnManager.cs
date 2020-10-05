using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnManager : MonoBehaviour
{
    public static BtnManager instance;

    public Transform cursorUI;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    public void PutCursor(Transform tr)
    {
        cursorUI.SetParent(tr);
        cursorUI.position = tr.position;
    }
}
