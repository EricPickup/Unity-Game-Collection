using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour {


	static public Main S;
	static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

	[Header("Set in Inspector")]
	public GameObject[] prefabEnemies;
	public float enemySpawnPerSecond = 0.5f;
	public float enemyDefaultPadding = 1.5f;
    public GameObject pauseButton;
    public GameObject levelStatus;
    public GameObject scoreText;
    public GameObject pausedText;
    public GameObject enemy0Score;
    public GameObject enemy1Score;
    public GameObject enemy2Score;
    public GameObject enemy3Score;
    public GameObject enemy4Score;
    public WeaponDefinition[] WeaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[] {
        WeaponType.blaster, WeaponType.blaster,
        WeaponType.spread, WeaponType.shield };

    private BoundsCheck bndCheck;
    private int score = 0;
    private int lvl = BRONZE;
    private Level currLvl = Settings.getLevel(BRONZE);

    const int BRONZE = 0;
    const int SILVER = 1;
    const int GOLD = 2;

    int[] enemyScores = new int[] { 0, 0, 0, 0, 0 };

    public void ShipDestroyed(Enemy e, int s)
    { // c
    	this.score+=s;
        scoreText.GetComponent<Text>().text = "Score: " + this.score;
        IncrementEnemyScore(e);
      // Potentially generate a PowerUp
        if (Random.value <= e.powerUpDropChance)
        { // d
          // Choose which PowerUp to pick
          // Pick one from the possibilities in powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length); // e
            WeaponType puType = powerUpFrequency[ndx];
            // Spawn a PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            // Set it to the proper WeaponType
            pu.SetType(puType); // f
                                // Set it to the position of the destroyed ship
            pu.transform.position = e.transform.position;
        }
    }

    public void IncrementEnemyScore(Enemy e)
    {
        if (e is Enemy_1) {
            enemyScores[1]++;
            enemy1Score.GetComponent<Text>().text = "" + enemyScores[1];
        } else if (e is Enemy_2)
        {
            enemyScores[2]++;
            enemy2Score.GetComponent<Text>().text = "" + enemyScores[2];
        } else if (e is Enemy_3)
        {
            enemyScores[3]++;
            enemy3Score.GetComponent<Text>().text = "" + enemyScores[3];
        } else if (e is Enemy_4)
        {
            enemyScores[4]++;
            enemy4Score.GetComponent<Text>().text = "" + enemyScores[4];
        } else if (e is Enemy)
        {
            enemyScores[0]++;
            enemy0Score.GetComponent<Text>().text = "" + enemyScores[0];
        }
    }

    void Awake(){
		S = this;
		bndCheck = GetComponent<BoundsCheck>();

		Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);

		WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
		foreach( WeaponDefinition def in WeaponDefinitions ){
			WEAP_DICT[def.type] = def;
		}
	}

	public void SpawnEnemy() {
		// random prefab
		int ndx = Random.Range(0, prefabEnemies.Length);
		GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

		float enemyPadding = enemyDefaultPadding;
		if (go.GetComponent<BoundsCheck>() != null){
			enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
		}

		Vector3 pos = Vector3.zero;
		float xMin = -bndCheck.camWidth + enemyPadding;
		float xMax = bndCheck.camWidth - enemyPadding;
		pos.x = Random.Range(xMin, xMax);
		pos.y = bndCheck.camHeight + enemyPadding;
		go.transform.position = pos;

		Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);
	}

	public void DelayedRestart (float delay){
		Invoke("Restart", delay);
	}

	public void Restart() {
		SceneManager.LoadScene("_Scene_0");
	}

	static public WeaponDefinition GetWeaponDefinition(WeaponType wt){
		if (WEAP_DICT.ContainsKey(wt)){
			return(WEAP_DICT[wt]);
		}
		return(new WeaponDefinition());
	}
    
    void Start()
    {
        if (lvl == BRONZE)
        {
            levelStatus.GetComponentInChildren<Text>().text = "Bronze";
            levelStatus.GetComponent<Image>().color = new Color32(189, 147, 84, 255);
        }
        else if (lvl == SILVER)
        {
            levelStatus.GetComponentInChildren<Text>().text = "Silver";
            levelStatus.GetComponent<Image>().color = new Color32(180, 180, 180, 255);
        }
        else
        {
            levelStatus.GetComponentInChildren<Text>().text = "Gold";
            levelStatus.GetComponent<Image>().color = new Color32(255, 199, 53, 255);
        }

    }

	void Update() {

		if(currLvl.getScoreValue() <= score && lvl != GOLD){
			lvl ++;
			currLvl = Settings.getLevel(lvl);
            if (lvl == BRONZE)
            {
                levelStatus.GetComponentInChildren<Text>().text = "Bronze";
                levelStatus.GetComponent<Image>().color = new Color32(189, 147, 84, 255);
            }
            else if (lvl == SILVER)
            {
                levelStatus.GetComponentInChildren<Text>().text = "Silver";
                levelStatus.GetComponent<Image>().color = new Color32(180, 180, 180, 255);
            }
            else
            {
                levelStatus.GetComponentInChildren<Text>().text = "Gold";
                levelStatus.GetComponent<Image>().color = new Color32(255, 199, 53, 255);
            }
        }
    }

    public void TogglePause()
    {
        if (Time.timeScale == 1)    //Not paused
        {
            pausedText.GetComponent<Text>().text = "PAUSED";
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
            pausedText.GetComponent<Text>().text = "";
        }
    }

}
