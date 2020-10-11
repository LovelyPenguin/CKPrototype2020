using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnOff : MonoBehaviour
{
    private PlayerMovement player;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.state == PlayerMovement.PLAYERSTATE.LANDED)
        {
            audio.mute = true;
        }
        else
        {
            audio.mute = false;
        }
    }
}
