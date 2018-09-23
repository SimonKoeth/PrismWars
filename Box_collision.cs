using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_collision : MonoBehaviour {

    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "cube")
        {
            Physics.IgnoreCollision(collision.collider,GetComponent<Collider>(),true);
        }
    }
}
