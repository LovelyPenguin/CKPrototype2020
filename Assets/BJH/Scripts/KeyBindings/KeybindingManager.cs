
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindingManager : MonoBehaviour
{
    //Inspector
    [SerializeField] bool isGettingkey = false;

    [Header("")]
    [SerializeField] GameObject keyBindUI;
    [SerializeField] Transform parent;
    [SerializeField] KeyBindBtn btnPrefab;
    //Inspector

    public static KeybindingManager instance;

    public KeyBindings keyBindings = new KeyBindings();

    KeyBindings.KeyBindIndex curKeyIndex;

    List<KeyBindBtn> btns = new List<KeyBindBtn>();

    private void Awake()
    {
        if(!instance)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        keyBindings.InitKeys();
    }

    private void Update()
    {
        if(Input.GetKeyDown(keyBindings.GetKeyCode(KeyBindings.KeyBindIndex.ToggleSettings)))
        {
            keyBindUI.SetActive(!keyBindUI.activeSelf);

            if (keyBindUI.activeSelf && btns.Count != (int)KeyBindings.KeyBindIndex.None)
            {
                SpawnBtns();
            }
        }
    }

    private void OnGUI()
    {
        if(isGettingkey)
        {
            KeyCode value = GetInputKey();
            if(value != KeyCode.None)
            {
                keyBindings.SetKeyCode(curKeyIndex, value);

                curKeyIndex = KeyBindings.KeyBindIndex.None;
                isGettingkey = false;
            }
        }
    }

    #region Methods:KeyBindings
    public void KeyBindBtn(KeyBindings.KeyBindIndex index)
    {
        isGettingkey = true;
        curKeyIndex = index;
    }

    KeyCode GetInputKey()
    {
        if (Input.anyKey)
        {
            Event e = Event.current;
            if (e != null && e.isKey)
                return e.keyCode;
        }
        return KeyCode.None;
    }
    #endregion

    #region Methods:UI
    void SpawnBtns()
    {
        keyBindUI.SetActive(true);
        int keyCount = (int)KeyBindings.KeyBindIndex.None;

        KeyBindBtn btn;
        for (int i = 0; i < keyCount; i++)
        {
            btn = Instantiate(btnPrefab);

            btn.transform.SetParent(parent);
            btn.SetIndex(i);
            btn.transform.localPosition = Vector3.zero;
        }
    }

    public bool IsGettingInput()
    {
        return isGettingkey;
    }
    #endregion
}
