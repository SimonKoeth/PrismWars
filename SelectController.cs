using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelectController : MonoBehaviour {

    GameGod GG;

    //public GameObject test;

    NavMeshAgent NMA;
    public Vector3 Targetdest;
    Manager manag;
    public bool isSelected = false;
    public Material unselect, select;
    Light selLight;
    public GameObject followobject;
    public GameObject ClosestEnemy;
    float updatetime;
    int shootingInvertal = 5; //Schieße einmal alle 5 ticks - ergo einmal die Sekunde
    public bool WasSpawned = false;
    public GameObject FocusEnemy; //Für Gebäude und Gegner
    GameObject ClosestWall;
    float WallRange = 9999f;
    float wallchecktime = 0.2f;
    float enemytimer = 2.0f;

    //public Unit unitprops;

    // Use this for initialization
    void Start() {
        updatetime = Random.Range(0f, 1f);
        //test.SetActive(false);

        GG = GameObject.Find("GameGod").GetComponent<GameGod>();
        if (this.gameObject.tag.Equals("Unit"))
        {
            GG.PlayerUnits.Add(this.gameObject); //Add All units to the gamegod to work with
        }
        else if (this.gameObject.tag.Equals("enemy")) {
            GG.Enemyunits.Add(this.gameObject);
        }

        manag = GameObject.Find("GameGod").GetComponent<Manager>();
        NMA = this.gameObject.GetComponent<NavMeshAgent>();
        //selLight = this.gameObject.transform.GetChild(0).GetComponent<Light>();

    }

    void isSelectedfunct()
    {
        if (isSelected)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = select;
        }
        else {
            this.gameObject.GetComponent<MeshRenderer>().material = unselect;
        }
    }

    void moveToTarget() {
            if (!followobject)
            {
                if (NMA.destination != Targetdest && Targetdest != Vector3.zero)
                {
                    NMA.SetDestination(Targetdest);
                }
                //NMA.destination = Targetdest;
            }
            else
            {
                if (NMA.destination != followobject.transform.position)
                {
                    NMA.SetDestination(followobject.transform.position);
                }
                //NMA.destination = followobject.transform.position;
        }
    }

    void ShootingFunction(GameObject CreatorUnit, GameObject TargetUnit) {
        GameObject tempBullet = (GameObject)Instantiate(this.gameObject.GetComponent<Unitprops>().newBullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
        Bullet Bulletscript = tempBullet.GetComponent<Bullet>();
        Bulletscript.CreatorUnit = CreatorUnit;
        Bulletscript.TargetUnit = TargetUnit;
        Bulletscript.damage = this.gameObject.GetComponent<Unitprops>().atk;
    }

    void DistanceToAllEnemies()
    {
        updatetime -= Time.deltaTime;

        if (updatetime < 0 && this.gameObject.tag.Equals("Unit"))
        {
            updatetime = 0.2f; //Prüfe alle 0.2 sekunden, wer die nächste Gegner Unit ist
            float closestRange = 10000f;

            if (GG.Enemyunits.Count > 1)
            {
                foreach (GameObject Enemyunit in GG.Enemyunits)
                {
                    float dist = Vector3.Distance(transform.position, Enemyunit.transform.position);
                    if (dist < closestRange && Enemyunit != ClosestEnemy)
                    {
                        //Neue unit gefunden, die näher ist
                        closestRange = dist;
                        ClosestEnemy = Enemyunit;
                        //Debug.Log("Found a new close unit");
                    }
                }
            }
            else if (GG.Enemyunits.Count == 1)
            {
                ClosestEnemy = GG.Enemyunits[0];
                closestRange = Vector3.Distance(transform.position, ClosestEnemy.transform.position);
            }
            if (closestRange <= this.gameObject.GetComponent<Unitprops>().AttackRange)
            {
                
                shootingInvertal -= 1;
                if (shootingInvertal <= 0)
                {
                    ShootingFunction(this.gameObject, ClosestEnemy);
                    shootingInvertal = 5;
                }
            }
        }
        else if (updatetime < 0 && this.gameObject.tag.Equals("enemy"))
        {
            updatetime = 0.2f; //Prüfe alle 0.2 sekunden, welcher die nächste Spielerunit ist
            float closestRange = 10000f;

            if (GG.PlayerUnits.Count > 1)
            {
                foreach (GameObject Enemyunit in GG.PlayerUnits)
                {
                    float dist = Vector3.Distance(transform.position, Enemyunit.transform.position);
                    if (dist < closestRange && Enemyunit != ClosestEnemy)
                    {
                        //Neue unit gefunden, die näher ist
                        closestRange = dist;
                        ClosestEnemy = Enemyunit;
                        //Debug.Log("Found a new close unit");
                    }
                }
            }
            else if (GG.PlayerUnits.Count == 1)
            {
                ClosestEnemy = GG.PlayerUnits[0];
                closestRange = Vector3.Distance(transform.position, ClosestEnemy.transform.position);
            }


            if (closestRange <= this.gameObject.GetComponent<Unitprops>().AttackRange)
            {
                //NMA.SetDestination(ClosestEnemy.transform.position);

                shootingInvertal -= 1;
                if (shootingInvertal <= 0)
                {
                    ShootingFunction(this.gameObject, ClosestEnemy);
                    shootingInvertal = 5;
                }
            }
            else if (closestRange <= (this.gameObject.GetComponent<Unitprops>().AttackRange * 2)) {
                NMA.SetDestination(ClosestEnemy.transform.position);
            }
            else
            {
                //Laufe zu Gebäuden von spielern und greife diese an
                if (WasSpawned)
                {
                    if (closestRange <= this.gameObject.GetComponent<Unitprops>().AttackRange)
                    {
                        FocusEnemy = ClosestEnemy;
                    }
                    else
                    {
                        if (closestRange > this.gameObject.GetComponent<Unitprops>().AttackRange || GG.PlayerUnits.Count <= 0)
                        {
                            FocusEnemy = GG.Playerbuildings[0].gameObject;
                        }

                    }
                }
            }
        }
    }

    void DistanceToWalls() {

        wallchecktime -= Time.deltaTime;

        if (ClosestWall == null ||!ClosestWall) {
            WallRange = 9999f;
        }

        if (wallchecktime <= 0)
        {
            wallchecktime = 2.0f;
            //Debug.Log("Checking for walls");
            foreach (GameObject wallobj in GG.Destructible)
            {
                float walldist = Vector3.Distance(transform.position, wallobj.transform.position);
                if (walldist < WallRange) {
                    ClosestWall = wallobj;
                    WallRange = walldist;
                }
                if (walldist < GetComponent<Unitprops>().AttackRange)
                {
                    shootingInvertal -= 1;
                    if (shootingInvertal <= 0) {
                        ShootingFunction(this.gameObject, ClosestWall);
                        shootingInvertal = 5;
                    }
                }
            }
        }
    }

    void Battlefunction() {
        if (!FocusEnemy)
        {
            DistanceToAllEnemies();
            if (NMA.speed == 0) {
                NMA.speed = this.gameObject.GetComponent<Unitprops>().speed;
            }
        }
        else {

            if (this.gameObject.tag.Equals("enemy")) {
                DistanceToAllEnemies();
            }
            followobject = FocusEnemy;

            if (Vector3.Distance(transform.position, FocusEnemy.transform.position) < this.gameObject.GetComponent<Unitprops>().AttackRange)
            {
                NMA.speed = 0;
                //Was auch immer getargeted wurde ist nun in range
                updatetime -= Time.deltaTime;
                if (updatetime <= 0f)
                {
                    updatetime = 1.0f;
                    shootingInvertal -= 1;

                    if (shootingInvertal <= 0)
                    {
                        ShootingFunction(this.gameObject, FocusEnemy);
                    }
                }
            }
            else {
                NMA.speed = this.gameObject.GetComponent<Unitprops>().speed;
            }
        }
    }

	// Update is called once per frame
	void Update () {
        moveToTarget();
        isSelectedfunct();
        DistanceToWalls();
        Battlefunction();
        }
    }
