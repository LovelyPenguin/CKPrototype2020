using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingProcess : MonoBehaviour
{
    public bool isLanded = false;

    public LayerMask colliderLayer;

    [HideInInspector] public Transform landedTransform;
    public Vector3 landingNormal;
    public Vector3 landingPos;



    private void OnCollisionStay(Collision collision)
    {
        if (isLanded) return;

        int mask = (1 << collision.transform.gameObject.layer);

        if ((mask & colliderLayer.value) != 0)
        {
            Debug.Log("AA");
            isLanded = true;
            landedTransform = collision.transform;
            landingNormal = (collision.contacts[0].normal);
            landingPos = collision.contacts[0].point + landingNormal * 0.02f; 
        }

    }
}
