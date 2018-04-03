using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy {

	[Header("Set in Inspector: Enemy_1")]

	public float waveFrequency = 2;

	public float waveWidth = 4;
	public float waveRotY = 45;
	private float x0; // The initial x value of pos
	private float birthTime;
	private int score;

	void Start() {
		score = Settings.getScore(1);
		x0 = pos.x; // b
		birthTime = Time.time;
		foreach (Transform child in this.transform){
			switch(Settings.getColor(1)){
				case 1: child.gameObject.GetComponent<Renderer>().material = Main.S.blue; break;
				case 2: child.gameObject.GetComponent<Renderer>().material = Main.S.green; break;
				case 3: child.gameObject.GetComponent<Renderer>().material = Main.S.red; break;
			}
        }
	}

	public int getScore(){
		return score;
	}

	public override void Move() { // c
		Vector3 tempPos = pos;
		float age = Time.time - birthTime;
		float theta = Mathf.PI * 2 * age / waveFrequency;
		float sin = Mathf.Sin(theta);

		tempPos.x = x0 + waveWidth * sin;
		pos = tempPos;

		Vector3 rot = new Vector3(0, sin*waveRotY, 0);
		this.transform.rotation = Quaternion.Euler(rot);

		base.Move(); // d
	}

	void OnCollisionEnter( Collision coll ) { 
		GameObject otherGO = coll.gameObject;
		switch (otherGO.tag) {

			//print("in switch");
			case "ProjectileEnemy":
			case "ProjectilePlayer":
				Projectile p = otherGO.GetComponent<Projectile>();

				base.health -= Main.GetWeaponDefinition(p.type).damageOnHit;
				base.ShowDamage();				
				if (base.health <= 0) {
                    // Tell the Main singleton that this ship was destroyed // b
                    if (!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this, score);
                    }
                    notifiedOfDestruction = true;
                    // Destroy this Enemy
                    Destroy(this.gameObject);
                }
				Destroy( otherGO );
				break;

			default:
				print( "Enemy hit by non-ProjectileHero: " + otherGO.name ); 
				break;
		}
	}

}
