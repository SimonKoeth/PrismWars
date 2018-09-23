using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallscript : MonoBehaviour {

    public int hp = 150;
    GameGod GG;

    private void Start()
    {
        GG = GameObject.Find("GameGod").GetComponent<GameGod>();
    }

    public void gothit(int damage) {
        hp -= damage;
    }

	// Update is called once per frame
	void Update () {
        if (hp <= 0) {
            GG.Destructible.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
	}
}
