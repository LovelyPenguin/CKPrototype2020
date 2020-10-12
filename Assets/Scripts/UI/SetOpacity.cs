using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetOpacity : MonoBehaviour
{
    public List<Text> texts;
    public List<Image> images;

    public float alpha = 0f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].color = new Color(1, 1, 1, alpha);
        }

        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].color = new Color(1, 1, 1, alpha);
        }
    }
}
