using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGod : MonoBehaviour {

    public List<GameObject> PlayerUnits, Enemyunits;
    public List<GameObject> Playerbuildings, Enemybuildings, Destructible;
    public bool IsBuilding = false;
    public GameObject SelBuilding;
    public int Supply;
    public int MaxSupply = 7;
    public GameObject UnitExplosion;
    public GameObject BuildingExplosion;

	/*void Start () {
		
	}
	
	void Update () {
		
	}*/
}
