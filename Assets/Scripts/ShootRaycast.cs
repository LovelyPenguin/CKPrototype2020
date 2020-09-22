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
    private Ray ray;

    private

    // Start is called before the first frame update
    void Start()
    {
        ray = new Ray(pin.position, pin.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.DrawRay(pin.position, pin.forward * distance, Color.blue, 0.3f);
            if (Physics.Raycast(pin.position, pin.forward, out hit, distance))
            {
                if (hit.transform.CompareTag("Finish"))
                {
                    Vector3 dir = hit.point - transform.position;

                    transform.LookAt(transform.position - hit.normal);
                    transform.position = hit.point - (transform.forward * 0.25f);
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
