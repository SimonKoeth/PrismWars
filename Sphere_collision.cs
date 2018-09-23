using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_collision : MonoBehaviour {

    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        if (!Equals(collision.gameObject.tag,"turtle"))
        {
            Physics.IgnoreCollision(collision.collider,GetComponent<Collider>(),true);
        }
    }
}
