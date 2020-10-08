using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OverlaySelect : MonoBehaviour, IPointerEnterHandler
{
    public GameObject localSelectImage;
    // Start is called before the first frame update
    void Start()
    {
        GameSettings.instance.GetComponent<PauseManager>().ToggleSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        localSelectImage.transform.position = gameObject.transform.position;
    }
}
