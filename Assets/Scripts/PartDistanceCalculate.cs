using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDistanceCalculate : MonoBehaviour
{
    [Range(1, 100)]
    public float slide;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.up;
        if (Input.GetKey(KeyCode.Space))
        {
            player.transform.parent = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("PLAYER");
            //Vector3 temp = player.transform.position;
            //player.transform.parent = gameObject.transform;
            //player.transform.position = temp;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 tra = transform.localPosition;
        //tra.y = player.transform.position.y;
        Gizmos.DrawSphere(tra * 1.3f, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.up * 0.5f, 0.1f);
        Gizmos.DrawSphere(transform.position - transform.up * 0.5f, 0.1f);
    }
}
