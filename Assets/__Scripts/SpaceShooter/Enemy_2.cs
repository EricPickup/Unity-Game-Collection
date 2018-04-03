using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_2 : Enemy { // a

	[Header("Set in Inspector: Enemy_2")]
	public float sinEccentricity = 0.6f;
	public float lifeTime = 10;

	[Header("Set Dynamically: Enemy_2")]
	public Vector3 p0;
	public Vector3 p1;
	public float birthTime;
	private int score;

	void Start () {

		score = Settings.getScore(2);

		p0 = Vector3.zero; // b
		p0.x = -bndCheck.camWidth - bndCheck.radius;
		p0.y = Random.Range( -bndCheck.camHeight, bndCheck.camHeight );

		p1 = Vector3.zero;
		p1.x = bndCheck.camWidth + bndCheck.radius;
		p1.y = Random.Range( -bndCheck.camHeight, bndCheck.camHeight );

		if (Random.value > 0.5f) {
			p0.x *= -1;
			p1.x *= -1;
		}

		birthTime = Time.time; // c

		foreach (Transform child in this.transform){
			switch(Settings.getColor(2)){
				case 1: child.gameObject.GetComponent<Renderer>().material = Main.S.blue; break;
				case 2: child.gameObject.GetComponent<Renderer>().material = Main.S.green; break;
				case 3: child.gameObject.GetComponent<Renderer>().material = Main.S.red; break;
			}
        }
	}

	public int getScore(){
		return score;
	}

	public override void Move() {
		float u = (Time.time - birthTime) / lifeTime;

		if (u > 1) {
			Destroy( this.gameObject ); // d
			return;
		}

		u = u + sinEccentricity*(Mathf.Sin(u*Mathf.PI*2));
		pos = (1-u)*p0 + u*p1;
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