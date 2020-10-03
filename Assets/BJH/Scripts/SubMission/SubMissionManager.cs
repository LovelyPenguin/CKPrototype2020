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

    private void Start()
    {
        InitializeMissions();
    }
    void InitializeMissions()
    {
        SubMission mission;
        for(int i = 0; i < missions.Count; i++)
        {
            mission = missions[i];
            mission = Instantiate(mission);

            switch(mission.missionType)
            {
                case SubMission.MissionType.NoFailSuck:
                    mission.isCompleted = true;
                    break;
            }
        }
    }
        


    public void OnSuck(SuckResult result)
    {
        SubMission mis;
        for(int i = 0; i < missions.Count; i++)
        {
            mis = missions[i];
            switch (mis.missionType)
            {
                case SubMission.MissionType.SuckTimes:
                    mis.currentValue += 1;
                    CheckSuckTimes(mis);
                    break;
                case SubMission.MissionType.SuckPart:
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
    public void OnFailedSuck()
    {
        SubMission mis;
        for (int i = 0; i < missions.Count; i++)
        {
            mis = missions[i];

            if(mis.missionType == SubMission.MissionType.NoFailSuck)
            {
                mis.isCompleted = false;
                mis.isFinished = true;
            }
        }
    }

    /// <summary>
    /// 서브 퀘스트 클리어 현황을 받아옵니다.
    /// </summary>
    public List<SubMission> GetResult()
    {
        CheckAllMissions();
        return missions;
    }

    #region Methods:CheckMissions
    void CheckAllMissions()
    {
        SubMission mission;
        for (int i = 0; i < missions.Count; i++)
        {
            mission = missions[i];

            if (mission.isFinished) continue;

            switch (mission.missionType)
            {
                case SubMission.MissionType.MakeNoiseSec:
                    {
                        CheckMakeNoiseSec(mission);
                    }
                    break;
                case SubMission.MissionType.SuckTimes:
                    {
                        CheckSuckTimes(mission);
                    }
                    break;
                case SubMission.MissionType.SuckPart:
                    {
                        CheckSuckPart(mission);
                    }
                    break;

                //잘못된 미션 삭제
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
