using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject movementTutorial;
    public Text messageBox;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TutorialStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TutorialStart()
    {
        yield return new WaitForSeconds(2f);
        Phase1();
    }

    public void Phase1()
    {
        movementTutorial.SetActive(true);
    }
    public void Phase2()
    {
        movementTutorial.SetActive(false);
        messageBox.text = "인간을 찾기";
    }

    public void Phase3()
    {
        messageBox.text = "";
    }
}
