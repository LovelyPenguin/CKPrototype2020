using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIClickChecker : MonoBehaviour
{
    static UIClickChecker instance;

    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    bool isInteractingWithUI = false;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }
    private void Start()
    {
        raycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        pointerEventData = new PointerEventData(eventSystem);
        //Set the Pointer Event Position to that of the mouse position
        pointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        raycaster.Raycast(pointerEventData, results);

        isInteractingWithUI = results.Count > 0;

        // foreach (RaycastResult result in results)
    }

    

    public static bool GetIsInteractingWithUI()
    {
        return instance.isInteractingWithUI;
    }
}
