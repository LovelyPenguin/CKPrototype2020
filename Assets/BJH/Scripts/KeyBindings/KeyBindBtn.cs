using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindBtn : MonoBehaviour
{
    public KeyBindings.KeyBindIndex keyIndex;
    [SerializeField] Text text;

    public void BindBtn()
    {
        KeybindingManager.instance.KeyBindBtn(keyIndex);
    }

    public void SetIndex(int i)
    {
        keyIndex = (KeyBindings.KeyBindIndex)i;

        text.text = keyIndex.ToString();
    }    
}
