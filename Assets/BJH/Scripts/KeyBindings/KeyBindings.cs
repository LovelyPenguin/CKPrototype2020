using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindings
{
    List<KeyCode> keyCodes = new List<KeyCode>();

    public enum KeyBindIndex
    {

        //Movements
        MoveForward,
        MoveRight,
        MoveLeft,
        MoveBackward,

        //UpDown
        MoveDown,
        MoveUp,

        //Else
        ToggleSettings,

        None
    }



    public float GetAxisRaw(string axisName)
    {
        KeyCode nagative;
        KeyCode positive;

        switch (axisName)
        {
            case "Horizontal":
                nagative = GetKeyCode(KeyBindIndex.MoveLeft);
                positive = GetKeyCode(KeyBindIndex.MoveRight);
                break;
            case "Vertical":
                nagative = GetKeyCode(KeyBindIndex.MoveBackward);
                positive = GetKeyCode(KeyBindIndex.MoveForward);
                break;
            default:
                return 0;
        }

        float ret = 0;
        if (Input.GetKey(nagative)) ret -= 1;
        if (Input.GetKey(positive)) ret += 1;

        return ret;
    }

    public void InitKeys()
    {
        for(int i = 0; i < (int)KeyBindIndex.None; i++)
            keyCodes.Add(KeyCode.None);

        SetKeyCode(KeyBindIndex.MoveLeft, KeyCode.A);
        SetKeyCode(KeyBindIndex.MoveRight, KeyCode.D);
        SetKeyCode(KeyBindIndex.MoveUp, KeyCode.E);
        SetKeyCode(KeyBindIndex.MoveDown, KeyCode.Q);
        SetKeyCode(KeyBindIndex.MoveForward, KeyCode.W);
        SetKeyCode(KeyBindIndex.MoveBackward, KeyCode.S);
        SetKeyCode(KeyBindIndex.ToggleSettings, KeyCode.Escape);
    }

    public KeyCode GetKeyCode(KeyBindIndex index)
    {
        return keyCodes[(int)index];
    }
    public void SetKeyCode(KeyBindIndex index, KeyCode value)
    {
        keyCodes[(int)index] = value;
    }
}
