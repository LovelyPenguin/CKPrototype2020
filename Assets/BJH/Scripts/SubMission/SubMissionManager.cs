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
        CheckMissions();
    }

    void CheckMissions()
    {
        SubMission mission;
        for (int i = 0; i < missions.Count; i++)
        {
            mission = missions[i];

            if (mission.isFinished) continue;

            switch (mission.missionKind)
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
        mission.argf += Time.deltaTime;
        if (mission.targetArg <= mission.argf)
        {
            mission.isCompleted = true;
            mission.isFinished = true;
        }
    }
    void CheckSuckTimes(SubMission mission)
    {
        //if(Suck())
        mission.argf += 1;

        if(mission.targetArg <= mission.argf)
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
}
