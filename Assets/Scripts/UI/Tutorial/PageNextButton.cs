using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PageAndPageIndex
{
    public GameObject page;
    public Image pageIndex;
}
public class PageNextButton : MonoBehaviour
{
    public List<PageAndPageIndex> tutorialPages;
    public GameObject indexSelector;
    public Text buttonText;
    public int currentPageIndex = 0;
    public TutorialManager tutorialMng;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        tutorialPages[0].page.SetActive(true);
        indexSelector.transform.position = tutorialPages[0].pageIndex.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        if (currentPageIndex < tutorialPages.Count - 1)
        {
            tutorialPages[currentPageIndex].page.SetActive(false);

            currentPageIndex++;
            if (currentPageIndex >= (tutorialPages.Count - 1))
            {
                buttonText.text = "PLAY";
            }

            tutorialPages[currentPageIndex].page.SetActive(true);
            indexSelector.transform.position = tutorialPages[currentPageIndex].pageIndex.transform.position;
        }
        else
        {
            Phase2Start();
        }
    }

    public void Phase2Start()
    {
        Debug.Log("Phase2");
        Time.timeScale = 1f;
        tutorialMng.Phase2();
    }
}
