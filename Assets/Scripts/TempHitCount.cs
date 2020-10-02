using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempHitCount : MonoBehaviour
{
    public Text hitText;
    public Text angryText;
    public AIMaster ai;

    public int hitCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angryText.text = "Angry : " + ai.angryGauge.ToString();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            hitCount++;
            hitText.text = "Hit : " + hitCount;
        }
    }
}
