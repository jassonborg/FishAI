using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class AOESkillSmall : MonoBehaviour
{
    public GameObject particleEffect;
    public float aoeRadius;
    public LayerMask layers;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            aoeHit();
        }
         
    }

    void aoeHit()
    {
        Instantiate(particleEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, aoeRadius);

        foreach (Collider nearbyObject in colliders)
        {

            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Destroy(gameObject);
            }
        }
        
    }

    //void aoeForce(Vector3 centerOfAOE)
    //{
    //    Collider[] monstersIHit = UnityEngine.Physics.OverlapSphere(centerOfAOE, aoeRadius, layers);


    //}

}
