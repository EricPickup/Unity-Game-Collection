using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level{

	private string name;
	private int topScore;
	private ArrayList enemies;
	private bool shootingEnabled;


    public Level(string name, int score){
		this.name = name;
		this.topScore = score;
		this.shootingEnabled = true;
		this.enemies = new ArrayList();
	}

	public void setTopScore(int score){
		this.topScore = score;
	}

	public void setShootingEnabled(bool flag){
		this.shootingEnabled = flag;
	}

	public bool addEnemy(int enemyID){
		// check if obj enemies already contains the enemy
		for (int i = 0; i < this.enemies.Count; i++){
			if (enemyID == (int)this.enemies[i]){
				return false;
			}
		}
		this.enemies.Add(enemyID);
		return true;
	}

	public bool removeEnemy(int enemyID){
		// check if object contains the enemy
		for (int i = 0; i < this.enemies.Count; i++){
			if (enemyID == (int)this.enemies[i]){
				// enemy found
				this.enemies.Remove(enemyID);
				return true;
			}
		}
		// return false as an error if enemy not found
		return false;
	}

	public bool toggleEnemy(int enemyID){
		// check if object contains the enemy
		for (int i = 0; i < this.enemies.Count; i++){
			if (enemyID == (int)this.enemies[i]){
				// enemy found
				this.enemies.Remove(enemyID);
				return false;
			}
		}
		this.enemies.Add(enemyID);
		return true;
	}

	public ArrayList getEnemies(){
		return this.enemies;
	}

	public bool getShootingEnabled(){
		return this.shootingEnabled;
	}

	public int getScoreValue(){
		return this.topScore;
	}

}

public class MainMenuController : MonoBehaviour {

	public GameObject MainCanvas, GameLevels, GameLevelsButtons, BronzeScreen, SilverScreen, GoldScreen;
	public GameObject Configuration, ConfigurationMore, ConfigurationFirst;
	public Dropdown BackgroundMusicDropdown, WinningMusicDropdown, ShootingSoundDropdown, DestroySoundDropdown;
	public Dropdown BackgroundMusicVolumeDropdown, WinningMusicVolumeDropdown, ShootingSoundVolumeDropdown, DestroySoundVolumeDropdown;

	public AudioClip[] BackgroundMusics = new AudioClip[3];

	public AudioSource pop;

	/*	BRONZE SCREEN  */
	public Image[] b_enemies;
	public Dropdown b_shootingDropdown;
	public InputField b_scoreInput;

	/*	SILVER SCREEN  */
	public Image[] s_enemies;
	public Dropdown s_shootingDropdown;
	public InputField s_scoreInput;

	/*	GOLD SCREEN  */
	public Image[] g_enemies;
	public Dropdown g_shootingDropdown;
	public InputField g_scoreInput;

	private Level bronze = new Level("bronze", 100);
	private Level silver = new Level("silver", 200);
	private Level gold = new Level("gold", 300);

	/* CONFIG */

	public InputField[] EnemyScores;
	public Dropdown[] EnemyColors;

    public Dropdown BronzeDropdown;
    public Dropdown SilverDropdown;
    public Dropdown GoldDropdown;
    public static int BronzeBG;
    public static int SilverBG;
    public static int GoldBG;

	public AudioSource BackgroundMusic;

	private Settings settings = new Settings();

    public Canvas ConfigCanvas;

    public Sprite[] sprites;

	void Start(){

        bronze.addEnemy(0);
		silver.addEnemy(0);
		silver.addEnemy(1);
		gold.addEnemy(0);
		gold.addEnemy(1);
		gold.addEnemy(2);		

		for (int i = 0; i < 5; i++){
			EnemyScores[i].text = "50";
			EnemyColors[i].value = 0;
		}

		BackgroundMusicDropdown.value = 0;

	}

	void Update(){
        BronzeBG = BronzeDropdown.value;
        SilverBG = SilverDropdown.value;
        GoldBG = GoldDropdown.value;

		AudioClip tmp = BackgroundMusic.clip;
		BackgroundMusic.clip = BackgroundMusics[BackgroundMusicDropdown.value];
		if (tmp != BackgroundMusic.clip)
			BackgroundMusic.Play();
       
    }

	/*
		MAIN MENU FUNCTIONS
	 */

	public void ShowGameLevelsMenu(){
		pop.Play();		
		MainCanvas.SetActive(false);
		GameLevels.SetActive(true);
	}

    public void BackButtonClick()
    {
        MainCanvas.SetActive(true);
        ConfigCanvas.gameObject.SetActive(false);
    }

	public Level getBronze(){
		return bronze;
	}

	public Level getSilver(){
		return silver;
	}

	public Level getGold(){
		return gold;
	}

	public void onPlayPress(){
		pop.Play();		
		settings.setLevels(bronze, silver, gold);
		settings.setScores(int.Parse(EnemyScores[0].text),int.Parse(EnemyScores[1].text),int.Parse(EnemyScores[2].text),int.Parse(EnemyScores[3].text),int.Parse(EnemyScores[4].text));
		SceneManager.LoadScene("_Scene_0");
	}


	/*
		GAME LEVELS FUNCTIONS
	 */

	public void GameLevelsToMain(){
		pop.Play();
		MainCanvas.SetActive(true);
		GameLevels.SetActive(false);
	}

		//	BRONZE
		//

	private void startBronze(){
		// set enemies color
		for (int i = 0; i < 5 ; i++){
			b_enemies[i].GetComponent<Image>().color = new Color32(186,186,186,255);
		}
		foreach ( int enemy in bronze.getEnemies()){
			b_enemies[enemy].GetComponent<Image>().color = new Color32(255,100,100,255);
		}

		// set shooting dropdown
		if (bronze.getShootingEnabled())
			b_shootingDropdown.value = 0;
		else
			b_shootingDropdown.value = 1;

		// set score
		b_scoreInput.text = bronze.getScoreValue().ToString();
	}
	private void updateBronze(){
		// set enemies color
		for (int i = 0; i < 5 ; i++){
			b_enemies[i].GetComponent<Image>().color = new Color32(186,186,186,255);
		}
		foreach ( int enemy in bronze.getEnemies()){
			b_enemies[enemy].GetComponent<Image>().color = new Color32(255,100,100,255);
		}

		// set shooting dropdown
		if (b_shootingDropdown.value == 0)
			bronze.setShootingEnabled(true);
		else
			bronze.setShootingEnabled(false);

		// set score
		bronze.setTopScore(int.Parse(b_scoreInput.text));
	}

	public void GameLevelsToBronze(){
		pop.Play();		
		GameLevelsButtons.SetActive(false);
		startBronze();
		BronzeScreen.SetActive(true);
	}

	public void BronzeToGameLevels(){
		pop.Play();		
		updateBronze();
		BronzeScreen.SetActive(false);		
		GameLevelsButtons.SetActive(true);
	}

	public void b_setEnemy0(){
		pop.Play();		
		if (bronze.toggleEnemy(0) == true){
			silver.addEnemy(0);
			gold.addEnemy(0);
		}
		updateBronze();		
	}
	public void b_setEnemy1(){
		pop.Play();		
		if (bronze.toggleEnemy(1) == true){
			silver.addEnemy(1);
			gold.addEnemy(1);
		}
		updateBronze();		
	}
	public void b_setEnemy2(){
		pop.Play();		
		if (bronze.toggleEnemy(2) == true){
			silver.addEnemy(2);
			gold.addEnemy(2);
		}
		updateBronze();		
	}
	public void b_setEnemy3(){
		pop.Play();		
		if (bronze.toggleEnemy(3) == true){
			silver.addEnemy(3);
			gold.addEnemy(3);
		}
		updateBronze();		
	}
	public void b_setEnemy4(){
		pop.Play();		
		if (bronze.toggleEnemy(4) == true){
			silver.addEnemy(4);
			gold.addEnemy(4);
		}
		updateBronze();		
	}

		// SILVER
		//

	private void startSilver(){
		// set enemies color
		for (int i = 0; i < 5 ; i++){
			s_enemies[i].GetComponent<Image>().color = new Color32(186,186,186,255);
		}
		foreach ( int enemy in silver.getEnemies()){
			s_enemies[enemy].GetComponent<Image>().color = new Color32(255,100,100,255);
		}

		// set shooting dropdown
		if (silver.getShootingEnabled())
			s_shootingDropdown.value = 0;
		else
			s_shootingDropdown.value = 1;

		// set score
		s_scoreInput.text = silver.getScoreValue().ToString();
	}
	private void updateSilver(){
		// set enemies color
		for (int i = 0; i < 5 ; i++){
			s_enemies[i].GetComponent<Image>().color = new Color32(186,186,186,255);
		}
		foreach ( int enemy in silver.getEnemies()){
			s_enemies[enemy].GetComponent<Image>().color = new Color32(255,100,100,255);
		}

		// set shooting dropdown
		if (s_shootingDropdown.value == 0)
			silver.setShootingEnabled(true);
		else
			silver.setShootingEnabled(false);

		// set score
		silver.setTopScore(int.Parse(s_scoreInput.text));
	}

	public void GameLevelsToSilver(){
		pop.Play();		
		GameLevelsButtons.SetActive(false);
		startSilver();
		SilverScreen.SetActive(true);
	}

	public void SilverToGameLevels(){
		pop.Play();	
		updateSilver();
		SilverScreen.SetActive(false);		
		GameLevelsButtons.SetActive(true);
	}

	public void s_setEnemy0(){
		pop.Play();		
		if (silver.toggleEnemy(0) == true){
			gold.addEnemy(0);
		} else {
			bronze.removeEnemy(0);
		}
		updateSilver();		
	}
	public void s_setEnemy1(){
		pop.Play();		
		if (silver.toggleEnemy(1) == true){
			gold.addEnemy(1);
		} else {
			bronze.removeEnemy(1);
		}
		updateSilver();		
	}
	public void s_setEnemy2(){
		pop.Play();		
		if (silver.toggleEnemy(2) == true){
			gold.addEnemy(2);		
		} else {
			bronze.removeEnemy(2);
		}
		updateSilver();				
	}
	public void s_setEnemy3(){
		pop.Play();		
		if (silver.toggleEnemy(3) == true){
			gold.addEnemy(3);
		} else {
			bronze.removeEnemy(3);
		}
		updateSilver();		
	}
	public void s_setEnemy4(){
		pop.Play();		
		if (silver.toggleEnemy(4) == true){
			gold.addEnemy(4);		
		} else {
			bronze.removeEnemy(4);
		}
		updateSilver();
	}

		// GOLD
		//

	private void startGold(){
		// set enemies color
		for (int i = 0; i < 5 ; i++){
			g_enemies[i].GetComponent<Image>().color = new Color32(186,186,186,255);
		}
		foreach ( int enemy in gold.getEnemies()){
			g_enemies[enemy].GetComponent<Image>().color = new Color32(255,100,100,255);
		}

		// set shooting dropdown
		if (gold.getShootingEnabled())
			g_shootingDropdown.value = 0;
		else
			g_shootingDropdown.value = 1;

		// set score
		g_scoreInput.text = gold.getScoreValue().ToString();
	}
	private void updateGold(){
		// set enemies color
		for (int i = 0; i < 5 ; i++){
			g_enemies[i].GetComponent<Image>().color = new Color32(186,186,186,255);
		}
		foreach ( int enemy in gold.getEnemies()){
			g_enemies[enemy].GetComponent<Image>().color = new Color32(255,100,100,255);
		}

		// set shooting dropdown
		if (g_shootingDropdown.value == 0)
			gold.setShootingEnabled(true);
		else
			gold.setShootingEnabled(false);

		// set score
		gold.setTopScore(int.Parse(g_scoreInput.text));
	}

	public void GameLevelsToGold(){
		pop.Play();		
		GameLevelsButtons.SetActive(false);
		startGold();
		GoldScreen.SetActive(true);
	}

	public void GoldToGameLevels(){
		pop.Play();		
		updateGold();
		GoldScreen.SetActive(false);		
		GameLevelsButtons.SetActive(true);
	}

	public void g_setEnemy0(){
		pop.Play();		
		if (gold.toggleEnemy(0) == false){
			bronze.removeEnemy(0);
			silver.removeEnemy(0);
		}
		updateGold();		
	}
	public void g_setEnemy1(){
		pop.Play();	
		if (gold.toggleEnemy(1) == false){
			bronze.removeEnemy(1);
			silver.removeEnemy(1);
		}
		updateGold();		
	}
	public void g_setEnemy2(){
		pop.Play();		
		if (gold.toggleEnemy(2) == false){
			bronze.removeEnemy(2);
			silver.removeEnemy(2);
		}
		updateGold();		
	}
	public void g_setEnemy3(){
		pop.Play();		
		if (gold.toggleEnemy(3) == false){
			bronze.removeEnemy(3);
			silver.removeEnemy(3);
		}
		updateGold();				
	}
	public void g_setEnemy4(){
		pop.Play();		
		if (gold.toggleEnemy(4) == false){
			bronze.removeEnemy(4);
			silver.removeEnemy(4);
		}
		updateGold();				
	}

	/*
		CONFIGURATION FUNCTIONS
	 */

	public void MainToConfiguration(){
		pop.Play();		
		MainCanvas.SetActive(false);
		Configuration.SetActive(true);
	}

	public void ConfigurationToMain(){
		pop.Play();
		MainCanvas.SetActive(true);
		GameLevels.SetActive(false);
        ConfigurationFirst.SetActive(true);
        ConfigurationMore.SetActive(false);
        ConfigCanvas.gameObject.SetActive(false);
        
    }

	public void ConfigurationToMoreConfig(){
		pop.Play();		
		ConfigurationFirst.SetActive(false);
		ConfigurationMore.SetActive(true);
	}
	public void ConfigurationMoreToFirstConfig(){
		pop.Play();		
		ConfigurationFirst.SetActive(true);
		ConfigurationMore.SetActive(false);
	}
	public void ConfigurationFirstToMoreConfig(){
		pop.Play();		
		ConfigurationFirst.SetActive(false);
		ConfigurationMore.SetActive(true);
	}


}
