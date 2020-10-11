using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSave : MonoBehaviour
{
    public string saveStageName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveStageData()
    {
        PlayerPrefs.SetInt(saveStageName, 1);
    }
}
