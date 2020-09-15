using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarPart : MonoBehaviour
{
    public AIMaster parent;

    // Start is called before the first frame update
    void Start()
    {
        if (parent == null)
        {
            Debug.LogError("EarPart : 부모 객체가 설정되지 않았습니다.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
