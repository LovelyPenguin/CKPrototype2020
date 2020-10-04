using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUISlot : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Image starImg;

    public void SetUI(SubMission mission)
    {
        SubMissionManager missionManager = SubMissionManager.instance;
        text.text = mission.GetMissionString();
        starImg.sprite = missionManager.GetStarSprite(mission.isCompleted);
    }
}
