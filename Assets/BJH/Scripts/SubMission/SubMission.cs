using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="SubMission", menuName ="Scriptable/Missions")]
public class SubMission : ScriptableObject
{
    public enum MissionType
    {
        SuckTimes,
        MakeNoiseSec,
        //SuckPart,
        NoFailSuck
    }

    //Inspector
    public MissionType missionType;
    public float targetValue;
    [HideInInspector]public int bodyPartCode;
    //Inspector

    [HideInInspector]
    public float currentValue;
    [HideInInspector]
    public bool isCompleted = false;
    [HideInInspector]
    public bool isFinished = false;

    public static string GetMissionString(SubMission mission)
    {
        switch(mission.missionType)
        {
            case MissionType.MakeNoiseSec:
                {
                    return $"소음 유발 {mission.targetValue}초 이상 하기";
                }
            //case MissionType.SuckPart:
            //    {
            //        return $"부위 흡혈";
            //    }
            case MissionType.SuckTimes:
                {
                    return $"{mission.targetValue}번 이상 흡혈 하기";
                }
            case MissionType.NoFailSuck:
                {
                    return $"흡혈 실패하지 않기";
                }
        }

        return "";
    }
    public string GetMissionString()
    {
        return GetMissionString(this);
    }

    public void Debugging()
    {
        string str = "";

        str += "MissionType : " + missionType + "\n";
        str += "targetValue : " + targetValue + "\n";
        str += "currentValue : " + currentValue + "\n";
        str += "isCompleted : " + isCompleted + "\n";
        str += "isFinished : " + isFinished + "\n";

        Debug.Log(str);
    }


    public void Initialize()
    {

        switch (missionType)
        {
            case MissionType.NoFailSuck:
                isCompleted = true;
                isFinished = false;
                break;
            case MissionType.MakeNoiseSec:
                currentValue = 0;
                isCompleted = false;
                isFinished = false;
                break;
            //case MissionType.SuckPart:
            //    break;
            case MissionType.SuckTimes:
                currentValue = 0;
                isCompleted = false;
                isFinished = false;
                break;

        }
    }
}
