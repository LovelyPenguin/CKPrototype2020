using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;

        pos.x += hori * Time.deltaTime * 5;
        pos.z += vert * Time.deltaTime * 5;

        transform.position = pos;
    }
}
