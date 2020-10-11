using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public int healthPoint = 10;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallDamage()
    {
        healthPoint--;
        if (healthPoint <= 0)
        {
            GameSettings.instance.GetComponent<GameEndCheck>().GameOverEvent();
        }
    }
}
