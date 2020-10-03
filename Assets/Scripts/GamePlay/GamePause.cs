using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public bool isGameStop;
    // Start is called before the first frame update
    void Start()
    {
        isGameStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameStop();
    }

    public void GameStop()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGameStop)
            {
                Time.timeScale = 0;
                isGameStop = true;
                return;
            }
            else
            {
                Time.timeScale = 1;
                isGameStop = false;
                return;
            }
        }
    }
}
