using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workers : MonoBehaviour {

    SelectController SC;
    GameObject MainBuilding, MemorizeMaterial;
    float Collectiontimer = 5f;
    float distanceToTarget, distanceToNexus;
    Manager manag;

    // Use this for initialization
    void Start () {
        SC = this.gameObject.GetComponent<SelectController>();
        manag = GameObject.Find("GameGod").GetComponent<Manager>();
        MainBuilding = GameObject.Find("Nexus");
	}

    void CollectResource() {
        //Collect resources when nearby

        distanceToTarget = Vector3.Distance(transform.position, SC.followobject.transform.position);
        distanceToNexus = Vector3.Distance(transform.position, MainBuilding.transform.position);

        if (distanceToTarget < 4f) {
            //If the worker is close enough to the resource to collect
            if (Collectiontimer > 0)
            {
                Collectiontimer -= Time.deltaTime;
            }
            else {
                //if done collecting, move to the nexus and hand in the resources
                if (SC.followobject.tag.Equals("Resource"))
                {
                    SC.followobject.GetComponent<Resource>().Resourceamount -= 10;
                    MemorizeMaterial = SC.followobject;
                }

                SC.followobject = MainBuilding;

                if (distanceToNexus < 10f) {
                    //If close enough to the nexus to hand it in
                    manag.Resource += 10;
                    Collectiontimer = 5f;
                    SC.followobject = MemorizeMaterial;
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
        if (SC.followobject)
        {
            if (SC.followobject.tag.Equals("Resource") || SC.followobject.name.Equals("Nexus"))
            {
                CollectResource();
            }
        }
	}
}
