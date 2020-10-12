using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    public GameObject obj;
    public float timer = 0;
    public bool trigger = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            timer += Time.deltaTime;
            if (timer >= 5f)
            {
                obj.SetActive(true);
                trigger = false;
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Enter");
            trigger = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Exit");
            obj.SetActive(false);
            trigger = false;
            timer = 0;
        }
    }
}
