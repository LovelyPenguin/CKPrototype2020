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
        SuckPart,
        NoFailSuck,
        OnlyNoiseToAngerState,
        SuckAtAngerState
    }

    //Inspector
    public MissionType missionType;
    public float targetValue;
    [Tooltip("부위 흡혈용 프로퍼티입니다.\n 그 외의 미션들은 사용하지 않아도 됩니다.")]
    public BodyPart.PART targetPart;
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
            case MissionType.SuckPart:
                {
                    return $"{BodyPart.GetPartString(mission.targetPart)} 흡혈하기";
                }
            case MissionType.SuckTimes:
                {
                    return $"{mission.targetValue}번 이상 흡혈 하기";
                }
            case MissionType.NoFailSuck:
                {
                    return $"흡혈 실패하지 않기";
                }
            case MissionType.OnlyNoiseToAngerState:
                {
                    return $"소음 유발만 이용해 싫증상태 만들기";
                }
            case MissionType.SuckAtAngerState:
                {
                    return $"분노 상태일 때 {mission.targetValue}번 흡혈";
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
