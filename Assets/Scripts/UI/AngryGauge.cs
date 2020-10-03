using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngryGauge : MonoBehaviour
{
    public List<Sprite> sprites;
    public Image img;
    public Slider slide;
    public AIMaster ai;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slide.value = ai.angryGauge;
        if (slide.value < 50)
        {
            img.sprite = sprites[0];
        }
        else if (slide.value >= 50 && slide.value < 100)
        {
            img.sprite = sprites[1];
        }
        else if (slide.value >= 100 && slide.value < 130)
        {
            img.sprite = sprites[2];
        }
        else if (slide.value >= 130)
        {
            img.sprite = sprites[3];
        }
    }
}
