using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	static public Player S; // Singleton
 
	[Header("Set in Inspector")]
	// ship movement
	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;
	public float gameRestartDelay = 2f;
	public GameObject projectilePrefab;
	public float projectileSpeed = 40;
    public Weapon[] weapons;

	[Header("Set Dynamically")]
	[SerializeField]
	private float _shieldLevel = 1;
	private GameObject lastTriggerGo = null;

	public delegate void WeaponFireDelegate();
	public WeaponFireDelegate fireDelegate;

    public AudioSource shoot;

    void Start() {
		if (S == null){
			S = this;    // set singleton
            // Reset the weapons to start _Hero with 1 blaster
            ClearWeapons();
            weapons[0].SetType(WeaponType.blaster);
        } else {
			Debug.LogError("Player.Awake() - Attempted to assign second Player.S!");
		}
		//fireDelegate += TempFire;
	}

	// Update is called once per frame
	void Update () {
		// get axis input
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");

		// change pos 
		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;

		// rotation
		transform.rotation = Quaternion.Euler(yAxis*pitchMult, xAxis*rollMult, 0);

		// if (Input.GetKeyDown(KeyCode.Space)){
		// 	TempFire();
		// }

		if (Input.GetAxis("Jump") == 1 && fireDelegate != null) { 
			fireDelegate();
            shoot.Play();
        }
	}

	void TempFire(){
     
		GameObject projGO = Instantiate<GameObject>(projectilePrefab);
		projGO.transform.position = transform.position;
		Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
		//rigidB.velocity = Vector3.up * projectileSpeed;

		Projectile proj = projGO.GetComponent<Projectile>(); // h
		proj.type = WeaponType.blaster;
		float tSpeed = Main.GetWeaponDefinition( proj.type ).velocity;
		rigidB.velocity = Vector3.down * tSpeed;

	}

	void OnTriggerEnter(Collider other){
		Transform rootT = other.gameObject.transform.root;
		GameObject go = rootT.gameObject;
		
		if (go == lastTriggerGo){
			return;
		}
		if (go.tag == "ProjectileEnemy"){
			shieldLevel--;
			Destroy(go);
		}
		lastTriggerGo = go;
		if (go.tag == "Enemy"){
			shieldLevel--;
			Destroy(go);
		} 

		else if (go.tag == "PowerUp"){
            //If the shield was triggered by a PowerUp
            AbsorbPowerUp(go);
        }

		else {
			print("Triggered by non-enemy: "+go.name);
		}
	}

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield: // a
                shieldLevel++;
                break;
            default: // b
                if (pu.type == weapons[0].type)
                { // If it is the same type // c
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        // Set it to pu.type
                        w.SetType(pu.type);
                    }
                }
                else
                { // If this is a different weapon type // d
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;

        }
        pu.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel {
		get {
			return(_shieldLevel);
		}
		set {
			_shieldLevel = Mathf.Min(value, 4);
			if (value < 0){ //game 
                if (Users.CurrentUser == null)
                {
                    MenuManager.logs.Add(new GameLog("admin", System.DateTime.Now.ToString(), Main.score.ToString(), Main.lvl.ToString()));
                } else
                {
                    MenuManager.logs.Add(new GameLog(Users.CurrentUser.username, System.DateTime.Now.ToString(), Main.score.ToString(), Main.lvl.ToString()));
                }
                
                MenuManager.SaveGameData();
				Destroy(this.gameObject);
				Main.S.DelayedRestart(gameRestartDelay);
			}
		}
	}

    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }
    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }



}