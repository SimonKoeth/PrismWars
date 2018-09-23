using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject CreatorUnit, TargetUnit;
    public int damage = 1;

	// Use this for initialization
	void Start () {
		
	}

    private void Update()
    {
        if (TargetUnit == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //Folge dem Targetobject
            transform.position = Vector3.MoveTowards(transform.position, TargetUnit.transform.position, 0.5f);
            if (Vector3.Distance(transform.position, TargetUnit.transform.position) < 1f)
            {
                if (TargetUnit.transform.tag.Equals("enemy") || TargetUnit.transform.tag.Equals("Unit"))
                {
                    TargetUnit.GetComponent<Unitprops>().gothit(damage);
                }
                else if (TargetUnit.transform.tag.Equals("Building") || TargetUnit.transform.tag.Equals("enemyBuilding"))
                {
                    TargetUnit.GetComponent<BuildingScript>().gothit(damage);
                }
                else if (TargetUnit.transform.tag.Equals("wall"))
                {
                    TargetUnit.GetComponent<wallscript>().gothit(damage);
                }
                Destroy(this.gameObject);
            }
        }

        
    }

}
