using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    public int Resourceamount = 1000;
	
	// Update is called once per frame
	void Update () {
        if (Resourceamount <= 0) {
            Destroy(this.gameObject);
        }
	}
}
