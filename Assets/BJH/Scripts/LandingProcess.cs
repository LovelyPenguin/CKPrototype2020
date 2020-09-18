using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingProcess : MonoBehaviour
{
    public bool isLanded = false;

    public LayerMask colliderLayer;

    public Transform landingTransform;
    public Vector3 landingNormal;
    public Vector3 landingPos;

    private void Update()
    {
        
    }

    void CheckLanding()
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position,Vector3.one/2f, -transform.up,Quaternion.identity,0.1f);

        if(hits.Length > 1)
        {
            Debug.Log(hits[0].transform.gameObject.name);


            MeshCollider collider = (MeshCollider)hits[0].collider;
            Mesh mesh = collider.sharedMesh;
            Vector3[] normals = mesh.normals;
            int[] triangles = mesh.triangles;
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (isLanded) return;

        int mask = (1 << collision.transform.gameObject.layer);

        if ((mask & colliderLayer.value) != 0)
        {
            isLanded = true;
            landingTransform = collision.transform;
            landingNormal = (collision.contacts[0].normal);
            landingPos = collision.contacts[0].point + landingNormal * 0.5f; 
        }

    }
}
