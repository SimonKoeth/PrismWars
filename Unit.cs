using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Prism Wars/Unit")]
public class Unit : ScriptableObject {

    public new string name;
    public string description;

    public Sprite UISprite;
    public GameObject original;
    public int cost;
    public GameObject Bullet;

    public float speed;
    public int hp;
    public int attack;
    public int def;
    public float AttackRange;
}
