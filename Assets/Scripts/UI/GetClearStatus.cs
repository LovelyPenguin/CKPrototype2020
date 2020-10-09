using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClearStatus : MonoBehaviour
{
    public SubMissionManager subMissons;
    private List<SubMission> clear;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        SubMission misson;
        int clearCount = 0;
        clear = subMissons.GetResult();
        for (int i = 0; i < clear.Count; i++)
        {
            misson = clear[i];
            if (misson.isCompleted)
            {
                clearCount++;
            }
        }
        Debug.Log(clearCount);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
