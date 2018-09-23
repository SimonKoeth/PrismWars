using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingScript : MonoBehaviour {

    public bool isClicked = false;
    public Building building;
    public bool isSet = true;
    Manager mg;
    public float buildTime, maxBuildTime;
    Material Copymat;
    GameGod GG;
    bool FinishedBuilding = false;

    public int hp, def;

	// Use this for initialization
	void Start () {
        Copymat = GetComponent<MeshRenderer>().material;
        mg = GameObject.Find("GameGod").GetComponent<Manager>();
        GG = GameObject.Find("GameGod").GetComponent<GameGod>();

        if (this.gameObject.tag.Equals("Building"))
        {
            GG.Playerbuildings.Add(this.gameObject);
        }
        else if (this.gameObject.tag.Equals("enemyBuilding")) {
            GG.Enemybuildings.Add(this.gameObject);
        }

        this.hp = building.hp;
        this.def = building.def;
        buildTime = 0;
        maxBuildTime = building.BuildTime;
	}

    public void BuildUnit(int index) {
        //Index wird via Button Event übergeben, welcher eine liste an möglichen baubaren units enthält
        if (mg.Resource > building.SpawnUnit[index].cost && GG.Supply < GG.MaxSupply)
        {
            GameObject NewUnit;
            Debug.Log("Building a unit");
            NewUnit = (GameObject)Instantiate(building.SpawnUnit[index].original, transform.position, transform.rotation);
            NewUnit.GetComponent<NavMeshAgent>().speed = building.SpawnUnit[index].speed;
            NewUnit.GetComponent<NavMeshAgent>().acceleration = 1000f;
            mg.Resource -= building.SpawnUnit[index].cost;
        }
    }

    public void gothit(int damage) {
        hp -= damage;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && !isSet)
        {
            isSet = true;
            GetComponent<MeshRenderer>().material = Copymat;
            Copymat.SetFloat("Vector1_84569363", 0.5f);
        }

        if (isSet && buildTime < maxBuildTime) {
            //Wird noch gebaut
            buildTime += Time.deltaTime;
            FinishedBuilding = false;
            float onepercent = maxBuildTime / 100;
            float percent = buildTime / maxBuildTime;
            Copymat.SetFloat("Vector1_84569363", 0.5f - (percent * 1.4f));
        }

        if (isSet && buildTime >= maxBuildTime && !FinishedBuilding) {
            FinishedBuilding = true;
            GG.MaxSupply += building.addsSupply;
        }

        if (hp <= 0) {
            if (this.gameObject.tag.Equals("enemyBuilding"))
            {
                GG.Enemybuildings.Remove(this.gameObject);
            }
            else if (this.gameObject.tag.Equals("Building")) {
                GG.Playerbuildings.Remove(this.gameObject);
                GG.MaxSupply -= building.addsSupply;
            }
            Instantiate(GG.BuildingExplosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
