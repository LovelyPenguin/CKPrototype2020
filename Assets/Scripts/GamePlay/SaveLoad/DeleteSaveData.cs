using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeleteSaveData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text text;
    public int clickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && text.text == "NO?")
        {
            text.text = "DELETE ALL SAVE DATA";
            clickCount = 0;
        }
    }

    public void OnClick()
    {
        clickCount++;
        if (text.text == "OK?")
        {
            Debug.Log("Delete All data");
            PlayerPrefs.DeleteAll();
            gameObject.GetComponent<MoveScene>().OnClick();
        }
        text.text = "OK?";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (clickCount != 0)
        {
            text.text = "OK?";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (clickCount != 0)
        {
            text.text = "NO?";
        }
    }
}
