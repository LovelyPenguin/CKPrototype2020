using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public enum State
    {
        PHASE1 = 0,
        PHASE2,
        PHASE3,
        PHASE4,
        PHASE5,
        PHASE6
    }

    private State phase = State.PHASE1;
    public GameObject movementTutorial;
    public Text messageBox;
    public Camera cam;
    public LandingProcess player;
    public AIMaster enemy;
    private Transform enemyPos;
    private Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TutorialStart());
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<LandingProcess>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<AIMaster>();
        enemyPos = enemy.GetComponent<Transform>();
        playerPos = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(phase)
        {
            case State.PHASE1:
                break;
            case State.PHASE2:
                if (Vector3.Distance(playerPos.position, enemyPos.position) <= 2f)
                {
                    Phase3();
                }
                break;
            case State.PHASE3:
                if (enemy.angryGauge >= 40)
                {
                    Phase4();
                }
                break;
            case State.PHASE4:
                if (player.isLanded)
                {
                    Phase5();
                }
                break;
            case State.PHASE5:
                if (enemy.angryGauge >= 100)
                {
                    Phase6();
                }
                break;
        }
    }

    IEnumerator TutorialStart()
    {
        yield return new WaitForSeconds(1f);
        Phase1();
    }

    public void Phase1()
    {
        movementTutorial.SetActive(true);
    }
    public void Phase2()
    {
        movementTutorial.SetActive(false);
        messageBox.text = "인간에게 다가가기";
        phase = State.PHASE2;
    }

    public void Phase3()
    {
        messageBox.text = "귀 쪽으로 다가가 짜증나게 하기";
        phase = State.PHASE3;
    }

    public void Phase4()
    {
        messageBox.text = "피부에 달라 붙기";
        phase = State.PHASE4;
    }
    public void Phase5()
    {
        messageBox.text = "흡혈하기";
        phase = State.PHASE5;
    }
    public void Phase6()
    {
        messageBox.text = "인간 괴롭히기";
        phase = State.PHASE6;
    }
}
