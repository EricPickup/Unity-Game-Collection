using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[Header("Set in Inspector: Enemy")]
	public float speed = 10f;
	public float fireRate = 0.3f;
	public float health = 10;
	public float showDamageDuration = 0.1f;
    public float powerUpDropChance = 1f; // Chance to drop a power-up

    [Header("Set Dynamically: Enemy")]
	public Color[] originalColors;
	public Material[] materials;
	public bool showingDamage = false;
	public float damageDoneTime; 
	public bool notifiedOfDestruction = false; 

	protected BoundsCheck bndCheck;
	private int score;


	void Awake(){
		bndCheck = GetComponent<BoundsCheck>();

		materials = Utils.GetAllMaterials( gameObject );
		originalColors = new Color[materials.Length];
		for (int i=0; i<materials.Length; i++) {
			originalColors[i] = materials[i].color;
		}
		
	}

	void Start(){
		score = Settings.getScore(0);
		foreach (Transform child in this.transform){
			switch(Settings.getColor(0)){
				case 1: child.gameObject.GetComponent<Renderer>().material = Main.S.blue; break;
				case 2: child.gameObject.GetComponent<Renderer>().material = Main.S.green; break;
				case 3: child.gameObject.GetComponent<Renderer>().material = Main.S.red; break;
			}
        }

	}

	public Vector3 pos {
		get {
			return(this.transform.position);
		}
		set {
			this.transform.position = value;
		}
	}

	void Update() {
		Move();

		if ( showingDamage && Time.time > damageDoneTime ) { 
			UnShowDamage();
		}

		if (bndCheck != null && bndCheck.offDown){
			if (pos.y < bndCheck.camHeight - bndCheck.radius){
				Destroy(gameObject);
			}
		}
	}

	public virtual void Move(){
		Vector3 tempPos = pos;
		tempPos.y -= speed * Time.deltaTime;
		pos = tempPos;
	}

	void OnCollisionEnter( Collision coll ) { 
		GameObject otherGO = coll.gameObject;
		switch (otherGO.tag) {
			//print("in switch");
			case "ProjectileEnemy":
			case "ProjectilePlayer":
				Projectile p = otherGO.GetComponent<Projectile>();

				health -= Main.GetWeaponDefinition(p.type).damageOnHit;
				ShowDamage();				
				if (health <= 0) {
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

	public void ShowDamage() {
		foreach (Material m in materials) {
			m.color = Color.red;
		}
		showingDamage = true;
		damageDoneTime = Time.time + showDamageDuration;
	}
	void UnShowDamage() {
		for ( int i=0; i<materials.Length; i++ ) {
			materials[i].color = originalColors[i];
		}
		showingDamage = false;
	}

}
