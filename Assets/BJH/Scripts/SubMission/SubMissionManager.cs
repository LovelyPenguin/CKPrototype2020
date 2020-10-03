using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMissionManager : MonoBehaviour
{
    public static SubMissionManager instance;

    public List<SubMission> missions;

    private void Awake()
    {
        if (!instance) instance = this;
    }

    private void Update()
    {

    }

    public void OnSuck(SuckResult result)
    {
        SubMission mis;
        for(int i = 0; i < missions.Count; i++)
        {
            mis = missions[i];
            switch (mis.missionType)
            {
                case SubMission.Kind.SuckTimes:
                    mis.currentValue += 1;
                    CheckSuckTimes(mis);
                    break;
                case SubMission.Kind.SuckPart:
                    if(mis.bodyPartCode == result.bodyPartCode)
                    {
                        mis.currentValue += 1;
                        CheckSuckPart(mis);
                    }
                    break;
                default:
                    break;
            }
            
        }
    }

    #region Methods:CheckMissions
    void CheckMissions()
    {
        SubMission mission;
        for (int i = 0; i < missions.Count; i++)
        {
            mission = missions[i];

            if (mission.isFinished) continue;

            switch (mission.missionType)
            {
                case SubMission.Kind.MakeNoiseSec:
                    {
                        CheckMakeNoiseSec(mission);
                    }
                    break;
                case SubMission.Kind.SuckTimes:
                    {
                        CheckSuckTimes(mission);
                    }
                    break;
                case SubMission.Kind.SuckPart:
                    {
                        CheckSuckPart(mission);
                    }
                    break;

                default:
                    {
                        missions.Remove(mission);
                    }
                    break;

            }
        }
    }

    void CheckMakeNoiseSec(SubMission mission)
    {
        //if(isMakingNose)
        mission.currentValue += Time.deltaTime;
        if (mission.targetValue <= mission.currentValue)
        {
            mission.isCompleted = true;
            mission.isFinished = true;
        }
    }
    void CheckSuckTimes(SubMission mission)
    {
        //if(Suck())
        mission.currentValue += 1;

        if(mission.targetValue <= mission.currentValue)
        {
            mission.isCompleted = true;
            mission.isFinished = true;
        }
    }
    void CheckSuckPart(SubMission mission)
    {
        //if(sucked.part == mission.part)
        //{
        //    mission.isCompleted = true;
        //    mission.isFinished = true;
        //}
    }
    #endregion
}
