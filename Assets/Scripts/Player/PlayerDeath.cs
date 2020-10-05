using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private PlayerMovement player;
    public int healthPoint = 10;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
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
            player.Die();
            GameSettings.instance.GetComponent<GameEndCheck>().GameOverEvent();
        }
    }
}
