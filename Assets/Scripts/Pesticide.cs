using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Pesticide : MonoBehaviour
{
    public Text hitText;
    public AIMaster ai;

    public int hitCount = 0;

    private PlayerDeath player;

    private void Awake()
    {
        if (ai == null)
        {
            ai = GameObject.FindGameObjectWithTag("Enemy").GetComponent<AIMaster>();
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeath>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            player.CallDamage();
        }
    }
}
