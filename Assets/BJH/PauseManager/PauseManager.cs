using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class PauseManager : MonoBehaviour
{
    //Inspector
    [SerializeField] float fadingTime = 0.5f;

    public bool isPaused = false;

    [SerializeField] Image pauseUIBg;
    [SerializeField] Transform missionParent;

    [Header("")]
    [SerializeField] MissionUISlot missionPrefab;
    //Inspector

    List<MissionUISlot> slots = new List<MissionUISlot>();

    SubMissionManager missionManager;
    KeyBindings keys;
    bool isProcessing = false;


    #region UnityCallbacks
    private void Start()
    {
        missionManager = SubMissionManager.instance;
        keys = KeybindingManager.instance.keyBindings;
    }

    private void Update()
    {
        GetInput();
    }
    #endregion

    public void GetInput()
    {
        if (isProcessing) return;
        KeyCode toggleKey = keys.GetKeyCode(KeyBindings.KeyBindIndex.ToggleSettings);

        if(Input.GetKeyDown(toggleKey))
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        isProcessing = true;
        //일시정지 켜기
        if(!isPaused)
        {
            StartCoroutine(StartPausing());
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
            pauseUIBg.gameObject.SetActive(false);
            isProcessing = false;
        }
    }

    public void DisplayMissions()
    {
        while(slots.Count > 0)
        {
            Destroy(slots[0].gameObject);
            slots.RemoveAt(0);
        }

        MissionUISlot slot;
        List<SubMission> missions = missionManager.GetMissions();
        for(int i = 0; i < missions.Count; i++)
        {
            slot = Instantiate(missionPrefab);
            slot.transform.SetParent(missionParent);

            slot.SetUI(missions[i]);
            //missions[i].Debugging();
            slot.transform.localScale = Vector3.one;

            slots.Add(slot);
        }
    }

    IEnumerator StartPausing()
    {
        float time = 0f;
        float curvValue;
        Color bgColor = new Color(1, 1, 1, 0);
        pauseUIBg.gameObject.SetActive(true);
        DisplayMissions();


        while (!isPaused)
        {
            time += Time.deltaTime;
            curvValue = Mathf.Lerp(Time.timeScale, 0, time / fadingTime);


            if (curvValue < 0.01f)
            {
                curvValue = 0;
                isPaused = true;
            }
            Time.timeScale = curvValue;

            bgColor.a = 1f - curvValue;
            pauseUIBg.color = bgColor;

            yield return null;
        }

        isProcessing = false;
    }
    IEnumerator StopPausing()
    {
        float time = 0f;
        float curvValue;
        Color bgColor = new Color(1, 1, 1, 0);
        pauseUIBg.gameObject.SetActive(true);
        DisplayMissions();


        while (!isPaused)
        {
            time += Time.deltaTime;
            curvValue = Mathf.Lerp(Time.timeScale, 0, time / fadingTime);


            if (curvValue < 0.01f)
            {
                curvValue = 0;
                isPaused = true;
            }
            Time.timeScale = curvValue;

            bgColor.a = 1f - curvValue;
            pauseUIBg.color = bgColor;

            yield return null;
        }

        isProcessing = false;
    }


    #region Methods:Btns
    public void ResumeBtn()
    {
        if (isProcessing) return;

        ToggleSettings();
    }

    public void RestartLevelBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OptionsBtn()
    {

    }

    public void ExitBtn()
    {
        //메인메뉴 씬 이름 넣기
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}
