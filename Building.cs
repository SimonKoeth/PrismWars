using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Prism Wars/Building")]
public class Building : ScriptableObject {

    public new string name;
    public string description;

    public Sprite UISprite;
    public GameObject original;
    public int cost;

    public List<Unit> SpawnUnit;

    public float BuildTime = 20f;

    public int addsSupply = 0;
    public int hp;
    public int def;

    public List<Building> Requirement;
}
