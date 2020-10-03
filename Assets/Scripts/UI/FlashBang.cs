using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashBang : MonoBehaviour
{
    public Image image;
    public float steadyTime = 1f;

    private bool start = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        start = false;
        image.color = new Color(1, 1, 1, 1);
        StartCoroutine(StartPhase());
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            float alpha = image.color.a;
            alpha -= Time.deltaTime * 0.2f;
            image.color = new Color(1, 1, 1, alpha);

            if (alpha <= 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator StartPhase()
    {
        yield return new WaitForSeconds(steadyTime);
        start = true;
    }
}
