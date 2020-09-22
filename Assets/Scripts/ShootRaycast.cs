using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRaycast : MonoBehaviour
{
    [Range(0.01f, 5)]
    public float distance;
    [SerializeField]
    private Transform pin;
    public RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.DrawRay(pin.position, pin.forward * distance, Color.blue, 0.3f);
            if (Physics.Raycast(pin.position, pin.forward, out hit, distance))
            {
                if (hit.transform.CompareTag("Finish"))
                {
                    transform.position = hit.point;
                    transform.parent = hit.transform;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawRay(pin.position, pin.forward * distance);
    }
}
