using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="SubMission", menuName ="Scriptable/Missions")]
public class SubMission : ScriptableObject
{
    public enum Kind
    {
        SuckTimes,
        MakeNoiseSec,
        SuckPart
    }

    //Inspector
    public Kind missionType;
    public float targetValue;
    public int bodyPartCode;
    //Inspector

    [HideInInspector]
    public float currentValue;
    [HideInInspector]
    public bool isCompleted = false;
    [HideInInspector]
    public bool isFinished = false;
}
