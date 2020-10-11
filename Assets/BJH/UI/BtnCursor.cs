using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerOver()
    {
        BtnManager.instance.PutCursor(transform);
    }
}
