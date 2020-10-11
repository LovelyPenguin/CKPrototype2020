using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageLoad : MonoBehaviour
{
    public string loadStageName;
    public GameObject clearLine;
    public List<Image> starImage;
    public Sprite clearStarImage;

    // Start is called before the first frame update
    void Start()
    {
        LoadStage();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadStage()
    {
        if (PlayerPrefs.GetInt(loadStageName) == 1)
        {
            clearLine.SetActive(true);

            if (starImage.Count != 0)
            {
                for (int i = 0; i < starImage.Count; i++)
                {
                    starImage[i].color = new Color(1, 1, 1, 1);
                }
            }
        }
        else
        {
            if (starImage.Count != 0)
            {
                for (int i = 0; i < starImage.Count; i++)
                {
                    starImage[i].color = new Color(1, 1, 1, 0);
                }
            }
        }

        Debug.Log(loadStageName + " : " + PlayerPrefs.GetInt(loadStageName + "Star"));

        if (starImage.Count != 0)
        {
            for (int i = 0; i < PlayerPrefs.GetInt(loadStageName + "Star"); i++)
            {
                starImage[i].sprite = clearStarImage;
            }
        }
    }
}
