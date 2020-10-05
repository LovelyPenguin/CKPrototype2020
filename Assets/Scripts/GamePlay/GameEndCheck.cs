using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEndCheck : MonoBehaviour
{
    public UnityEvent gameClear;
    public UnityEvent gameOver;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameClearEvent()
    {
        Debug.Log("CLEAR");
        gameClear.Invoke();
    }

    public void GameOverEvent()
    {
        Debug.Log("FAILED");
        gameOver.Invoke();
    }
}
