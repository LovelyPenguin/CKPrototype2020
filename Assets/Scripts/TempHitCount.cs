using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TempHitCount : MonoBehaviour
{
    public Text hitText;
    public AIMaster ai;

    public int hitCount = 0;
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
            hitCount++;
            hitText.text = "Hit : " + hitCount;
            if (hitCount >= 10)
            {
                GameSettings.instance.GetComponent<PlayerDead>().playerDead.Invoke();
            }
        }
    }
}
