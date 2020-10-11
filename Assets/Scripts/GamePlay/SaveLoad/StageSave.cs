using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSave : MonoBehaviour
{
    public string saveStageName;
    public List<SubMission> subMisson;

    // Start is called before the first frame update
    void Start()
    {
        subMisson = SubMissionManager.instance.GetMissions();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SaveStageData()
    {
        int count = 0;
        for (int i = 0; i < subMisson.Count; i++)
        {
            count++;
        }
        PlayerPrefs.SetInt(saveStageName, 1);

        if (PlayerPrefs.GetInt(saveStageName + "Star") <= count)
        {
            PlayerPrefs.SetInt(saveStageName + "Star", count + 1);
        }
    }
}
