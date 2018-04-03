using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy { 

	[Header("Set in Inspector: Enemy_3")]
	public float lifeTime = 5;

	[Header("Set Dynamically: Enemy_3")]
	public Vector3 [] points;
	public float birthTime;

	private int score;

	void Start () {

		score = Settings.getScore(3);

	
		points = new Vector3[3]; 
		points[0] = pos;

		float xMin = -bndCheck.camWidth + bndCheck.radius;
		float xMax = bndCheck.camWidth - bndCheck.radius;
		Vector3 v;

		v = Vector3.zero;
		v.x = Random.Range( xMin, xMax );
		v.y = -bndCheck.camHeight * Random.Range( 2.75f, 2 );
		points[1] = v;
		v = Vector3.zero;

		v.y = pos.y;
		v.x = Random.Range( xMin, xMax );
		points[2] = v;

		birthTime = Time.time;

		foreach (Transform child in this.transform){
			switch(Settings.getColor(3)){
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
			Destroy( this.gameObject );
			return;
		}

		Vector3 p01, p12;
		u = u - 0.2f*Mathf.Sin(u * Mathf.PI * 2);
		p01 = (1-u)*points[0] + u*points[1];
		p12 = (1-u)*points[1] + u*points[2];
		pos = (1-u)*p01 + u*p12;
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