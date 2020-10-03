using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowText : MonoBehaviour
{
    public Text text;
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();

        transform.SetParent(GameObject.Find("Canvas").transform);
        transform.localScale = Vector3.one;
        transform.position = new Vector3(Screen.width / 2, Screen.height / 6*5);
    }

    public IEnumerator StartTimer(float t)
    {
        yield return new WaitForSeconds(t);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while(image.color.a > 0)
        {
            Color col = image.color;
            col.a-= 0.05f;
            image.color = col;
            text.color = col;

            yield return null;
        }

        Destroy(gameObject);
    }
}
