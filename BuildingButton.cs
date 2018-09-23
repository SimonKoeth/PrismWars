using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour {

    public bool wasClicked;
    public GameObject curBuilding;
    RaycastHit hit;
    int buffercost;


    Manager mg;

    // Use this for initialization
    void Start () {
        mg = GameObject.Find("GameGod").GetComponent<Manager>();
    }

    public void Build_It(Building build) {
        Debug.Log("clicked Build_It");
        if (mg.Resource >= build.cost)
        {
            curBuilding = (GameObject)Instantiate(build.original, transform.position, transform.rotation);
            wasClicked = true;
            curBuilding.GetComponent<BuildingScript>().isSet = false;
            mg.Resource -= build.cost;
            buffercost = build.cost;
        }
    }

    public void chooseLocation()
    {
        if (wasClicked)
        {
            if (!curBuilding.GetComponent<BuildingScript>().isSet)
            {
                Debug.Log("Choosing location");
                //Button was clicked, building now follows the mouse.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    curBuilding.transform.position = new Vector3(hit.point.x, 3.2f, hit.point.z);

                    if (Input.GetButtonDown("Fire1"))
                    {
                        wasClicked = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Destroy(curBuilding);
                        wasClicked = false;
                        mg.Resource += buffercost;
                    }
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
        chooseLocation();
	}
}
