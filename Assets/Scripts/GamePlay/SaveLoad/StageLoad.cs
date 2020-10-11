using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoad : MonoBehaviour
{
    public string loadStageName;
    public GameObject clearLine;

    // Start is called before the first frame update
    void Start()
    {
        LoadStage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadStage()
    {
        if (PlayerPrefs.GetInt(loadStageName) == 1)
        {
            Debug.Log("Load");
            clearLine.SetActive(true);
        }
    }
}
