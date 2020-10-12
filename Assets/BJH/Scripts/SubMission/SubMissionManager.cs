using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMissionManager : MonoBehaviour
{
    public static SubMissionManager instance;

    [HideInInspector]
    public AIMaster ai;

    //Inspector
    [SerializeField] int stageNum = 0;

    [SerializeField] List<SubMission> missions;

    [SerializeField] Sprite clearedSpr;
    [SerializeField] Sprite notClearedSpr;
    //Inspector

    public Sprite GetStarSprite(bool isCleared)
    {
        return isCleared ? clearedSpr : notClearedSpr;
    }

    private void Awake()
    {
        if (!instance) instance = this;
    }

    private void Update()
    {
        SubMission mis;
        for(int i = 0; i < missions.Count; i++)
        {
            mis = missions[i];
            switch(mis.missionType)
            {
                case SubMission.MissionType.OnlyNoiseToAngerState:
                    CheckOnlyNoiseToAngerState(missions[i]);
                    break;
            }
        }
    }

    private void Start()
    {
        ai = FindObjectOfType<AIMaster>();
        LoadMissions(stageNum);
    }
    void InitializeMissions()
    {
        

        SubMission mission;
        for(int i = 0; i < missions.Count; i++)
        {
            mission = missions[i];
            mission = Instantiate(mission);

            mission.Initialize();

            missions[i] = mission;
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
                    if (mis.targetPart == result.bodyPart)
                    {
                        mis.currentValue += 1;
                        CheckSuckPart(mis);
                    }
                    break;
                case SubMission.MissionType.OnlyNoiseToAngerState:
                    {
                        if (mis.isFinished || mis.isCompleted) break;
                        mis.isFinished = true;
                        mis.isCompleted = false;
                    }
                    break;
                case SubMission.MissionType.SuckAtAngerState:
                    {
                        Debug.Log(mis.targetAngerState + ", " + ai.currentState);
                        if (mis.targetAngerState == ai.currentState)
                        {
                            mis.currentValue += 1;
                            CheckSuckAtAngerState(mis);
                        }
                    }
                    break;
                default:
                    break;
            }
            
        }
    }

    public void OnMakeNoise()
    {
        SubMission mis;
        for (int i = 0; i < missions.Count; i++)
        {
            mis = missions[i];

            switch (mis.missionType)
            {
                case SubMission.MissionType.MakeNoiseSec:
                    {
                        mis.currentValue += Time.deltaTime;
                        CheckMakeNoiseSec(mis);
                        break;
                    }
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
    
    public List<SubMission> GetMissions()
    {
        return missions;
    }
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
                case SubMission.MissionType.SuckAtAngerState:
                    {
                        CheckSuckAtAngerState(mission);
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
        if (mission.targetValue <= mission.currentValue)
        {
            mission.isCompleted = true;
            mission.isFinished = true;
        }
    }
    void CheckSuckAtAngerState(SubMission mission)
    {
        if (mission.targetValue <= mission.currentValue)
        {
            mission.isCompleted = true;
            mission.isFinished = true;
        }
    }
    void CheckOnlyNoiseToAngerState(SubMission mission)
    {
        if (mission.isFinished || mission.isCompleted) return;

        //미션 성공 조건 달성시
        if (ai.currentState == mission.targetAngerState)
        {
            mission.isCompleted = true;
            mission.isFinished = true;
        }
    }
    #endregion

    #region Methods:LoadMissions
    string missionsAddress = "SubMissions";
    public void LoadMissions(int stage)
    {
        string adr = missionsAddress + $"/Stage_{stage}";

        SubMission[] missions = Resources.LoadAll<SubMission>(adr);

        this.missions.Clear();

        for(int i = 0; i < missions.Length; i++)
        {
            this.missions.Add(missions[i]);
        }

        InitializeMissions();
    }

    #endregion

    public static string GetAngerStateString(AIMaster.AIState state)
    {
        switch(state)
        {
            case AIMaster.AIState.NORMAL:
                return "평온";
            case AIMaster.AIState.ANOYING:
                return "싫증";
            case AIMaster.AIState.ANGRY:
                return "짜증";
            case AIMaster.AIState.RAGE:
                return "분노";
        }
        return "";
    }
}
